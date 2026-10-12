using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los informes de la aplicación (menú
    /// Informes): resumen mensual de facturación, motivos de bloqueo,
    /// antigüedad de tareas abiertas, ranking de usuarios, facturación
    /// por proyecto/segmento, desviación estimado/facturado,
    /// trazabilidad de una tarea y cancelaciones. Todas las consultas
    /// que necesitan "el estado actual de una tarea" se apoyan en
    /// VW_TAREAS_ESTADO_ACTUAL (no en TBL_CONTROL directamente) para
    /// no contar más de una vez una tarea con varios eventos
    /// históricos del mismo tipo (p. ej. una tarea facturada, corregida
    /// y vuelta a facturar).
    /// </summary>
    internal class InformeRepositorio
    {
        /// <summary>
        /// Obtiene, agrupado por mes (columna PERIODO, formato
        /// "yyyy-MM"), el importe de cada una de las tres series del
        /// informe de facturación (columna SERIE: "FACTURADO",
        /// "PENDIENTE", "BLOQUEADO"; columna IMPORTE):
        ///   - FACTURADO: suma de TBL_FACTURADO.IMP_FACT de las tareas
        ///     ya facturadas, agrupada por su fecha de factura.
        ///   - PENDIENTE: suma del Importe Estimado de las tareas
        ///     actualmente "En Proceso" (pendientes de facturar),
        ///     agrupada por la fecha en que entraron en ese estado.
        ///   - BLOQUEADO: igual que PENDIENTE pero para las tareas
        ///     actualmente en el estado "Pendiente" (ver
        ///     Datos/EstadosTareaConocidos.cs: es el que usa el Pool
        ///     de Tareas al bloquear una tarea).
        /// Los filtros de fecha y usuario son opcionales (null/0 =
        /// sin filtrar); se aplican sobre la fecha propia de cada
        /// serie (fecha de factura para FACTURADO, fecha del último
        /// cambio de estado para las otras dos) y sobre el usuario
        /// actualmente asociado a la tarea en las tres. Cada estado
        /// se identifica por su ID (COD_SEQ_EST), no por su texto
        /// (DES_ESTADO es editable desde negocio y no es fiable para
        /// comparar en código).
        /// </summary>
        public DataTable ObtenerResumenFacturacion(DateTime? fechaDesde, DateTime? fechaHasta, int idUsuario)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    FORMAT(FEC_FACT, 'yyyy-MM') AS PERIODO,
                    'FACTURADO' AS SERIE,
                    SUM(IMP_FACT) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstFacturado
                    AND (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)
                    AND (@FEC_DESDE IS NULL OR FEC_FACT >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_FACT < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY FORMAT(FEC_FACT, 'yyyy-MM')

                UNION ALL

                SELECT
                    FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM') AS PERIODO,
                    'PENDIENTE' AS SERIE,
                    SUM(IMP_ESTIMADO) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstEnProceso
                    AND (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)
                    AND (@FEC_DESDE IS NULL OR FEC_ULTIMO_CAMBIO >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_ULTIMO_CAMBIO < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM')

                UNION ALL

                SELECT
                    FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM') AS PERIODO,
                    'BLOQUEADO' AS SERIE,
                    SUM(IMP_ESTIMADO) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstPendiente
                    AND (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)
                    AND (@FEC_DESDE IS NULL OR FEC_ULTIMO_CAMBIO >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_ULTIMO_CAMBIO < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM')

                ORDER BY PERIODO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;
                comando.Parameters.Add("@IdEstEnProceso", SqlDbType.Int).Value = EstadosTareaConocidos.EnProceso;
                comando.Parameters.Add("@IdEstPendiente", SqlDbType.Int).Value = EstadosTareaConocidos.Pendiente;

                comando.Parameters.Add("@COD_SEQ_USER", SqlDbType.Int).Value = idUsuario;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.Date).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.Date).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por motivo de bloqueo, el número de tareas y la
        /// suma de su Importe Estimado, para las tareas actualmente
        /// Bloqueadas (Pendiente). Columnas: DES_MOTIVO, NUM_TAREAS,
        /// IMPORTE. Filtros de fecha (sobre la fecha de registro,
        /// igual que el resto de pantallas) y usuario opcionales.
        /// </summary>
        public DataTable ObtenerMotivosBloqueo(DateTime? fechaDesde, DateTime? fechaHasta, int idUsuario)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ISNULL(DES_MOTIVO, N'(Sin motivo)') AS DES_MOTIVO,
                    COUNT(*) AS NUM_TAREAS,
                    SUM(ISNULL(IMP_ESTIMADO, 0)) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstPendiente
                    AND (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)
                    AND (@FEC_DESDE IS NULL OR FEC_REG >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_REG < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY DES_MOTIVO
                ORDER BY NUM_TAREAS DESC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstPendiente", SqlDbType.Int).Value = EstadosTareaConocidos.Pendiente;
                comando.Parameters.Add("@COD_SEQ_USER", SqlDbType.Int).Value = idUsuario;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.Date).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.Date).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene las tareas actualmente abiertas (Registrado, En
        /// Proceso o Bloqueada, es decir, todo lo que no sea Facturado
        /// ni Cancelado) junto con los días transcurridos desde su
        /// último cambio de estado, para detectar tareas "olvidadas".
        /// Columnas: ID_TAREA, DES_DOC, DES_PROYECTO, DES_SEGMENTO,
        /// NOMBRE_USUARIO, DES_ESTADO, IMP_ESTIMADO,
        /// DIAS_SIN_MOVIMIENTO. Filtro de usuario opcional.
        /// </summary>
        public DataTable ObtenerAntiguedadTareasAbiertas(int idUsuario)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ID_TAREA, DES_DOC, DES_PROYECTO, DES_SEGMENTO,
                    ISNULL(NOMBRE_USUARIO, N'(Sin asignar)') AS NOMBRE_USUARIO,
                    DES_ESTADO, IMP_ESTIMADO,
                    DATEDIFF(day, FEC_ULTIMO_CAMBIO, GETDATE()) AS DIAS_SIN_MOVIMIENTO
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST IN (@IdRegistrado, @IdEnProceso, @IdPendiente)
                    AND (@COD_SEQ_USER = 0 OR COD_SEQ_USER = @COD_SEQ_USER)
                ORDER BY DIAS_SIN_MOVIMIENTO DESC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdRegistrado", SqlDbType.Int).Value = EstadosTareaConocidos.Registrado;
                comando.Parameters.Add("@IdEnProceso", SqlDbType.Int).Value = EstadosTareaConocidos.EnProceso;
                comando.Parameters.Add("@IdPendiente", SqlDbType.Int).Value = EstadosTareaConocidos.Pendiente;
                comando.Parameters.Add("@COD_SEQ_USER", SqlDbType.Int).Value = idUsuario;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por usuario, el número de tareas que tiene
        /// actualmente asignadas en un estado "abierto" (Registrado,
        /// En Proceso o Bloqueada). Columnas: NOMBRE_USUARIO,
        /// NUM_TAREAS. Usado por el ranking de usuarios junto con
        /// ResumenPrincipalRepositorio (facturado/pendiente) y
        /// ObtenerDiasMediosHastaFacturar (tiempo medio).
        /// </summary>
        public DataTable ObtenerTareasAbiertasPorUsuario()
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    NOMBRE_USUARIO,
                    COUNT(*) AS NUM_TAREAS
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST IN (@IdRegistrado, @IdEnProceso, @IdPendiente)
                    AND NOMBRE_USUARIO IS NOT NULL
                GROUP BY NOMBRE_USUARIO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdRegistrado", SqlDbType.Int).Value = EstadosTareaConocidos.Registrado;
                comando.Parameters.Add("@IdEnProceso", SqlDbType.Int).Value = EstadosTareaConocidos.EnProceso;
                comando.Parameters.Add("@IdPendiente", SqlDbType.Int).Value = EstadosTareaConocidos.Pendiente;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por usuario, el número medio de días transcurridos
        /// entre el registro de una tarea (TBL_TAREAS.FEC_REG) y el
        /// momento en que se facturó (fecha del evento "Facturado" de
        /// TBL_CONTROL), para las tareas facturadas en el rango de
        /// fechas indicado. Columnas: NOMBRE_USUARIO, DIAS_MEDIOS.
        /// </summary>
        public DataTable ObtenerDiasMediosHastaFacturar(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    USR.NOMBRE AS NOMBRE_USUARIO,
                    AVG(DATEDIFF(day, T.FEC_REG, C.FEC_ALTA)) AS DIAS_MEDIOS
                FROM dbo.TBL_CONTROL AS C
                INNER JOIN dbo.TBL_TAREAS AS T ON T.ID = C.COD_SEQ_TAR
                INNER JOIN dbo.TBL_USUARIOS AS USR ON USR.ID = C.COD_SEQ_USER
                WHERE C.COD_SEQ_EST = @IdEstFacturado
                    AND (@FEC_DESDE IS NULL OR C.FEC_ALTA >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR C.FEC_ALTA < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY USR.NOMBRE;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.DateTime2).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.DateTime2).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date.AddDays(1) : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, por proyecto y divisa, el importe facturado en el
        /// rango de fechas indicado (sobre la fecha de factura).
        /// Columnas: GRUPO (nombre del proyecto), COD_DIV, IMPORTE.
        /// </summary>
        public DataTable ObtenerFacturacionPorProyecto(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return ObtenerFacturacionAgrupada("DES_PROYECTO", fechaDesde, fechaHasta);
        }

        /// <summary>
        /// Igual que <see cref="ObtenerFacturacionPorProyecto"/> pero
        /// agrupado por segmento en vez de por proyecto.
        /// </summary>
        public DataTable ObtenerFacturacionPorSegmento(DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return ObtenerFacturacionAgrupada("DES_SEGMENTO", fechaDesde, fechaHasta);
        }

        /// <summary>
        /// Consulta común a <see cref="ObtenerFacturacionPorProyecto"/>
        /// y <see cref="ObtenerFacturacionPorSegmento"/>: agrupa el
        /// importe facturado por la columna indicada (nombre de tabla
        /// fijo, nunca viene del usuario) y por divisa.
        /// </summary>
        private DataTable ObtenerFacturacionAgrupada(string columnaAgrupacion, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            DataTable tabla = new DataTable();

            string sql = @"
                SELECT
                    ISNULL(" + columnaAgrupacion + @", N'(Sin asignar)') AS GRUPO,
                    COD_DIV,
                    SUM(IMP_FACT) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstFacturado
                    AND (@FEC_DESDE IS NULL OR FEC_FACT >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_FACT < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY " + columnaAgrupacion + @", COD_DIV
                ORDER BY GRUPO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.Date).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.Date).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, para las tareas actualmente Facturadas que tienen
        /// Importe Estimado informado, la desviación entre ese importe
        /// estimado y el importe finalmente facturado. Columnas:
        /// ID_TAREA, DES_DOC, DES_PROYECTO, NOMBRE_USUARIO,
        /// IMP_ESTIMADO, IMP_FACT, COD_DIV, FEC_FACT, DIFERENCIA,
        /// DIFERENCIA_PCT. El Importe Estimado no tiene divisa propia
        /// (solo la factura la tiene, ver COD_DIV): la desviación
        /// compara magnitudes, no hace conversión de cambio.
        /// </summary>
        public DataTable ObtenerDesviacionEstimadoFacturado(DateTime? fechaDesde, DateTime? fechaHasta, int idProyecto)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ID_TAREA, DES_DOC, DES_PROYECTO,
                    ISNULL(NOMBRE_USUARIO, N'(Sin asignar)') AS NOMBRE_USUARIO,
                    IMP_ESTIMADO, IMP_FACT, COD_DIV, FEC_FACT,
                    (IMP_FACT - IMP_ESTIMADO) AS DIFERENCIA,
                    CASE WHEN IMP_ESTIMADO = 0 THEN NULL
                         ELSE (IMP_FACT - IMP_ESTIMADO) / IMP_ESTIMADO * 100 END AS DIFERENCIA_PCT
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstFacturado
                    AND IMP_ESTIMADO IS NOT NULL
                    AND (@COD_SEQ_PRY = 0 OR COD_SEQ_PRY = @COD_SEQ_PRY)
                    AND (@FEC_DESDE IS NULL OR FEC_FACT >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_FACT < DATEADD(day, 1, @FEC_HASTA))
                ORDER BY ABS(ISNULL(CASE WHEN IMP_ESTIMADO = 0 THEN NULL
                         ELSE (IMP_FACT - IMP_ESTIMADO) / IMP_ESTIMADO * 100 END, 0)) DESC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstFacturado", SqlDbType.Int).Value = EstadosTareaConocidos.Facturado;
                comando.Parameters.Add("@COD_SEQ_PRY", SqlDbType.Int).Value = idProyecto;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.Date).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.Date).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Obtiene, en orden cronológico, todos los eventos históricos
        /// (TBL_CONTROL) de una tarea: alta, asignaciones, bloqueos
        /// (con motivo) y facturación (con los datos de la factura).
        /// A diferencia del resto de consultas de este repositorio, no
        /// pasa por VW_TAREAS_ESTADO_ACTUAL a propósito: aquí se
        /// quiere ver el histórico completo, no solo el evento vigente.
        /// </summary>
        public List<EventoTarea> ObtenerTrazabilidad(int idTarea)
        {
            List<EventoTarea> eventos = new List<EventoTarea>();

            const string sql = @"
                SELECT
                    C.FEC_ALTA,
                    EST.DES_ESTADO,
                    USR.NOMBRE AS NOMBRE_USUARIO,
                    MOT.DES_MOTIVO,
                    FAC.COD_FACT,
                    FAC.IMP_FACT,
                    DIV.COD_DIV
                FROM dbo.TBL_CONTROL AS C
                LEFT JOIN dbo.TBL_ESTADOS_TAREA AS EST ON EST.ID = C.COD_SEQ_EST
                LEFT JOIN dbo.TBL_USUARIOS AS USR ON USR.ID = C.COD_SEQ_USER
                LEFT JOIN dbo.TBL_MOTIVO AS MOT ON MOT.ID = C.COD_SEQ_MOTIVO
                LEFT JOIN dbo.TBL_FACTURADO AS FAC ON FAC.ID = C.COD_SEQ_FAC
                LEFT JOIN dbo.TBL_DIVISAS AS DIV ON DIV.ID = FAC.COD_SEQ_MON
                WHERE C.COD_SEQ_TAR = @COD_SEQ_TAR
                ORDER BY C.FEC_ALTA ASC, C.ID ASC;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@COD_SEQ_TAR", SqlDbType.Int).Value = idTarea;

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        eventos.Add(new EventoTarea
                        {
                            Fecha = Convert.ToDateTime(lector["FEC_ALTA"]),
                            DescripcionEstado = lector["DES_ESTADO"].ToString(),

                            NombreUsuario = lector["NOMBRE_USUARIO"] == DBNull.Value
                                ? null : lector["NOMBRE_USUARIO"].ToString(),

                            DescripcionMotivo = lector["DES_MOTIVO"] == DBNull.Value
                                ? null : lector["DES_MOTIVO"].ToString(),

                            CodigoFactura = lector["COD_FACT"] == DBNull.Value
                                ? null : lector["COD_FACT"].ToString(),

                            ImporteFactura = lector["IMP_FACT"] == DBNull.Value
                                ? (decimal?)null : Convert.ToDecimal(lector["IMP_FACT"]),

                            CodigoDivisa = lector["COD_DIV"] == DBNull.Value
                                ? null : lector["COD_DIV"].ToString()
                        });
                    }
                }
            }

            return eventos;
        }

        /// <summary>
        /// Obtiene, agrupado por mes (columna PERIODO, formato
        /// "yyyy-MM"), el número de tareas canceladas y la suma de su
        /// Importe Estimado. Columnas: PERIODO, NUM_TAREAS, IMPORTE.
        /// </summary>
        public DataTable ObtenerCancelacionesMensuales(DateTime? fechaDesde, DateTime? fechaHasta, int idProyecto)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM') AS PERIODO,
                    COUNT(*) AS NUM_TAREAS,
                    SUM(ISNULL(IMP_ESTIMADO, 0)) AS IMPORTE
                FROM dbo.VW_TAREAS_ESTADO_ACTUAL
                WHERE COD_SEQ_EST = @IdEstCancelado
                    AND (@COD_SEQ_PRY = 0 OR COD_SEQ_PRY = @COD_SEQ_PRY)
                    AND (@FEC_DESDE IS NULL OR FEC_ULTIMO_CAMBIO >= @FEC_DESDE)
                    AND (@FEC_HASTA IS NULL OR FEC_ULTIMO_CAMBIO < DATEADD(day, 1, @FEC_HASTA))
                GROUP BY FORMAT(FEC_ULTIMO_CAMBIO, 'yyyy-MM')
                ORDER BY PERIODO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@IdEstCancelado", SqlDbType.Int).Value = EstadosTareaConocidos.Cancelado;
                comando.Parameters.Add("@COD_SEQ_PRY", SqlDbType.Int).Value = idProyecto;

                comando.Parameters.Add("@FEC_DESDE", SqlDbType.DateTime2).Value =
                    fechaDesde.HasValue ? (object)fechaDesde.Value.Date : DBNull.Value;

                comando.Parameters.Add("@FEC_HASTA", SqlDbType.DateTime2).Value =
                    fechaHasta.HasValue ? (object)fechaHasta.Value.Date.AddDays(1) : DBNull.Value;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }
    }
}
