using System;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Excepción controlada para cualquier fallo al preparar o abrir
    /// la conexión a la base de datos: cadena de conexión ausente o
    /// mal configurada en App.config, servidor no disponible,
    /// credenciales incorrectas, etc.
    ///
    /// Los formularios ya capturan <see cref="Exception"/> alrededor
    /// de cada operación contra los repositorios y muestran
    /// <c>ex.Message</c> al usuario; este tipo permite que ese
    /// mensaje sea siempre comprensible, en vez de propagar tal cual
    /// una <see cref="System.Data.SqlClient.SqlException"/> o una
    /// <see cref="NullReferenceException"/> de ADO.NET/Configuration.
    /// </summary>
    internal class ConexionBDException : Exception
    {
        public ConexionBDException(string mensaje)
            : base(mensaje)
        {
        }

        public ConexionBDException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {
        }
    }
}
