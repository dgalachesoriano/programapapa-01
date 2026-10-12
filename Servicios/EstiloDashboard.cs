using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionFacturas.Servicios
{
    /// <summary>
    /// Estilo visual compartido por las pantallas de tipo "cuadro de
    /// mando" (el resumen de FrmPrincipal y los informes del menú
    /// Informes): lienzo de fondo gris claro, tarjetas KPI, rejillas
    /// con cabecera oscura y rayado cebra, y gráficas con fondo blanco
    /// y rejilla suave. Centraliza aquí lo que se hizo la primera vez
    /// en el resumen del formulario principal, para no repetirlo en
    /// cada informe nuevo.
    /// </summary>
    internal static class EstiloDashboard
    {
        /// <summary>Fondo del lienzo detrás de las tarjetas/paneles blancos.</summary>
        public static readonly Color ColorCanvas = Color.FromArgb(245, 246, 248);

        /// <summary>Color de cabecera de las rejillas de datos.</summary>
        public static readonly Color ColorCabeceraGrid = Color.FromArgb(55, 71, 79);

        /// <summary>Color de las filas pares (efecto cebra) de las rejillas.</summary>
        public static readonly Color ColorFilaAlterna = Color.FromArgb(245, 246, 248);

        /// <summary>Color de los títulos de sección.</summary>
        public static readonly Color ColorTituloSeccion = Color.FromArgb(55, 71, 79);

        /// <summary>Color del valor grande de una tarjeta KPI.</summary>
        public static readonly Color ColorTextoKpi = Color.FromArgb(33, 33, 33);

        /// <summary>
        /// Paleta de acento para series/categorías (barras, tarjetas),
        /// asignada de forma estable mediante <see cref="ColorDePaleta"/>.
        /// </summary>
        public static readonly Color[] Paleta =
        {
            Color.FromArgb(25, 118, 210),   // azul
            Color.FromArgb(46, 125, 50),    // verde
            Color.FromArgb(245, 124, 0),    // naranja
            Color.FromArgb(0, 121, 107),    // verde azulado
            Color.FromArgb(94, 53, 177),    // morado
            Color.FromArgb(198, 40, 40),    // rojo
            Color.FromArgb(2, 119, 189),    // azul claro
            Color.FromArgb(174, 79, 0)      // marrón
        };

        /// <summary>
        /// Asigna a una clave (divisa, proyecto, motivo...) un color
        /// estable de <see cref="Paleta"/> según el orden en que se va
        /// viendo, para que la misma clave use siempre el mismo color
        /// en tarjetas, gráficas y grids de un mismo informe.
        /// </summary>
        public static Color ColorDePaleta(Dictionary<string, Color> asignados, string clave)
        {
            Color color;

            if (!asignados.TryGetValue(clave, out color))
            {
                color = Paleta[asignados.Count % Paleta.Length];
                asignados[clave] = color;
            }

            return color;
        }

        /// <summary>
        /// Da a una rejilla de datos un aspecto de informe (cabecera
        /// oscura, sin selector de filas, rayado cebra) en vez del
        /// gris plano por defecto de WinForms.
        /// </summary>
        public static void EstilizarGrid(DataGridView grid)
        {
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.GridColor = Color.FromArgb(230, 230, 230);
            grid.RowTemplate.Height = 26;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.ColumnHeadersDefaultCellStyle.BackColor = ColorCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorFilaAlterna;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(227, 242, 253);
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;
        }

        /// <summary>
        /// Prepara una gráfica con un área/leyenda de aspecto limpio
        /// (fondo blanco, rejilla suave, leyenda abajo), sin series:
        /// se dan de alta después según los datos del informe.
        /// </summary>
        public static void PrepararGrafica(Chart chart, string nombreArea = "AreaPrincipal", string nombreLeyenda = "Leyenda")
        {
            chart.Series.Clear();
            chart.ChartAreas.Clear();
            chart.Legends.Clear();

            chart.BackColor = Color.White;
            chart.BorderlineColor = Color.FromArgb(230, 230, 230);
            chart.BorderlineWidth = 1;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;

            ChartArea area = new ChartArea(nombreArea);
            area.BackColor = Color.White;
            area.AxisX.LineColor = Color.Gainsboro;
            area.AxisX.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisX.MajorTickMark.LineColor = Color.Gainsboro;
            area.AxisY.LineColor = Color.Gainsboro;
            area.AxisY.MajorGrid.LineColor = Color.WhiteSmoke;
            area.AxisY.LabelStyle.Format = "N0";

            chart.ChartAreas.Add(area);

            Legend leyenda = new Legend(nombreLeyenda)
            {
                Docking = Docking.Bottom,
                Alignment = StringAlignment.Center,
                BackColor = Color.Transparent
            };

            chart.Legends.Add(leyenda);
        }

        /// <summary>
        /// Crea una tarjeta KPI: un panel blanco con una franja de
        /// color a la izquierda, un título pequeño en mayúsculas y un
        /// valor grande debajo.
        /// </summary>
        public static Panel CrearTarjetaKpi(Font fontBase, string titulo, string valor, Color colorAcento)
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
                Font = new Font(fontBase.FontFamily, 8F, FontStyle.Bold),
                ForeColor = Color.Gray,
                Text = titulo
            };

            Label lblValor = new Label
            {
                AutoSize = false,
                Location = new Point(18, 38),
                Size = new Size(205, 40),
                Font = new Font(fontBase.FontFamily, 16F, FontStyle.Bold),
                ForeColor = ColorTextoKpi,
                Text = valor
            };

            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(acento);

            return tarjeta;
        }

        /// <summary>
        /// Hace clicable una tarjeta KPI (o cualquier panel): engancha
        /// el mismo manejador al propio panel y a todos sus controles
        /// hijos, porque en WinForms un clic sobre un control hijo (p.
        /// ej. un Label) no burbujea el evento Click al Panel padre
        /// por sí solo.
        /// </summary>
        public static void HacerClicable(Panel tarjeta, EventHandler alPulsar)
        {
            tarjeta.Cursor = Cursors.Hand;
            tarjeta.Click += alPulsar;

            foreach (Control hijo in tarjeta.Controls)
            {
                hijo.Cursor = Cursors.Hand;
                hijo.Click += alPulsar;
            }
        }

        /// <summary>
        /// Crea el título de una sección del informe (Dock = Top,
        /// negrita, color corporativo del cuadro de mando).
        /// </summary>
        public static Label CrearTituloSeccion(string texto)
        {
            return new Label
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold),
                ForeColor = ColorTituloSeccion,
                Padding = new Padding(0, 0, 0, 8),
                Margin = new Padding(0, 0, 0, 8),
                Text = texto
            };
        }
    }
}
