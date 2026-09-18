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
                comando.Parameters.Add("@COD_ENT_SAL", SqlDbType.NVarChar).Value =
                    string.IsNullOrWhiteSpace(facturado.EntidadSalida)
                        ? (object)DBNull.Value
                        : facturado.EntidadSalida;

                comando.Parameters.Add("@FEC_FACT", SqlDbType.Date).Value = facturado.FechaFactura;

                comando.Parameters.Add("@COD_FACT", SqlDbType.NVarChar).Value = facturado.CodigoFactura;

                comando.Parameters.Add("@IMP_FACT", SqlDbType.Decimal).Value = facturado.ImporteFactura;

                comando.Parameters.Add("@COD_SEQ_MON", SqlDbType.Int).Value = facturado.IdDivisa;

                comando.Parameters.Add("@COD_SEQ_TPF", SqlDbType.Int).Value = facturado.IdTipoFactura;

                return Convert.ToInt32(comando.ExecuteScalar());
            }
        }
    }
}
