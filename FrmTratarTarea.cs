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
    /// Pantalla de búsqueda y tratamiento de facturas/tareas.
    /// Permite filtrar por estado, usuario, documento y proyecto,
    /// y abrir una factura concreta para editarla.
    /// </summary>
    public partial class FrmTratarTarea : Form
    {
        private readonly EstadoFacturaRepositorio estadoRepositorio =
            new EstadoFacturaRepositorio();

        private readonly UsuarioRepositorio usuarioRepositorio =
            new UsuarioRepositorio();

        private readonly FacturaRepositorio facturaRepositorio =
            new FacturaRepositorio();

        /// <summary>
        /// Inicializa el formulario: prepara la rejilla de resultados,
        /// carga los combos de filtro y lanza una primera búsqueda
        /// sin filtros para mostrar todas las tareas.
        /// </summary>
        public FrmTratarTarea()
        {
            InitializeComponent();

            CargarEstados();
            CargarUsuarios();
            ConfigurarGrid();
            BuscarTareas();
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Enter del GroupBox de resultados;
        /// no requiere lógica adicional.
        /// </summary>
        private void grpResultados_Enter(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Configura el comportamiento y aspecto general de la
        /// rejilla de resultados (selección de fila completa, solo
        /// lectura, ajuste automático de columnas, etc.).
        /// </summary>
        private void ConfigurarGrid()
        {
            dgvTareas.AutoGenerateColumns = true;
            dgvTareas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTareas.MultiSelect = false;
            dgvTareas.ReadOnly = true;
            dgvTareas.AllowUserToAddRows = false;
            dgvTareas.AllowUserToDeleteRows = false;
            dgvTareas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        /// <summary>
        /// Carga en el combo de estados todos los estados existentes
        /// más la opción "Todos" (IdEstado = 0) para no filtrar por
        /// estado.
        /// </summary>
        private void CargarEstados()
        {
            try
            {
                List<EstadoFactura> estados = estadoRepositorio.ObtenerTodos();

                estados.Insert(0, new EstadoFactura
                {
                    IdEstado = 0,
                    Estado = "Todos"
                });

                cboEstado.DataSource = estados;
                cboEstado.DisplayMember = "Estado";
                cboEstado.ValueMember = "IdEstado";
                cboEstado.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                RegistradorErrores.Registrar("FrmTratarTarea.CargarEstados", ex);

                MessageBox.Show(
                    "No se han podido cargar los estados.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Carga en el combo de usuarios todos los usuarios activos
        /// más la opción "Todos" (IdUsuario = 0) para no filtrar por
        /// usuario asignado.
        /// </summary>
        private void CargarUsuarios()
        {
            try
            {
                List<Usuario> usuarios = usuarioRepositorio.ObtenerActivos();

                usuarios.Insert(0, new Usuario
                {
                    IdUsuario = 0,
                    Nombre = "Todos"
                });

                cboUsuario.DataSource = usuarios;
                cboUsuario.DisplayMember = "Nombre";
                cboUsuario.ValueMember = "IdUsuario";
                cboUsuario.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                RegistradorErrores.Registrar("FrmTratarTarea.CargarUsuarios", ex);

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
        /// rejilla de resultados.
        /// </summary>
        private void BuscarTareas()
        {
            try
            {
                FiltroBusquedaFacturas filtro = new FiltroBusquedaFacturas
                {
                    IdEstado = Convert.ToInt32(cboEstado.SelectedValue),
                    IdUsuario = Convert.ToInt32(cboUsuario.SelectedValue),
                    Documento = txtDocumento.Text.Trim(),
                    Proyecto = txtProyecto.Text.Trim()
                };

                DataTable resultado = facturaRepositorio.BuscarPorFiltro(filtro);

                dgvTareas.DataSource = resultado;

                FormatearGrid();
            }
            catch (Exception ex)
            {
                RegistradorErrores.Registrar("FrmTratarTarea.BuscarTareas", ex);

                MessageBox.Show(
                    "No se han podido buscar las tareas.\n\n" + ex.Message,
                    "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Da formato de presentación a las columnas de la rejilla de
        /// resultados: cabeceras legibles y formatos de fecha/importe.
        /// No hace nada si la rejilla aún no tiene columnas (p. ej.
        /// una búsqueda sin resultados).
        /// </summary>
        private void FormatearGrid()
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
        /// Relanza la búsqueda aplicando los filtros actuales de
        /// pantalla.
        /// </summary>
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            BuscarTareas();
        }

        /// <summary>
        /// Cierra la pantalla de tratamiento de tareas.
        /// </summary>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Click de la etiqueta del filtro de
        /// proyecto; no requiere lógica adicional.
        /// </summary>
        private void label1_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento TextChanged del filtro de proyecto;
        /// no requiere lógica adicional (la búsqueda se dispara con
        /// el botón Buscar).
        /// </summary>
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Abre en modo edición la factura seleccionada en la
        /// rejilla de resultados y, al cerrarse ese formulario,
        /// refresca la búsqueda para reflejar los cambios.
        /// </summary>
        private void btnAbrir_Click(object sender, EventArgs e)
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

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Load del formulario; toda la carga
        /// inicial se realiza en el constructor.
        /// </summary>
        private void FrmTratarTarea_Load(object sender, EventArgs e)
        {
        }
    }
}
