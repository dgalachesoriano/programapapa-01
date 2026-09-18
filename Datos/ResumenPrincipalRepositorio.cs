using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para el resumen del formulario principal
    /// (paneles de facturado/pendiente del mes y gráfico anual).
    ///
    /// Las tareas no tienen ninguna divisa propia (solo la tienen las
    /// facturas, vía TBL_FACTURADO.COD_SEQ_MON): por eso lo facturado
    /// se desglosa siempre por divisa, y lo pendiente de facturar
    /// (Importe Estimado de las tareas actualmente "En Proceso") se
    /// da como un único total, sin desglose.
    /// </summary>
    internal class ResumenPrincipalRepositorio
    {
        /// <summary>
        /// Obtiene, por usuario y divisa, el importe facturado cuya
        /// fecha de factura cae en el rango [<paramref name="desde"/>,
        /// <paramref name="hasta"/>) (hasta exclusive). Columnas:
        /// NOMBRE_USUARIO, COD_DIV, IMPORTE. El usuario es el que
        /// tenía asignada la tarea en el momento de facturarla.
        /// </summary>
        public DataTable ObtenerFacturadoPorUsuarioYDivisa(DateTime desde, DateTime hasta)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    ISNULL(U.NOMBRE, N'(Sin usuario)') AS NOMBRE_USUARIO,
                    DIV.COD_DIV,
                    SUM(FAC.IMP_FACT) AS IMPORTE
                FROM dbo.TBL_CONTROL AS C
                INNER JOIN dbo.TBL_FACTURADO AS FAC ON FAC.ID = C.COD_SEQ_FAC
                INNER JOIN dbo.TBL_DIVISAS AS DIV ON DIV.ID = FAC.COD_SEQ_MON
                LEFT JOIN dbo.TBL_USUARIOS AS U ON U.ID = C.COD_SEQ_USER
                WHERE C.COD_SEQ_EST = @IdEstFacturado
                    AND FAC.FEC_FACT >= @DESDE
                    AND FAC.FEC_FACT < @HASTA
                GROUP BY U.NOMBRE, DIV.COD_DIV
                ORDER BY NOMBRE_USUARIO, DIV.COD_DIV;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
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
        /// Obtiene, por usuario, la suma del Importe Estimado de las
        /// tareas actualmente en estado "En Proceso" (pendientes de
        /// facturar) que entraron en ese estado dentro del rango
        /// [<paramref name="desde"/>, <paramref name="hasta"/>).
        /// Columnas: NOMBRE_USUARIO, IMPORTE.
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
        /// indicado. Columnas: MES (1-12), COD_DIV, IMPORTE.
        /// </summary>
        public DataTable ObtenerFacturadoMensual(int anio)
        {
            DataTable tabla = new DataTable();

            const string sql = @"
                SELECT
                    MONTH(FAC.FEC_FACT) AS MES,
                    DIV.COD_DIV,
                    SUM(FAC.IMP_FACT) AS IMPORTE
                FROM dbo.TBL_FACTURADO AS FAC
                INNER JOIN dbo.TBL_DIVISAS AS DIV ON DIV.ID = FAC.COD_SEQ_MON
                WHERE YEAR(FAC.FEC_FACT) = @ANIO
                GROUP BY MONTH(FAC.FEC_FACT), DIV.COD_DIV
                ORDER BY MES;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                comando.Parameters.Add("@ANIO", SqlDbType.Int).Value = anio;

                using (SqlDataAdapter adaptador = new SqlDataAdapter(comando))
                {
                    adaptador.Fill(tabla);
                }
            }

            return tabla;
        }
    }
}
