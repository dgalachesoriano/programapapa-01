using System;
using System.Windows.Forms;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Punto único para los cuadros de diálogo de usuario (errores,
    /// avisos, confirmaciones e información) que antes se repetían,
    /// casi idénticos, en cada formulario. <see cref="MostrarError"/>
    /// centraliza además el registro en el log de la excepción
    /// mostrada, para que ningún formulario pueda olvidarlo.
    /// </summary>
    internal static class Dialogos
    {
        /// <summary>
        /// Registra la excepción (contexto "Formulario.Método") y
        /// muestra al usuario <paramref name="mensaje"/> seguido del
        /// mensaje de la excepción.
        /// </summary>
        public static void MostrarError(string contexto, string mensaje, Exception excepcion)
        {
            RegistradorErrores.Registrar(contexto, excepcion);

            MessageBox.Show(
                mensaje + "\n\n" + excepcion.Message,
                "Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }

        /// <summary>Muestra un mensaje informativo simple.</summary>
        public static void MostrarInformacion(string mensaje, string titulo)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>Muestra un aviso de validación.</summary>
        public static void MostrarAviso(string mensaje, string titulo)
        {
            MessageBox.Show(mensaje, titulo, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Pide confirmación Sí/No y devuelve true solo si el usuario
        /// elige "Sí".
        /// </summary>
        public static bool Confirmar(string mensaje, string titulo)
        {
            return MessageBox.Show(
                mensaje,
                titulo,
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }
    }
}
