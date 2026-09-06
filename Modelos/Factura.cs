using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa la cabecera de una factura/tarea (tabla Facturas).
    /// </summary>
    internal class Factura
    {
        /// <summary>Identificador de la factura (clave primaria).</summary>
        public int Registro { get; set; }

        /// <summary>Número de documento de la factura.</summary>
        public string Documento { get; set; }

        /// <summary>Fecha de entrada en el departamento de calidad.</summary>
        public DateTime FechaEntradaCalidad { get; set; }

        /// <summary>Fecha de registro de la tarea en el sistema.</summary>
        public DateTime FechaRegistro { get; set; }

        /// <summary>Sociedad a la que pertenece la factura.</summary>
        public string Sociedad { get; set; }

        /// <summary>Proyecto asociado a la factura.</summary>
        public string Proyecto { get; set; }

        /// <summary>Importe estimado (puede no haberse informado aún).</summary>
        public decimal? ImporteEstimado { get; set; }

        /// <summary>
        /// Usuario asignado para tratar la factura, o null si no
        /// tiene ninguno asignado.
        /// </summary>
        public int? IdUsuarioAsignado { get; set; }

        /// <summary>Segmento de negocio al que pertenece la factura.</summary>
        public int IdSegmento { get; set; }
    }
}
