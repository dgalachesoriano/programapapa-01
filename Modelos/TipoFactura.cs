namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un tipo de factura (tabla TBL_TIPFACTURAS).
    /// </summary>
    internal class TipoFactura
    {
        /// <summary>Identificador del tipo de factura en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Descripción mostrada en pantalla.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si el tipo de factura está activo (XTI_ACTIVO = 'S').
        /// Solo los tipos activos son seleccionables en los combos.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve la descripción del tipo de factura. Los controles
        /// ComboBox enlazados a esta clase usan este valor como texto
        /// mostrado (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
