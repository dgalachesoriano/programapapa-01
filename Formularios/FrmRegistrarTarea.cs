using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Pantalla de alta y edición de una tarea, incluyendo sus líneas
    /// de detalle. Se usa tanto para registrar una tarea nueva
    /// (constructor sin parámetros) como para editar una ya existente
    /// (constructor con el identificador). Al dar de alta una tarea
    /// nueva, se crea además el evento inicial de TBL_CONTROL con
    /// estado "REGISTRADO" (ver TareaRepositorio.Insertar); editar
    /// una tarea existente nunca toca su estado en el flujo.
    /// </summary>
    public partial class FrmRegistrarTarea : Form
    {
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();
        private readonly SegmentoRepositorio segmentoRepositorio = new SegmentoRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();

        private readonly PegadoPortapapelesServicio pegadoPortapapelesServicio =
            new PegadoPortapapelesServicio();

        /// <summary>
        /// Cultura usada en todo el formulario para interpretar y
        /// formatear valores decimales (coma como separador).
        /// </summary>
        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        /// <summary>
        /// Nombre, dentro de <c>dgvDetalle</c>, de la columna cuyo
        /// contenido debe interpretarse como número decimal al pegar
        /// desde el portapapeles.
        /// </summary>
        private const string NombreColumnaDecimalDetalle = "Unidades";

        /// <summary>
        /// Identificador de la tarea en edición, o 0 cuando el
        /// formulario se usa para dar de alta una tarea nueva.
        /// </summary>
        private int idTareaEdicion = 0;

        /// <summary>
        /// Crea el formulario en modo "nueva tarea".
        /// </summary>
        public FrmRegistrarTarea()
        {
            InitializeComponent();

            ConfigurarGridDetalle();
            CargarProyectos();
            CargarSegmentos();
        }

        /// <summary>
        /// Crea el formulario en modo edición, cargando los datos de
        /// la tarea indicada.
        /// </summary>
        /// <param name="idTarea">Identificador de la tarea a editar.</param>
        public FrmRegistrarTarea(int idTarea)
        {
            InitializeComponent();

            ConfigurarGridDetalle();
            CargarProyectos();
            CargarSegmentos();

            idTareaEdicion = idTarea;

            this.Text = "Tratar Tarea";

            CargarTarea(idTarea);
        }

        /// <summary>
        /// Define las columnas del grid de detalle y su
        /// comportamiento de edición.
        /// </summary>
        private void ConfigurarGridDetalle()
        {
            dgvDetalle.Columns.Clear();

            dgvDetalle.AllowUserToAddRows = true;
            dgvDetalle.AllowUserToDeleteRows = true;
            dgvDetalle.AllowUserToResizeRows = false;

            dgvDetalle.AutoGenerateColumns = false;
            dgvDetalle.SelectionMode = DataGridViewSelectionMode.CellSelect;
            dgvDetalle.MultiSelect = true;
            dgvDetalle.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithoutHeaderText;

            dgvDetalle.Columns.Add(CrearColumnaTexto("PedidoVenta", "Pedido de Venta", 130));
            dgvDetalle.Columns.Add(CrearColumnaTexto("PedidoCompra", "Pedido de Compra", 130));
            dgvDetalle.Columns.Add(CrearColumnaTexto("PedidoInspeccion", "Pedido de Inspección", 140));
            dgvDetalle.Columns.Add(CrearColumnaTexto("EntidadEntrega", "Entidad de Entrega", 140));
            dgvDetalle.Columns.Add(CrearColumnaDecimal("Unidades", "Unidades", 90));
        }

        /// <summary>
        /// Intercepta teclas a nivel de formulario para dar soporte a
        /// pegado especial (Ctrl+V) en el grid de detalle y a la
        /// navegación con Enter entre controles.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.V))
            {
                if (dgvDetalle.ContainsFocus)
                {
                    PegarDesdePortapapeles();
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }

            if (keyData == Keys.Enter)
            {
                if (dgvDetalle.ContainsFocus)
                {
                    return base.ProcessCmdKey(ref msg, keyData);
                }

                Control controlActual = GetControlConFoco(this);

                if (controlActual != null)
                {
                    SelectNextControl(controlActual, true, true, true, true);
                }

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Recorre la cadena de controles activos hasta encontrar el
        /// que tiene el foco realmente (el ActiveControl más profundo).
        /// </summary>
        private Control GetControlConFoco(Control control)
        {
            Control controlConFoco = control;

            while (controlConFoco is ContainerControl &&
                   ((ContainerControl)controlConFoco).ActiveControl != null)
            {
                controlConFoco = ((ContainerControl)controlConFoco).ActiveControl;
            }

            return controlConFoco;
        }

        /// <summary>
        /// Pega en el grid de detalle el contenido de texto del
        /// portapapeles (p. ej. copiado desde Excel).
        /// </summary>
        private void PegarDesdePortapapeles()
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    Dialogos.MostrarInformacion(
                        "El portapapeles no contiene texto.",
                        "Pegado");

                    return;
                }

                string texto = Clipboard.GetText();

                int columnaDecimal = dgvDetalle.Columns[NombreColumnaDecimalDetalle].Index;

                ResultadoPegadoPortapapeles resultado =
                    pegadoPortapapelesServicio.Pegar(dgvDetalle, texto, columnaDecimal, CulturaDecimal);

                if (resultado == ResultadoPegadoPortapapeles.SinCeldaSeleccionada)
                {
                    Dialogos.MostrarInformacion(
                        "Seleccione primero una celda del detalle.",
                        "Pegado");
                }
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.PegarDesdePortapapeles",
                    "Se ha producido un error al pegar los datos.",
                    ex);
            }
        }

        /// <summary>
        /// Crea una columna de texto libre para el grid de detalle.
        /// </summary>
        private DataGridViewTextBoxColumn CrearColumnaTexto(string nombre, string titulo, int ancho)
        {
            return new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = titulo,
                Width = ancho
            };
        }

        /// <summary>
        /// Crea una columna numérica (formato "N2", alineada a la
        /// derecha) para el grid de detalle.
        /// </summary>
        private DataGridViewTextBoxColumn CrearColumnaDecimal(string nombre, string titulo, int ancho)
        {
            var columna = new DataGridViewTextBoxColumn
            {
                Name = nombre,
                HeaderText = titulo,
                Width = ancho
            };

            columna.DefaultCellStyle.Format = "N2";
            columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            return columna;
        }

        /// <summary>
        /// Carga en el combo de proyectos los proyectos activos.
        /// </summary>
        private void CargarProyectos()
        {
            try
            {
                List<Proyecto> proyectos = proyectoRepositorio.ObtenerActivos();

                cboProyecto.DataSource = proyectos;
                cboProyecto.DisplayMember = "Descripcion";
                cboProyecto.ValueMember = "Id";
                cboProyecto.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarProyectos",
                    "No se han podido cargar los proyectos.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de segmentos los segmentos activos.
        /// </summary>
        private void CargarSegmentos()
        {
            try
            {
                List<Segmento> segmentos = segmentoRepositorio.ObtenerActivos();

                cboSegmento.DataSource = segmentos;
                cboSegmento.DisplayMember = "Descripcion";
                cboSegmento.ValueMember = "Id";
                cboSegmento.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarSegmentos",
                    "No se han podido cargar los segmentos.",
                    ex);
            }
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Load del formulario; toda la carga
        /// inicial se realiza en los constructores.
        /// </summary>
        private void FrmRegistrarTarea_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Valida los datos, pide confirmación al usuario y, si
        /// confirma, graba la tarea.
        /// </summary>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
                return;

            if (!Dialogos.Confirmar(
                "¿Está seguro de que quiere grabar la tarea?",
                "Confirmar grabación"))
            {
                return;
            }

            GrabarTarea();
        }

        /// <summary>
        /// Valida que los campos obligatorios de cabecera (Documento,
        /// Organización de Ventas, Proyecto y Segmento) estén
        /// informados. Si falta alguno, muestra un aviso, pone el
        /// foco en el campo y devuelve false.
        /// </summary>
        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                Dialogos.MostrarAviso("Debe introducir el Documento.", "Validación");

                txtDocumento.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtOrgVentas.Text))
            {
                Dialogos.MostrarAviso("Debe introducir la Organización de Ventas.", "Validación");

                txtOrgVentas.Focus();

                return false;
            }

            if (cboProyecto.SelectedIndex == -1 || cboProyecto.SelectedValue == null)
            {
                Dialogos.MostrarAviso("Debe seleccionar un Proyecto.", "Validación");

                cboProyecto.Focus();

                return false;
            }

            if (cboSegmento.SelectedIndex == -1 || cboSegmento.SelectedValue == null)
            {
                Dialogos.MostrarAviso("Debe seleccionar un Segmento.", "Validación");

                cboSegmento.Focus();

                return false;
            }

            decimal importeEstimado;

            if (!string.IsNullOrWhiteSpace(txtImporteEstimado.Text) &&
                !IntentarObtenerDecimal(txtImporteEstimado.Text, out importeEstimado))
            {
                Dialogos.MostrarAviso("El Importe Estimado no es un número válido.", "Validación");

                txtImporteEstimado.Focus();

                return false;
            }

            if (!ValidarUnidadesDetalle())
                return false;

            return true;
        }

        /// <summary>
        /// Comprueba que, en las filas del detalle que no estén
        /// completamente vacías, la columna "Unidades" (si está
        /// informada) contenga un número válido.
        /// </summary>
        private bool ValidarUnidadesDetalle()
        {
            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                string texto = ObtenerTextoCelda(fila, "Unidades");

                if (string.IsNullOrWhiteSpace(texto))
                    continue;

                decimal valor;

                if (!IntentarObtenerDecimal(texto, out valor))
                {
                    Dialogos.MostrarAviso(
                        "La fila " + (fila.Index + 1) + " del detalle tiene un " +
                        "valor de Unidades no válido.",
                        "Validación");

                    dgvDetalle.CurrentCell = fila.Cells["Unidades"];
                    dgvDetalle.Focus();

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Construye la cabecera y el detalle a partir de los
        /// controles de pantalla y delega en el repositorio la
        /// inserción (tarea nueva) o actualización (edición).
        /// </summary>
        private void GrabarTarea()
        {
            try
            {
                Tarea tarea = ConstruirTareaDesdeFormulario();
                List<DetalleTarea> detalles = ObtenerDetallesDesdeGrid();

                if (idTareaEdicion == 0)
                {
                    int nuevoId = tareaRepositorio.Insertar(tarea, detalles);

                    Dialogos.MostrarInformacion(
                        "La tarea se ha registrado correctamente.",
                        "Grabación correcta");
                }
                else
                {
                    tarea.Id = idTareaEdicion;

                    tareaRepositorio.Actualizar(tarea, detalles);

                    Dialogos.MostrarInformacion(
                        "La tarea se ha actualizado correctamente.",
                        "Grabación correcta");
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.GrabarTarea",
                    "No se ha podido grabar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Construye el objeto <see cref="Tarea"/> de cabecera a
        /// partir del estado actual de los controles del formulario.
        /// </summary>
        private Tarea ConstruirTareaDesdeFormulario()
        {
            decimal importeEstimado;

            bool importeValido = IntentarObtenerDecimal(txtImporteEstimado.Text, out importeEstimado);

            return new Tarea
            {
                Documento = txtDocumento.Text.Trim(),
                FechaEntradaCalidad = dtpFEntCalidad.Value.Date,
                FechaRegistro = dtpFRegistro.Value.Date,
                OrganizacionVentas = txtOrgVentas.Text.Trim(),
                IdProyecto = Convert.ToInt32(cboProyecto.SelectedValue),
                IdSegmento = Convert.ToInt32(cboSegmento.SelectedValue),
                ImporteEstimado = importeValido ? (decimal?)importeEstimado : null
            };
        }

        /// <summary>
        /// Recorre las filas del grid de detalle (excluyendo la fila
        /// vacía de nueva entrada) y las convierte en objetos
        /// <see cref="DetalleTarea"/>.
        /// </summary>
        private List<DetalleTarea> ObtenerDetallesDesdeGrid()
        {
            List<DetalleTarea> detalles = new List<DetalleTarea>();

            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                decimal unidades;

                bool unidadesValidas = IntentarObtenerDecimal(
                    ObtenerTextoCelda(fila, "Unidades"), out unidades);

                detalles.Add(new DetalleTarea
                {
                    PedidoVenta = ObtenerTextoCelda(fila, "PedidoVenta"),
                    PedidoCompra = ObtenerTextoCelda(fila, "PedidoCompra"),
                    PedidoInspeccion = ObtenerTextoCelda(fila, "PedidoInspeccion"),
                    EntidadEntrega = ObtenerTextoCelda(fila, "EntidadEntrega"),
                    Unidades = unidadesValidas ? (decimal?)unidades : null
                });
            }

            return detalles;
        }

        /// <summary>
        /// Obtiene el texto de una celda del grid de detalle,
        /// devolviendo cadena vacía si la celda está sin informar.
        /// </summary>
        private string ObtenerTextoCelda(DataGridViewRow fila, string nombreColumna)
        {
            object valor = fila.Cells[nombreColumna].Value;

            if (valor == null)
                return "";

            return valor.ToString().Trim();
        }

        /// <summary>
        /// Intenta interpretar un texto como número decimal usando
        /// el formato español (es-ES), donde la coma es el separador
        /// decimal.
        /// </summary>
        private bool IntentarObtenerDecimal(string texto, out decimal valor)
        {
            return decimal.TryParse(texto, NumberStyles.Number, CulturaDecimal, out valor);
        }

        /// <summary>
        /// Al salir del campo de importe estimado, si el texto es un
        /// número válido lo reformatea con dos decimales.
        /// </summary>
        private void txtImporteEstimado_Leave(object sender, EventArgs e)
        {
            decimal importe;

            if (IntentarObtenerDecimal(txtImporteEstimado.Text, out importe))
            {
                txtImporteEstimado.Text = importe.ToString("N2", CulturaDecimal);
            }
        }

        /// <summary>
        /// Carga en pantalla los datos de cabecera y detalle de la
        /// tarea indicada. Si no se encuentra, muestra un aviso y
        /// deja el formulario sin rellenar.
        /// </summary>
        private void CargarTarea(int idTarea)
        {
            try
            {
                Tarea tarea = tareaRepositorio.ObtenerPorId(idTarea);

                if (tarea == null)
                {
                    Dialogos.MostrarAviso("No se ha encontrado la tarea.", "Error");

                    return;
                }

                txtDocumento.Text = tarea.Documento;
                dtpFEntCalidad.Value = tarea.FechaEntradaCalidad;
                dtpFRegistro.Value = tarea.FechaRegistro;
                txtOrgVentas.Text = tarea.OrganizacionVentas;

                txtImporteEstimado.Text = tarea.ImporteEstimado.HasValue
                    ? tarea.ImporteEstimado.Value.ToString("N2", CulturaDecimal)
                    : string.Empty;

                cboProyecto.SelectedValue = tarea.IdProyecto;
                cboSegmento.SelectedValue = tarea.IdSegmento;

                List<DetalleTarea> detalles = tareaRepositorio.ObtenerDetalle(idTarea);

                CargarDetalleEnGrid(detalles);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarTarea",
                    "No se ha podido cargar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Vuelca en el grid de detalle la lista de líneas obtenida
        /// del repositorio, añadiendo una fila por cada línea.
        /// </summary>
        private void CargarDetalleEnGrid(List<DetalleTarea> detalles)
        {
            foreach (DetalleTarea detalle in detalles)
            {
                int fila = dgvDetalle.Rows.Add();

                dgvDetalle.Rows[fila].Cells["PedidoVenta"].Value = detalle.PedidoVenta;
                dgvDetalle.Rows[fila].Cells["PedidoCompra"].Value = detalle.PedidoCompra;
                dgvDetalle.Rows[fila].Cells["PedidoInspeccion"].Value = detalle.PedidoInspeccion;
                dgvDetalle.Rows[fila].Cells["EntidadEntrega"].Value = detalle.EntidadEntrega;
                dgvDetalle.Rows[fila].Cells["Unidades"].Value = detalle.Unidades;
            }
        }

        /// <summary>
        /// Cierra el formulario sin grabar cambios.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
