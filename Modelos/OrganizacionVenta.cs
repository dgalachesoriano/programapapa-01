namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa una organización de ventas a la que puede
    /// pertenecer una tarea (tabla TBL_ORGANIZACIONES_VENTAS).
    /// </summary>
    internal class OrganizacionVenta
    {
        /// <summary>Identificador de la organización de ventas en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Descripción mostrada en pantalla.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Indica si la organización de ventas está activa
        /// (XTI_ACTIVO = 'S'). Solo las organizaciones activas son
        /// seleccionables en los combos.
        /// </summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve la descripción de la organización de ventas. Los
        /// controles ComboBox enlazados a esta clase usan este valor
        /// como texto mostrado (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Descripcion;
        }
    }
}
