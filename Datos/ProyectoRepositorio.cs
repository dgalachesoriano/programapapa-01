using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los proyectos (tabla TBL_PROYECTOS).
    /// </summary>
    internal class ProyectoRepositorio
    {
        /// <summary>
        /// Obtiene los proyectos activos (XTI_ACTIVO = 'S'), ordenados
        /// alfabéticamente por descripción.
        /// </summary>
        public List<Proyecto> ObtenerActivos()
        {
            List<Proyecto> proyectos = new List<Proyecto>();

            const string sql = @"
                SELECT
                    ID,
                    DES_PROYECTO
                FROM dbo.TBL_PROYECTOS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY DES_PROYECTO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    proyectos.Add(new Proyecto
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_PROYECTO"].ToString(),
                        Activo = true
                    });
                }
            }

            return proyectos;
        }
    }
}
