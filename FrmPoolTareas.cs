using GestionFacturas.Datos;
using GestionFacturas.Formularios;
using GestionFacturas.Modelos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace GestionFacturas
{
    /// <summary>
    /// Pantalla del "pool de tareas": permite consultar las facturas
    /// pendientes filtrando por estado, proyecto, usuario asignado y
    /// rango de fechas de registro, ver el detalle de cada una, y
    /// asignar (o desasignar) de una sola vez el usuario responsable
    /// de una o varias tareas seleccionadas.
    /// </summary>
    public partial class FrmPoolTareas : Form
    {
        private readonly EstadoFacturaRepositorio estadoRepositorio =
            new EstadoFacturaRepositorio();

        private readonly UsuarioRepositorio usuarioRepositorio =
            new UsuarioRepositorio();

        private readonly FacturaRepositorio facturaRepositorio =
            new FacturaRepositorio();

        /// <summary>
        /// Valor usado en los combos de filtro para representar "no
        /// filtrar por este campo" (Estado/Usuario) y, en el combo de
        /// asignación, para representar "Sin asignar".
        /// </summary>
        private const int IdTodos = 0;

        public FrmPoolTareas()
        {
            InitializeComponent();

            ConfigurarGrids();
            CargarFiltroEstados();
            CargarFiltroProyectos();
            CargarFiltroUsuarios();
            CargarUsuarioAsignado();

            BuscarTareas();
        }

        /// <summary>
        /// Configura el comportamiento general de las dos rejillas:
        /// tareas (selección múltiple, para poder asignar varias de
        /// golpe) y detalle (solo lectura, siempre de una tarea).
        /// </summary>
        private void ConfigurarGrids()
        {
            dgvTareas.AutoGenerateColumns = true;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvDetalle.AutoGenerateColumns = true;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Carga en el combo de filtro de estados todos los estados
        /// existentes más la opción "Todos" (IdEstado = 0).
        /// </summary>
        private void CargarFiltroEstados()
        {
            try
            {
                List<EstadoFactura> estados = estadoRepositorio.ObtenerTodos();

                estados.Insert(0, new EstadoFactura
                {
                    IdEstado = IdTodos,
                    Estado = "Todos"
                });

                cboFiltroEstado.DataSource = estados;
                cboFiltroEstado.DisplayMember = "Estado";
                cboFiltroEstado.ValueMember = "IdEstado";
                cboFiltroEstado.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se han podido cargar los estados.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga en el combo de filtro de proyectos los nombres de
        /// proyecto que ya tienen alguna factura registrada, más la
        /// opción "Todos". No existe una tabla de proyectos propia:
        /// se toman los valores distintos de Facturas.Proyecto.
        /// </summary>
        private void CargarFiltroProyectos()
        {
            try
            {
                List<string> proyectos = facturaRepositorio.ObtenerProyectosDistintos();

                proyectos.Insert(0, "Todos");

                cboFiltroProyecto.DataSource = proyectos;
                cboFiltroProyecto.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se han podido cargar los proyectos.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga en el combo de filtro de usuarios los usuarios
        /// activos más la opción "Todos" (IdUsuario = 0).
        /// </summary>
        private void CargarFiltroUsuarios()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario
                {
                    IdUsuario = IdTodos,
                    Nombre = "Todos"
                });

                cboFiltroUsuario.DataSource = usuarios;
                cboFiltroUsuario.DisplayMember = "Nombre";
                cboFiltroUsuario.ValueMember = "IdUsuario";
                cboFiltroUsuario.SelectedIndex = 0;
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
        /// Carga en el combo de usuario destino de la asignación los
        /// usuarios activos más la opción "Sin asignar" (IdUsuario =
        /// 0), que permite también desasignar tareas en bloque.
        /// </summary>
        private void CargarUsuarioAsignado()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario
                {
                    IdUsuario = IdTodos,
                    UsuarioLogin = "",
                    Nombre = "Sin asignar"
                });

                cboUsuarioAsignado.DataSource = usuarios;
                cboUsuarioAsignado.DisplayMember = "Nombre";
                cboUsuarioAsignado.ValueMember = "IdUsuario";
                cboUsuarioAsignado.SelectedIndex = 0;
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
        /// Construye el filtro a partir de los controles de pantalla,
        /// busca las facturas correspondientes y las muestra en la
        /// rejilla de tareas.
        /// </summary>
        private void BuscarTareas()
        {
            try
            {
                FiltroBusquedaFacturas filtro = new FiltroBusquedaFacturas
                {
                    IdEstado = cboFiltroEstado.SelectedValue != null
                        ? Convert.ToInt32(cboFiltroEstado.SelectedValue)
                        : IdTodos,

                    IdUsuario = cboFiltroUsuario.SelectedValue != null
                        ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                        : IdTodos,

                    Proyecto = cboFiltroProyecto.SelectedIndex > 0
                        ? (cboFiltroProyecto.SelectedItem as string ?? string.Empty)
                        : string.Empty,

                    FechaDesde = dtpFechaDesde.Checked
                        ? (DateTime?)dtpFechaDesde.Value.Date
                        : null,

                    FechaHasta = dtpFechaHasta.Checked
                        ? (DateTime?)dtpFechaHasta.Value.Date
                        : null
                };

                DataTable resultado = facturaRepositorio.BuscarPorFiltro(filtro);

                dgvTareas.DataSource = resultado;

                FormatearGridTareas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se han podido buscar las tareas.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Da formato de presentación a las columnas de la rejilla de
        /// tareas: cabeceras legibles y formatos de fecha/importe. No
        /// hace nada si la rejilla aún no tiene columnas (p. ej. una
        /// búsqueda sin resultados).
        /// </summary>
        private void FormatearGridTareas()
        {
            if (dgvTareas.Columns.Count == 0)
                return;

            dgvTareas.Columns["Registro"].HeaderText = "Registro";
            dgvTareas.Columns["Documento"].HeaderText = "Documento";
            dgvTareas.Columns["F_Ent_Calidad"].HeaderText = "F. Ent. Calidad";
            dgvTareas.Columns["F_Registro"].HeaderText = "F. Registro";
            dgvTareas.Columns["Sociedad"].HeaderText = "Sociedad";
            dgvTareas.Columns["Proyecto"].HeaderText = "Proyecto";
            dgvTareas.Columns["Segmento"].HeaderText = "Segmento";
            dgvTareas.Columns["Estado"].HeaderText = "Estado";
            dgvTareas.Columns["Usuario"].HeaderText = "Usuario";
            dgvTareas.Columns["Importe_Estimado"].HeaderText = "Importe Estimado";
            dgvTareas.Columns["Num_Factura"].HeaderText = "Factura";
            dgvTareas.Columns["F_Factura"].HeaderText = "F. Factura";
            dgvTareas.Columns["Importe_Total"].HeaderText = "Importe Total";

            dgvTareas.Columns["F_Ent_Calidad"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvTareas.Columns["F_Registro"].DefaultCellStyle.Format = "dd/MM/yyyy";
            dgvTareas.Columns["F_Factura"].DefaultCellStyle.Format = "dd/MM/yyyy";

            dgvTareas.Columns["Importe_Estimado"].DefaultCellStyle.Format = "N2";
            dgvTareas.Columns["Importe_Total"].DefaultCellStyle.Format = "N2";
        }

        /// <summary>
        /// Da formato de presentación a las columnas de la rejilla de
        /// detalle. No hace nada si aún no tiene columnas (p. ej.
        /// ninguna tarea seleccionada, o sin líneas de detalle).
        /// </summary>
        private void FormatearGridDetalle()
        {
            if (dgvDetalle.Columns.Count == 0)
                return;

            dgvDetalle.Columns["RC"].HeaderText = "RC";
            dgvDetalle.Columns["Unidades"].HeaderText = "Unidades";
            dgvDetalle.Columns["PInspeccion"].HeaderText = "P. Inspección";
            dgvDetalle.Columns["PCompra"].HeaderText = "P. Compra";
            dgvDetalle.Columns["PVenta"].HeaderText = "P. Venta";
            dgvDetalle.Columns["Albaran"].HeaderText = "Albarán";
        }

        /// <summary>
        /// Al cambiar la fila activa de la rejilla de tareas, carga
        /// en la rejilla de detalle las líneas de la tarea
        /// correspondiente. Si no hay ninguna fila activa (rejilla
        /// vacía), limpia el detalle.
        /// </summary>
        private void dgvTareas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTareas.CurrentRow == null ||
                dgvTareas.CurrentRow.Cells["Registro"].Value == null)
            {
                dgvDetalle.DataSource = null;
                return;
            }

            int registro = Convert.ToInt32(dgvTareas.CurrentRow.Cells["Registro"].Value);

            try
            {
                List<DetalleFactura> detalles = facturaRepositorio.ObtenerDetalle(registro);

                dgvDetalle.DataSource = detalles;

                FormatearGridDetalle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se ha podido cargar el detalle de la tarea.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Obtiene, sin duplicados, los números de registro de las
        /// filas actualmente seleccionadas en la rejilla de tareas.
        /// </summary>
        private List<int> ObtenerRegistrosSeleccionados()
        {
            List<int> registros = new List<int>();
            HashSet<int> vistos = new HashSet<int>();

            foreach (DataGridViewRow fila in dgvTareas.SelectedRows)
            {
                if (fila.Cells["Registro"].Value == null)
                    continue;

                int registro = Convert.ToInt32(fila.Cells["Registro"].Value);

                if (vistos.Add(registro))
                {
                    registros.Add(registro);
                }
            }

            return registros;
        }

        /// <summary>
        /// Relanza la búsqueda aplicando los filtros actuales de
        /// pantalla.
        /// </summary>
        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            BuscarTareas();
        }

        /// <summary>
        /// Restablece todos los filtros a "sin filtrar" y vuelve a
        /// buscar.
        /// </summary>
        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            cboFiltroEstado.SelectedIndex = 0;
            cboFiltroProyecto.SelectedIndex = 0;
            cboFiltroUsuario.SelectedIndex = 0;
            dtpFechaDesde.Checked = false;
            dtpFechaHasta.Checked = false;

            BuscarTareas();
        }

        /// <summary>
        /// Vuelve a lanzar la búsqueda con los filtros actuales, por
        /// ejemplo para refrescar la lista tras asignar tareas o si
        /// otro usuario ha modificado datos entre tanto.
        /// </summary>
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BuscarTareas();
        }

        /// <summary>
        /// Asigna (o desasigna, si se elige "Sin asignar") de una
        /// sola vez el usuario del combo de asignación a todas las
        /// tareas seleccionadas en la rejilla, previa confirmación.
        /// </summary>
        private void btnAsignarTarea_Click(object sender, EventArgs e)
        {
            List<int> registros = ObtenerRegistrosSeleccionados();

            if (registros.Count == 0)
            {
                MessageBox.Show(
                    "Seleccione al menos una tarea de la lista.",
                    "Asignar tarea",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            if (cboUsuarioAsignado.SelectedValue == null)
                return;

            int idUsuarioSeleccionado = Convert.ToInt32(cboUsuarioAsignado.SelectedValue);

            int? idUsuario = idUsuarioSeleccionado == IdTodos
                ? (int?)null
                : idUsuarioSeleccionado;

            string mensaje = idUsuario.HasValue
                ? "¿Asignar " + registros.Count + " tarea(s) a '" + cboUsuarioAsignado.Text + "'?"
                : "¿Quitar la asignación de " + registros.Count + " tarea(s)?";

            DialogResult respuesta = MessageBox.Show(
                mensaje,
                "Confirmar asignación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (respuesta != DialogResult.Yes)
                return;

            try
            {
                facturaRepositorio.AsignarUsuario(registros, idUsuario);

                MessageBox.Show(
                    "Asignación realizada correctamente.",
                    "Asignar tarea",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                BuscarTareas();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "No se ha podido asignar la tarea.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Abre en modo edición la tarea con el foco en la rejilla de
        /// resultados y, al cerrarse ese formulario, refresca la
        /// búsqueda para reflejar los cambios.
        /// </summary>
        private void btnAbrirTarea_Click(object sender, EventArgs e)
        {
            if (dgvTareas.CurrentRow == null)
            {
                MessageBox.Show(
                    "Debe seleccionar una tarea.",
                    "Abrir tarea",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                return;
            }

            int registro = Convert.ToInt32(
                dgvTareas.CurrentRow.Cells["Registro"].Value);

            using (FrmRegistrarTarea formulario = new FrmRegistrarTarea(registro))
            {
                formulario.ShowDialog();
            }

            BuscarTareas();
        }
    }
}
