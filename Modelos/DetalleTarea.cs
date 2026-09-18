namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa una línea de detalle asociada a una tarea (tabla
    /// TBL_DETALLETAREA).
    /// </summary>
    internal class DetalleTarea
    {
        /// <summary>Código de pedido de venta de la línea.</summary>
        public string PedidoVenta { get; set; }

        /// <summary>Código de pedido de compra de la línea.</summary>
        public string PedidoCompra { get; set; }

        /// <summary>Código de pedido de inspección de la línea.</summary>
        public string PedidoInspeccion { get; set; }

        /// <summary>Código de la entidad de entrega de la línea.</summary>
        public string EntidadEntrega { get; set; }

        /// <summary>Número de unidades de la línea.</summary>
        public decimal? Unidades { get; set; }

        /// <summary>
        /// Indica si todos los campos de la línea están vacíos, para
        /// poder descartarla al guardar (p. ej. filas en blanco del
        /// DataGridView de detalle).
        /// </summary>
        public bool EstaVacia()
        {
            return string.IsNullOrWhiteSpace(PedidoVenta)
                && string.IsNullOrWhiteSpace(PedidoCompra)
                && string.IsNullOrWhiteSpace(PedidoInspeccion)
                && string.IsNullOrWhiteSpace(EntidadEntrega)
                && Unidades == null;
        }
    }
}
