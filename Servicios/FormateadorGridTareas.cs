using System.Windows.Forms;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Da formato de presentación (cabeceras legibles, formatos de
    /// fecha/importe, y ocultación de la columna de identificador) a
    /// un DataGridView enlazado al resultado de
    /// TareaRepositorio.BuscarPorFiltro (basado en
    /// VW_TAREAS_ESTADO_ACTUAL). Compartido por el Pool de Tareas y
    /// la pantalla de Facturación, que muestran la misma rejilla de
    /// tareas.
    /// </summary>
    internal static class FormateadorGridTareas
    {
        /// <summary>
        /// Aplica el formato de columnas al grid. No hace nada si
        /// aún no tiene columnas (p. ej. una búsqueda sin resultados).
        /// </summary>
        public static void AplicarFormato(DataGridView grid)
        {
            if (grid.Columns.Count == 0)
                return;

            // El identificador, el id de estado y el id del usuario
            // asignado son información interna (para saber qué fila
            // está seleccionada, decidir acciones según su estado real
            // y poder preseleccionar el combo de asignación), nunca
            // deben mostrarse.
            grid.Columns["ID_TAREA"].Visible = false;
            grid.Columns["COD_SEQ_EST"].Visible = false;
            grid.Columns["COD_SEQ_USER"].Visible = false;

            grid.Columns["DES_DOC"].HeaderText = "Documento";
            grid.Columns["FEC_ENT_CAL"].HeaderText = "F. Ent. Calidad";
            grid.Columns["FEC_REG"].HeaderText = "F. Registro";
            grid.Columns["DES_ORG_VENTAS"].HeaderText = "Organización de Ventas";
            grid.Columns["DES_PROYECTO"].HeaderText = "Proyecto";
            grid.Columns["DES_SEGMENTO"].HeaderText = "Segmento";
            grid.Columns["IMP_ESTIMADO"].HeaderText = "Importe Estimado";
            grid.Columns["DES_ESTADO"].HeaderText = "Estado";
            grid.Columns["NOMBRE_USUARIO"].HeaderText = "Usuario";
            grid.Columns["DES_MOTIVO"].HeaderText = "Motivo";

            grid.Columns["FEC_ENT_CAL"].DefaultCellStyle.Format = "dd/MM/yyyy";
            grid.Columns["FEC_REG"].DefaultCellStyle.Format = "dd/MM/yyyy";
            grid.Columns["IMP_ESTIMADO"].DefaultCellStyle.Format = "N2";
        }
    }
}
