namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un estado del flujo de una tarea (Registrado / En
    /// Proceso / Bloqueado / Facturado), tabla TBL_ESTADOS_TAREA.
    /// </summary>
    internal class EstadoTarea
    {
        /// <summary>Identificador del estado en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Descripción del estado mostrada en pantalla.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si el estado está activo (XTI_ACTIVO = 'S'). Solo
        /// los estados activos son seleccionables en los combos de
        /// filtro.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve la descripción del estado. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
