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
    /// Informe "Facturación por Proyecto/Segmento" (menú Informes):
    /// importe facturado agrupado por proyecto o por segmento (a
    /// elegir con un combo), desglosado por divisa. Es interactivo: al
    /// hacer clic sobre una barra, se abre debajo la lista de tareas
    /// facturadas de ese grupo.
    /// </summary>
    public partial class FrmInformeFacturacionProyecto : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();
        private readonly SegmentoRepositorio segmentoRepositorio = new SegmentoRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        private const string AgruparPorProyecto = "Proyecto";
        private const string AgruparPorSegmento = "Segmento";

        private DateTimePicker dtpFechaDesde;
        private DateTimePicker dtpFechaHasta;
        private ComboBox cboAgruparPor;
        private FlowLayoutPanel flpKpis;
        private Chart chartFacturacion;
        private DataGridView dgvDetalle;
        private Label lblFiltroActivo;

        private readonly Dictionary<string, Color> coloresPorDivisa = new Dictionary<string, Color>();
        private List<Proyecto> proyectos;
        private List<Segmento> segmentos;

        public FrmInformeFacturacionProyecto()
        {
            InitializeComponent();

            ConstruirInterfaz();
            CargarCatalogos();

            dtpFechaDesde.Value = new DateTime(DateTime.Today.Year, 1, 1);
            dtpFechaHasta.Value = DateTime.Today;
            cboAgruparPor.SelectedIndex = 0;

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
                SplitterDistance = 280,
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

            Label lblAgrupar = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Agrupar por:" };
            cboAgruparPor = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(100, 30),
                Size = new Size(130, 21)
            };
            cboAgruparPor.Items.Add(AgruparPorProyecto);
            cboAgruparPor.Items.Add(AgruparPorSegmento);
            cboAgruparPor.SelectedIndexChanged += (s, e) => Generar();

            Label lblDesde = new Label { AutoSize = true, Location = new Point(260, 33), Text = "Desde:" };
            dtpFechaDesde = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(320, 30), Size = new Size(130, 21) };

            Label lblHasta = new Label { AutoSize = true, Location = new Point(470, 33), Text = "Hasta:" };
            dtpFechaHasta = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(530, 30), Size = new Size(130, 21) };

            Button btnAplicar = new Button { Text = "Aplicar filtros", Location = new Point(680, 27), Size = new Size(120, 30) };
            btnAplicar.Click += (s, e) => Generar();

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(1000, 27),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

            grupo.Controls.Add(lblAgrupar);
            grupo.Controls.Add(cboAgruparPor);
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
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                Text = "Importe facturado (haz clic en una barra para ver sus tareas)"
            };

            chartFacturacion = new Chart { Dock = DockStyle.Fill };
            chartFacturacion.MouseClick += chartFacturacion_MouseClick;

            grupo.Controls.Add(chartFacturacion);

            return grupo;
        }

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Tareas facturadas" };

            Panel pnlCabecera = new Panel { Dock = DockStyle.Top, Height = 22 };

            lblFiltroActivo = new Label
            {
                AutoSize = true,
                Location = new Point(0, 3),
                Font = new Font(this.Font, FontStyle.Italic),
                ForeColor = Color.Gray,
                Text = "Haz clic en una barra de la gráfica para ver el detalle de un grupo."
            };

            pnlCabecera.Controls.Add(lblFiltroActivo);

            dgvDetalle = new DataGridView
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

            EstiloDashboard.EstilizarGrid(dgvDetalle);

            dgvDetalle.DataBindingComplete += (s, e) => FormateadorGridTareas.AplicarFormato(dgvDetalle);
            dgvDetalle.CellDoubleClick += dgvDetalle_CellDoubleClick;

            grupo.Controls.Add(dgvDetalle);
            grupo.Controls.Add(pnlCabecera);

            return grupo;
        }

        private void CargarCatalogos()
        {
            try
            {
                proyectos = proyectoRepositorio.ObtenerActivos();
                segmentos = segmentoRepositorio.ObtenerActivos();
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeFacturacionProyecto.CargarCatalogos",
                    "No se han podido cargar los catálogos.",
                    ex);
            }
        }

        private bool AgrupadoPorProyecto()
        {
            return cboAgruparPor.SelectedItem as string == AgruparPorProyecto;
        }

        private void Generar()
        {
            try
            {
                DateTime? desde = dtpFechaDesde.Value.Date;
                DateTime? hasta = dtpFechaHasta.Value.Date;

                DataTable resumen = AgrupadoPorProyecto()
                    ? informeRepositorio.ObtenerFacturacionPorProyecto(desde, hasta)
                    : informeRepositorio.ObtenerFacturacionPorSegmento(desde, hasta);

                PintarKpis(resumen);
                PintarGrafica(resumen);

                dgvDetalle.DataSource = null;
                lblFiltroActivo.Text = "Haz clic en una barra de la gráfica para ver el detalle de un grupo.";
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeFacturacionProyecto.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis(DataTable resumen)
        {
            flpKpis.Controls.Clear();

            SortedSet<string> grupos = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            Dictionary<string, decimal> totalPorGrupo = new Dictionary<string, decimal>();
            decimal totalGeneral = 0m;

            foreach (DataRow fila in resumen.Rows)
            {
                string grupo = fila["GRUPO"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                grupos.Add(grupo);
                totalGeneral += importe;

                decimal acumulado;
                totalPorGrupo.TryGetValue(grupo, out acumulado);
                totalPorGrupo[grupo] = acumulado + importe;
            }

            string grupoTop = "-";
            decimal importeTop = 0m;

            foreach (KeyValuePair<string, decimal> par in totalPorGrupo)
            {
                if (par.Value > importeTop)
                {
                    importeTop = par.Value;
                    grupoTop = par.Key;
                }
            }

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "GRUPOS CON FACTURACIÓN", grupos.Count.ToString(CulturaDecimal), EstiloDashboard.Paleta[0]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "IMPORTE TOTAL", totalGeneral.ToString("N2", CulturaDecimal), EstiloDashboard.Paleta[1]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "GRUPO CON MÁS FACTURACIÓN", grupoTop, EstiloDashboard.Paleta[2]));
        }

        private void PintarGrafica(DataTable resumen)
        {
            EstiloDashboard.PrepararGrafica(chartFacturacion);
            coloresPorDivisa.Clear();

            SortedSet<string> grupos = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            SortedSet<string> divisas = new SortedSet<string>(StringComparer.Ordinal);
            Dictionary<string, decimal> valores = new Dictionary<string, decimal>();

            foreach (DataRow fila in resumen.Rows)
            {
                string grupo = fila["GRUPO"].ToString();
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                grupos.Add(grupo);
                divisas.Add(divisa);
                valores[grupo + "|" + divisa] = importe;
            }

            foreach (string divisa in divisas)
            {
                Series serie = new Series(divisa)
                {
                    ChartType = SeriesChartType.Column,
                    ChartArea = "AreaPrincipal",
                    Legend = "Leyenda",
                    LegendText = divisa,
                    Color = EstiloDashboard.ColorDePaleta(coloresPorDivisa, divisa)
                };

                foreach (string grupo in grupos)
                {
                    decimal importe;
                    valores.TryGetValue(grupo + "|" + divisa, out importe);

                    serie.Points.AddXY(grupo, importe);
                }

                chartFacturacion.Series.Add(serie);
            }
        }

        private void chartFacturacion_MouseClick(object sender, MouseEventArgs e)
        {
            HitTestResult resultado = chartFacturacion.HitTest(e.X, e.Y);

            if (resultado.ChartElementType != ChartElementType.DataPoint ||
                resultado.Series == null || resultado.PointIndex < 0)
            {
                return;
            }

            string grupo = resultado.Series.Points[resultado.PointIndex].AxisLabel;

            MostrarDetalleGrupo(grupo);
        }

        /// <summary>
        /// Busca y muestra las tareas facturadas del grupo (proyecto o
        /// segmento) indicado, en el rango de fechas actual.
        /// </summary>
        private void MostrarDetalleGrupo(string grupo)
        {
            try
            {
                FiltroBusquedaTareas filtro = new FiltroBusquedaTareas
                {
                    IdEstado = EstadosTareaConocidos.Facturado,
                    FechaDesde = null,
                    FechaHasta = null
                };

                if (AgrupadoPorProyecto())
                {
                    Proyecto proyecto = proyectos.Find(p => p.Descripcion == grupo);

                    if (proyecto != null)
                        filtro.IdProyecto = proyecto.Id;
                }
                else
                {
                    Segmento segmento = segmentos.Find(s => s.Descripcion == grupo);

                    if (segmento != null)
                        filtro.IdSegmento = segmento.Id;
                }

                dgvDetalle.DataSource = tareaRepositorio.BuscarPorFiltro(filtro);

                lblFiltroActivo.Text = "Grupo seleccionado: " + grupo;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeFacturacionProyecto.MostrarDetalleGrupo",
                    "No se ha podido cargar el detalle del grupo.",
                    ex);
            }
        }

        private void dgvDetalle_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            object valor = dgvDetalle.Rows[e.RowIndex].Cells["ID_TAREA"].Value;

            if (valor == null)
                return;

            using (FrmDetalleTarea formulario = new FrmDetalleTarea(Convert.ToInt32(valor)))
            {
                formulario.ShowDialog();
            }
        }
    }
}
