using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa la cabecera de una tarea (tabla TBL_TAREAS).
    /// </summary>
    internal class Tarea
    {
        /// <summary>Identificador de la tarea (clave primaria).</summary>
        public int Id { get; set; }

        /// <summary>Documento de la tarea. Obligatorio y único.</summary>
        public string Documento { get; set; }

        /// <summary>Fecha de entrada en calidad.</summary>
        public DateTime FechaEntradaCalidad { get; set; }

        /// <summary>Fecha de registro de la tarea en el sistema.</summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>Organización de ventas asociada a la tarea.</summary>
        public int IdOrganizacionVentas { get; set; }

        /// <summary>Proyecto al que pertenece la tarea.</summary>
        public int IdProyecto { get; set; }

        /// <summary>Segmento de negocio al que pertenece la tarea.</summary>
        public int IdSegmento { get; set; }

        /// <summary>Importe estimado (puede no haberse informado aún).</summary>
        public decimal? ImporteEstimado { get; set; }

        /// <summary>
        /// Tipo de documento de la tarea. Se da de alta en el proceso
        /// de registro (antes vivía en TBL_FACTURADO.COD_SEQ_TPF, pero
        /// en realidad corresponde a la tarea, no a su facturación).
        /// </summary>
        public int IdTipoDocumento { get; set; }

        /// <summary>Divisa estimada en la que se espera facturar la tarea.</summary>
        public int IdDivisaEstimada { get; set; }

        /// <summary>Notas libres sobre la tarea (máximo 255 caracteres). Opcional.</summary>
        public string Notas { get; set; }
    }
}
