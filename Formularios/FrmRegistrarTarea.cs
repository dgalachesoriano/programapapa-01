using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Pantalla de alta y edición de una factura/tarea, incluyendo
    /// sus líneas de detalle. Se usa tanto para registrar una tarea
    /// nueva (constructor sin parámetros) como para editar una ya
    /// existente (constructor con el número de registro).
    /// </summary>
    public partial class FrmRegistrarTarea : Form
    {
        private readonly UsuarioRepositorio usuarioRepositorio =
            new UsuarioRepositorio();

        private readonly SegmentoRepositorio segmentoRepositorio =
            new SegmentoRepositorio();

        private readonly FacturaRepositorio facturaRepositorio =
            new FacturaRepositorio();

        private readonly PegadoPortapapelesServicio pegadoPortapapelesServicio =
            new PegadoPortapapelesServicio();

        /// <summary>
        /// Nombre, dentro de <c>dgvDetalle</c>, de la columna cuyo
        /// contenido debe interpretarse como número decimal al
        /// pegar desde el portapapeles. Se resuelve a su índice de
        /// columna en tiempo de ejecución antes de invocar al
        /// servicio de pegado.
        /// </summary>
        private const string NombreColumnaDecimalDetalle = "Unidades";

        /// <summary>
        /// Número de registro de la factura en edición, o 0 cuando
        /// el formulario se usa para dar de alta una tarea nueva.
        /// </summary>
        private int registroEdicion = 0;

        /// <summary>
        /// Crea el formulario en modo "nueva tarea".
        /// </summary>
        public FrmRegistrarTarea()
        {
            InitializeComponent();

            ConfigurarGridDetalle();
            CargarUsuarios();
            CargarSegmentos();
        }

        /// <summary>
        /// Crea el formulario en modo edición, cargando los datos de
        /// la factura indicada.
        /// </summary>
        /// <param name="registro">Número de registro a editar.</param>
        public FrmRegistrarTarea(int registro)
        {
            InitializeComponent();

            ConfigurarGridDetalle();
            CargarUsuarios();
            CargarSegmentos();

            registroEdicion = registro;

            this.Text = "Tratar Tarea - Registro " + registro;

            CargarTarea(registro);
        }

        /// <summary>
        /// Define las columnas del grid de detalle (RC, unidades,
        /// precios, albarán) y su comportamiento de edición.
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

            dgvDetalle.Columns.Add(CrearColumnaTexto("RC", "RC", 150));
            dgvDetalle.Columns.Add(CrearColumnaDecimal("Unidades", "Unidades", 80));
            dgvDetalle.Columns.Add(CrearColumnaTexto("PInspeccion", "P. Inspección", 130));
            dgvDetalle.Columns.Add(CrearColumnaTexto("PCompra", "P. Compra", 120));
            dgvDetalle.Columns.Add(CrearColumnaTexto("PVenta", "P. Venta", 120));
            dgvDetalle.Columns.Add(CrearColumnaTexto("Albaran", "Albarán", 140));
        }

        /// <summary>
        /// Intercepta teclas a nivel de formulario para dar soporte a
        /// pegado especial (Ctrl+V) en el grid de detalle y a la
        /// navegación con Enter entre controles.
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // CTRL + V → pegado especial en el DataGridView
            if (keyData == (Keys.Control | Keys.V))
            {
                if (dgvDetalle.ContainsFocus)
                {
                    PegarDesdePortapapeles();
                    return true;
                }

                return base.ProcessCmdKey(ref msg, keyData);
            }

            // ENTER → siguiente control
            if (keyData == Keys.Enter)
            {
                // Dentro del DataGridView dejamos que funcione su
                // comportamiento normal.
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
        /// portapapeles (p. ej. copiado desde Excel). Solo se ocupa
        /// de la interacción con el portapapeles del sistema y de
        /// traducir el resultado a los avisos que ve el usuario; el
        /// análisis del texto (filas, columnas, valores decimales) y
        /// su volcado sobre el grid vive en
        /// <see cref="PegadoPortapapelesServicio"/>, reutilizable
        /// fuera de este formulario.
        /// </summary>
        private void PegarDesdePortapapeles()
        {
            try
            {
                if (!Clipboard.ContainsText())
                {
                    MessageBox.Show(
                        "El portapapeles no contiene texto.",
                        "Pegado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    return;
                }

                string texto = Clipboard.GetText();

                int columnaDecimal =
                    dgvDetalle.Columns[NombreColumnaDecimalDetalle].Index;

                ResultadoPegadoPortapapeles resultado =
                    pegadoPortapapelesServicio.Pegar(
                        dgvDetalle,
                        texto,
                        columnaDecimal,
                        new CultureInfo("es-ES"));

                if (resultado == ResultadoPegadoPortapapeles.SinCeldaSeleccionada)
                {
                    MessageBox.Show(
                        "Seleccione primero una celda del detalle.",
                        "Pegado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Se ha producido un error al pegar los datos.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Crea una columna de texto libre para el grid de detalle.
        /// </summary>
        private DataGridViewTextBoxColumn CrearColumnaTexto(
            string nombre,
            string titulo,
            int ancho)
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
        private DataGridViewTextBoxColumn CrearColumnaDecimal(
            string nombre,
            string titulo,
            int ancho)
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
        /// Carga en el combo de usuarios los usuarios activos más la
        /// opción "Sin asignar" (IdUsuario = 0).
        /// </summary>
        private void CargarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario
                {
                    IdUsuario = 0,
                    UsuarioLogin = "",
                    Nombre = "Sin asignar"
                });

                cboUsuario.DataSource = usuarios;
                cboUsuario.DisplayMember = "Nombre";
                cboUsuario.ValueMember = "IdUsuario";
                cboUsuario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se han podido cargar los usuarios.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga en el combo de segmentos todos los segmentos de
        /// negocio disponibles, sin ninguno preseleccionado.
        /// </summary>
        private void CargarSegmentos()
        {
            try
            {
                List<Segmento> segmentos = segmentoRepositorio.ObtenerTodos();

                cboSegmento.DataSource = segmentos;
                cboSegmento.DisplayMember = "Nombre";
                cboSegmento.ValueMember = "IdSegmento";
                cboSegmento.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se han podido cargar los segmentos.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

            DialogResult respuesta = MessageBox.Show(
                "¿Está seguro de que quiere grabar la tarea?",
                "Confirmar grabación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            GrabarTarea();
        }

        /// <summary>
        /// Valida que los campos obligatorios de cabecera (Documento,
        /// Sociedad, Proyecto y Segmento) estén informados. Si falta
        /// alguno, muestra un aviso, pone el foco en el campo y
        /// devuelve false.
        /// </summary>
        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtDocumento.Text))
            {
                MessageBox.Show(
                    "Debe introducir el Documento.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtDocumento.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtSociedad.Text))
            {
                MessageBox.Show(
                    "Debe introducir la Sociedad.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtSociedad.Focus();

                return false;
            }

            if (string.IsNullOrWhiteSpace(txtProyecto.Text))
            {
                MessageBox.Show(
                    "Debe introducir el Proyecto.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                txtProyecto.Focus();

                return false;
            }

            if (cboSegmento.SelectedIndex == -1 || cboSegmento.SelectedValue == null)
            {
                MessageBox.Show(
                    "Debe seleccionar un Segmento.",
                    "Validación",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                cboSegmento.Focus();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Construye la cabecera y el detalle a partir de los
        /// controles de pantalla y delega en el repositorio la
        /// inserción (tarea nueva) o actualización (edición) de la
        /// factura, dentro de una única transacción.
        /// </summary>
        private void GrabarTarea()
        {
            try
            {
                Factura factura = ConstruirFacturaDesdeFormulario();
                List<DetalleFactura> detalles = ObtenerDetallesDesdeGrid();

                if (registroEdicion == 0)
                {
                    int nuevoRegistro = facturaRepositorio.Insertar(factura, detalles);

                    MessageBox.Show(
                        "La tarea se ha grabado correctamente.\n\n" +
                        "Registro: " + nuevoRegistro,
                        "Grabación correcta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
                else
                {
                    factura.Registro = registroEdicion;

                    facturaRepositorio.Actualizar(factura, detalles);

                    MessageBox.Show(
                        "La tarea se ha actualizado correctamente.\n\n" +
                        "Registro: " + registroEdicion,
                        "Grabación correcta",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se ha podido grabar la tarea.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Construye el objeto <see cref="Factura"/> de cabecera a
        /// partir del estado actual de los controles del formulario.
        /// El importe estimado se deja sin informar si el texto
        /// introducido no es un número válido.
        /// </summary>
        private Factura ConstruirFacturaDesdeFormulario()
        {
            decimal importeEstimado;

            bool importeValido = IntentarObtenerDecimal(
                txtImporteEstimado.Text,
                out importeEstimado);

            int? idUsuario = null;

            if (cboUsuario.SelectedValue != null &&
                Convert.ToInt32(cboUsuario.SelectedValue) != 0)
            {
                idUsuario = Convert.ToInt32(cboUsuario.SelectedValue);
            }

            return new Factura
            {
                Documento = txtDocumento.Text.Trim(),
                FechaEntradaCalidad = dtpFEntCalidad.Value.Date,
                FechaRegistro = dtpFRegistro.Value.Date,
                Sociedad = txtSociedad.Text.Trim(),
                Proyecto = txtProyecto.Text.Trim(),
                ImporteEstimado = importeValido ? (decimal?)importeEstimado : null,
                IdUsuarioAsignado = idUsuario,
                IdSegmento = Convert.ToInt32(cboSegmento.SelectedValue)
            };
        }

        /// <summary>
        /// Recorre las filas del grid de detalle (excluyendo la fila
        /// vacía de nueva entrada) y las convierte en objetos
        /// <see cref="DetalleFactura"/>. Las filas completamente
        /// vacías se conservan aquí y se filtran más adelante en el
        /// repositorio antes de insertarlas.
        /// </summary>
        private List<DetalleFactura> ObtenerDetallesDesdeGrid()
        {
            List<DetalleFactura> detalles = new List<DetalleFactura>();

            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                decimal unidades;

                bool unidadesValidas = IntentarObtenerDecimal(
                    ObtenerTextoCelda(fila, "Unidades"),
                    out unidades);

                detalles.Add(new DetalleFactura
                {
                    RC = ObtenerTextoCelda(fila, "RC"),
                    Unidades = unidadesValidas ? (decimal?)unidades : null,
                    PInspeccion = ObtenerTextoCelda(fila, "PInspeccion"),
                    PCompra = ObtenerTextoCelda(fila, "PCompra"),
                    PVenta = ObtenerTextoCelda(fila, "PVenta"),
                    Albaran = ObtenerTextoCelda(fila, "Albaran")
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
            return decimal.TryParse(
                texto,
                NumberStyles.Number,
                new CultureInfo("es-ES"),
                out valor);
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
                txtImporteEstimado.Text = importe.ToString("N2", new CultureInfo("es-ES"));
            }
        }

        /// <summary>
        /// Carga en pantalla los datos de cabecera y detalle de la
        /// factura indicada. Si no se encuentra, muestra un aviso y
        /// deja el formulario sin rellenar.
        /// </summary>
        private void CargarTarea(int registro)
        {
            try
            {
                Factura factura = facturaRepositorio.ObtenerPorRegistro(registro);

                if (factura == null)
                {
                    MessageBox.Show(
                        "No se ha encontrado la tarea.",
                        "Error",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    return;
                }

                txtDocumento.Text = factura.Documento;
                dtpFEntCalidad.Value = factura.FechaEntradaCalidad;
                dtpFRegistro.Value = factura.FechaRegistro;
                txtSociedad.Text = factura.Sociedad;
                txtProyecto.Text = factura.Proyecto;

                txtImporteEstimado.Text = factura.ImporteEstimado.HasValue
                    ? factura.ImporteEstimado.Value.ToString("N2", new CultureInfo("es-ES"))
                    : string.Empty;

                cboUsuario.SelectedValue = factura.IdUsuarioAsignado ?? 0;
                cboSegmento.SelectedValue = factura.IdSegmento;

                List<DetalleFactura> detalles = facturaRepositorio.ObtenerDetalle(registro);

                CargarDetalleEnGrid(detalles);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se ha podido cargar la tarea.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Vuelca en el grid de detalle la lista de líneas obtenida
        /// del repositorio, añadiendo una fila por cada línea.
        /// </summary>
        private void CargarDetalleEnGrid(List<DetalleFactura> detalles)
        {
            foreach (DetalleFactura detalle in detalles)
            {
                int fila = dgvDetalle.Rows.Add();

                dgvDetalle.Rows[fila].Cells["RC"].Value = detalle.RC;
                dgvDetalle.Rows[fila].Cells["Unidades"].Value = detalle.Unidades;
                dgvDetalle.Rows[fila].Cells["PInspeccion"].Value = detalle.PInspeccion;
                dgvDetalle.Rows[fila].Cells["PCompra"].Value = detalle.PCompra;
                dgvDetalle.Rows[fila].Cells["PVenta"].Value = detalle.PVenta;
                dgvDetalle.Rows[fila].Cells["Albaran"].Value = detalle.Albaran;
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
