using GestionFacturas.Datos;
using GestionFacturas.Formularios;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
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
            CargarNuevoEstado();

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
                Dialogos.MostrarError(
                    "FrmPoolTareas.CargarFiltroEstados",
                    "No se han podido cargar los estados.",
                    ex);
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
                Dialogos.MostrarError(
                    "FrmPoolTareas.CargarFiltroProyectos",
                    "No se han podido cargar los proyectos.",
                    ex);
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
                Dialogos.MostrarError(
                    "FrmPoolTareas.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
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
                Dialogos.MostrarError(
                    "FrmPoolTareas.CargarUsuarioAsignado",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de nuevo estado (usado para el cambio de
        /// estado en bloque) todos los estados existentes, sin la
        /// opción "Todos": aquí siempre hay que elegir un estado
        /// concreto al que mover las tareas seleccionadas. No hay
        /// restricción de transición: se permite pasar de cualquier
        /// estado a cualquier otro.
        /// </summary>
        private void CargarNuevoEstado()
        {
            try
            {
                List<EstadoFactura> estados = estadoRepositorio.ObtenerTodos();

                cboNuevoEstado.DataSource = estados;
                cboNuevoEstado.DisplayMember = "Estado";
                cboNuevoEstado.ValueMember = "IdEstado";

                if (estados.Count > 0)
                {
                    cboNuevoEstado.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.CargarNuevoEstado",
                    "No se han podido cargar los estados.",
                    ex);
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

                FormateadorGridFacturas.AplicarFormato(dgvTareas);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.BuscarTareas",
                    "No se han podido buscar las tareas.",
                    ex);
            }
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
                Dialogos.MostrarError(
                    "FrmPoolTareas.dgvTareas_SelectionChanged",
                    "No se ha podido cargar el detalle de la tarea.",
                    ex);
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
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Asignar tarea");

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

            if (!Dialogos.Confirmar(mensaje, "Confirmar asignación"))
                return;

            try
            {
                facturaRepositorio.AsignarUsuario(registros, idUsuario);

                Dialogos.MostrarInformacion(
                    "Asignación realizada correctamente.",
                    "Asignar tarea");

                BuscarTareas();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.btnAsignarTarea_Click",
                    "No se ha podido asignar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Cambia de una sola vez el estado del combo "Nuevo estado"
        /// a todas las tareas seleccionadas en la rejilla, previa
        /// confirmación. No hay restricción de transición: se admite
        /// cualquier estado de origen y destino.
        /// </summary>
        private void btnCambiarEstado_Click(object sender, EventArgs e)
        {
            List<int> registros = ObtenerRegistrosSeleccionados();

            if (registros.Count == 0)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Cambiar estado");

                return;
            }

            if (cboNuevoEstado.SelectedValue == null)
                return;

            int idEstado = Convert.ToInt32(cboNuevoEstado.SelectedValue);

            string mensaje = "¿Cambiar el estado de " + registros.Count + " tarea(s) a '" +
                cboNuevoEstado.Text + "'?";

            if (!Dialogos.Confirmar(mensaje, "Confirmar cambio de estado"))
                return;

            try
            {
                facturaRepositorio.CambiarEstado(registros, idEstado);

                Dialogos.MostrarInformacion(
                    "Estado actualizado correctamente.",
                    "Cambiar estado");

                BuscarTareas();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.btnCambiarEstado_Click",
                    "No se ha podido cambiar el estado.",
                    ex);
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
                Dialogos.MostrarInformacion("Debe seleccionar una tarea.", "Abrir tarea");

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
