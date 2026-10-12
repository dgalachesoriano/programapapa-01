using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para las organizaciones de venta (tabla
    /// TBL_ORGANIZACIONES_VENTAS).
    /// </summary>
    internal class OrganizacionVentaRepositorio
    {
        /// <summary>
        /// Obtiene las organizaciones de venta activas (XTI_ACTIVO = 'S'),
        /// ordenadas alfabéticamente por descripción.
        /// </summary>
        public List<OrganizacionVenta> ObtenerActivos()
        {
            List<OrganizacionVenta> organizaciones = new List<OrganizacionVenta>();

            const string sql = @"
                SELECT
                    ID,
                    DES_ORG_VENTAS
                FROM dbo.TBL_ORGANIZACIONES_VENTAS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY DES_ORG_VENTAS;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    organizaciones.Add(new OrganizacionVenta
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_ORG_VENTAS"].ToString(),
                        Activo = true
                    });
                }
            }

            return organizaciones;
        }
    }
}
