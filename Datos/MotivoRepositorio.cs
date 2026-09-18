using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los motivos de bloqueo de una tarea
    /// (tabla TBL_MOTIVO).
    /// </summary>
    internal class MotivoRepositorio
    {
        /// <summary>
        /// Obtiene los motivos activos (XTI_ACTIVO = 'S'), ordenados
        /// alfabéticamente por descripción.
        /// </summary>
        public List<Motivo> ObtenerActivos()
        {
            List<Motivo> motivos = new List<Motivo>();

            const string sql = @"
                SELECT
                    ID,
                    DES_MOTIVO
                FROM dbo.TBL_MOTIVO
                WHERE XTI_ACTIVO = 'S'
                ORDER BY DES_MOTIVO;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    motivos.Add(new Motivo
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_MOTIVO"].ToString(),
                        Activo = true
                    });
                }
            }

            return motivos;
        }
    }
}
