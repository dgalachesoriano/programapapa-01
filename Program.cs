using System;
using System.Windows.Forms;

namespace GestionFacturas
{
    /// <summary>
    /// Clase de arranque de la aplicación WinForms.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación. Configura
        /// los estilos visuales y lanza el formulario principal.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmPrincipal());
        }
    }
}
