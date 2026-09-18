using GestionFacturas.Datos;
using GestionFacturas.Formularios;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionFacturas
{
    /// <summary>
    /// Formulario principal (menú) de la aplicación. Desde aquí se
    /// abren el resto de pantallas como ventanas modales, y se
    /// muestra un cuadro de mando con el resumen del mes en curso:
    /// una fila de tarjetas KPI arriba (facturado por divisa y
    /// pendiente de facturar), y debajo, repartiendo el espacio
    /// restante, la tabla por usuario y el gráfico de facturado
    /// mensual del año en curso.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        private readonly ResumenPrincipalRepositorio resumenRepositorio = new ResumenPrincipalRepositorio();

        /// <summary>
        /// Cultura usada para formatear los importes y fechas del
        /// resumen (punto de millares, coma decimal, meses en
        /// español).
        /// </summary>
        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        /// <summary>Color de fondo del lienzo y acento de las tarjetas "Pendiente".</summary>
        private static readonly Color ColorPendiente = Color.FromArgb(245, 124, 0);

        /// <summary>
        /// Paleta de acento para las tarjetas/series "Facturado", una
        /// por divisa, en el orden en que se vayan encontrando. Si
        /// hay más divisas que colores, se reciclan desde el principio.
        /// </summary>
        private static readonly Color[] PaletaDivisas =
        {
            Color.FromArgb(25, 118, 210),   // azul
            Color.FromArgb(46, 125, 50),    // verde
            Color.FromArgb(0, 121, 107),    // verde azulado
            Color.FromArgb(94, 53, 177),    // morado
            Color.FromArgb(198, 40, 40)     // rojo
        };

        /// <summary>Color de cabecera de las rejillas del cuadro de mando.</summary>
        private static readonly Color ColorCabeceraGrid = Color.FromArgb(55, 71, 79);

        /// <summary>Color de las filas pares (efecto cebra) de las rejillas.</summary>
        private static readonly Color ColorFilaAlterna = Color.FromArgb(245, 246, 248);

        /// <summary>
        /// Asigna a cada divisa un color estable de <see cref="PaletaDivisas"/>
        /// según el orden en que se van viendo, para que la misma
        /// divisa use siempre el mismo color en tarjetas y gráfica.
        /// </summary>
        private readonly Dictionary<string, Color> coloresPorDivisa = new Dictionary<string, Color>();

        public FrmPrincipal()
        {
            InitializeComponent();

            EstilizarGrid(dgvPorUsuario);
            ConfigurarGrafica();

            // Se recarga cada vez que la ventana recupera el foco
            // (p. ej. al cerrar cualquiera de las pantallas hijas),
            // para que el resumen no se quede desactualizado durante
            // toda la sesión.
            this.Activated += (s, e) => CargarResumen();
        }

        /// <summary>
        /// Da a una rejilla del cuadro de mando un aspecto más propio
        /// de un informe (cabecera oscura, sin selector de filas, con
        /// rayado cebra) en vez del gris plano por defecto de WinForms.
        /// </summary>
        private void EstilizarGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.ColumnHeadersDefaultCellStyle.BackColor = ColorCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorFilaAlterna;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(227, 242, 253);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        }

        /// <summary>
        /// Prepara el área de la gráfica de facturado mensual, con un
        /// aspecto limpio (fondo blanco, rejilla suave, leyenda
        /// abajo), sin series todavía: se dan de alta dinámicamente
        /// en PintarGraficoMensual según las divisas que tengan datos
        /// ese año.
        /// </summary>
        private void ConfigurarGrafica()
        {
            chartMensual.Series.Clear();
            chartMensual.ChartAreas.Clear();
            chartMensual.Legends.Clear();

            ChartArea area = new ChartArea("AreaPrincipal");
            area.BackColor = Color.White;
            area.AxisX.Interval = 1;
            area.AxisX.LineColor = Color.Gainsboro;
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.MajorTickMark.LineColor = Color.Gainsboro;
            area.AxisY.LineColor = Color.Gainsboro;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.LabelStyle.Format = "N0";

            chartMensual.ChartAreas.Add(area);

            Legend leyenda = new Legend("Leyenda")
            {
                Docking = Docking.Bottom,
                Alignment = System.Drawing.StringAlignment.Center,
                BackColor = Color.Transparent
            };

            chartMensual.Legends.Add(leyenda);
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Load del formulario; la primera carga
        /// del resumen la dispara ya el propio Activated al mostrarse
        /// la ventana.
        /// </summary>
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Recarga los tres bloques del cuadro de mando (tarjetas KPI,
        /// tabla por usuario y gráfico mensual) a partir de los datos
        /// actuales.
        /// </summary>
        private void CargarResumen()
        {
            try
            {
                DateTime hoy = DateTime.Today;
                DateTime primerDiaMes = new DateTime(hoy.Year, hoy.Month, 1);
                DateTime primerDiaMesSiguiente = primerDiaMes.AddMonths(1);

                DataTable facturadoPorUsuario = resumenRepositorio.ObtenerFacturadoPorUsuarioYDivisa(
                    primerDiaMes, primerDiaMesSiguiente);

                DataTable pendientePorUsuario = resumenRepositorio.ObtenerPendientePorUsuario(
                    primerDiaMes, primerDiaMesSiguiente);

                DataTable facturadoMensual = resumenRepositorio.ObtenerFacturadoMensual(hoy.Year);

                lblTituloResumen.Text = "Resumen de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                    hoy.ToString("MMMM yyyy", CulturaDecimal));

                PintarTarjetasKpi(facturadoPorUsuario, pendientePorUsuario);
                PintarTablaPorUsuario(facturadoPorUsuario, pendientePorUsuario);
                PintarGraficoMensual(facturadoMensual);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmPrincipal.CargarResumen",
                    "No se ha podido cargar el resumen del mes.",
                    ex);
            }
        }

        /// <summary>
        /// Devuelve el color de acento asignado a una divisa,
        /// asignando el siguiente de la paleta la primera vez que se
        /// ve esa divisa (y reutilizándolo después, tanto en las
        /// tarjetas KPI como en la gráfica).
        /// </summary>
        private Color ColorDeDivisa(string divisa)
        {
            Color color;

            if (!coloresPorDivisa.TryGetValue(divisa, out color))
            {
                color = PaletaDivisas[coloresPorDivisa.Count % PaletaDivisas.Length];
                coloresPorDivisa[divisa] = color;
            }

            return color;
        }

        /// <summary>
        /// Reconstruye la franja de tarjetas KPI: una por cada divisa
        /// con importe facturado este mes, más una tarjeta final de
        /// "Pendiente de Facturar" (sin divisa, ver
        /// ResumenPrincipalRepositorio). El número de tarjetas varía
        /// según los datos, por eso se generan en tiempo de ejecución
        /// en vez de ser controles fijos del diseñador.
        /// </summary>
        private void PintarTarjetasKpi(DataTable facturadoPorUsuario, DataTable pendientePorUsuario)
        {
            flpKpis.Controls.Clear();
            coloresPorDivisa.Clear();

            Dictionary<string, decimal> totalPorDivisa = new Dictionary<string, decimal>();

            foreach (DataRow fila in facturadoPorUsuario.Rows)
            {
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                decimal acumulado;
                totalPorDivisa.TryGetValue(divisa, out acumulado);
                totalPorDivisa[divisa] = acumulado + importe;
            }

            foreach (KeyValuePair<string, decimal> par in totalPorDivisa)
            {
                flpKpis.Controls.Add(CrearTarjetaKpi(
                    "FACTURADO " + par.Key,
                    par.Value.ToString("N2", CulturaDecimal),
                    ColorDeDivisa(par.Key)));
            }

            if (totalPorDivisa.Count == 0)
            {
                flpKpis.Controls.Add(CrearTarjetaKpi(
                    "FACTURADO",
                    "Sin datos",
                    Color.Gainsboro));
            }

            decimal totalPendiente = 0m;

            foreach (DataRow fila in pendientePorUsuario.Rows)
            {
                totalPendiente += fila["IMPORTE"] == DBNull.Value ? 0m : Convert.ToDecimal(fila["IMPORTE"]);
            }

            flpKpis.Controls.Add(CrearTarjetaKpi(
                "PENDIENTE DE FACTURAR",
                totalPendiente.ToString("N2", CulturaDecimal),
                ColorPendiente));
        }

        /// <summary>
        /// Crea una tarjeta KPI: un panel blanco con una franja de
        /// color a la izquierda, un título pequeño en mayúsculas y un
        /// valor grande debajo, imitando el aspecto de los indicadores
        /// de un cuadro de mando.
        /// </summary>
        private Panel CrearTarjetaKpi(string titulo, string valor, Color colorAcento)
        {
            Panel tarjeta = new Panel
            {
                Size = new Size(230, 95),
                Margin = new Padding(0, 0, 15, 0),
                BackColor = Color.White
            };

            Panel acento = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                BackColor = colorAcento
            };

            Label lblTitulo = new Label
            {
                AutoSize = false,
                Location = new Point(21, 16),
                Size = new Size(200, 18),
                Font = new Font(this.Font.FontFamily, 8F, FontStyle.Bold),
                ForeColor = Color.Gray,
                Text = titulo
            };

            Label lblValor = new Label
            {
                AutoSize = false,
                Location = new Point(18, 38),
                Size = new Size(205, 40),
                Font = new Font(this.Font.FontFamily, 18F, FontStyle.Bold),
                ForeColor = Color.FromArgb(33, 33, 33),
                Text = valor
            };

            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(acento);

            return tarjeta;
        }

        /// <summary>
        /// Rellena la rejilla "Por usuario": una fila por cada usuario
        /// que aparezca en cualquiera de los dos orígenes (facturado o
        /// pendiente), con el facturado desglosado por divisa en una
        /// sola celda de texto y el pendiente como un único total.
        /// </summary>
        private void PintarTablaPorUsuario(DataTable facturadoPorUsuario, DataTable pendientePorUsuario)
        {
            Dictionary<string, List<string>> facturadoTextoPorUsuario = new Dictionary<string, List<string>>();

            foreach (DataRow fila in facturadoPorUsuario.Rows)
            {
                string usuario = fila["NOMBRE_USUARIO"].ToString();
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

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
            {
                string usuario = fila["NOMBRE_USUARIO"].ToString();
                decimal importe = fila["IMPORTE"] == DBNull.Value ? 0m : Convert.ToDecimal(fila["IMPORTE"]);

                pendientePorUsuarioDic[usuario] = importe;
            }

            SortedSet<string> usuarios = new SortedSet<string>(StringComparer.CurrentCultureIgnoreCase);

            foreach (string usuario in facturadoTextoPorUsuario.Keys)
                usuarios.Add(usuario);

            foreach (string usuario in pendientePorUsuarioDic.Keys)
                usuarios.Add(usuario);

            DataTable tabla = new DataTable();
            tabla.Columns.Add("Usuario", typeof(string));
            tabla.Columns.Add("Facturado", typeof(string));
            tabla.Columns.Add("Pendiente", typeof(string));

            foreach (string usuario in usuarios)
            {
                List<string> lineasFacturado;

                string textoFacturado = facturadoTextoPorUsuario.TryGetValue(usuario, out lineasFacturado)
                    ? string.Join(", ", lineasFacturado)
                    : "-";

                decimal pendiente;

                string textoPendiente = pendientePorUsuarioDic.TryGetValue(usuario, out pendiente)
                    ? pendiente.ToString("N2", CulturaDecimal)
                    : "-";

                tabla.Rows.Add(usuario, textoFacturado, textoPendiente);
            }

            dgvPorUsuario.DataSource = tabla;

            if (dgvPorUsuario.Columns.Count > 0)
            {
                dgvPorUsuario.Columns["Facturado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                dgvPorUsuario.Columns["Pendiente"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
        }

        /// <summary>
        /// Vuelca el resultado (MES, COD_DIV, IMPORTE) sobre la
        /// gráfica de barras: una serie por cada divisa con datos ese
        /// año (con el mismo color que su tarjeta KPI), con un punto
        /// por cada uno de los 12 meses (0 donde no haya importe),
        /// para que las barras de las distintas divisas se agrupen
        /// mes a mes.
        /// </summary>
        private void PintarGraficoMensual(DataTable facturadoMensual)
        {
            chartMensual.Series.Clear();

            SortedSet<string> divisas = new SortedSet<string>(StringComparer.Ordinal);
            Dictionary<string, decimal> valores = new Dictionary<string, decimal>();

            foreach (DataRow fila in facturadoMensual.Rows)
            {
                int mes = Convert.ToInt32(fila["MES"]);
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                divisas.Add(divisa);
                valores[divisa + "|" + mes] = importe;
            }

            string[] nombresMeses = CulturaDecimal.DateTimeFormat.AbbreviatedMonthNames;

            foreach (string divisa in divisas)
            {
                Series serie = new Series(divisa)
                {
                    ChartType = SeriesChartType.Column,
                    ChartArea = "AreaPrincipal",
                    Legend = "Leyenda",
                    LegendText = divisa,
                    Color = ColorDeDivisa(divisa),
                    BorderWidth = 0
                };

                for (int mes = 1; mes <= 12; mes++)
                {
                    decimal importe;
                    valores.TryGetValue(divisa + "|" + mes, out importe);

                    string etiquetaMes = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                        nombresMeses[mes - 1].TrimEnd('.'));

                    serie.Points.AddXY(etiquetaMes, importe);
                }

                chartMensual.Series.Add(serie);
            }
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla de registro de una
        /// tarea nueva.
        /// </summary>
        private void registrarTareaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRegistrarTarea formulario = new FrmRegistrarTarea();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla de facturación
        /// (paso 3 del flujo).
        /// </summary>
        private void facturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFacturar formulario = new FrmFacturar();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla del pool de tareas.
        /// </summary>
        private void poolDeTareasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPoolTareas formulario = new FrmPoolTareas();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de facturación
        /// (gráfica de importe facturado/pendiente/bloqueado).
        /// </summary>
        private void facturaciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeFacturacion formulario = new FrmInformeFacturacion();

            formulario.ShowDialog();
        }
    }
}
