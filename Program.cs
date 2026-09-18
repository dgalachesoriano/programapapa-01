using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GestionFacturas
{
    /// <summary>
    /// Clase de arranque de la aplicación WinForms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación. Fija la
        /// cultura española como cultura por defecto (para que fechas
        /// e importes -incluidos los formatos "N2" de las rejillas,
        /// que no especifican cultura explícita- se muestren en
        /// formato español: punto de millares, coma decimal),
        /// configura los estilos visuales y lanza el formulario
        /// principal.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo culturaEspanola = new CultureInfo("es-ES");

            Thread.CurrentThread.CurrentCulture = culturaEspanola;
            Thread.CurrentThread.CurrentUICulture = culturaEspanola;
            CultureInfo.DefaultThreadCurrentCulture = culturaEspanola;
            CultureInfo.DefaultThreadCurrentUICulture = culturaEspanola;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}
