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
    /// Pantalla de alta de una tarea nueva, incluyendo sus líneas de
    /// detalle. Crea además el evento inicial de TBL_CONTROL con
    /// estado "Registrado" (ver TareaRepositorio.Insertar). Tras
    /// grabar correctamente, el formulario NO se cierra: se reinicia
    /// en blanco (ver GrabarTarea/LimpiarFormularioParaNuevaTarea)
    /// para registrar varias tareas seguidas sin tener que volver a
    /// abrirlo; solo se cierra con el botón Cancelar. Editar una
    /// tarea ya existente (cabecera, detalle, estado, usuario y
    /// facturación) es responsabilidad de FrmDetalleTarea, que se abre
    /// desde el Pool de Tareas.
    /// </summary>
    public partial class FrmRegistrarTarea : Form
    {
        private readonly OrganizacionVentaRepositorio organizacionVentaRepositorio = new OrganizacionVentaRepositorio();
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();
        private readonly SegmentoRepositorio segmentoRepositorio = new SegmentoRepositorio();
        private readonly TipoFacturaRepositorio tipoFacturaRepositorio = new TipoFacturaRepositorio();
        private readonly DivisaRepositorio divisaRepositorio = new DivisaRepositorio();
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

        public FrmRegistrarTarea()
        {
            InitializeComponent();

            ConfigurarGridDetalle();
            ConfigurarLimpiezaDeBordes();
            CargarOrganizacionesVenta();
            CargarProyectos();
            CargarSegmentos();
            CargarTiposDocumento();
            CargarMonedasEstimadas();
        }

        /// <summary>
        /// Engancha, en cada campo obligatorio, el evento que quita
        /// su borde rojo en cuanto el usuario empieza a escribir o a
        /// elegir un valor (sin esperar a que vuelva a ser válido: el
        /// aviso desaparece nada más empezar a corregirlo). En el
        /// grid de detalle, el borde se quita en cuanto se empieza a
        /// editar cualquier celda.
        /// </summary>
        private void ConfigurarLimpiezaDeBordes()
        {
            txtDocumento.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlDocumentoBorde);
            cboOrgVentas.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlOrgVentasBorde);
            cboProyecto.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlProyectoBorde);
            cboSegmento.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlSegmentoBorde);
            txtImporteEstimado.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlImporteEstimadoBorde);
            cboTipoDocumento.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlTipoDocumentoBorde);
            cboMonedaEstimada.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlMonedaEstimadaBorde);
            dgvDetalle.CellBeginEdit += (s, e) => ValidadorCampos.Limpiar(pnlDetalleBorde);
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
        /// Carga en el combo de organizaciones de ventas las
        /// organizaciones de ventas activas.
        /// </summary>
        private void CargarOrganizacionesVenta()
        {
            try
            {
                List<OrganizacionVenta> organizaciones = organizacionVentaRepositorio.ObtenerActivos();

                cboOrgVentas.DataSource = organizaciones;
                cboOrgVentas.DisplayMember = "Descripcion";
                cboOrgVentas.ValueMember = "Id";
                cboOrgVentas.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarOrganizacionesVenta",
                    "No se han podido cargar las organizaciones de ventas.",
                    ex);
            }
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
        /// Carga en el combo de tipo de documento los tipos activos.
        /// </summary>
        private void CargarTiposDocumento()
        {
            try
            {
                List<TipoFactura> tipos = tipoFacturaRepositorio.ObtenerActivos();

                cboTipoDocumento.DataSource = tipos;
                cboTipoDocumento.DisplayMember = "Descripcion";
                cboTipoDocumento.ValueMember = "Id";
                cboTipoDocumento.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarTiposDocumento",
                    "No se han podido cargar los tipos de documento.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de moneda estimada las divisas activas.
        /// </summary>
        private void CargarMonedasEstimadas()
        {
            try
            {
                List<Divisa> divisas = divisaRepositorio.ObtenerActivas();

                cboMonedaEstimada.DataSource = divisas;
                cboMonedaEstimada.DisplayMember = "Codigo";
                cboMonedaEstimada.ValueMember = "Id";
                cboMonedaEstimada.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmRegistrarTarea.CargarMonedasEstimadas",
                    "No se han podido cargar las divisas.",
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
        /// Valida los datos; si falta algo, marca en rojo los campos
        /// pendientes y ofrece completarlos o descartar la tarea. Si
        /// todo está correcto, pide confirmación y graba.
        /// </summary>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
            {
                using (FrmFaltaInformacion dialogo = new FrmFaltaInformacion(
                    "No se puede dar de alta la tarea porque falta información.\n\n" +
                    "Los campos y el detalle pendientes de rellenar han quedado " +
                    "marcados en rojo."))
                {
                    dialogo.ShowDialog(this);

                    if (dialogo.SeHaDescartado)
                    {
                        Close();
                    }
                }

                return;
            }

            if (!Dialogos.Confirmar(
                "¿Está seguro de que quiere grabar la tarea?",
                "Confirmar grabación"))
            {
                return;
            }

            GrabarTarea();
        }

        /// <summary>
        /// Valida que todos los campos de cabecera (Documento,
        /// Organización de Ventas, Proyecto, Segmento e Importe
        /// Estimado) estén informados, y que el detalle tenga al
        /// menos una línea completamente rellena. A diferencia de la
        /// validación anterior, no se detiene en el primer campo que
        /// falla: comprueba todos y marca en rojo (ver
        /// <see cref="ValidadorCampos"/>) cada uno de los que falten,
        /// para que se vean todos a la vez.
        /// </summary>
        private bool ValidarDatos()
        {
            bool documentoValido = ValidadorCampos.ValidarTexto(txtDocumento, pnlDocumentoBorde);
            bool orgVentasValido = ValidadorCampos.ValidarCombo(cboOrgVentas, pnlOrgVentasBorde);
            bool proyectoValido = ValidadorCampos.ValidarCombo(cboProyecto, pnlProyectoBorde);
            bool segmentoValido = ValidadorCampos.ValidarCombo(cboSegmento, pnlSegmentoBorde);
            bool importeValido = ValidarImporteEstimado();
            bool tipoDocumentoValido = ValidadorCampos.ValidarCombo(cboTipoDocumento, pnlTipoDocumentoBorde);
            bool monedaEstimadaValida = ValidadorCampos.ValidarCombo(cboMonedaEstimada, pnlMonedaEstimadaBorde);
            bool detalleValido = ValidarDetalle();

            return documentoValido && orgVentasValido && proyectoValido
                && segmentoValido && importeValido && tipoDocumentoValido
                && monedaEstimadaValida && detalleValido;
        }

        /// <summary>
        /// Valida el Importe Estimado: ahora es obligatorio, así que
        /// debe estar relleno y ser un número válido.
        /// </summary>
        private bool ValidarImporteEstimado()
        {
            decimal importe;

            bool valido = !string.IsNullOrWhiteSpace(txtImporteEstimado.Text) &&
                IntentarObtenerDecimal(txtImporteEstimado.Text, out importe);

            pnlImporteEstimadoBorde.BackColor = valido ? ValidadorCampos.ColorNormal : ValidadorCampos.ColorInvalido;

            return valido;
        }

        /// <summary>
        /// Valida el grid de detalle: tiene que haber, como mínimo,
        /// una línea con las cinco columnas completamente rellenas, y
        /// en ninguna línea la columna "Unidades" (si está informada)
        /// puede contener un valor no numérico. Cualquiera de los dos
        /// problemas marca en rojo el grid completo.
        /// </summary>
        private bool ValidarDetalle()
        {
            bool hayLineaCompleta = false;
            bool unidadesValidasEnTodas = true;

            foreach (DataGridViewRow fila in dgvDetalle.Rows)
            {
                if (fila.IsNewRow)
                    continue;

                if (FilaCompleta(fila))
                {
                    hayLineaCompleta = true;
                }

                string textoUnidades = ObtenerTextoCelda(fila, "Unidades");
                decimal valor;

                if (!string.IsNullOrWhiteSpace(textoUnidades) &&
                    !IntentarObtenerDecimal(textoUnidades, out valor))
                {
                    unidadesValidasEnTodas = false;
                }
            }

            bool valido = hayLineaCompleta && unidadesValidasEnTodas;

            pnlDetalleBorde.BackColor = valido ? ValidadorCampos.ColorNormal : ValidadorCampos.ColorInvalido;

            return valido;
        }

        /// <summary>
        /// Indica si una fila del detalle tiene sus cinco columnas
        /// (Pedido de Venta, Pedido de Compra, Pedido de Inspección,
        /// Entidad de Entrega y Unidades) rellenas.
        /// </summary>
        private bool FilaCompleta(DataGridViewRow fila)
        {
            return !string.IsNullOrWhiteSpace(ObtenerTextoCelda(fila, "PedidoVenta"))
                && !string.IsNullOrWhiteSpace(ObtenerTextoCelda(fila, "PedidoCompra"))
                && !string.IsNullOrWhiteSpace(ObtenerTextoCelda(fila, "PedidoInspeccion"))
                && !string.IsNullOrWhiteSpace(ObtenerTextoCelda(fila, "EntidadEntrega"))
                && !string.IsNullOrWhiteSpace(ObtenerTextoCelda(fila, "Unidades"));
        }

        /// <summary>
        /// Construye la cabecera y el detalle a partir de los
        /// controles de pantalla y delega en el repositorio el alta.
        /// No cierra el formulario al terminar: lo reinicia (ver
        /// <see cref="LimpiarFormularioParaNuevaTarea"/>) para poder
        /// registrar varias tareas seguidas sin volver a abrirlo.
        /// </summary>
        private void GrabarTarea()
        {
            try
            {
                Tarea tarea = ConstruirTareaDesdeFormulario();
                List<DetalleTarea> detalles = ObtenerDetallesDesdeGrid();

                tareaRepositorio.Insertar(tarea, detalles);

                Dialogos.MostrarInformacion(
                    "La tarea se ha registrado correctamente.",
                    "Grabación correcta");

                LimpiarFormularioParaNuevaTarea();
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
        /// Deja la pantalla como recién abierta (todos los campos en
        /// blanco, sin filas de detalle) tras un alta correcta, y
        /// devuelve el foco al Documento para poder encadenar el
        /// registro de la siguiente tarea.
        /// </summary>
        private void LimpiarFormularioParaNuevaTarea()
        {
            txtDocumento.Clear();
            dtpFEntCalidad.Value = DateTime.Today;
            dtpFRegistro.Value = DateTime.Today;
            cboOrgVentas.SelectedIndex = -1;
            cboProyecto.SelectedIndex = -1;
            cboSegmento.SelectedIndex = -1;
            cboTipoDocumento.SelectedIndex = -1;
            cboMonedaEstimada.SelectedIndex = -1;
            txtImporteEstimado.Clear();
            txtNotas.Clear();
            dgvDetalle.Rows.Clear();

            txtDocumento.Focus();
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
                IdOrganizacionVentas = Convert.ToInt32(cboOrgVentas.SelectedValue),
                IdProyecto = Convert.ToInt32(cboProyecto.SelectedValue),
                IdSegmento = Convert.ToInt32(cboSegmento.SelectedValue),
                ImporteEstimado = importeValido ? (decimal?)importeEstimado : null,
                IdTipoDocumento = Convert.ToInt32(cboTipoDocumento.SelectedValue),
                IdDivisaEstimada = Convert.ToInt32(cboMonedaEstimada.SelectedValue),
                Notas = string.IsNullOrWhiteSpace(txtNotas.Text) ? null : txtNotas.Text.Trim()
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
        /// Cierra el formulario sin grabar cambios.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
