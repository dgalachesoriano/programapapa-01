using System;
using System.IO;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Registro mínimo de errores de la aplicación en un fichero de
    /// texto plano, para poder diagnosticar una incidencia una vez
    /// cerrado el aviso que ve el usuario (hasta ahora, cada
    /// excepción se mostraba con <c>ex.Message</c> y no quedaba
    /// ningún rastro; ver work/propuestas-de-mejora.md, punto 2.1).
    ///
    /// Deliberadamente simple (sin dependencias de NLog/Serilog) para
    /// no añadir un paquete NuGet nuevo a un proyecto donde la
    /// gestión de paquetes ya presentaba alguna incoherencia (ver
    /// punto 2.4 del mismo documento). Si el volumen de logs lo
    /// justifica en el futuro, este es el punto único a sustituir por
    /// una librería de logging completa.
    /// </summary>
    internal static class RegistradorErrores
    {
        private const string CarpetaLogs = "Logs";
        private const string NombreFichero = "GestionFacturas.log";

        private static readonly object bloqueo = new object();
        private static readonly string rutaFichero = ObtenerRutaFichero();

        /// <summary>
        /// Registra una excepción junto con el contexto en el que se
        /// produjo (normalmente "Formulario.Método") al final del
        /// fichero de log. Pensado para llamarse justo antes de
        /// mostrar el aviso al usuario en cada <c>catch</c>.
        ///
        /// Nunca lanza: un fallo al escribir el log (disco lleno,
        /// permisos, ruta no accesible) no debe impedir que la
        /// aplicación siga funcionando ni ocultar el error original
        /// que se le va a mostrar al usuario.
        /// </summary>
        public static void Registrar(string contexto, Exception excepcion)
        {
            try
            {
                string entrada =
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") +
                    " [" + contexto + "]" + Environment.NewLine +
                    excepcion +
                    Environment.NewLine + Environment.NewLine;

                lock (bloqueo)
                {
                    File.AppendAllText(rutaFichero, entrada);
                }
            }
            catch
            {
                // Se ignora deliberadamente: ver comentario del método.
            }
        }

        /// <summary>
        /// Calcula la ruta del fichero de log, en una carpeta "Logs"
        /// junto al ejecutable, creándola si no existe.
        /// </summary>
        private static string ObtenerRutaFichero()
        {
            try
            {
                string carpeta = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, CarpetaLogs);
                Directory.CreateDirectory(carpeta);

                return Path.Combine(carpeta, NombreFichero);
            }
            catch
            {
                // Si ni siquiera se puede crear la carpeta de logs
                // (permisos, ruta de solo lectura...), se usa la
                // carpeta temporal del usuario como último recurso.
                return Path.Combine(Path.GetTempPath(), NombreFichero);
            }
        }
    }
}
