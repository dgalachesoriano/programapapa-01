namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un estado del ciclo de vida de una factura
    /// (p. ej. "Registrado", "Pendiente", "Facturado"), tabla
    /// Estados_Facturas.
    /// </summary>
    internal class EstadoFactura
    {
        /// <summary>Identificador del estado en base de datos.</summary>
        public int IdEstado { get; set; }

        /// <summary>Nombre del estado mostrado en pantalla.</summary>
        public string Estado { get; set; }

        /// <summary>
        /// Devuelve el nombre del estado. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Estado;
        }
    }
}
