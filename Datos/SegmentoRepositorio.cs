using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los segmentos de negocio
    /// (tabla Segmento_Negocio).
    /// </summary>
    internal class SegmentoRepositorio
    {
        /// <summary>
        /// Obtiene todos los segmentos de negocio, ordenados por
        /// su identificador.
        /// </summary>
        public List<Segmento> ObtenerTodos()
        {
            List<Segmento> segmentos = new List<Segmento>();

            const string sql = @"
                SELECT
                    IdSegmento,
                    Segmento
                FROM Segmento_Negocio
                ORDER BY IdSegmento;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        segmentos.Add(new Segmento
                        {
                            IdSegmento = Convert.ToInt32(lector["IdSegmento"]),
                            Nombre = lector["Segmento"].ToString()
                        });
                    }
                }
            }

            return segmentos;
        }
    }
}
