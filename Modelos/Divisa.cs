namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa una divisa en la que puede expresarse el importe
    /// de una factura (tabla TBL_DIVISAS).
    /// </summary>
    internal class Divisa
    {
        /// <summary>Identificador de la divisa en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Código de la divisa (p. ej. "EUR") mostrado en pantalla.</summary>
        public string Codigo { get; set; }

        /// <summary>
        /// Indica si la divisa está activa (XTI_ACTIVO = 'S'). Solo
        /// las divisas activas son seleccionables en los combos.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve el código de la divisa. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Codigo;
        }
    }
}
