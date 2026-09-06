using System;
using System.Configuration;
using System.Data.SqlClient;

namespace GestionFacturas.Datos
{
    /// <summary>
    /// Fábrica de conexiones a la base de datos de la aplicación.
    /// La cadena de conexión se lee de App.config (sección
    /// &lt;connectionStrings&gt;) en lugar de estar hardcodeada,
    /// para poder cambiar de servidor/entorno sin recompilar.
    /// </summary>
    internal static class ConexionBD
    {
        /// <summary>
        /// Nombre de la cadena de conexión definida en App.config.
        /// </summary>
        private const string NombreCadenaConexion = "GestionFacturasConnection";

        /// <summary>
        /// Crea una nueva conexión SQL sin abrir. El llamador es
        /// responsable de abrirla y liberarla (habitualmente con
        /// "using"). Lanza <see cref="ConexionBDException"/> si la
        /// cadena de conexión no está configurada correctamente en
        /// App.config; nunca deja escapar una excepción "en crudo"
        /// de <see cref="ConfigurationManager"/>.
        /// </summary>
        public static SqlConnection CrearConexion()
        {
            string cadenaConexion = ObtenerCadenaConexion();

            return new SqlConnection(cadenaConexion);
        }

        /// <summary>
        /// Crea la conexión y la abre en un solo paso. Cualquier
        /// fallo al abrirla (servidor no disponible, instancia
        /// incorrecta, credenciales inválidas, etc.) se convierte en
        /// <see cref="ConexionBDException"/> con un mensaje
        /// comprensible para el usuario, en vez de propagar la
        /// <see cref="SqlException"/> original tal cual. El llamador
        /// sigue siendo responsable de liberar la conexión
        /// (habitualmente con "using").
        /// </summary>
        public static SqlConnection AbrirConexion()
        {
            SqlConnection conexion = CrearConexion();

            try
            {
                conexion.Open();

                return conexion;
            }
            catch (Exception ex)
            {
                conexion.Dispose();

                throw new ConexionBDException(
                    "No se ha podido conectar con la base de datos. " +
                    "Compruebe que el servidor SQL indicado en la " +
                    "configuración de la aplicación está disponible y " +
                    "que los datos de conexión son correctos.",
                    ex);
            }
        }

        /// <summary>
        /// Obtiene y valida la cadena de conexión configurada en
        /// App.config. Lanza <see cref="ConexionBDException"/> si la
        /// sección &lt;connectionStrings&gt; no existe, está mal
        /// formada, o no contiene la entrada esperada.
        /// </summary>
        private static string ObtenerCadenaConexion()
        {
            ConnectionStringSettings configuracion;

            try
            {
                configuracion = ConfigurationManager.ConnectionStrings[NombreCadenaConexion];
            }
            catch (ConfigurationErrorsException ex)
            {
                throw new ConexionBDException(
                    "El archivo de configuración de la aplicación " +
                    "(App.config) no es válido o no se puede leer.",
                    ex);
            }

            if (configuracion == null || string.IsNullOrWhiteSpace(configuracion.ConnectionString))
            {
                throw new ConexionBDException(
                    "No se ha encontrado la cadena de conexión '" +
                    NombreCadenaConexion + "' en la configuración de la " +
                    "aplicación (App.config). Revise la sección " +
                    "<connectionStrings>.");
            }

            return configuracion.ConnectionString;
        }
    }
}
