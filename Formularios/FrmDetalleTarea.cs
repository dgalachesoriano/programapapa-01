using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Pantalla de detalle/edición completa de una tarea: estado
    /// actual y usuario asignado (editable), datos de cabecera y de
    /// detalle (igual que en el alta, con la misma validación), datos
    /// de facturación (vacíos si aún no se ha facturado) y las
    /// acciones de cambio de estado (bloquear/desbloquear/cancelar).
    /// Se abre desde el Pool de Tareas (botón "Abrir tarea" o doble
    /// clic sobre una fila).
    ///
    /// El botón Guardar aplica una regla automática de estado (ver
    /// TareaRepositorio.GuardarEdicionCompleta): sin datos de
    /// facturación, la tarea queda en Registrado o En Proceso según
    /// haya o no usuario asignado; con los datos de facturación
    /// completos, pasa a Facturado (o se corrige la factura ya
    /// existente si ya lo estaba). Esa regla NO se aplica mientras la
    /// tarea está Bloqueada: guardar cabecera/detalle no la
    /// desbloquea sin querer, solo lo hace el botón Desbloquear. Una
    /// tarea Cancelada es irreversible y esta pantalla se muestra
    /// entera en solo lectura.
    /// </summary>
    public partial class FrmDetalleTarea : Form
    {
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();
        private readonly SegmentoRepositorio segmentoRepositorio = new SegmentoRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
        private readonly DivisaRepositorio divisaRepositorio = new DivisaRepositorio();
        private readonly TipoFacturaRepositorio tipoFacturaRepositorio = new TipoFacturaRepositorio();

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

        /// <summary>Alto del panel de detalle cuando está desplegado.</summary>
        private const int AltoDetalleExpandido = 200;

        /// <summary>
        /// Valor usado en el combo de usuario asignado para
        /// representar "sin usuario".
        /// </summary>
        private const int IdSinAsignar = 0;

        private readonly int idTarea;
        private EstadoActualTarea estadoActual;

        public FrmDetalleTarea(int idTarea)
        {
            InitializeComponent();

            this.idTarea = idTarea;

            ConfigurarGridDetalle();
            ConfigurarLimpiezaDeBordes();

            CargarProyectos();
            CargarSegmentos();
            CargarUsuarios();
            CargarDivisas();
            CargarTiposFactura();

            CargarTarea();
        }

        /// <summary>
        /// Engancha, en cada campo obligatorio, el evento que quita
        /// su borde rojo en cuanto el usuario empieza a corregirlo
        /// (igual que FrmRegistrarTarea/FrmFacturar).
        /// </summary>
        private void ConfigurarLimpiezaDeBordes()
        {
            txtDocumento.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlDocumentoBorde);
            txtOrgVentas.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlOrgVentasBorde);
            cboProyecto.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlProyectoBorde);
            cboSegmento.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlSegmentoBorde);
            txtImporteEstimado.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlImporteEstimadoBorde);
            dgvDetalle.CellBeginEdit += (s, e) => ValidadorCampos.Limpiar(pnlDetalleBorde);

            txtEntidadSalida.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlEntidadSalidaBorde);
            txtCodigoFactura.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlCodigoFacturaBorde);
            txtImporteFactura.TextChanged += (s, e) => ValidadorCampos.Limpiar(pnlImporteFacturaBorde);
            cboDivisa.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlDivisaBorde);
            cboTipoFactura.SelectedIndexChanged += (s, e) => ValidadorCampos.Limpiar(pnlTipoFacturaBorde);
        }

        /// <summary>
        /// Define las columnas del grid de detalle y su comportamiento
        /// de edición (igual que FrmRegistrarTarea).
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
        /// navegación con Enter entre controles (igual que
        /// FrmRegistrarTarea).
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.V))
            {
                if (dgvDetalle.ContainsFocus && dgvDetalle.Enabled)
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
                    "FrmDetalleTarea.PegarDesdePortapapeles",
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

        /// <summary>Carga en el combo de proyectos los proyectos activos.</summary>
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
                    "FrmDetalleTarea.CargarProyectos",
                    "No se han podido cargar los proyectos.",
                    ex);
            }
        }

        /// <summary>Carga en el combo de segmentos los segmentos activos.</summary>
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
                    "FrmDetalleTarea.CargarSegmentos",
                    "No se han podido cargar los segmentos.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de usuario asignado los usuarios activos
        /// más la opción "Sin asignar" (Id = 0).
        /// </summary>
        private void CargarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario { Id = IdSinAsignar, Nombre = "Sin asignar" });

                cboUsuarioAsignado.DataSource = usuarios;
                cboUsuarioAsignado.DisplayMember = "Nombre";
                cboUsuarioAsignado.ValueMember = "Id";
                cboUsuarioAsignado.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.CargarUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>Carga en el combo de divisas las divisas activas.</summary>
        private void CargarDivisas()
        {
            try
            {
                List<Divisa> divisas = divisaRepositorio.ObtenerActivas();

                cboDivisa.DataSource = divisas;
                cboDivisa.DisplayMember = "Codigo";
                cboDivisa.ValueMember = "Id";
                cboDivisa.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.CargarDivisas",
                    "No se han podido cargar las divisas.",
                    ex);
            }
        }

        /// <summary>Carga en el combo de tipos de factura los tipos activos.</summary>
        private void CargarTiposFactura()
        {
            try
            {
                List<TipoFactura> tipos = tipoFacturaRepositorio.ObtenerActivos();

                cboTipoFactura.DataSource = tipos;
                cboTipoFactura.DisplayMember = "Descripcion";
                cboTipoFactura.ValueMember = "Id";
                cboTipoFactura.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.CargarTiposFactura",
                    "No se han podido cargar los tipos de factura.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en pantalla la cabecera, el detalle, el estado/
        /// usuario/motivo y los datos de facturación (si los hay) de
        /// la tarea, y ajusta qué controles quedan habilitados según
        /// su estado actual.
        /// </summary>
        private void CargarTarea()
        {
            try
            {
                Tarea tarea = tareaRepositorio.ObtenerPorId(idTarea);

                if (tarea == null)
                {
                    Dialogos.MostrarAviso("No se ha encontrado la tarea.", "Error");

                    Close();
                    return;
                }

                estadoActual = tareaRepositorio.ObtenerEstadoActual(idTarea);

                this.Text = "Tarea " + tarea.Documento;

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

                CargarEstadoYUsuario();
                CargarFacturacion();

                AjustarDisponibilidadSegunEstado();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.CargarTarea",
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
        /// Muestra el estado actual (con su color) y preselecciona el
        /// usuario asignado; si la tarea está Bloqueada, muestra
        /// también el motivo.
        /// </summary>
        private void CargarEstadoYUsuario()
        {
            lblEstadoValor.Text = estadoActual.DescripcionEstado;
            lblEstadoValor.ForeColor = ColorDeEstado(estadoActual.IdEstado);

            cboUsuarioAsignado.SelectedValue = estadoActual.IdUsuario ?? IdSinAsignar;

            bool bloqueada = estadoActual.IdEstado == EstadosTareaConocidos.Pendiente;

            lblMotivoTitulo.Visible = bloqueada;
            lblMotivoValor.Visible = bloqueada;
            lblMotivoValor.Text = bloqueada ? estadoActual.DescripcionMotivo : string.Empty;
        }

        /// <summary>
        /// Rellena los campos de facturación si la tarea ya está
        /// facturada, o los deja vacíos si todavía no lo está.
        /// </summary>
        private void CargarFacturacion()
        {
            if (!estadoActual.IdFacturado.HasValue)
            {
                txtEntidadSalida.Clear();
                dtpFechaFactura.Value = DateTime.Today;
                txtCodigoFactura.Clear();
                txtImporteFactura.Clear();
                cboDivisa.SelectedIndex = -1;
                cboTipoFactura.SelectedIndex = -1;

                return;
            }

            txtEntidadSalida.Text = estadoActual.EntidadSalida;
            dtpFechaFactura.Value = estadoActual.FechaFactura ?? DateTime.Today;
            txtCodigoFactura.Text = estadoActual.CodigoFactura;

            txtImporteFactura.Text = estadoActual.ImporteFactura.HasValue
                ? estadoActual.ImporteFactura.Value.ToString("N2", CulturaDecimal)
                : string.Empty;

            cboDivisa.SelectedValue = estadoActual.IdDivisa;
            cboTipoFactura.SelectedValue = estadoActual.IdTipoFactura;
        }

        /// <summary>
        /// Traduce el ID de un estado al color con que se muestra su
        /// nombre (misma familia de colores que las filas del Pool de
        /// Tareas, ver FrmPoolTareas.ColorDeEstado, pero en tono
        /// sólido en vez de pastel: aquí es texto sobre fondo blanco,
        /// no el fondo de una fila).
        /// </summary>
        private Color ColorDeEstado(int idEstado)
        {
            switch (idEstado)
            {
                case EstadosTareaConocidos.Registrado:
                    return Color.FromArgb(97, 97, 97);

                case EstadosTareaConocidos.EnProceso:
                    return Color.FromArgb(21, 101, 192);

                case EstadosTareaConocidos.Pendiente:
                    return Color.FromArgb(245, 127, 23);

                case EstadosTareaConocidos.Facturado:
                    return Color.FromArgb(46, 125, 50);

                case EstadosTareaConocidos.Cancelado:
                    return Color.FromArgb(198, 40, 40);

                default:
                    return SystemColors.ControlText;
            }
        }

        /// <summary>
        /// Habilita/deshabilita botones y controles según el estado
        /// actual de la tarea: una tarea Cancelada se muestra entera
        /// en solo lectura (es irreversible, no admite más cambios);
        /// para el resto, Bloquear solo tiene sentido si no está ya
        /// Bloqueada/Facturada, Desbloquear solo si está Bloqueada, y
        /// Cancelar solo si no está ya Facturada/Cancelada.
        /// </summary>
        private void AjustarDisponibilidadSegunEstado()
        {
            bool cancelada = estadoActual.IdEstado == EstadosTareaConocidos.Cancelado;

            if (cancelada)
            {
                AplicarSoloLectura();

                btnBloquearTarea.Visible = false;
                btnDesbloquearTarea.Visible = false;
                btnCancelarTarea.Visible = false;
                btnGuardar.Visible = false;

                return;
            }

            btnBloquearTarea.Enabled =
                estadoActual.IdEstado == EstadosTareaConocidos.Registrado ||
                estadoActual.IdEstado == EstadosTareaConocidos.EnProceso;

            btnDesbloquearTarea.Enabled = estadoActual.IdEstado == EstadosTareaConocidos.Pendiente;

            btnCancelarTarea.Enabled = estadoActual.IdEstado != EstadosTareaConocidos.Facturado;
        }

        /// <summary>
        /// Deshabilita todos los controles editables del formulario
        /// (usado para una tarea Cancelada: solo se puede consultar).
        /// </summary>
        private void AplicarSoloLectura()
        {
            cboUsuarioAsignado.Enabled = false;

            txtDocumento.Enabled = false;
            dtpFEntCalidad.Enabled = false;
            dtpFRegistro.Enabled = false;
            txtOrgVentas.Enabled = false;
            cboProyecto.Enabled = false;
            cboSegmento.Enabled = false;
            txtImporteEstimado.Enabled = false;
            dgvDetalle.Enabled = false;

            txtEntidadSalida.Enabled = false;
            dtpFechaFactura.Enabled = false;
            txtCodigoFactura.Enabled = false;
            txtImporteFactura.Enabled = false;
            cboDivisa.Enabled = false;
            cboTipoFactura.Enabled = false;
        }

        /// <summary>
        /// Muestra u oculta el grid de detalle, para dejar más
        /// espacio al resto de la pantalla cuando no hace falta verlo
        /// (igual que FrmFacturar).
        /// </summary>
        private void btnColapsarDetalle_Click(object sender, EventArgs e)
        {
            bool expandir = !dgvDetalle.Visible;

            dgvDetalle.Visible = expandir;

            pnlDetalle.Height = expandir
                ? AltoDetalleExpandido
                : pnlDetalleHeader.Height;

            btnColapsarDetalle.Text = expandir ? "▼ Ocultar detalle" : "▲ Mostrar detalle";
        }

        /// <summary>
        /// Bloquea la tarea (mismo comportamiento que en el Pool de
        /// Tareas: motivo obligatorio de una lista y confirmación).
        /// Al terminar, cierra la pantalla: el Pool refresca su
        /// búsqueda al recuperar el foco.
        /// </summary>
        private void btnBloquearTarea_Click(object sender, EventArgs e)
        {
            int idMotivo;

            using (FrmSeleccionarMotivo formulario = new FrmSeleccionarMotivo())
            {
                if (formulario.ShowDialog(this) != DialogResult.OK)
                    return;

                idMotivo = formulario.IdMotivoSeleccionado;
            }

            if (!Dialogos.Confirmar("¿Bloquear esta tarea?", "Confirmar bloqueo"))
                return;

            try
            {
                tareaRepositorio.Bloquear(new[] { idTarea }, idMotivo);

                Dialogos.MostrarInformacion("Bloqueo realizado correctamente.", "Bloquear tarea");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.btnBloquearTarea_Click",
                    "No se ha podido bloquear la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Desbloquea la tarea, devolviéndola a su estado anterior al
        /// bloqueo (mismo comportamiento que en el Pool de Tareas).
        /// </summary>
        private void btnDesbloquearTarea_Click(object sender, EventArgs e)
        {
            if (!Dialogos.Confirmar(
                "¿Desbloquear esta tarea y devolverla a su estado anterior?",
                "Confirmar desbloqueo"))
            {
                return;
            }

            try
            {
                tareaRepositorio.Desbloquear(new[] { idTarea });

                Dialogos.MostrarInformacion("Desbloqueo realizado correctamente.", "Desbloquear tarea");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.btnDesbloquearTarea_Click",
                    "No se ha podido desbloquear la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Cancela la tarea de forma definitiva, avisando antes de
        /// que la acción no se puede deshacer.
        /// </summary>
        private void btnCancelarTarea_Click(object sender, EventArgs e)
        {
            if (!Dialogos.Confirmar(
                "¿Cancelar esta tarea?\n\n" +
                "Esta acción NO se puede deshacer: la tarea quedará definitivamente " +
                "en estado Cancelado.",
                "Confirmar cancelación"))
            {
                return;
            }

            try
            {
                tareaRepositorio.Cancelar(new[] { idTarea });

                Dialogos.MostrarInformacion("Cancelación realizada correctamente.", "Cancelar tarea");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.btnCancelarTarea_Click",
                    "No se ha podido cancelar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Valida los datos; si falta algo, marca en rojo los campos
        /// pendientes y ofrece completarlos o descartar los cambios.
        /// Si todo está correcto, pide confirmación y graba cabecera,
        /// detalle y, salvo que la tarea esté Bloqueada, el estado y
        /// la facturación (ver TareaRepositorio.GuardarEdicionCompleta).
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (!ValidarDatos())
            {
                using (FrmFaltaInformacion dialogo = new FrmFaltaInformacion(
                    "No se puede grabar la tarea porque falta información, o los datos " +
                    "de facturación están incompletos (deben estar todos vacíos o todos " +
                    "rellenos).\n\n" +
                    "Los campos pendientes han quedado marcados en rojo."))
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
                "¿Está seguro de que quiere grabar los cambios?",
                "Confirmar grabación"))
            {
                return;
            }

            try
            {
                Tarea tarea = ConstruirTareaDesdeFormulario();
                List<DetalleTarea> detalles = ObtenerDetallesDesdeGrid();

                bool bloqueada = estadoActual.IdEstado == EstadosTareaConocidos.Pendiente;

                int? idUsuarioNuevo = cboUsuarioAsignado.SelectedValue == null
                    ? (int?)null
                    : ConvertirIdUsuario(Convert.ToInt32(cboUsuarioAsignado.SelectedValue));

                Facturado facturado = FacturacionEstaVacia() ? null : ConstruirFacturacionDesdeFormulario();

                tareaRepositorio.GuardarEdicionCompleta(
                    tarea, detalles,
                    aplicarReglaEstado: !bloqueada,
                    idUsuarioNuevo: idUsuarioNuevo,
                    facturado: facturado);

                Dialogos.MostrarInformacion(
                    "La tarea se ha actualizado correctamente.",
                    "Grabación correcta");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmDetalleTarea.btnGuardar_Click",
                    "No se ha podido grabar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Traduce el valor del combo de usuario asignado a un
        /// identificador nulo cuando representa "Sin asignar".
        /// </summary>
        private int? ConvertirIdUsuario(int idSeleccionado)
        {
            return idSeleccionado == IdSinAsignar ? (int?)null : idSeleccionado;
        }

        /// <summary>
        /// Valida cabecera, detalle y facturación. No se detiene en
        /// el primer campo que falla: comprueba todos y marca en rojo
        /// cada uno de los que falten, para que se vean todos a la
        /// vez (igual que FrmRegistrarTarea/FrmFacturar).
        /// </summary>
        private bool ValidarDatos()
        {
            bool documentoValido = ValidadorCampos.ValidarTexto(txtDocumento, pnlDocumentoBorde);
            bool orgVentasValido = ValidadorCampos.ValidarTexto(txtOrgVentas, pnlOrgVentasBorde);
            bool proyectoValido = ValidadorCampos.ValidarCombo(cboProyecto, pnlProyectoBorde);
            bool segmentoValido = ValidadorCampos.ValidarCombo(cboSegmento, pnlSegmentoBorde);
            bool importeValido = ValidarImporteEstimado();
            bool detalleValido = ValidarDetalle();
            bool facturacionValida = ValidarFacturacion();

            return documentoValido && orgVentasValido && proyectoValido
                && segmentoValido && importeValido && detalleValido && facturacionValida;
        }

        /// <summary>
        /// Valida el Importe Estimado: obligatorio y numérico.
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
        /// una línea con las cinco columnas completamente rellenas
        /// (igual que en el alta).
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
        /// rellenas.
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
        /// Indica si los datos de facturación están completamente
        /// vacíos (ni un solo campo relleno o seleccionado): es una
        /// situación válida, significa que la tarea aún no se ha
        /// facturado.
        /// </summary>
        private bool FacturacionEstaVacia()
        {
            return string.IsNullOrWhiteSpace(txtEntidadSalida.Text)
                && string.IsNullOrWhiteSpace(txtCodigoFactura.Text)
                && string.IsNullOrWhiteSpace(txtImporteFactura.Text)
                && cboDivisa.SelectedValue == null
                && cboTipoFactura.SelectedValue == null;
        }

        /// <summary>
        /// Valida los datos de facturación: si están completamente
        /// vacíos, es válido (aún no se ha facturado); si hay algo
        /// relleno, entonces deben estarlo TODOS (igual que
        /// FrmFacturar.ValidarDatos), o se marcan en rojo los que
        /// falten.
        /// </summary>
        private bool ValidarFacturacion()
        {
            if (FacturacionEstaVacia())
            {
                ValidadorCampos.Limpiar(pnlEntidadSalidaBorde);
                ValidadorCampos.Limpiar(pnlCodigoFacturaBorde);
                ValidadorCampos.Limpiar(pnlImporteFacturaBorde);
                ValidadorCampos.Limpiar(pnlDivisaBorde);
                ValidadorCampos.Limpiar(pnlTipoFacturaBorde);

                return true;
            }

            bool entidadValida = ValidadorCampos.ValidarTexto(txtEntidadSalida, pnlEntidadSalidaBorde);
            bool codigoValido = ValidadorCampos.ValidarTexto(txtCodigoFactura, pnlCodigoFacturaBorde);
            bool importeValido = ValidarImporteFactura();
            bool divisaValida = ValidadorCampos.ValidarCombo(cboDivisa, pnlDivisaBorde);
            bool tipoValido = ValidadorCampos.ValidarCombo(cboTipoFactura, pnlTipoFacturaBorde);

            return entidadValida && codigoValido && importeValido && divisaValida && tipoValido;
        }

        /// <summary>
        /// Valida el Importe de Factura: debe estar relleno y ser un
        /// número válido.
        /// </summary>
        private bool ValidarImporteFactura()
        {
            decimal importe;

            bool valido = !string.IsNullOrWhiteSpace(txtImporteFactura.Text) &&
                IntentarObtenerDecimal(txtImporteFactura.Text, out importe);

            pnlImporteFacturaBorde.BackColor = valido ? ValidadorCampos.ColorNormal : ValidadorCampos.ColorInvalido;

            return valido;
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
                Id = idTarea,
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
        /// Construye el objeto <see cref="Facturado"/> a partir del
        /// estado actual de los controles del formulario. Solo debe
        /// llamarse cuando <see cref="FacturacionEstaVacia"/> es
        /// false.
        /// </summary>
        private Facturado ConstruirFacturacionDesdeFormulario()
        {
            decimal importe;
            IntentarObtenerDecimal(txtImporteFactura.Text, out importe);

            return new Facturado
            {
                EntidadSalida = txtEntidadSalida.Text.Trim(),
                FechaFactura = dtpFechaFactura.Value.Date,
                CodigoFactura = txtCodigoFactura.Text.Trim(),
                ImporteFactura = importe,
                IdDivisa = Convert.ToInt32(cboDivisa.SelectedValue),
                IdTipoFactura = Convert.ToInt32(cboTipoFactura.SelectedValue)
            };
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
        /// Al salir del campo de importe de factura, si el texto es
        /// un número válido lo reformatea con dos decimales en
        /// formato español.
        /// </summary>
        private void txtImporteFactura_Leave(object sender, EventArgs e)
        {
            decimal importe;

            if (IntentarObtenerDecimal(txtImporteFactura.Text, out importe))
            {
                txtImporteFactura.Text = importe.ToString("N2", CulturaDecimal);
            }
        }

        /// <summary>
        /// Cierra la pantalla sin grabar cambios pendientes.
        /// </summary>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
