using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Informe "Motivos de Bloqueo" (menú Informes): de las tareas
    /// actualmente bloqueadas, cuántas y qué importe hay por cada
    /// motivo (gráfico tipo Pareto), con la rejilla de tareas
    /// bloqueadas debajo. Es interactivo: al hacer clic sobre una
    /// barra, la rejilla se filtra a las tareas de ese motivo.
    /// </summary>
    public partial class FrmInformeMotivosBloqueo : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");
        private const int IdTodos = 0;

        private ComboBox cboFiltroUsuario;
        private DateTimePicker dtpFechaDesde;
        private DateTimePicker dtpFechaHasta;
        private FlowLayoutPanel flpKpis;
        private Chart chartMotivos;
        private DataGridView dgvTareas;
        private Label lblFiltroActivo;
        private Button btnQuitarFiltro;

        private DataTable tablaTareasBloqueadas;
        private string motivoSeleccionado;

        public FrmInformeMotivosBloqueo()
        {
            InitializeComponent();

            ConstruirInterfaz();
            CargarFiltroUsuarios();

            Generar();
        }

        /// <summary>
        /// Construye toda la interfaz por código: lienzo de fondo,
        /// panel de filtros, tarjetas KPI, gráfica y rejilla de
        /// detalle.
        /// </summary>
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
            GroupBox grpGrafica = ConstruirPanelGrafica();
            GroupBox grpGrid = ConstruirPanelGrid();

            // Orden de adición: los últimos Dock=Top añadidos quedan
            // más arriba visualmente, así que se añaden en orden
            // inverso al que se quiere ver (grid -> gráfica -> kpis ->
            // filtros), y el grid, al ser Dock=Fill, absorbe el resto
            // del espacio.
            pnlCanvas.Controls.Add(grpGrid);
            pnlCanvas.Controls.Add(grpGrafica);
            pnlCanvas.Controls.Add(pnlKpis);
            pnlCanvas.Controls.Add(grpFiltros);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelFiltros()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Top,
                Height = 90,
                Text = "Filtros"
            };

            Label lblUsuario = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Usuario:" };
            cboFiltroUsuario = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(90, 30),
                Size = new Size(180, 21)
            };

            Label lblDesde = new Label { AutoSize = true, Location = new Point(290, 33), Text = "Desde:" };
            dtpFechaDesde = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false,
                Location = new Point(350, 30),
                Size = new Size(130, 21)
            };

            Label lblHasta = new Label { AutoSize = true, Location = new Point(500, 33), Text = "Hasta:" };
            dtpFechaHasta = new DateTimePicker
            {
                Format = DateTimePickerFormat.Short,
                ShowCheckBox = true,
                Checked = false,
                Location = new Point(560, 30),
                Size = new Size(130, 21)
            };

            Button btnAplicar = new Button
            {
                Text = "Aplicar filtros",
                Location = new Point(730, 27),
                Size = new Size(120, 30)
            };
            btnAplicar.Click += (s, e) => Generar();

            Button btnLimpiar = new Button
            {
                Text = "Limpiar",
                Location = new Point(860, 27),
                Size = new Size(100, 30)
            };
            btnLimpiar.Click += (s, e) =>
            {
                cboFiltroUsuario.SelectedIndex = 0;
                dtpFechaDesde.Checked = false;
                dtpFechaHasta.Checked = false;
                Generar();
            };

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
            grupo.Controls.Add(lblDesde);
            grupo.Controls.Add(dtpFechaDesde);
            grupo.Controls.Add(lblHasta);
            grupo.Controls.Add(dtpFechaHasta);
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

        private GroupBox ConstruirPanelGrafica()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Top,
                Height = 300,
                Padding = new Padding(12),
                Text = "Tareas bloqueadas por motivo (haz clic en una barra para filtrar la lista)"
            };

            chartMotivos = new Chart { Dock = DockStyle.Fill };
            chartMotivos.MouseClick += chartMotivos_MouseClick;

            grupo.Controls.Add(chartMotivos);

            return grupo;
        }

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                Text = "Tareas bloqueadas"
            };

            Panel pnlCabeceraGrid = new Panel { Dock = DockStyle.Top, Height = 26 };

            lblFiltroActivo = new Label
            {
                AutoSize = true,
                Location = new Point(0, 5),
                Font = new Font(this.Font, FontStyle.Italic),
                ForeColor = Color.Gray,
                Text = ""
            };

            btnQuitarFiltro = new Button
            {
                Text = "Quitar filtro",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(880, 0),
                Size = new Size(110, 24),
                Visible = false
            };
            btnQuitarFiltro.Click += (s, e) => FiltrarGridPorMotivo(null);

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

            dgvTareas.DataBindingComplete += (s, e) => FormateadorGridTareas.AplicarFormato(dgvTareas);
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
                    "FrmInformeMotivosBloqueo.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>
        /// Vuelve a consultar y a pintar tarjetas, gráfica y rejilla
        /// con los filtros actuales de pantalla.
        /// </summary>
        private void Generar()
        {
            try
            {
                DateTime? fechaDesde = dtpFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value.Date : null;
                DateTime? fechaHasta = dtpFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value.Date : null;

                int idUsuario = cboFiltroUsuario.SelectedValue != null
                    ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                    : IdTodos;

                DataTable resumen = informeRepositorio.ObtenerMotivosBloqueo(fechaDesde, fechaHasta, idUsuario);

                PintarKpis(resumen);
                PintarGrafica(resumen);

                tablaTareasBloqueadas = tareaRepositorio.BuscarPorFiltro(new FiltroBusquedaTareas
                {
                    IdEstado = EstadosTareaConocidos.Pendiente,
                    IdUsuario = idUsuario,
                    FechaDesde = fechaDesde,
                    FechaHasta = fechaHasta
                });

                motivoSeleccionado = null;
                FiltrarGridPorMotivo(null);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeMotivosBloqueo.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis(DataTable resumen)
        {
            flpKpis.Controls.Clear();

            int totalTareas = 0;
            decimal totalImporte = 0m;

            foreach (DataRow fila in resumen.Rows)
            {
                totalTareas += Convert.ToInt32(fila["NUM_TAREAS"]);
                totalImporte += Convert.ToDecimal(fila["IMPORTE"]);
            }

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "TAREAS BLOQUEADAS", totalTareas.ToString(CulturaDecimal),
                EstiloDashboard.Paleta[2]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "IMPORTE BLOQUEADO", totalImporte.ToString("N2", CulturaDecimal),
                EstiloDashboard.Paleta[5]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "MOTIVOS DISTINTOS", resumen.Rows.Count.ToString(CulturaDecimal),
                EstiloDashboard.Paleta[4]));
        }

        private void PintarGrafica(DataTable resumen)
        {
            EstiloDashboard.PrepararGrafica(chartMotivos);

            Series serie = new Series("Motivos")
            {
                ChartType = SeriesChartType.Column,
                ChartArea = "AreaPrincipal",
                IsValueShownAsLabel = true,
                Color = EstiloDashboard.Paleta[2]
            };

            chartMotivos.Legends.Clear();

            foreach (DataRow fila in resumen.Rows)
            {
                serie.Points.AddXY(fila["DES_MOTIVO"].ToString(), Convert.ToInt32(fila["NUM_TAREAS"]));
            }

            chartMotivos.Series.Add(serie);
        }

        /// <summary>
        /// Filtra la rejilla de tareas bloqueadas al motivo indicado,
        /// o quita el filtro si es null.
        /// </summary>
        private void FiltrarGridPorMotivo(string motivo)
        {
            if (tablaTareasBloqueadas == null)
                return;

            motivoSeleccionado = motivo;

            dgvTareas.DataSource = tablaTareasBloqueadas.DefaultView;

            if (string.IsNullOrEmpty(motivo))
            {
                tablaTareasBloqueadas.DefaultView.RowFilter = string.Empty;
                lblFiltroActivo.Text = string.Empty;
                btnQuitarFiltro.Visible = false;
            }
            else
            {
                tablaTareasBloqueadas.DefaultView.RowFilter =
                    "DES_MOTIVO = '" + motivo.Replace("'", "''") + "'";

                lblFiltroActivo.Text = "Filtrado por motivo: " + motivo;
                btnQuitarFiltro.Visible = true;
            }
        }

        private void chartMotivos_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult resultado = chartMotivos.HitTest(e.X, e.Y);

            if (resultado.ChartElementType != ChartElementType.DataPoint ||
                resultado.Series == null || resultado.PointIndex < 0)
            {
                return;
            }

            string motivo = resultado.Series.Points[resultado.PointIndex].AxisLabel;

            FiltrarGridPorMotivo(motivo);
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
