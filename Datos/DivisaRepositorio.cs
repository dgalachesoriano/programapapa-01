using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para las divisas (tabla TBL_DIVISAS).
    /// </summary>
    internal class DivisaRepositorio
    {
        /// <summary>
        /// Obtiene las divisas activas (XTI_ACTIVO = 'S'), ordenadas
        /// alfabéticamente por código.
        /// </summary>
        public List<Divisa> ObtenerActivas()
        {
            List<Divisa> divisas = new List<Divisa>();

            const string sql = @"
                SELECT
                    ID,
                    COD_DIV
                FROM dbo.TBL_DIVISAS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY COD_DIV;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    divisas.Add(new Divisa
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Codigo = lector["COD_DIV"].ToString(),
                        Activo = true
                    });
                }
            }

            return divisas;
        }
    }
}
