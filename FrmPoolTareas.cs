using GestionFacturas.Datos;
using GestionFacturas.Formularios;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace GestionFacturas
{
    /// <summary>
    /// Pantalla del "pool de tareas": permite consultar las tareas
    /// filtrando por estado, usuario asignado y rango de fechas de
    /// registro, ver el detalle de cada una, asignar (paso 2 del
    /// flujo: pasa a estado "En Proceso") de una sola vez el usuario
    /// responsable de una o varias tareas seleccionadas, y bloquear
    /// tareas (pasa a estado "Bloqueado") indicando un motivo
    /// obligatorio de una lista.
    /// </summary>
    public partial class FrmPoolTareas : Form
    {
        private readonly EstadoTareaRepositorio estadoRepositorio = new EstadoTareaRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();

        /// <summary>
        /// Valor usado en los combos de filtro para representar "no
        /// filtrar por este campo" (Estado/Usuario).
        /// </summary>
        private const int IdTodos = 0;

        /// <summary>
        /// Colores pastel de fondo para las filas de la rejilla de
        /// tareas, según su estado (identificado por ID, ver
        /// Datos/EstadosTareaConocidos.cs).
        /// </summary>
        private static readonly Color ColorRegistrado = Color.FromArgb(224, 224, 224);
        private static readonly Color ColorEnProceso = Color.FromArgb(187, 222, 251);
        private static readonly Color ColorPendiente = Color.FromArgb(255, 245, 157);
        private static readonly Color ColorFacturado = Color.FromArgb(200, 230, 201);
        private static readonly Color ColorCancelado = Color.FromArgb(255, 205, 210);

        public FrmPoolTareas()
        {
            InitializeComponent();

            ConfigurarGrids();
            CargarFiltroEstados();
            CargarFiltroUsuarios();
            CargarUsuarioAsignado();

            BuscarTareas();
        }

        /// <summary>
        /// Configura el comportamiento general de las dos rejillas:
        /// tareas (selección múltiple, para poder asignar o bloquear
        /// varias de golpe) y detalle (solo lectura, siempre de una
        /// tarea).
        /// </summary>
        private void ConfigurarGrids()
        {
            dgvTareas.AutoGenerateColumns = true;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // El formato de columnas y el coloreado por estado se
            // aplican aquí, tras que la rejilla termine de enlazar
            // los datos (evento DataBindingComplete), en vez de justo
            // después de asignar DataSource: si se hace justo después
            // y la rejilla todavía no tiene ventana creada (p. ej. la
            // primera búsqueda, lanzada desde el propio constructor
            // antes de mostrar el formulario), sus filas aún no
            // existen de verdad y no hay nada que colorear.
            dgvTareas.DataBindingComplete += dgvTareas_DataBindingComplete;

            dgvDetalle.AutoGenerateColumns = true;
            dgvDetalle.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Una vez la rejilla de tareas ha terminado de enlazar sus
        /// datos, aplica el formato de columnas y el coloreado por
        /// estado. Se dispara tanto en la carga inicial como en cada
        /// búsqueda posterior.
        /// </summary>
        private void dgvTareas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            FormateadorGridTareas.AplicarFormato(dgvTareas);

            ColorearFilasPorEstado();

            AplicarEstiloSeleccion();
        }

        /// <summary>
        /// Carga en el combo de filtro de estados todos los estados
        /// activos más la opción "Todos" (Id = 0).
        /// </summary>
        private void CargarFiltroEstados()
        {
            try
            {
                List<EstadoTarea> estados = estadoRepositorio.ObtenerActivos();

                estados.Insert(0, new EstadoTarea { Id = IdTodos, Descripcion = "Todos" });

                cboFiltroEstado.DataSource = estados;
                cboFiltroEstado.DisplayMember = "Descripcion";
                cboFiltroEstado.ValueMember = "Id";
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
                    "FrmPoolTareas.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>
        /// Carga en el combo de usuario destino de la asignación los
        /// usuarios activos. A diferencia del filtro, aquí no hay
        /// opción "Todos"/"Sin asignar": asignar una tarea siempre
        /// implica elegir un usuario concreto.
        /// </summary>
        private void CargarUsuarioAsignado()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                // "Sin asignar" no es aquí una acción real (asignar
                // exige elegir un usuario concreto, ver
                // btnAsignarTarea_Click), sino el valor por defecto
                // del combo: lo que se muestra cuando no hay ninguna
                // tarea seleccionada, o cuando las seleccionadas no
                // comparten un único usuario asignado (ver
                // ActualizarUsuarioAsignadoSegunSeleccion).
                usuarios.Insert(0, new Usuario { Id = IdTodos, Nombre = "Sin asignar" });

                cboUsuarioAsignado.DataSource = usuarios;
                cboUsuarioAsignado.DisplayMember = "Nombre";
                cboUsuarioAsignado.ValueMember = "Id";
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
        /// Construye el filtro a partir de los controles de pantalla,
        /// busca las tareas correspondientes y las muestra en la
        /// rejilla de tareas.
        /// </summary>
        private void BuscarTareas()
        {
            try
            {
                FiltroBusquedaTareas filtro = new FiltroBusquedaTareas
                {
                    IdEstado = cboFiltroEstado.SelectedValue != null
                        ? Convert.ToInt32(cboFiltroEstado.SelectedValue)
                        : IdTodos,

                    IdUsuario = cboFiltroUsuario.SelectedValue != null
                        ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                        : IdTodos,

                    FechaDesde = dtpFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value.Date : null,
                    FechaHasta = dtpFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value.Date : null
                };

                DataTable resultado = tareaRepositorio.BuscarPorFiltro(filtro);

                // El formato de columnas y el coloreado por estado se
                // aplican en el evento DataBindingComplete de
                // dgvTareas (ver ConfigurarGrids), no aquí.
                dgvTareas.DataSource = resultado;
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
        /// Colorea el fondo de cada fila de la rejilla de tareas
        /// según su estado actual (Registrado en gris, En Proceso en
        /// azul, Pendiente/bloqueado en rojo, Facturado en verde,
        /// Cancelado en naranja, todos en tonos pastel), para poder
        /// distinguirlas de un vistazo. Identifica el estado por su
        /// ID (columna oculta COD_SEQ_EST), no por su texto: ver
        /// Datos/EstadosTareaConocidos.cs.
        /// </summary>
        private void ColorearFilasPorEstado()
        {
            if (dgvTareas.Columns.Count == 0)
                return;

            foreach (DataGridViewRow fila in dgvTareas.Rows)
            {
                if (fila.Cells["COD_SEQ_EST"].Value == null)
                    continue;

                int idEstado = Convert.ToInt32(fila.Cells["COD_SEQ_EST"].Value);

                fila.DefaultCellStyle.BackColor = ColorDeEstado(idEstado);
            }
        }

        /// <summary>
        /// Traduce el ID de un estado al color pastel que le
        /// corresponde en la rejilla. Devuelve el color de fondo por
        /// defecto de la rejilla si no es ninguno de los conocidos.
        /// </summary>
        private Color ColorDeEstado(int idEstado)
        {
            switch (idEstado)
            {
                case EstadosTareaConocidos.Registrado:
                    return ColorRegistrado;

                case EstadosTareaConocidos.EnProceso:
                    return ColorEnProceso;

                case EstadosTareaConocidos.Pendiente:
                    return ColorPendiente;

                case EstadosTareaConocidos.Facturado:
                    return ColorFacturado;

                case EstadosTareaConocidos.Cancelado:
                    return ColorCancelado;

                default:
                    return dgvTareas.DefaultCellStyle.BackColor;
            }
        }

        /// <summary>
        /// Ajusta el color de selección de cada fila según cuántas
        /// tareas haya seleccionadas: con una sola fila seleccionada,
        /// se resalta con una versión más oscura de su propio color
        /// de estado (en vez del azul de selección por defecto), para
        /// que se siga viendo a qué estado pertenece; con selección
        /// múltiple se mantiene el resaltado azul estándar, más fácil
        /// de distinguir cuando hay varias filas marcadas a la vez.
        /// </summary>
        private void AplicarEstiloSeleccion()
        {
            bool seleccionUnica = dgvTareas.SelectedRows.Count == 1;

            foreach (DataGridViewRow fila in dgvTareas.Rows)
            {
                if (seleccionUnica && fila.Selected)
                {
                    int idEstado = fila.Cells["COD_SEQ_EST"].Value == null
                        ? 0
                        : Convert.ToInt32(fila.Cells["COD_SEQ_EST"].Value);

                    fila.DefaultCellStyle.SelectionBackColor = Oscurecer(ColorDeEstado(idEstado), 0.75f);
                    fila.DefaultCellStyle.SelectionForeColor = Color.Black;
                }
                else
                {
                    fila.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                    fila.DefaultCellStyle.SelectionForeColor = SystemColors.HighlightText;
                }
            }
        }

        /// <summary>
        /// Oscurece un color multiplicando cada componente RGB por
        /// <paramref name="factor"/> (p. ej. 0.75 = un 25% más oscuro),
        /// conservando su tonalidad.
        /// </summary>
        private Color Oscurecer(Color color, float factor)
        {
            return Color.FromArgb(
                (int)(color.R * factor),
                (int)(color.G * factor),
                (int)(color.B * factor));
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

            dgvDetalle.Columns["DES_DOC"].HeaderText = "Documento";
            dgvDetalle.Columns["COD_PED_VENTA"].HeaderText = "Pedido de Venta";
            dgvDetalle.Columns["COD_PED_COMPRA"].HeaderText = "Pedido de Compra";
            dgvDetalle.Columns["COD_PED_INSPEC"].HeaderText = "Pedido de Inspección";
            dgvDetalle.Columns["COD_ENT_ENTR"].HeaderText = "Entidad de Entrega";
            dgvDetalle.Columns["NBR_UNIDADES"].HeaderText = "Unidades";
        }

        /// <summary>
        /// Al cambiar la selección de la rejilla de tareas, carga en
        /// la rejilla de detalle las líneas de TODAS las tareas
        /// seleccionadas (incluye la columna Documento para poder
        /// distinguir a cuál pertenece cada línea cuando hay más de
        /// una). Si no hay ninguna fila seleccionada, limpia el
        /// detalle.
        /// </summary>
        private void dgvTareas_SelectionChanged(object sender, EventArgs e)
        {
            AplicarEstiloSeleccion();

            ActualizarUsuarioAsignadoSegunSeleccion();

            List<int> idsSeleccionados = ObtenerIdsSeleccionados();

            if (idsSeleccionados.Count == 0)
            {
                dgvDetalle.DataSource = null;
                return;
            }

            try
            {
                DataTable detalles = tareaRepositorio.ObtenerDetalle(idsSeleccionados);

                dgvDetalle.DataSource = detalles;

                FormatearGridDetalle();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.dgvTareas_SelectionChanged",
                    "No se ha podido cargar el detalle de las tareas seleccionadas.",
                    ex);
            }
        }

        /// <summary>
        /// Preselecciona en el combo de asignación el usuario que
        /// corresponde a la selección actual de la rejilla de tareas:
        /// "Sin asignar" si no hay ninguna fila seleccionada, o si las
        /// seleccionadas no comparten todas el mismo usuario asignado
        /// (incluida ninguno); en caso contrario, ese usuario común
        /// (el de la primera tarea, que al ser todas iguales
        /// representa a cualquiera de ellas).
        /// </summary>
        private void ActualizarUsuarioAsignadoSegunSeleccion()
        {
            if (dgvTareas.SelectedRows.Count == 0)
            {
                cboUsuarioAsignado.SelectedValue = IdTodos;
                return;
            }

            int? primerUsuario = null;
            bool esLaPrimera = true;
            bool todasComparten = true;

            foreach (DataGridViewRow fila in dgvTareas.SelectedRows)
            {
                object valor = fila.Cells["COD_SEQ_USER"].Value;

                int? idUsuarioFila = (valor == null || valor == DBNull.Value)
                    ? (int?)null
                    : Convert.ToInt32(valor);

                if (esLaPrimera)
                {
                    primerUsuario = idUsuarioFila;
                    esLaPrimera = false;
                }
                else if (idUsuarioFila != primerUsuario)
                {
                    todasComparten = false;
                    break;
                }
            }

            cboUsuarioAsignado.SelectedValue =
                (todasComparten && primerUsuario.HasValue) ? primerUsuario.Value : IdTodos;
        }

        /// <summary>
        /// Obtiene, sin duplicados, los identificadores de las filas
        /// actualmente seleccionadas en la rejilla de tareas.
        /// </summary>
        private List<int> ObtenerIdsSeleccionados()
        {
            List<int> ids = new List<int>();
            HashSet<int> vistos = new HashSet<int>();

            foreach (DataGridViewRow fila in dgvTareas.SelectedRows)
            {
                if (fila.Cells["ID_TAREA"].Value == null)
                    continue;

                int id = Convert.ToInt32(fila.Cells["ID_TAREA"].Value);

                if (vistos.Add(id))
                {
                    ids.Add(id);
                }
            }

            return ids;
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
            cboFiltroUsuario.SelectedIndex = 0;
            dtpFechaDesde.Checked = false;
            dtpFechaHasta.Checked = false;

            BuscarTareas();
        }

        /// <summary>
        /// Vuelve a lanzar la búsqueda con los filtros actuales.
        /// </summary>
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            BuscarTareas();
        }

        /// <summary>
        /// Asigna de una sola vez el usuario del combo de asignación
        /// a todas las tareas seleccionadas en la rejilla (pasan a
        /// estado "En Proceso"), previa confirmación.
        /// </summary>
        private void btnAsignarTarea_Click(object sender, EventArgs e)
        {
            List<int> ids = ObtenerIdsSeleccionados();

            if (ids.Count == 0)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Asignar tarea");

                return;
            }

            if (cboUsuarioAsignado.SelectedValue == null)
                return;

            int idUsuario = Convert.ToInt32(cboUsuarioAsignado.SelectedValue);

            if (idUsuario == IdTodos)
            {
                Dialogos.MostrarAviso(
                    "Debe seleccionar un usuario concreto para asignar la tarea.",
                    "Asignar tarea");

                return;
            }

            string mensaje = "¿Asignar " + ids.Count + " tarea(s) a '" + cboUsuarioAsignado.Text + "'?";

            if (!Dialogos.Confirmar(mensaje, "Confirmar asignación"))
                return;

            try
            {
                tareaRepositorio.AsignarUsuario(ids, idUsuario);

                Dialogos.MostrarInformacion("Asignación realizada correctamente.", "Asignar tarea");

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
        /// Bloquea de una sola vez todas las tareas seleccionadas en
        /// la rejilla (pasan a estado "Bloqueado"), pidiendo primero
        /// un motivo obligatorio de una lista desplegable.
        /// </summary>
        private void btnBloquearTarea_Click(object sender, EventArgs e)
        {
            List<int> ids = ObtenerIdsSeleccionados();

            if (ids.Count == 0)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Bloquear tarea");

                return;
            }

            int idMotivo;

            using (FrmSeleccionarMotivo formulario = new FrmSeleccionarMotivo())
            {
                if (formulario.ShowDialog() != DialogResult.OK)
                    return;

                idMotivo = formulario.IdMotivoSeleccionado;
            }

            if (!Dialogos.Confirmar(
                "¿Bloquear " + ids.Count + " tarea(s)?",
                "Confirmar bloqueo"))
            {
                return;
            }

            try
            {
                tareaRepositorio.Bloquear(ids, idMotivo);

                Dialogos.MostrarInformacion("Bloqueo realizado correctamente.", "Bloquear tarea");

                BuscarTareas();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.btnBloquearTarea_Click",
                    "No se ha podido bloquear la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Desbloquea de una sola vez todas las tareas seleccionadas
        /// en la rejilla, devolviendo cada una al estado en que
        /// estaba justo antes de bloquearse, previa confirmación.
        /// Exige que todas las tareas seleccionadas estén actualmente
        /// en estado Bloqueado.
        /// </summary>
        private void btnDesbloquearTarea_Click(object sender, EventArgs e)
        {
            List<int> ids = ObtenerIdsSeleccionados();

            if (ids.Count == 0)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Desbloquear tarea");

                return;
            }

            if (!TodasLasSeleccionadasEnEstado(EstadosTareaConocidos.Pendiente))
            {
                Dialogos.MostrarAviso(
                    "Solo se pueden desbloquear tareas que estén actualmente en estado Pendiente (bloqueadas).",
                    "Desbloquear tarea");

                return;
            }

            if (!Dialogos.Confirmar(
                "¿Desbloquear " + ids.Count + " tarea(s) y devolverla(s) a su estado anterior?",
                "Confirmar desbloqueo"))
            {
                return;
            }

            try
            {
                tareaRepositorio.Desbloquear(ids);

                Dialogos.MostrarInformacion("Desbloqueo realizado correctamente.", "Desbloquear tarea");

                BuscarTareas();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.btnDesbloquearTarea_Click",
                    "No se ha podido desbloquear la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Cancela de una sola vez todas las tareas seleccionadas en
        /// la rejilla (pasan a estado "Cancelado"), previa
        /// confirmación.
        /// </summary>
        private void btnCancelarTarea_Click(object sender, EventArgs e)
        {
            List<int> ids = ObtenerIdsSeleccionados();

            if (ids.Count == 0)
            {
                Dialogos.MostrarInformacion(
                    "Seleccione al menos una tarea de la lista.",
                    "Cancelar tarea");

                return;
            }

            if (!Dialogos.Confirmar(
                "¿Cancelar " + ids.Count + " tarea(s)?",
                "Confirmar cancelación"))
            {
                return;
            }

            try
            {
                tareaRepositorio.Cancelar(ids);

                Dialogos.MostrarInformacion("Cancelación realizada correctamente.", "Cancelar tarea");

                BuscarTareas();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPoolTareas.btnCancelarTarea_Click",
                    "No se ha podido cancelar la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Comprueba que todas las filas actualmente seleccionadas en
        /// la rejilla de tareas tengan el estado indicado (por ID: ver
        /// Datos/EstadosTareaConocidos.cs).
        /// </summary>
        private bool TodasLasSeleccionadasEnEstado(int idEstado)
        {
            foreach (DataGridViewRow fila in dgvTareas.SelectedRows)
            {
                if (fila.Cells["COD_SEQ_EST"].Value == null ||
                    Convert.ToInt32(fila.Cells["COD_SEQ_EST"].Value) != idEstado)
                {
                    return false;
                }
            }

            return true;
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

            int idTarea = Convert.ToInt32(dgvTareas.CurrentRow.Cells["ID_TAREA"].Value);

            using (FrmRegistrarTarea formulario = new FrmRegistrarTarea(idTarea))
            {
                formulario.ShowDialog();
            }

            BuscarTareas();
        }
    }
}
