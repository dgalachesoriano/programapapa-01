using System;
using System.Data;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los datos de facturación (tabla
    /// TBL_FACTURADO). Se inserta siempre dentro de la misma
    /// transacción que el evento de TBL_CONTROL que la referencia
    /// (ver TareaRepositorio.Facturar), de ahí que reciba la
    /// conexión/transacción ya abiertas en vez de gestionarlas él
    /// mismo.
    /// </summary>
    internal class FacturadoRepositorio
    {
        /// <summary>
        /// Inserta un nuevo registro de facturación y devuelve el
        /// identificador generado.
        /// </summary>
        public int Insertar(Facturado facturado, SqlConnection conexion, SqlTransaction transaccion)
        {
            const string sql = @"
                INSERT INTO dbo.TBL_FACTURADO
                (
                    COD_ENT_SAL,
                    FEC_FACT,
                    COD_FACT,
                    IMP_FACT,
                    COD_SEQ_MON,
                    COD_SEQ_TPF
                )
                VALUES
                (
                    @COD_ENT_SAL,
                    @FEC_FACT,
                    @COD_FACT,
                    @IMP_FACT,
                    @COD_SEQ_MON,
                    @COD_SEQ_TPF
                );

                SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametros(comando, facturado);

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }

        /// <summary>
        /// Actualiza en el sitio un registro de facturación ya
        /// existente, en vez de crear uno nuevo. Usado al editar desde
        /// FrmDetalleTarea una tarea que ya estaba Facturada: se
        /// corrige la factura ya emitida sin duplicarla ni generar un
        /// nuevo evento de TBL_CONTROL (ver
        /// TareaRepositorio.AplicarReglaEstadoAutomatica).
        /// </summary>
        public void Actualizar(int idFacturado, Facturado facturado, SqlConnection conexion, SqlTransaction transaccion)
        {
            const string sql = @"
                UPDATE dbo.TBL_FACTURADO
                SET
                    COD_ENT_SAL = @COD_ENT_SAL,
                    FEC_FACT = @FEC_FACT,
                    COD_FACT = @COD_FACT,
                    IMP_FACT = @IMP_FACT,
                    COD_SEQ_MON = @COD_SEQ_MON,
                    COD_SEQ_TPF = @COD_SEQ_TPF
                WHERE ID = @ID;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                AgregarParametros(comando, facturado);

                comando.Parameters.Add("@ID", SqlDbType.Int).Value = idFacturado;

                comando.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Agrega al comando los parámetros comunes a INSERT y UPDATE
        /// de TBL_FACTURADO.
        /// </summary>
        private void AgregarParametros(SqlCommand comando, Facturado facturado)
        {
            comando.Parameters.Add("@COD_ENT_SAL", SqlDbType.NVarChar).Value =
                string.IsNullOrWhiteSpace(facturado.EntidadSalida)
                    ? (object)DBNull.Value
                    : facturado.EntidadSalida;

            comando.Parameters.Add("@FEC_FACT", SqlDbType.Date).Value = facturado.FechaFactura;

            comando.Parameters.Add("@COD_FACT", SqlDbType.NVarChar).Value = facturado.CodigoFactura;

            comando.Parameters.Add("@IMP_FACT", SqlDbType.Decimal).Value = facturado.ImporteFactura;

            comando.Parameters.Add("@COD_SEQ_MON", SqlDbType.Int).Value = facturado.IdDivisa;

            comando.Parameters.Add("@COD_SEQ_TPF", SqlDbType.Int).Value = facturado.IdTipoFactura;
        }
    }
}
