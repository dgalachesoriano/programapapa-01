namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un motivo de bloqueo de una tarea (tabla
    /// TBL_MOTIVO).
    /// </summary>
    internal class Motivo
    {
        /// <summary>Identificador del motivo en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Descripción mostrada en pantalla.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si el motivo está activo (XTI_ACTIVO = 'S'). Solo
        /// los motivos activos son seleccionables en el desplegable
        /// de bloqueo.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve la descripción del motivo. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
