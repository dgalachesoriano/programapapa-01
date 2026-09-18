namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un proyecto al que puede pertenecer una tarea
    /// (tabla TBL_PROYECTOS).
    /// </summary>
    internal class Proyecto
    {
        /// <summary>Identificador del proyecto en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Descripción mostrada en pantalla.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si el proyecto está activo (XTI_ACTIVO = 'S'). Solo
        /// los proyectos activos son seleccionables en los combos.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve la descripción del proyecto. Los controles
        /// ComboBox enlazados a esta clase usan este valor como texto
        /// mostrado (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
