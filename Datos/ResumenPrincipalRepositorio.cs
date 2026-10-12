using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para el resumen del formulario principal
    /// (tarjetas y desgloses de facturado/pendiente, y gráfico anual).
    ///
    /// Todo se calcula sobre el estado ACTUAL de cada tarea
    /// (VW_TAREAS_ESTADO_ACTUAL, una fila por tarea), igual que el
    /// Pool de Tareas, para que las cifras cuadren entre pantallas:
    ///   - Facturado: tareas cuyo estado actual es "Facturado" y cuya
    ///     factura (FEC_FACT) cae en el periodo pedido; importe real
    ///     facturado (IMP_FACT) en la divisa de la factura.
    ///   - Pendiente de facturar: TODAS las tareas cuyo estado actual
    ///     es "En Proceso" o "Pendiente" (bloqueadas), sin filtro de
    ///     fechas (es la cartera pendiente a día de hoy); Importe
    ///     Estimado en la moneda estimada de la tarea.
    ///   - Sin asignar: tareas en estado "Registrado" sin usuario,
    ///     también a día de hoy.
    /// No se suma sobre el histórico TBL_CONTROL: una misma factura
    /// puede aparecer en varios eventos (cambio de usuario,
    /// bloqueo/desbloqueo de una tarea facturada) y se contaría varias
    /// veces, y los eventos antiguos de tareas que ya no están
    /// facturadas seguirían sumando.
    ///
    /// Los importes se agrupan siempre también por divisa, para no
    /// sumar nunca importes de divisas distintas. Todas las consultas
    /// devuelven las columnas: (grupo), COD_DIV, IMPORTE, NUM_DOC.
    /// </summary>
    internal class ResumenPrincipalRepositorio
    {
        /// <summary>
        /// Obtiene, por usuario asignado y divisa, el importe
        /// facturado cuya fecha de factura cae en el rango
        /// [<paramref name="desde"/>, <paramref name="hasta"/>) (hasta
        /// exclusive). Columnas: NOMBRE_USUARIO, COD_DIV, IMPORTE,
        /// NUM_DOC.
        /// </summary>
        public DataTable ObtenerFacturadoPorUsuarioYDivisa(DateTime desde, DateTime hasta)
        {
            return ObtenerFacturadoAgrupado("NOMBRE_USUARIO", "(Sin usuario)", desde, hasta);
        }

        /// <summary>
        /// Obtiene, por segmento y divisa, el importe facturado cuya
        /// fecha de factura cae en el rango [<paramref name="desde"/>,
        /// <paramref name="hasta"/>). Columnas: DES_SEGMENTO, COD_DIV,
        /// IMPORTE, NUM_DOC.
        /// </summary>
        public DataTable ObtenerFacturadoPorSegmentoYDivisa(DateTime desde, DateTime hasta)
        {
            return ObtenerFacturadoAgrupado("DES_SEGMENTO", "(Sin segmento)", desde, hasta);
        }

        /// <summary>
        /// Obtiene, por tipo de documento y divisa, el importe
        /// facturado cuya fecha de factura cae en el rango
        /// [<paramref name="desde"/>, <paramref name="hasta"/>).
        /// Columnas: DES_TIP_FACT, COD_DIV, IMPORTE, NUM_DOC.
        /// </summary>
        public DataTable ObtenerFacturadoPorTipoDocumentoYDivisa(DateTime desde, DateTime hasta)
        {
            return ObtenerFacturadoAgrupado("DES_TIP_FACT", "(Sin tipo)", desde, hasta);
        }

        /// <summary>
        /// Obtiene, por usuario asignado y moneda estimada, el
        /// pendiente de facturar actual. Columnas: NOMBRE_USUARIO,
        /// COD_DIV, IMPORTE, NUM_DOC.
        /// </summary>
        public DataTable ObtenerPendientePorUsuarioYDivisa()
        {
            return ObtenerPendienteAgrupado("NOMBRE_USUARIO", "(Sin usuario)");
        }

        /// <summary>
        /// Obtiene, por segmento y moneda estimada, el pendiente de
        /// facturar actual. Columnas: DES_SEGMENTO, COD_DIV, IMPORTE,
        /// NUM_DOC.
        /// </summary>
        public DataTable ObtenerPendientePorSegmentoYDivisa()
        {
            return ObtenerPendienteAgrupado("DES_SEGMENTO", "(Sin segmento)");
        }

        /// <summary>
        /// Obtiene, por tipo de documento y moneda estimada, el
        /// pendiente de facturar actual. Columnas: DES_TIP_FACT,
        /// COD_DIV, IMPORTE, NUM_DOC.
        /// </summary>
        public DataTable ObtenerPendientePorTipoDocumentoYDivisa()
        {
            return ObtenerPendienteAgrupado("DES_TIP_FACT", "(Sin tipo)");
        }

        /// <summary>
        /// Obtiene, por moneda estimada, el Importe Estimado y el
        /// número de las tareas actualmente en estado "Registrado" que
        /// todavía no tienen usuario asignado, sin filtro de fechas
        /// (a día de hoy). Columnas: COD_DIV, IMPORTE, NUM_DOC.
        /// </summary>
        public DataTable ObtenerSinAsignarPorDivisa()
        {
            const string sql = @"
                SELECT
                    COD_DIV_EST AS COD_DIV,
                    ISNULL(SUM(IMP_ESTIMADO), 0) AS IMPORTE,
                    COUNT(*) AS NUM_DOC
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstRegistrado
                    AND COD_SEQ_USER IS NULL
                GROUP BY COD_DIV_EST;";

            DataTable tabla = new DataTable();

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstRegistrado", SqlDbType.Int).Value = EstadosTareaConocidos.Registrado;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por usuario, la suma del Importe Estimado de las
        /// tareas actualmente en estado "En Proceso" que entraron en
        /// ese estado dentro del rango [<paramref name="desde"/>,
        /// <paramref name="hasta"/>). Columnas: NOMBRE_USUARIO,
        /// IMPORTE. Usada por el informe de Ranking de Usuarios (no
        /// por el cuadro de mando, que usa ObtenerPendientePorUsuarioYDivisa).
        /// </summary>
        public DataTable ObtenerPendientePorUsuario(DateTime desde, DateTime hasta)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ISNULL(NOMBRE_USUARIO, N'(Sin usuario)') AS NOMBRE_USUARIO,
                    SUM(IMP_ESTIMADO) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstEnProceso
                    AND FEC_ULTIMO_CAMBIO >= @DESDE
                    AND FEC_ULTIMO_CAMBIO < @HASTA
                GROUP BY NOMBRE_USUARIO
                ORDER BY NOMBRE_USUARIO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstEnProceso", SqlDbType.Int).Value = EstadosTareaConocidos.EnProceso;
                comando.Parameters.Add("@DESDE", SqlDbType.DateTime2).Value = desde;
                comando.Parameters.Add("@HASTA", SqlDbType.DateTime2).Value = hasta;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por mes y divisa, el importe facturado del año
        /// indicado (tareas actualmente en estado "Facturado", según
        /// la fecha de su factura). Columnas: MES (1-12), COD_DIV,
        /// IMPORTE.
        /// </summary>
        public DataTable ObtenerFacturadoMensual(int anio)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    MONTH(FEC_FACT) AS MES,
                    COD_DIV_FAC AS COD_DIV,
                    SUM(IMP_FACT) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstFacturado
                    AND FEC_FACT >= DATEFROMPARTS(@ANIO, 1, 1)
                    AND FEC_FACT < DATEFROMPARTS(@ANIO + 1, 1, 1)
                GROUP BY MONTH(FEC_FACT), COD_DIV_FAC
                ORDER BY MES;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;
                comando.Parameters.Add("@ANIO", SqlDbType.Int).Value = anio;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Suma el importe facturado (IMP_FACT, divisa de la factura)
        /// y cuenta las tareas actualmente "Facturado" cuya factura
        /// cae en [<paramref name="desde"/>, <paramref name="hasta"/>),
        /// agrupando por <paramref name="columnaGrupo"/> (columna de
        /// VW_TAREAS_ESTADO_ACTUAL; siempre una constante del código,
        /// nunca un dato del usuario) y divisa. Los valores nulos del
        /// grupo se muestran como <paramref name="textoSinValor"/>.
        /// </summary>
        private DataTable ObtenerFacturadoAgrupado(
            string columnaGrupo, string textoSinValor, DateTime desde, DateTime hasta)
        {
            string sql = @"
                SELECT
                    ISNULL(" + columnaGrupo + @", @SIN_VALOR) AS " + columnaGrupo + @",
                    COD_DIV_FAC AS COD_DIV,
                    SUM(IMP_FACT) AS IMPORTE,
                    COUNT(*) AS NUM_DOC
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstFacturado
                    AND FEC_FACT >= @DESDE
                    AND FEC_FACT < @HASTA
                GROUP BY " + columnaGrupo + @", COD_DIV_FAC;";

            DataTable tabla = new DataTable();

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@SIN_VALOR", SqlDbType.NVarChar, 50).Value = textoSinValor;
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;
                comando.Parameters.Add("@DESDE", SqlDbType.Date).Value = desde.Date;
                comando.Parameters.Add("@HASTA", SqlDbType.Date).Value = hasta.Date;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Suma el Importe Estimado (moneda estimada de la tarea) y
        /// cuenta las tareas actualmente pendientes de facturar ("En
        /// Proceso" o "Pendiente"/bloqueadas), sin filtro de fechas,
        /// agrupando por <paramref name="columnaGrupo"/> (columna de
        /// VW_TAREAS_ESTADO_ACTUAL; siempre una constante del código,
        /// nunca un dato del usuario) y divisa. Las tareas sin Importe
        /// Estimado cuentan en NUM_DOC pero no suman importe.
        /// </summary>
        private DataTable ObtenerPendienteAgrupado(string columnaGrupo, string textoSinValor)
        {
            string sql = @"
                SELECT
                    ISNULL(" + columnaGrupo + @", @SIN_VALOR) AS " + columnaGrupo + @",
                    COD_DIV_EST AS COD_DIV,
                    ISNULL(SUM(IMP_ESTIMADO), 0) AS IMPORTE,
                    COUNT(*) AS NUM_DOC
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST IN (@IdEstEnProceso, @IdEstPendiente)
                GROUP BY " + columnaGrupo + @", COD_DIV_EST;";

            DataTable tabla = new DataTable();

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@SIN_VALOR", SqlDbType.NVarChar, 50).Value = textoSinValor;
                comando.Parameters.Add("@IdEstEnProceso", SqlDbType.Int).Value = EstadosTareaConocidos.EnProceso;
                comando.Parameters.Add("@IdEstPendiente", SqlDbType.Int).Value = EstadosTareaConocidos.Pendiente;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }
    }
}
