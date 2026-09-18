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
    /// Pantalla del paso 3 del flujo: seleccionar una tarea pendiente
    /// (estado "En Proceso") y cumplimentar sus datos de facturación.
    /// Al grabar, se inserta el registro de TBL_FACTURADO y la tarea
    /// pasa a estado "Facturado" (ver TareaRepositorio.Facturar). La
    /// pantalla permanece abierta tras cada grabación para poder
    /// facturar varias tareas seguidas.
    /// </summary>
    public partial class FrmFacturar : Form
    {
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
        private readonly DivisaRepositorio divisaRepositorio = new DivisaRepositorio();
        private readonly TipoFacturaRepositorio tipoFacturaRepositorio = new TipoFacturaRepositorio();
        private readonly EstadoTareaRepositorio estadoRepositorio = new EstadoTareaRepositorio();

        /// <summary>
        /// Cultura usada para interpretar y formatear el importe de
        /// factura (coma como separador decimal).
        /// </summary>
        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        /// <summary>Nombre del estado que deja visibles las tareas pendientes de facturar.</summary>
        private const string EstadoEnProceso = "EN_PROCESO";

        /// <summary>
        /// Valor usado en el combo de filtro de usuario para
        /// representar "no filtrar por este campo".
        /// </summary>
        private const int IdTodos = 0;

        /// <summary>Alto del panel de detalle cuando está desplegado.</summary>
        private const int AltoDetalleExpandido = 220;

        public FrmFacturar()
        {
            InitializeComponent();

            dgvTareas.AutoGenerateColumns = true;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetalleTarea.AutoGenerateColumns = true;
            dgvDetalleTarea.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            CargarFiltroUsuarios();
            CargarDivisas();
            CargarTiposFactura();
            BuscarTareasPendientes();
        }

        /// <summary>
        /// Carga en el combo de filtro de usuarios los usuarios
        /// activos más la opción "Todos" (Id = 0).
        /// </summary>
        private void CargarFiltroUsuarios()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario { Id = IdTodos, Nombre = "Todos" });

                cboFiltroUsuario.DataSource = usuarios;
                cboFiltroUsuario.DisplayMember = "Nombre";
                cboFiltroUsuario.ValueMember = "Id";
                cboFiltroUsuario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmFacturar.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de divisas las divisas activas.
        /// </summary>
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
                    "FrmFacturar.CargarDivisas",
                    "No se han podido cargar las divisas.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de tipos de factura los tipos activos.
        /// </summary>
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
                    "FrmFacturar.CargarTiposFactura",
                    "No se han podido cargar los tipos de factura.",
                    ex);
            }
        }

        /// <summary>
        /// Busca y muestra las tareas actualmente en estado "En
        /// Proceso" (las únicas candidatas a facturar), aplicando
        /// además los filtros de Documento, Usuario y fechas de
        /// pantalla.
        /// </summary>
        private void BuscarTareasPendientes()
        {
            try
            {
                FiltroBusquedaTareas filtro = new FiltroBusquedaTareas
                {
                    IdEstado = ObtenerIdEstadoEnProceso(),

                    IdUsuario = cboFiltroUsuario.SelectedValue != null
                        ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                        : IdTodos,

                    Documento = txtFiltroDocumento.Text.Trim(),

                    FechaDesde = dtpFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value.Date : null,
                    FechaHasta = dtpFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value.Date : null
                };

                DataTable resultado = tareaRepositorio.BuscarPorFiltro(filtro);

                dgvTareas.DataSource = resultado;

                FormateadorGridTareas.AplicarFormato(dgvTareas);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmFacturar.BuscarTareasPendientes",
                    "No se han podido buscar las tareas pendientes de facturar.",
                    ex);
            }
        }

        /// <summary>
        /// Resuelve el identificador del estado "En Proceso" a partir
        /// de los estados activos, sin necesitar una transacción
        /// abierta (a diferencia de TareaRepositorio, que lo hace
        /// dentro de cada operación de escritura).
        /// </summary>
        private int ObtenerIdEstadoEnProceso()
        {
            List<EstadoTarea> estados = estadoRepositorio.ObtenerActivos();

            foreach (EstadoTarea estado in estados)
            {
                if (estado.Descripcion == EstadoEnProceso)
                    return estado.Id;
            }

            throw new InvalidOperationException(
                "No existe el estado '" + EstadoEnProceso + "' en la tabla TBL_ESTADOS_TAREA.");
        }

        /// <summary>
        /// Da formato de presentación a las columnas de la rejilla de
        /// detalle. No hace nada si aún no tiene columnas.
        /// </summary>
        private void FormatearGridDetalle()
        {
            if (dgvDetalleTarea.Columns.Count == 0)
                return;

            dgvDetalleTarea.Columns["PedidoVenta"].HeaderText = "Pedido de Venta";
            dgvDetalleTarea.Columns["PedidoCompra"].HeaderText = "Pedido de Compra";
            dgvDetalleTarea.Columns["PedidoInspeccion"].HeaderText = "Pedido de Inspección";
            dgvDetalleTarea.Columns["EntidadEntrega"].HeaderText = "Entidad de Entrega";
            dgvDetalleTarea.Columns["Unidades"].HeaderText = "Unidades";
        }

        /// <summary>
        /// Al cambiar la fila activa de la rejilla de tareas, carga
        /// en la rejilla de detalle las líneas de la tarea
        /// correspondiente. Si no hay ninguna fila activa (rejilla
        /// vacía), limpia el detalle.
        /// </summary>
        private void dgvTareas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTareas.CurrentRow == null || dgvTareas.CurrentRow.Cells["ID_TAREA"].Value == null)
            {
                dgvDetalleTarea.DataSource = null;
                return;
            }

            int idTarea = Convert.ToInt32(dgvTareas.CurrentRow.Cells["ID_TAREA"].Value);

            try
            {
                List<DetalleTarea> detalles = tareaRepositorio.ObtenerDetalle(idTarea);

                dgvDetalleTarea.DataSource = detalles;

                FormatearGridDetalle();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmFacturar.dgvTareas_SelectionChanged",
                    "No se ha podido cargar el detalle de la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Relanza la búsqueda aplicando los filtros actuales de
        /// pantalla.
        /// </summary>
        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            BuscarTareasPendientes();
        }

        /// <summary>
        /// Restablece todos los filtros a "sin filtrar" y vuelve a
        /// buscar.
        /// </summary>
        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            txtFiltroDocumento.Clear();
            cboFiltroUsuario.SelectedIndex = 0;
            dtpFechaDesde.Checked = false;
            dtpFechaHasta.Checked = false;

            BuscarTareasPendientes();
        }

        /// <summary>
        /// Muestra u oculta el panel de detalle de la tarea
        /// seleccionada, para dejar más espacio a la rejilla de
        /// tareas cuando no hace falta verlo.
        /// </summary>
        private void btnColapsarDetalle_Click(object sender, EventArgs e)
        {
            bool expandir = !dgvDetalleTarea.Visible;

            dgvDetalleTarea.Visible = expandir;

            pnlDetalle.Height = expandir
                ? AltoDetalleExpandido
                : pnlDetalleHeader.Height;

            btnColapsarDetalle.Text = expandir ? "▼ Ocultar detalle" : "▲ Mostrar detalle";
        }

        /// <summary>
        /// Valida los datos, pide confirmación y, si el usuario
        /// confirma, registra la facturación de la tarea seleccionada.
        /// </summary>
        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (dgvTareas.CurrentRow == null || dgvTareas.CurrentRow.Cells["ID_TAREA"].Value == null)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione una tarea pendiente de facturar.",
                    "Facturar");

                return;
            }

            if (!ValidarDatos())
                return;

            if (!Dialogos.Confirmar(
                "¿Confirma la facturación de la tarea seleccionada?",
                "Confirmar facturación"))
            {
                return;
            }

            int idTarea = Convert.ToInt32(dgvTareas.CurrentRow.Cells["ID_TAREA"].Value);

            try
            {
                Facturado facturado = ConstruirFacturadoDesdeFormulario();

                tareaRepositorio.Facturar(idTarea, facturado);

                Dialogos.MostrarInformacion(
                    "La tarea se ha facturado correctamente.",
                    "Grabación correcta");

                LimpiarFormulario();
                BuscarTareasPendientes();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmFacturar.btnGrabar_Click",
                    "No se ha podido registrar la facturación.",
                    ex);
            }
        }

        /// <summary>
        /// Valida que los campos obligatorios de facturación
        /// (Código de Factura, Importe, Divisa y Tipo de Factura)
        /// estén informados.
        /// </summary>
        private bool ValidarDatos()
        {
            if (string.IsNullOrWhiteSpace(txtCodigoFactura.Text))
            {
                Dialogos.MostrarAviso("Debe introducir el Código de Factura.", "Validación");

                txtCodigoFactura.Focus();

                return false;
            }

            decimal importe;

            if (!IntentarObtenerDecimal(txtImporteFactura.Text, out importe))
            {
                Dialogos.MostrarAviso("El Importe de Factura no es un número válido.", "Validación");

                txtImporteFactura.Focus();

                return false;
            }

            if (cboDivisa.SelectedIndex == -1 || cboDivisa.SelectedValue == null)
            {
                Dialogos.MostrarAviso("Debe seleccionar una Divisa.", "Validación");

                cboDivisa.Focus();

                return false;
            }

            if (cboTipoFactura.SelectedIndex == -1 || cboTipoFactura.SelectedValue == null)
            {
                Dialogos.MostrarAviso("Debe seleccionar un Tipo de Factura.", "Validación");

                cboTipoFactura.Focus();

                return false;
            }

            return true;
        }

        /// <summary>
        /// Construye el objeto <see cref="Facturado"/> a partir del
        /// estado actual de los controles del formulario.
        /// </summary>
        private Facturado ConstruirFacturadoDesdeFormulario()
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
        /// Intenta interpretar un texto como número decimal usando
        /// el formato español (es-ES), donde la coma es el separador
        /// decimal.
        /// </summary>
        private bool IntentarObtenerDecimal(string texto, out decimal valor)
        {
            return decimal.TryParse(texto, NumberStyles.Number, CulturaDecimal, out valor);
        }

        /// <summary>
        /// Limpia los campos de facturación tras grabar, dejando el
        /// formulario listo para facturar la siguiente tarea.
        /// </summary>
        private void LimpiarFormulario()
        {
            txtEntidadSalida.Clear();
            dtpFechaFactura.Value = DateTime.Today;
            txtCodigoFactura.Clear();
            txtImporteFactura.Clear();
            cboDivisa.SelectedIndex = -1;
            cboTipoFactura.SelectedIndex = -1;
        }

        /// <summary>
        /// Cierra la pantalla de facturación.
        /// </summary>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
