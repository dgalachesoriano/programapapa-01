using System;
using System.Data;
using System.Data.SqlClient;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los informes de la aplicación. De momento
    /// concentra el resumen mensual de importes usado en
    /// Informes &gt; Facturación.
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
    }
}
