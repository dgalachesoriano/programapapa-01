namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa una consulta SQL predefinida (tabla TBL_CONSULTAS)
    /// lanzable desde la pantalla Datos > Consultas SQL.
    /// </summary>
    internal class Consulta
    {
        /// <summary>Identificador de la consulta en base de datos.</summary>
        public int Id { get; set; }

        /// <summary>Nombre mostrado en el desplegable.</summary>
        public string Nombre { get; set; }

        /// <summary>Descripción opcional de lo que devuelve la consulta.</summary>
        public string Descripcion { get; set; }

        /// <summary>
        /// Texto SQL de la consulta, con los parámetros en sintaxis
        /// :Nombre (ver Servicios/ParametrosConsultaServicio.cs).
        /// </summary>
        public string TextoSql { get; set; }

        /// <summary>Indica si la consulta está activa (XTI_ACTIVO = 'S').</summary>
        public bool Activo { get; set; }

        /// <summary>
        /// Devuelve el nombre de la consulta. Los controles ComboBox
        /// enlazados a esta clase usan este valor como texto mostrado
        /// (DisplayMember).
        /// </summary>
        public override string ToString()
        {
            return Nombre;
        }
    }
}
