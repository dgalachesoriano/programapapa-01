namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un segmento de negocio al que puede pertenecer
    /// una factura (tabla Segmento_Negocio).
    /// </summary>
    internal class Segmento
    {
        /// <summary>Identificador del segmento en base de datos.</summary>
        public int IdSegmento { get; set; }

        /// <summary>Nombre del segmento mostrado en pantalla.</summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Devuelve el nombre del segmento. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Nombre;
        }
    }
}
