using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GestionFacturas.Modelos;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Acceso a datos para los estados del flujo de una tarea (tabla
    /// TBL_ESTADOS_TAREA).
    /// </summary>
    internal class EstadoTareaRepositorio
    {
        /// <summary>
        /// Obtiene los estados activos (XTI_ACTIVO = 'S'), ordenados
        /// por su identificador (que refleja el orden natural del
        /// flujo: Registrado, En Proceso, Bloqueado, Facturado).
        /// </summary>
        public List<EstadoTarea> ObtenerActivos()
        {
            List<EstadoTarea> estados = new List<EstadoTarea>();

            const string sql = @"
                SELECT
                    ID,
                    DES_ESTADO
                FROM dbo.TBL_ESTADOS_TAREA
                WHERE XTI_ACTIVO = 'S'
                ORDER BY ID;";

            using (SqlConnection conexion = ConexionBD.AbrirConexion())
            using (SqlCommand comando = new SqlCommand(sql, conexion))
            using (SqlDataReader lector = comando.ExecuteReader())
            {
                while (lector.Read())
                {
                    estados.Add(new EstadoTarea
                    {
                        Id = Convert.ToInt32(lector["ID"]),
                        Descripcion = lector["DES_ESTADO"].ToString(),
                        Activo = true
                    });
                }
            }

            return estados;
        }

        /// <summary>
        /// Obtiene el identificador del estado a partir de su
        /// descripción exacta (p. ej. "REGISTRADO"), dentro de una
        /// conexión/transacción ya abiertas. Lanza
        /// <see cref="InvalidOperationException"/> si el estado no
        /// existe en la tabla.
        /// </summary>
        public int ObtenerIdPorDescripcion(
            string descripcion,
            SqlConnection conexion,
            SqlTransaction transaccion)
        {
            const string sql = @"
                SELECT ID
                FROM dbo.TBL_ESTADOS_TAREA
                WHERE DES_ESTADO = @DES_ESTADO;";

            using (SqlCommand comando = new SqlCommand(sql, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@DES_ESTADO", descripcion);

                object resultado = comando.ExecuteScalar();

                if (resultado == null)
                {
                    throw new InvalidOperationException(
                        "No existe el estado '" + descripcion +
                        "' en la tabla TBL_ESTADOS_TAREA.");
                }

                return Convert.ToInt32(resultado);
            }
        }
    }
}
