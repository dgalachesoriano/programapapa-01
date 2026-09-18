namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un usuario al que se le pueden asignar tareas
    /// (tabla TBL_USUARIOS).
    /// </summary>
    internal class Usuario
    {
        /// <summary>Identificador del usuario en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Nombre mostrado en pantalla.</summary>
        public string Nombre { get; set; }

        /// <summary>
        /// Indica si el usuario está activo (XTI_ACTIVO = 'S'). Solo
        /// los usuarios activos son seleccionables en los combos.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve el nombre del usuario. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Nombre;
        }
    }
}
