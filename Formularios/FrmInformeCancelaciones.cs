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
    /// Informe "Cancelaciones" (menú Informes): tendencia mensual del
    /// número de tareas canceladas y de su Importe Estimado, con la
    /// rejilla de tareas canceladas debajo. Cancelar una tarea no pide
    /// motivo (a diferencia de bloquear), así que este informe se
    /// centra en el volumen y su evolución, no en el porqué.
    /// </summary>
    public partial class FrmInformeCancelaciones : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");
        private const int IdTodos = 0;

        private DateTimePicker dtpFechaDesde;
        private DateTimePicker dtpFechaHasta;
        private ComboBox cboFiltroProyecto;
        private FlowLayoutPanel flpKpis;
        private Chart chartCancelaciones;
        private DataGridView dgvTareas;

        public FrmInformeCancelaciones()
        {
            InitializeComponent();

            ConstruirInterfaz();
            CargarFiltroProyectos();

            dtpFechaDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpFechaHasta.Value = DateTime.Today;

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

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 260,
                SplitterWidth = 8
            };

            GroupBox grpGrafica = ConstruirPanelGrafica();
            GroupBox grpGrid = ConstruirPanelGrid();

            split.Panel1.Controls.Add(grpGrafica);
            split.Panel2.Controls.Add(grpGrid);

            pnlCanvas.Controls.Add(split);
            pnlCanvas.Controls.Add(pnlKpis);
            pnlCanvas.Controls.Add(grpFiltros);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelFiltros()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Top, Height = 70, Text = "Filtros" };

            Label lblProyecto = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Proyecto:" };
            cboFiltroProyecto = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(80, 30),
                Size = new Size(180, 21)
            };

            Label lblDesde = new Label { AutoSize = true, Location = new Point(280, 33), Text = "Desde:" };
            dtpFechaDesde = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(340, 30), Size = new Size(130, 21) };

            Label lblHasta = new Label { AutoSize = true, Location = new Point(490, 33), Text = "Hasta:" };
            dtpFechaHasta = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(550, 30), Size = new Size(130, 21) };

            Button btnAplicar = new Button { Text = "Aplicar filtros", Location = new Point(700, 27), Size = new Size(120, 30) };
            btnAplicar.Click += (s, e) => Generar();

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(1000, 27),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

            grupo.Controls.Add(lblProyecto);
            grupo.Controls.Add(cboFiltroProyecto);
            grupo.Controls.Add(lblDesde);
            grupo.Controls.Add(dtpFechaDesde);
            grupo.Controls.Add(lblHasta);
            grupo.Controls.Add(dtpFechaHasta);
            grupo.Controls.Add(btnAplicar);
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
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Cancelaciones por mes" };

            chartCancelaciones = new Chart { Dock = DockStyle.Fill };

            grupo.Controls.Add(chartCancelaciones);

            return grupo;
        }

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Tareas canceladas" };

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

            return grupo;
        }

        private void CargarFiltroProyectos()
        {
            try
            {
                List<Proyecto> proyectos = proyectoRepositorio.ObtenerActivos();

                proyectos.Insert(0, new Proyecto { Id = IdTodos, Descripcion = "Todos" });

                cboFiltroProyecto.DataSource = proyectos;
                cboFiltroProyecto.DisplayMember = "Descripcion";
                cboFiltroProyecto.ValueMember = "Id";
                cboFiltroProyecto.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeCancelaciones.CargarFiltroProyectos",
                    "No se han podido cargar los proyectos.",
                    ex);
            }
        }

        private void Generar()
        {
            try
            {
                DateTime? desde = dtpFechaDesde.Value.Date;
                DateTime? hasta = dtpFechaHasta.Value.Date;

                int idProyecto = cboFiltroProyecto.SelectedValue != null
                    ? Convert.ToInt32(cboFiltroProyecto.SelectedValue)
                    : IdTodos;

                DataTable mensual = informeRepositorio.ObtenerCancelacionesMensuales(desde, hasta, idProyecto);

                PintarKpis(mensual);
                PintarGrafica(mensual);

                dgvTareas.DataSource = tareaRepositorio.BuscarPorFiltro(new FiltroBusquedaTareas
                {
                    IdEstado = EstadosTareaConocidos.Cancelado,
                    IdProyecto = idProyecto,
                    FechaDesde = desde,
                    FechaHasta = hasta
                });
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeCancelaciones.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis(DataTable mensual)
        {
            flpKpis.Controls.Clear();

            int totalTareas = 0;
            decimal totalImporte = 0m;

            foreach (DataRow fila in mensual.Rows)
            {
                totalTareas += Convert.ToInt32(fila["NUM_TAREAS"]);
                totalImporte += Convert.ToDecimal(fila["IMPORTE"]);
            }

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "TAREAS CANCELADAS", totalTareas.ToString(CulturaDecimal), EstiloDashboard.Paleta[5]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "IMPORTE ESTIMADO CANCELADO", totalImporte.ToString("N2", CulturaDecimal), EstiloDashboard.Paleta[2]));
        }

        private void PintarGrafica(DataTable mensual)
        {
            EstiloDashboard.PrepararGrafica(chartCancelaciones);

            Series serie = new Series("Cancelaciones")
            {
                ChartType = SeriesChartType.Column,
                ChartArea = "AreaPrincipal",
                Color = EstiloDashboard.Paleta[5],
                IsValueShownAsLabel = true
            };

            chartCancelaciones.Legends.Clear();

            foreach (DataRow fila in mensual.Rows)
            {
                string etiqueta = FormatearPeriodo(fila["PERIODO"].ToString());

                serie.Points.AddXY(etiqueta, Convert.ToInt32(fila["NUM_TAREAS"]));
            }

            chartCancelaciones.Series.Add(serie);
        }

        private string FormatearPeriodo(string periodoIso)
        {
            DateTime fecha;

            bool valido = DateTime.TryParseExact(
                periodoIso, "yyyy-MM", CultureInfo.InvariantCulture, DateTimeStyles.None, out fecha);

            return valido ? fecha.ToString("MMM yyyy", CulturaDecimal) : periodoIso;
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
        }
    }
}
