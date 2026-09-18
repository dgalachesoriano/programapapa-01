using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Diálogo modal mostrado cuando una pantalla no puede grabar
    /// porque falta información obligatoria por rellenar (los campos
    /// concretos quedan ya marcados en la propia pantalla; este
    /// diálogo solo explica la situación y ofrece elegir entre volver
    /// a completarlos o descartar los datos introducidos y cerrar).
    /// </summary>
    public partial class FrmFaltaInformacion : Form
    {
        /// <summary>
        /// True si el usuario ha elegido "Descartar" (cerrar sin
        /// guardar); false si ha elegido "Completar" (volver a la
        /// pantalla para rellenar lo que falta).
        /// </summary>
        public bool SeHaDescartado { get; private set; }

        public FrmFaltaInformacion(string mensaje)
        {
            InitializeComponent();

            lblMensaje.Text = mensaje;
        }

        private void btnDescartar_Click(object sender, System.EventArgs e)
        {
            SeHaDescartado = true;
            Close();
        }

        private void btnCompletar_Click(object sender, System.EventArgs e)
        {
            SeHaDescartado = false;
            Close();
        }
    }
}
