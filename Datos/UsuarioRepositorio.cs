using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los usuarios que pueden tener facturas
    /// asignadas (tabla Usuarios_Facturas).
    /// </summary>
    internal class UsuarioRepositorio
    {
        /// <summary>
        /// Obtiene los usuarios activos (Activo = 1), ordenados
        /// alfabéticamente por nombre.
        /// </summary>
        public List<Usuario> ObtenerActivos()
        {
            List<Usuario> usuarios = new List<Usuario>();

            const string sql = @"
                SELECT
                    IdUsuario,
                    Usuario,
                    Nombre
                FROM Usuarios_Facturas
                WHERE Activo = 1
                ORDER BY Nombre;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            IdUsuario = Convert.ToInt32(lector["IdUsuario"]),
                            UsuarioLogin = lector["Usuario"].ToString(),
                            Nombre = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return usuarios;
        }
    }
}
