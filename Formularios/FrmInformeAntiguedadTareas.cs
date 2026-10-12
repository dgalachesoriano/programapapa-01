using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Informe "Antigüedad de Tareas Abiertas" (menú Informes): todas
    /// las tareas actualmente Registradas, En Proceso o Bloqueadas
    /// (es decir, todo lo que no es Facturado ni Cancelado), con los
    /// días transcurridos desde su último cambio de estado, coloreadas
    /// por tramo de antigüedad para detectar de un vistazo las tareas
    /// "olvidadas". Es interactivo: las tarjetas KPI de cada tramo
    /// filtran la rejilla al pulsarlas.
    /// </summary>
    public partial class FrmInformeAntiguedadTareas : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();

        private const int IdTodos = 0;

        // Límites (en días) de cada tramo de antigüedad.
        private const int LimiteTramo1 = 7;
        private const int LimiteTramo2 = 15;
        private const int LimiteTramo3 = 30;

        private static readonly Color ColorTramo1 = Color.FromArgb(200, 230, 201); // verde
        private static readonly Color ColorTramo2 = Color.FromArgb(255, 245, 157); // amarillo
        private static readonly Color ColorTramo3 = Color.FromArgb(255, 204, 128); // naranja
        private static readonly Color ColorTramo4 = Color.FromArgb(255, 205, 210); // rojo

        private ComboBox cboFiltroUsuario;
        private FlowLayoutPanel flpKpis;
        private DataGridView dgvTareas;
        private Label lblFiltroActivo;
        private Button btnQuitarFiltro;

        private DataTable tablaTareas;
        private int? tramoSeleccionado;

        public FrmInformeAntiguedadTareas()
        {
            InitializeComponent();

            ConstruirInterfaz();
            CargarFiltroUsuarios();

            Generar();
        }

        private void ConstruirInterfaz()
        {
            this.BackColor = EstiloDashboard.ColorCanvas;

            Panel pnlCanvas = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = EstiloDashboard.ColorCanvas,
                Padding = new Padding(15)
            };

            GroupBox grpFiltros = ConstruirPanelFiltros();
            Panel pnlKpis = ConstruirPanelKpis();
            GroupBox grpGrid = ConstruirPanelGrid();

            pnlCanvas.Controls.Add(grpGrid);
            pnlCanvas.Controls.Add(pnlKpis);
            pnlCanvas.Controls.Add(grpFiltros);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelFiltros()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Top, Height = 70, Text = "Filtros" };

            Label lblUsuario = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Usuario:" };
            cboFiltroUsuario = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(90, 30),
                Size = new Size(220, 21)
            };

            Button btnAplicar = new Button { Text = "Aplicar filtros", Location = new Point(330, 27), Size = new Size(120, 30) };
            btnAplicar.Click += (s, e) => Generar();

            Button btnLimpiar = new Button { Text = "Limpiar", Location = new Point(460, 27), Size = new Size(100, 30) };
            btnLimpiar.Click += (s, e) => { cboFiltroUsuario.SelectedIndex = 0; Generar(); };

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(1000, 27),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

            grupo.Controls.Add(lblUsuario);
            grupo.Controls.Add(cboFiltroUsuario);
            grupo.Controls.Add(btnAplicar);
            grupo.Controls.Add(btnLimpiar);
            grupo.Controls.Add(btnCerrar);

            return grupo;
        }

        private Panel ConstruirPanelKpis()
        {
            Panel pnl = new Panel { Dock = DockStyle.Top, Height = 110, Padding = new Padding(0, 15, 0, 0) };

            flpKpis = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                AutoScroll = true
            };

            pnl.Controls.Add(flpKpis);

            return pnl;
        }

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Tareas abiertas" };

            Panel pnlCabeceraGrid = new Panel { Dock = DockStyle.Top, Height = 26 };

            lblFiltroActivo = new Label
            {
                AutoSize = true,
                Location = new Point(0, 5),
                Font = new Font(this.Font, FontStyle.Italic),
                ForeColor = Color.Gray
            };

            btnQuitarFiltro = new Button
            {
                Text = "Quitar filtro",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(880, 0),
                Size = new Size(110, 24),
                Visible = false
            };
            btnQuitarFiltro.Click += (s, e) => FiltrarPorTramo(null);

            pnlCabeceraGrid.Controls.Add(lblFiltroActivo);
            pnlCabeceraGrid.Controls.Add(btnQuitarFiltro);

            dgvTareas = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };

            EstiloDashboard.EstilizarGrid(dgvTareas);

            dgvTareas.DataBindingComplete += dgvTareas_DataBindingComplete;
            dgvTareas.CellDoubleClick += dgvTareas_CellDoubleClick;

            grupo.Controls.Add(dgvTareas);
            grupo.Controls.Add(pnlCabeceraGrid);

            return grupo;
        }

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
                    "FrmInformeAntiguedadTareas.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        private void Generar()
        {
            try
            {
                int idUsuario = cboFiltroUsuario.SelectedValue != null
                    ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                    : IdTodos;

                tablaTareas = informeRepositorio.ObtenerAntiguedadTareasAbiertas(idUsuario);

                PintarKpis();

                tramoSeleccionado = null;
                FiltrarPorTramo(null);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeAntiguedadTareas.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis()
        {
            flpKpis.Controls.Clear();

            int[] contadores = new int[4];

            foreach (DataRow fila in tablaTareas.Rows)
            {
                contadores[TramoDe(Convert.ToInt32(fila["DIAS_SIN_MOVIMIENTO"]))]++;
            }

            AgregarTarjetaClicable(
                "0-" + LimiteTramo1 + " DÍAS", contadores[0].ToString(), ColorTramo1, 0);

            AgregarTarjetaClicable(
                (LimiteTramo1 + 1) + "-" + LimiteTramo2 + " DÍAS", contadores[1].ToString(), ColorTramo2, 1);

            AgregarTarjetaClicable(
                (LimiteTramo2 + 1) + "-" + LimiteTramo3 + " DÍAS", contadores[2].ToString(), ColorTramo3, 2);

            AgregarTarjetaClicable(
                "MÁS DE " + LimiteTramo3 + " DÍAS", contadores[3].ToString(), ColorTramo4, 3);
        }

        /// <summary>
        /// Crea una tarjeta KPI de un tramo de antigüedad y la deja
        /// clicable para filtrar la rejilla a ese tramo al pulsarla.
        /// </summary>
        private void AgregarTarjetaClicable(string titulo, string valor, Color color, int tramo)
        {
            Panel tarjeta = EstiloDashboard.CrearTarjetaKpi(this.Font, titulo, valor, color);

            EstiloDashboard.HacerClicable(tarjeta, (s, e) => FiltrarPorTramo(tramo));

            flpKpis.Controls.Add(tarjeta);
        }

        /// <summary>
        /// Clasifica un número de días en uno de los cuatro tramos
        /// (0=0-7, 1=8-15, 2=16-30, 3=+30).
        /// </summary>
        private int TramoDe(int dias)
        {
            if (dias <= LimiteTramo1) return 0;
            if (dias <= LimiteTramo2) return 1;
            if (dias <= LimiteTramo3) return 2;
            return 3;
        }

        private Color ColorDeTramo(int tramo)
        {
            switch (tramo)
            {
                case 0: return ColorTramo1;
                case 1: return ColorTramo2;
                case 2: return ColorTramo3;
                default: return ColorTramo4;
            }
        }

        /// <summary>
        /// Filtra la rejilla a un tramo de antigüedad concreto (0-3),
        /// o quita el filtro si es null. Invocado tanto al pulsar una
        /// tarjeta KPI como al pulsar "Quitar filtro".
        /// </summary>
        private void FiltrarPorTramo(int? tramo)
        {
            if (tablaTareas == null)
                return;

            tramoSeleccionado = tramo;

            dgvTareas.DataSource = tablaTareas.DefaultView;

            if (!tramo.HasValue)
            {
                tablaTareas.DefaultView.RowFilter = string.Empty;
                lblFiltroActivo.Text = string.Empty;
                btnQuitarFiltro.Visible = false;
            }
            else
            {
                string filtro;

                switch (tramo.Value)
                {
                    case 0: filtro = "DIAS_SIN_MOVIMIENTO <= " + LimiteTramo1; break;
                    case 1: filtro = "DIAS_SIN_MOVIMIENTO > " + LimiteTramo1 + " AND DIAS_SIN_MOVIMIENTO <= " + LimiteTramo2; break;
                    case 2: filtro = "DIAS_SIN_MOVIMIENTO > " + LimiteTramo2 + " AND DIAS_SIN_MOVIMIENTO <= " + LimiteTramo3; break;
                    default: filtro = "DIAS_SIN_MOVIMIENTO > " + LimiteTramo3; break;
                }

                tablaTareas.DefaultView.RowFilter = filtro;
                lblFiltroActivo.Text = "Filtrado por tramo de antigüedad";
                btnQuitarFiltro.Visible = true;
            }
        }

        /// <summary>
        /// Da formato a la rejilla y colorea cada fila según su tramo
        /// de antigüedad (no según su estado, a diferencia del Pool
        /// de Tareas: aquí lo relevante es cuánto lleva parada).
        /// </summary>
        private void dgvTareas_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvTareas.Columns.Count == 0)
                return;

            dgvTareas.Columns["ID_TAREA"].Visible = false;
            dgvTareas.Columns["DES_DOC"].HeaderText = "Documento";
            dgvTareas.Columns["DES_PROYECTO"].HeaderText = "Proyecto";
            dgvTareas.Columns["DES_SEGMENTO"].HeaderText = "Segmento";
            dgvTareas.Columns["NOMBRE_USUARIO"].HeaderText = "Usuario";
            dgvTareas.Columns["DES_ESTADO"].HeaderText = "Estado";
            dgvTareas.Columns["IMP_ESTIMADO"].HeaderText = "Importe Estimado";
            dgvTareas.Columns["DIAS_SIN_MOVIMIENTO"].HeaderText = "Días sin movimiento";

            dgvTareas.Columns["IMP_ESTIMADO"].DefaultCellStyle.Format = "N2";
            dgvTareas.Columns["IMP_ESTIMADO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvTareas.Columns["DIAS_SIN_MOVIMIENTO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            foreach (DataGridViewRow fila in dgvTareas.Rows)
            {
                if (fila.Cells["DIAS_SIN_MOVIMIENTO"].Value == null)
                    continue;

                int dias = Convert.ToInt32(fila.Cells["DIAS_SIN_MOVIMIENTO"].Value);

                fila.DefaultCellStyle.BackColor = ColorDeTramo(TramoDe(dias));
            }
        }

        private void dgvTareas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            object valor = dgvTareas.Rows[e.RowIndex].Cells["ID_TAREA"].Value;

            if (valor == null)
                return;

            using (FrmDetalleTarea formulario = new FrmDetalleTarea(Convert.ToInt32(valor)))
            {
                formulario.ShowDialog();
            }

            Generar();
        }
    }
}
