using System;

namespace GestionFacturas.Modelos
{
    /// <summary>
    /// Representa los datos de facturación de una tarea (tabla
    /// TBL_FACTURADO). Entidad independiente: se relaciona con la
    /// tarea únicamente a través de TBL_CONTROL.
    /// </summary>
    internal class Facturado
    {
        /// <summary>Código de la entidad de salida.</summary>
        public string EntidadSalida { get; set; }

        /// <summary>Fecha de la factura.</summary>
        public DateTime FechaFactura { get; set; }

        /// <summary>Código/número de la factura. No es único: puede repetirse.</summary>
        public string CodigoFactura { get; set; }

        /// <summary>Importe de la factura.</summary>
        public decimal ImporteFactura { get; set; }

        /// <summary>Divisa en la que está expresado el importe.</summary>
        public int IdDivisa { get; set; }

        /// <summary>Tipo de factura.</summary>
        public int IdTipoFactura { get; set; }
    }
}
