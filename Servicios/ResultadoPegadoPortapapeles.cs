namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Resultado de intentar pegar el contenido del portapapeles
    /// sobre un DataGridView mediante
    /// <see cref="PegadoPortapapelesServicio"/>.
    /// </summary>
    internal enum ResultadoPegadoPortapapeles
    {
        /// <summary>El pegado se realizó correctamente.</summary>
        Correcto,

        /// <summary>El texto a pegar estaba vacío.</summary>
        SinTexto,

        /// <summary>No había ninguna celda seleccionada en el grid destino.</summary>
        SinCeldaSeleccionada
    }
}
