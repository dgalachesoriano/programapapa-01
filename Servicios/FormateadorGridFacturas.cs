using System.Windows.Forms;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Da formato de presentación (cabeceras legibles y formatos de
    /// fecha/importe) a un DataGridView enlazado al resultado de
    /// FacturaRepositorio.BuscarPorFiltro. Compartido por
    /// FrmTratarTarea y FrmPoolTareas, que muestran la misma rejilla
    /// de resultados de facturas.
    /// </summary>
    internal static class FormateadorGridFacturas
    {
        /// <summary>
        /// Aplica el formato de columnas al grid. No hace nada si
        /// aún no tiene columnas (p. ej. una búsqueda sin resultados).
        /// </summary>
        public static void AplicarFormato(DataGridView grid)
        {
            if (grid.Columns.Count == 0)
                return;

            grid.Columns["Registro"].HeaderText = "Registro";
            grid.Columns["Documento"].HeaderText = "Documento";
            grid.Columns["F_Ent_Calidad"].HeaderText = "F. Ent. Calidad";
            grid.Columns["F_Registro"].HeaderText = "F. Registro";
            grid.Columns["Sociedad"].HeaderText = "Sociedad";
            grid.Columns["Proyecto"].HeaderText = "Proyecto";
            grid.Columns["Segmento"].HeaderText = "Segmento";
            grid.Columns["Estado"].HeaderText = "Estado";
            grid.Columns["Usuario"].HeaderText = "Usuario";
            grid.Columns["Importe_Estimado"].HeaderText = "Importe Estimado";
            grid.Columns["Num_Factura"].HeaderText = "Factura";
            grid.Columns["F_Factura"].HeaderText = "F. Factura";
            grid.Columns["Importe_Total"].HeaderText = "Importe Total";

            grid.Columns["F_Ent_Calidad"].DefaultCellStyle.Format = "dd/MM/yyyy";
            grid.Columns["F_Registro"].DefaultCellStyle.Format = "dd/MM/yyyy";
            grid.Columns["F_Factura"].DefaultCellStyle.Format = "dd/MM/yyyy";

            grid.Columns["Importe_Estimado"].DefaultCellStyle.Format = "N2";
            grid.Columns["Importe_Total"].DefaultCellStyle.Format = "N2";
        }
    }
}
