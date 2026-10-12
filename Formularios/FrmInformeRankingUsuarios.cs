using GestionFacturas.Datos;
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
    /// Informe "Ranking de Usuarios" (menú Informes): por usuario,
    /// cuántas tareas tiene abiertas ahora mismo, cuánto ha facturado
    /// y cuánto tiene pendiente en el rango de fechas indicado, y su
    /// tiempo medio (en días) desde que se registra una tarea hasta
    /// que se factura. Combina rejilla ordenable (clic en cabecera) y
    /// gráfica de barras del importe facturado por usuario y divisa.
    /// </summary>
    public partial class FrmInformeRankingUsuarios : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly ResumenPrincipalRepositorio resumenRepositorio = new ResumenPrincipalRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        private DateTimePicker dtpFechaDesde;
        private DateTimePicker dtpFechaHasta;
        private FlowLayoutPanel flpKpis;
        private Chart chartFacturadoPorUsuario;
        private DataGridView dgvRanking;

        private readonly Dictionary<string, Color> coloresPorDivisa = new Dictionary<string, Color>();

        public FrmInformeRankingUsuarios()
        {
            InitializeComponent();

            ConstruirInterfaz();

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
            GroupBox grupo = new GroupBox { Dock = DockStyle.Top, Height = 70, Text = "Filtros (facturado, pendiente y tiempo medio)" };

            Label lblDesde = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Desde:" };
            dtpFechaDesde = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(70, 30), Size = new Size(130, 21) };

            Label lblHasta = new Label { AutoSize = true, Location = new Point(220, 33), Text = "Hasta:" };
            dtpFechaHasta = new DateTimePicker { Format = DateTimePickerFormat.Short, Location = new Point(280, 30), Size = new Size(130, 21) };

            Button btnAplicar = new Button { Text = "Aplicar filtros", Location = new Point(430, 27), Size = new Size(120, 30) };
            btnAplicar.Click += (s, e) => Generar();

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(1000, 27),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

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
                Text = "Facturado por usuario y divisa"
            };

            chartFacturadoPorUsuario = new Chart { Dock = DockStyle.Fill };

            grupo.Controls.Add(chartFacturadoPorUsuario);

            return grupo;
        }

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Detalle por usuario" };

            dgvRanking = new DataGridView
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

            EstiloDashboard.EstilizarGrid(dgvRanking);

            grupo.Controls.Add(dgvRanking);

            return grupo;
        }

        private void Generar()
        {
            try
            {
                DateTime desde = dtpFechaDesde.Value.Date;
                DateTime hasta = dtpFechaHasta.Value.Date.AddDays(1);

                DataTable tareasAbiertas = informeRepositorio.ObtenerTareasAbiertasPorUsuario();
                DataTable diasMedios = informeRepositorio.ObtenerDiasMediosHastaFacturar(desde, dtpFechaHasta.Value.Date);
                DataTable facturadoPorUsuario = resumenRepositorio.ObtenerFacturadoPorUsuarioYDivisa(desde, hasta);
                DataTable pendientePorUsuario = resumenRepositorio.ObtenerPendientePorUsuario(desde, hasta);

                PintarKpis(facturadoPorUsuario, pendientePorUsuario, tareasAbiertas);
                PintarGrafica(facturadoPorUsuario);
                PintarGrid(tareasAbiertas, facturadoPorUsuario, pendientePorUsuario, diasMedios);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeRankingUsuarios.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis(DataTable facturadoPorUsuario, DataTable pendientePorUsuario, DataTable tareasAbiertas)
        {
            flpKpis.Controls.Clear();

            SortedSet<string> usuarios = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (DataRow fila in tareasAbiertas.Rows) usuarios.Add(fila["NOMBRE_USUARIO"].ToString());
            foreach (DataRow fila in facturadoPorUsuario.Rows) usuarios.Add(fila["NOMBRE_USUARIO"].ToString());

            decimal totalFacturado = 0m;

            foreach (DataRow fila in facturadoPorUsuario.Rows)
                totalFacturado += Convert.ToDecimal(fila["IMPORTE"]);

            int totalTareasAbiertas = 0;

            foreach (DataRow fila in tareasAbiertas.Rows)
                totalTareasAbiertas += Convert.ToInt32(fila["NUM_TAREAS"]);

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "USUARIOS ACTIVOS", usuarios.Count.ToString(CulturaDecimal), EstiloDashboard.Paleta[0]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "TAREAS ABIERTAS (TOTAL)", totalTareasAbiertas.ToString(CulturaDecimal), EstiloDashboard.Paleta[2]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "FACTURADO EN EL PERIODO", totalFacturado.ToString("N2", CulturaDecimal), EstiloDashboard.Paleta[1]));
        }

        private void PintarGrafica(DataTable facturadoPorUsuario)
        {
            EstiloDashboard.PrepararGrafica(chartFacturadoPorUsuario);
            coloresPorDivisa.Clear();

            SortedSet<string> usuarios = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            SortedSet<string> divisas = new SortedSet<string>(StringComparer.Ordinal);
            Dictionary<string, decimal> valores = new Dictionary<string, decimal>();

            foreach (DataRow fila in facturadoPorUsuario.Rows)
            {
                string usuario = fila["NOMBRE_USUARIO"].ToString();
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                usuarios.Add(usuario);
                divisas.Add(divisa);
                valores[usuario + "|" + divisa] = importe;
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

                foreach (string usuario in usuarios)
                {
                    decimal importe;
                    valores.TryGetValue(usuario + "|" + divisa, out importe);

                    serie.Points.AddXY(usuario, importe);
                }

                chartFacturadoPorUsuario.Series.Add(serie);
            }
        }

        private void PintarGrid(
            DataTable tareasAbiertas, DataTable facturadoPorUsuario,
            DataTable pendientePorUsuario, DataTable diasMedios)
        {
            Dictionary<string, int> tareasPorUsuario = new Dictionary<string, int>();
            foreach (DataRow fila in tareasAbiertas.Rows)
                tareasPorUsuario[fila["NOMBRE_USUARIO"].ToString()] = Convert.ToInt32(fila["NUM_TAREAS"]);

            Dictionary<string, List<string>> facturadoTextoPorUsuario = new Dictionary<string, List<string>>();
            foreach (DataRow fila in facturadoPorUsuario.Rows)
            {
                string usuario = fila["NOMBRE_USUARIO"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);
                string divisa = fila["COD_DIV"].ToString();

                List<string> lineas;
                if (!facturadoTextoPorUsuario.TryGetValue(usuario, out lineas))
                {
                    lineas = new List<string>();
                    facturadoTextoPorUsuario[usuario] = lineas;
                }

                lineas.Add(importe.ToString("N2", CulturaDecimal) + " " + divisa);
            }

            Dictionary<string, decimal> pendientePorUsuarioDic = new Dictionary<string, decimal>();
            foreach (DataRow fila in pendientePorUsuario.Rows)
                pendientePorUsuarioDic[fila["NOMBRE_USUARIO"].ToString()] =
                    fila["IMPORTE"] == DBNull.Value ? 0m : Convert.ToDecimal(fila["IMPORTE"]);

            Dictionary<string, int> diasMediosPorUsuario = new Dictionary<string, int>();
            foreach (DataRow fila in diasMedios.Rows)
                diasMediosPorUsuario[fila["NOMBRE_USUARIO"].ToString()] =
                    fila["DIAS_MEDIOS"] == DBNull.Value ? 0 : Convert.ToInt32(fila["DIAS_MEDIOS"]);

            SortedSet<string> usuarios = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);
            foreach (string usuario in tareasPorUsuario.Keys) usuarios.Add(usuario);
            foreach (string usuario in facturadoTextoPorUsuario.Keys) usuarios.Add(usuario);
            foreach (string usuario in pendientePorUsuarioDic.Keys) usuarios.Add(usuario);

            DataTable tabla = new DataTable();
            tabla.Columns.Add("Usuario", typeof(string));
            tabla.Columns.Add("Tareas Abiertas", typeof(int));
            tabla.Columns.Add("Facturado", typeof(string));
            tabla.Columns.Add("Pendiente", typeof(decimal));
            tabla.Columns.Add("Días Medios hasta Facturar", typeof(string));

            foreach (string usuario in usuarios)
            {
                int tareas;
                tareasPorUsuario.TryGetValue(usuario, out tareas);

                List<string> lineasFacturado;
                string textoFacturado = facturadoTextoPorUsuario.TryGetValue(usuario, out lineasFacturado)
                    ? string.Join(", ", lineasFacturado)
                    : "-";

                decimal pendiente;
                pendientePorUsuarioDic.TryGetValue(usuario, out pendiente);

                int dias;
                string textoDias = diasMediosPorUsuario.TryGetValue(usuario, out dias) ? dias.ToString() : "-";

                tabla.Rows.Add(usuario, tareas, textoFacturado, pendiente, textoDias);
            }

            dgvRanking.DataSource = tabla;

            if (dgvRanking.Columns.Count > 0)
            {
                dgvRanking.Columns["Pendiente"].DefaultCellStyle.Format = "N2";
                dgvRanking.Columns["Pendiente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvRanking.Columns["Facturado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvRanking.Columns["Tareas Abiertas"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvRanking.Columns["Días Medios hasta Facturar"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }
    }
}
