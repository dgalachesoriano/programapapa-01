using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Agrupa los criterios de búsqueda usados para filtrar facturas,
    /// tanto en la pantalla de tratamiento de tareas (FrmTratarTarea)
    /// como en el pool de tareas (FrmPoolTareas). Un valor de 0 en
    /// IdEstado/IdUsuario, cadena vacía en Documento/Proyecto, o null
    /// en FechaDesde/FechaHasta, significa "sin filtrar por ese
    /// campo".
    /// </summary>
    internal class FiltroBusquedaFacturas
    {
        public int IdEstado { get; set; }

        public int IdUsuario { get; set; }

        public string Documento { get; set; } = string.Empty;

        public string Proyecto { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro (F_Registro) mínima a incluir, o null
        /// para no filtrar por fecha de inicio.
        /// </summary>
        public DateTime? FechaDesde { get; set; }

        /// <summary>
        /// Fecha de registro (F_Registro) máxima a incluir (inclusive,
        /// con independencia de la hora), o null para no filtrar por
        /// fecha de fin.
        /// </summary>
        public DateTime? FechaHasta { get; set; }
    }
}
