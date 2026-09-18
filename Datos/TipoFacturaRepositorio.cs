using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los tipos de factura (tabla
    /// TBL_TIPFACTURAS).
    /// </summary>
    internal class TipoFacturaRepositorio
    {
        /// <summary>
        /// Obtiene los tipos de factura activos (XTI_ACTIVO = 'S'),
        /// ordenados alfabéticamente por descripción.
        /// </summary>
        public List<TipoFactura> ObtenerActivos()
        {
            List<TipoFactura> tipos = new List<TipoFactura>();

            const string sql = @"
                SELECT
                    ID,
                    DES_TIP_FACT
                FROM dbo.TBL_TIPFACTURAS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY DES_TIP_FACT;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    tipos.Add(new TipoFactura
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_TIP_FACT"].ToString(),
                        Activo = true
                    });
                }
            }

            return tipos;
        }
    }
}
