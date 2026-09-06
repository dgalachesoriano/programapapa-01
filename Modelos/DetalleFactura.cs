namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa una línea de detalle (recepción/albarán) asociada
    /// a una factura (tabla Detalle_Facturas).
    /// </summary>
    internal class DetalleFactura
    {
        /// <summary>Referencia de control (RC) de la línea.</summary>
        public string RC { get; set; }

        /// <summary>Número de unidades recibidas/tratadas.</summary>
        public decimal? Unidades { get; set; }

        /// <summary>Precio de inspección asociado a la línea.</summary>
        public string PInspeccion { get; set; }

        /// <summary>Precio de compra asociado a la línea.</summary>
        public string PCompra { get; set; }

        /// <summary>Precio de venta asociado a la línea.</summary>
        public string PVenta { get; set; }

        /// <summary>Número de albarán asociado a la línea.</summary>
        public string Albaran { get; set; }

        /// <summary>
        /// Indica si todos los campos de la línea están vacíos, para
        /// poder descartarla al guardar (p. ej. filas en blanco del
        /// DataGridView de detalle).
        /// </summary>
        public bool EstaVacia()
        {
            return string.IsNullOrWhiteSpace(RC)
                && Unidades == null
                && string.IsNullOrWhiteSpace(PInspeccion)
                && string.IsNullOrWhiteSpace(PCompra)
                && string.IsNullOrWhiteSpace(PVenta)
                && string.IsNullOrWhiteSpace(Albaran);
        }
    }
}
