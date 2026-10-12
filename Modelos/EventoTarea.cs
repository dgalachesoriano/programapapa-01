using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa un evento histórico de una tarea (una fila de
    /// TBL_CONTROL, sin resolver a "el vigente" como hace
    /// VW_TAREAS_ESTADO_ACTUAL): usado por el informe de trazabilidad
    /// para mostrar la línea de tiempo completa de una tarea, no solo
    /// su estado actual.
    /// </summary>
    internal class EventoTarea
    {
        public DateTime Fecha { get; set; }
        public string DescripcionEstado { get; set; }
        public string NombreUsuario { get; set; }
        public string DescripcionMotivo { get; set; }
        public string CodigoFactura { get; set; }
        public decimal? ImporteFactura { get; set; }
        public string CodigoDivisa { get; set; }
    }
}
