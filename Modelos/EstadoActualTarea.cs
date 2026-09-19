using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Instantánea del estado actual de una tarea: su evento más
    /// reciente de TBL_CONTROL (estado, usuario, motivo de bloqueo si
    /// lo hay) más los datos de facturación asociados si los hay.
    /// Usada por la pantalla de detalle de tarea
    /// (Formularios/FrmDetalleTarea) para mostrar y editar todo eso
    /// sin duplicar en C# la lógica de "evento vigente" que ya
    /// resuelve VW_TAREAS_ESTADO_ACTUAL.
    /// </summary>
    internal class EstadoActualTarea
    {
        public int IdEstado { get; set; }
        public string DescripcionEstado { get; set; }

        public int? IdUsuario { get; set; }
        public string NombreUsuario { get; set; }

        public int? IdMotivo { get; set; }
        public string DescripcionMotivo { get; set; }

        /// <summary>
        /// Identificador de TBL_FACTURADO vigente, o null si la tarea
        /// todavía no se ha facturado.
        /// </summary>
        public int? IdFacturado { get; set; }

        public string EntidadSalida { get; set; }
        public DateTime? FechaFactura { get; set; }
        public string CodigoFactura { get; set; }
        public decimal? ImporteFactura { get; set; }
        public int? IdDivisa { get; set; }
        public string CodigoDivisa { get; set; }
        public int? IdTipoFactura { get; set; }
        public string DescripcionTipoFactura { get; set; }
    }
}
