using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Agrupa los criterios de búsqueda usados para filtrar tareas
    /// (sobre VW_TAREAS_ESTADO_ACTUAL), tanto en el Pool de Tareas
    /// como en la pantalla de Facturación. Un valor de 0 en
    /// IdEstado/IdUsuario, cadena vacía en Documento, o null en
    /// FechaDesde/FechaHasta, significa "sin filtrar por ese campo".
    /// </summary>
    internal class FiltroBusquedaTareas
    {
        public int IdEstado { get; set; }

        public int IdUsuario { get; set; }

        /// <summary>Coincidencia parcial sobre el Documento de la tarea.</summary>
        public string Documento { get; set; } = string.Empty;

        /// <summary>
        /// Fecha de registro (FEC_REG) mínima a incluir, o null para
        /// no filtrar por fecha de inicio.
        /// </summary>
        public DateTime? FechaDesde { get; set; }

        /// <summary>
        /// Fecha de registro (FEC_REG) máxima a incluir (inclusive,
        /// con independencia de la hora), o null para no filtrar por
        /// fecha de fin.
        /// </summary>
        public DateTime? FechaHasta { get; set; }
    }
}
