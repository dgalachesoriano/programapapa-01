using System.Drawing;
using System.Windows.Forms;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Marca en rojo (simulando un borde) los campos obligatorios que
    /// falten por rellenar en un formulario. Cada campo validable
    /// debe estar envuelto en un Panel con relleno (Padding) cuyo
    /// color de fondo es el que se cambia; WinForms no permite pintar
    /// el borde nativo de un TextBox/ComboBox directamente, así que
    /// este panel-contenedor hace de borde. Usado por
    /// FrmRegistrarTarea y FrmFacturar.
    /// </summary>
    internal static class ValidadorCampos
    {
        /// <summary>Color "normal" (sin marcar) de los paneles-borde.</summary>
        public static readonly Color ColorNormal = SystemColors.Control;

        /// <summary>Color de aviso de los paneles-borde.</summary>
        public static readonly Color ColorInvalido = Color.Red;

        /// <summary>
        /// Valida que un campo de texto no esté vacío, marcando en
        /// rojo (o quitando la marca) el panel que lo envuelve.
        /// </summary>
        public static bool ValidarTexto(TextBox campo, Panel panelBorde)
        {
            bool relleno = !string.IsNullOrWhiteSpace(campo.Text);

            panelBorde.BackColor = relleno ? ColorNormal : ColorInvalido;

            return relleno;
        }

        /// <summary>
        /// Valida que un combo tenga un elemento seleccionado,
        /// marcando en rojo (o quitando la marca) el panel que lo
        /// envuelve.
        /// </summary>
        public static bool ValidarCombo(ComboBox combo, Panel panelBorde)
        {
            bool seleccionado = combo.SelectedIndex != -1 && combo.SelectedValue != null;

            panelBorde.BackColor = seleccionado ? ColorNormal : ColorInvalido;

            return seleccionado;
        }

        /// <summary>
        /// Quita la marca de un panel-borde. Pensado para engancharlo
        /// al evento que indica que el usuario ha empezado a corregir
        /// ese campo (TextChanged, SelectedIndexChanged...), sin
        /// esperar a que el valor vuelva a ser válido.
        /// </summary>
        public static void Limpiar(Panel panelBorde)
        {
            panelBorde.BackColor = ColorNormal;
        }
    }
}
