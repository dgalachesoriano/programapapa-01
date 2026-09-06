using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los estados posibles de una factura
    /// (tabla Estados_Facturas).
    /// </summary>
    internal class EstadoFacturaRepositorio
    {
        /// <summary>
        /// Obtiene todos los estados de factura dados de alta,
        /// ordenados por su identificador.
        /// </summary>
        public List<EstadoFactura> ObtenerTodos()
        {
            List<EstadoFactura> estados = new List<EstadoFactura>();

            const string sql = @"
                SELECT
                    IdEstado,
                    Estado
                FROM Estados_Facturas
                ORDER BY IdEstado;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            {
                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        estados.Add(new EstadoFactura
                        {
                            IdEstado = Convert.ToInt32(lector["IdEstado"]),
                            Estado = lector["Estado"].ToString()
                        });
                    }
                }
            }

            return estados;
        }

        /// <summary>
        /// Obtiene el identificador del estado a partir de su nombre
        /// (p. ej. "Registrado"), dentro de una conexión/transacción
        /// ya abiertas. Lanza <see cref="InvalidOperationException"/>
        /// si el estado no existe en la tabla.
        /// </summary>
        public int ObtenerIdPorNombre(
            string nombreEstado,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                SELECT IdEstado
                FROM Estados_Facturas
                WHERE Estado = @Estado;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@Estado", nombreEstado);

                object resultado = comando.ExecuteScalar();

                if (resultado == null)
                {
                    throw new InvalidOperationException(
                        "No existe el estado '" + nombreEstado +
                        "' en la tabla Estados_Facturas.");
                }

                return Convert.ToInt32(resultado);
            }
        }
    }
}
