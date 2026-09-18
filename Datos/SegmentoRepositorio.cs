using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los segmentos de negocio (tabla
    /// TBL_SEGMENTOS).
    /// </summary>
    internal class SegmentoRepositorio
    {
        /// <summary>
        /// Obtiene los segmentos activos (XTI_ACTIVO = 'S'), ordenados
        /// alfabéticamente por descripción.
        /// </summary>
        public List<Segmento> ObtenerActivos()
        {
            List<Segmento> segmentos = new List<Segmento>();

            const string sql = @"
                SELECT
                    ID,
                    DES_SEGMENTO
                FROM dbo.TBL_SEGMENTOS
                WHERE XTI_ACTIVO = 'S'
                ORDER BY DES_SEGMENTO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    segmentos.Add(new Segmento
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_SEGMENTO"].ToString(),
                        Activo = true
                    });
                }
            }

            return segmentos;
        }
    }
}
