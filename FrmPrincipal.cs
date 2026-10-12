using GestionFacturas.Datos;
using GestionFacturas.Formularios;
using GestionFacturas.Modelos;
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
    /// una fila de tarjetas KPI arriba (facturado y pendiente de
    /// facturar, cada uno por divisa), y debajo, repartiendo el
    /// espacio restante, los desgloses por usuario, por segmento y por
    /// tipo de documento (facturado y pendiente por divisa) y el
    /// gráfico de facturado
    /// mensual del año en curso.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        private readonly ResumenPrincipalRepositorio resumenRepositorio = new ResumenPrincipalRepositorio();
        private readonly DivisaRepositorio divisaRepositorio = new DivisaRepositorio();

        /// <summary>
        /// Cultura usada para formatear los importes y fechas del
        /// resumen (punto de millares, coma decimal, meses en
        /// español).
        /// </summary>
        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        /// <summary>Color de fondo del lienzo y acento de las tarjetas "Pendiente".</summary>
        private static readonly Color ColorPendiente = Color.FromArgb(245, 124, 0);

        /// <summary>
        /// Acento de las tarjetas "Facturado", el mismo en todas las
        /// divisas (verde = facturado, naranja = por facturar, gris =
        /// sin asignar). La gráfica mensual sí usa un color por divisa
        /// (PaletaDivisas), para distinguir sus barras.
        /// </summary>
        private static readonly Color ColorFacturado = Color.FromArgb(46, 125, 50);

        /// <summary>Acento de las tarjetas "Sin asignar" (tareas registradas sin usuario).</summary>
        private static readonly Color ColorSinAsignar = Color.FromArgb(117, 117, 117);

        /// <summary>
        /// Paleta de colores de las series de la gráfica mensual, una
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

        /// <summary>Fuentes de las tarjetas KPI (compartidas, para no crear una por tarjeta en cada recarga).</summary>
        private static readonly Font FuenteTituloTarjeta = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);
        private static readonly Font FuenteValorTarjeta = new Font("Microsoft Sans Serif", 18F, FontStyle.Bold);
        private static readonly Font FuenteValorTarjetaCompacta = new Font("Microsoft Sans Serif", 15F, FontStyle.Bold);
        private static readonly Font FuenteDetalleTarjeta = new Font("Microsoft Sans Serif", 9F, FontStyle.Regular);

        /// <summary>Nombre de la etiqueta del importe dentro de cada tarjeta KPI.</summary>
        private const string NombreEtiquetaValor = "lblValor";

        /// <summary>
        /// Ancho máximo y mínimo de una tarjeta KPI: las tarjetas se
        /// reparten el ancho disponible entre estos dos límites (ver
        /// AjustarAnchoTarjetas). Por debajo de
        /// <see cref="AnchoTarjetaCompacta"/> el importe usa una
        /// fuente algo menor para que siga cabiendo.
        /// </summary>
        private const int AnchoTarjetaMaximo = 230;
        private const int AnchoTarjetaMinimo = 165;
        private const int AnchoTarjetaCompacta = 205;

        /// <summary>Fuente de las etiquetas de valor sobre las barras de la gráfica mensual.</summary>
        private static readonly Font FuenteEtiquetasGrafica = new Font("Microsoft Sans Serif", 8F, FontStyle.Bold);

        /// <summary>
        /// Colores de cabecera de las rejillas del cuadro de mando:
        /// fondo gris muy claro con texto negro, salvo la cabecera de
        /// la columna de agrupación (Usuario / Segmento / Tipo de
        /// documento), en azul, para distinguirla de las de importes.
        /// </summary>
        private static readonly Color ColorCabeceraGrid = Color.FromArgb(240, 241, 243);
        private static readonly Color ColorTextoCabeceraGrid = Color.Black;
        private static readonly Color ColorTextoCabeceraGrupo = Color.FromArgb(25, 118, 210);

        /// <summary>Color de las filas pares (efecto cebra) de las rejillas.</summary>
        private static readonly Color ColorFilaAlterna = Color.FromArgb(245, 246, 248);

        /// <summary>
        /// Asigna a cada divisa un color estable de <see cref="PaletaDivisas"/>
        /// según el orden en que se van viendo, para que la misma
        /// divisa use siempre el mismo color en la gráfica.
        /// </summary>
        private readonly Dictionary<string, Color> coloresPorDivisa = new Dictionary<string, Color>();

        public FrmPrincipal()
        {
            InitializeComponent();

            EstilizarGrid(dgvPorUsuario);
            EstilizarGrid(dgvPorSegmento);
            EstilizarGrid(dgvPorTipoDocumento);
            ConfigurarGrafica();

            btnActualizarResumen.Image = CrearIconoActualizar(16, Color.FromArgb(25, 118, 210));

            flpKpis.Resize += (s, e) => AjustarAnchoTarjetas();

            // F5 actualiza los indicadores igual que el botón.
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.F5)
                {
                    CargarResumen();
                    e.Handled = true;
                }
            };

            // Se recarga cada vez que la ventana recupera el foco
            // (p. ej. al cerrar cualquiera de las pantallas hijas),
            // para que el resumen no se quede desactualizado durante
            // toda la sesión. Para cambios hechos sin salir de esta
            // ventana (o desde otro equipo) está el botón Actualizar.
            this.Activated += (s, e) => CargarResumen();
        }

        /// <summary>
        /// Recarga a demanda todos los indicadores del cuadro de mando.
        /// </summary>
        private void btnActualizarResumen_Click(object sender, EventArgs e)
        {
            CargarResumen();
        }

        /// <summary>
        /// Dibuja el icono de "actualizar" del botón (una flecha
        /// circular), para no depender de ficheros de imagen ni de
        /// fuentes de iconos que pueden no estar instaladas.
        /// </summary>
        private static Bitmap CrearIconoActualizar(int tamano, Color color)
        {
            Bitmap icono = new Bitmap(tamano, tamano);

            float centro = tamano / 2f;
            float radio = tamano * 0.34f;
            float anguloFinal = 240f;

            using (Graphics g = Graphics.FromImage(icono))
            using (Pen lapiz = new Pen(color, tamano / 8f))
            using (SolidBrush pincel = new SolidBrush(color))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // Arco de 300º dejando un hueco arriba, donde va la punta.
                g.DrawArc(lapiz, centro - radio, centro - radio, radio * 2, radio * 2, -60f, 300f);

                // Punta de flecha al final del arco, en la dirección
                // en que avanza (sentido horario).
                double rad = anguloFinal * Math.PI / 180.0;
                PointF fin = new PointF(
                    centro + radio * (float)Math.Cos(rad),
                    centro + radio * (float)Math.Sin(rad));

                PointF direccion = new PointF(-(float)Math.Sin(rad), (float)Math.Cos(rad));
                PointF normal = new PointF((float)Math.Cos(rad), (float)Math.Sin(rad));
                float largo = tamano * 0.22f;

                PointF[] punta =
                {
                    new PointF(fin.X + direccion.X * largo, fin.Y + direccion.Y * largo),
                    new PointF(fin.X + normal.X * largo, fin.Y + normal.Y * largo),
                    new PointF(fin.X - normal.X * largo, fin.Y - normal.Y * largo)
                };

                g.FillPolygon(pincel, punta);
            }

            return icono;
        }

        /// <summary>
        /// Da a una rejilla del cuadro de mando un aspecto más propio
        /// de un informe (cabecera gris clara, sin selector de filas,
        /// con rayado cebra) en vez del gris plano por defecto de
        /// WinForms. Son rejillas de solo consulta, así que la
        /// selección no se resalta (ni en azul ni en ningún otro
        /// color): cada fila conserva siempre su propio fondo.
        /// </summary>
        private void EstilizarGrid(DataGridView grid)
        {
            grid.AutoGenerateColumns = true;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            grid.ColumnHeadersDefaultCellStyle.BackColor = ColorCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = ColorTextoCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = ColorCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = ColorTextoCabeceraGrid;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font(grid.Font, FontStyle.Bold);
            grid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.White;
            grid.DefaultCellStyle.SelectionForeColor = Color.Black;

            grid.AlternatingRowsDefaultCellStyle.BackColor = ColorFilaAlterna;
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = ColorFilaAlterna;

            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;

            grid.CellFormatting += GridDesglose_CellFormatting;
        }

        /// <summary>
        /// Prepara el área de la gráfica de facturado mensual, con un
        /// aspecto limpio (fondo blanco, rejilla suave, leyenda
        /// abajo), sin series todavía: se dan de alta dinámicamente
        /// en PintarGraficoMensual según las divisas activas.
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

            // Las etiquetas del eje Y se muestran abreviadas (K, M,
            // MM) en vez del número completo: ver chartMensual_FormatNumber.
            chartMensual.FormatNumber += chartMensual_FormatNumber;

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
        /// Recarga los bloques del cuadro de mando (tarjetas KPI,
        /// desgloses por usuario, segmento y tipo de documento, y
        /// gráfico mensual) a
        /// partir de los datos actuales.
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

                DataTable pendientePorUsuario = resumenRepositorio.ObtenerPendientePorUsuarioYDivisa();

                DataTable facturadoPorSegmento = resumenRepositorio.ObtenerFacturadoPorSegmentoYDivisa(
                    primerDiaMes, primerDiaMesSiguiente);

                DataTable pendientePorSegmento = resumenRepositorio.ObtenerPendientePorSegmentoYDivisa();

                DataTable facturadoPorTipoDocumento = resumenRepositorio.ObtenerFacturadoPorTipoDocumentoYDivisa(
                    primerDiaMes, primerDiaMesSiguiente);

                DataTable pendientePorTipoDocumento = resumenRepositorio.ObtenerPendientePorTipoDocumentoYDivisa();

                DataTable sinAsignar = resumenRepositorio.ObtenerSinAsignarPorDivisa();

                // Divisas activas, ordenadas por código: fijan qué
                // tarjetas salen siempre (a 0 si no hay datos) y el
                // color de cada divisa en la gráfica.
                List<string> divisas = new List<string>();

                foreach (Divisa divisa in divisaRepositorio.ObtenerActivas())
                {
                    divisas.Add(divisa.Codigo);
                }

                divisas.Sort(StringComparer.Ordinal);

                coloresPorDivisa.Clear();

                foreach (string divisa in divisas)
                {
                    ColorDeDivisa(divisa);
                }

                DataTable facturadoMensual = resumenRepositorio.ObtenerFacturadoMensual(hoy.Year);

                lblTituloResumen.Text = "Resumen de " + CultureInfo.CurrentCulture.TextInfo.ToTitleCase(
                    hoy.ToString("MMMM yyyy", CulturaDecimal)) +
                    "   ·   actualizado a las " + DateTime.Now.ToString("HH:mm:ss", CulturaDecimal);

                PintarTarjetasKpi(facturadoPorUsuario, pendientePorUsuario, sinAsignar, divisas);

                PintarDesglose(dgvPorUsuario, "Usuario",
                    CrearTablaDesglose(facturadoPorUsuario, pendientePorUsuario, "NOMBRE_USUARIO", "Usuario"));

                PintarDesglose(dgvPorSegmento, "Segmento",
                    CrearTablaDesglose(facturadoPorSegmento, pendientePorSegmento, "DES_SEGMENTO", "Segmento"));

                PintarDesglose(dgvPorTipoDocumento, "Tipo de documento",
                    CrearTablaDesglose(facturadoPorTipoDocumento, pendientePorTipoDocumento, "DES_TIP_FACT", "Tipo de documento"));

                PintarGraficoMensual(facturadoMensual, divisas);
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
        /// Devuelve el color asignado a una divisa en la gráfica
        /// mensual, asignando el siguiente de la paleta la primera vez
        /// que se ve esa divisa (y reutilizándolo después).
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
        /// Reconstruye la franja de tarjetas KPI, en tres grupos
        /// separados: "Facturado" este mes (con el color de cada
        /// divisa), "Pendiente de Facturar" (acento naranja) y "Sin
        /// asignar" (tareas registradas todavía sin usuario, acento
        /// gris). Cada grupo tiene siempre una tarjeta por cada divisa
        /// de <paramref name="divisas"/> (las activas), con importe y
        /// N. Doc. a 0 si no hay datos en esa divisa. Se generan en
        /// tiempo de ejecución porque dependen de las divisas dadas de
        /// alta, en vez de ser controles fijos del diseñador.
        /// </summary>
        private void PintarTarjetasKpi(
            DataTable facturadoPorUsuario, DataTable pendientePorUsuario, DataTable sinAsignar, List<string> divisas)
        {
            flpKpis.Controls.Clear();

            AgregarGrupoTarjetas("FACTURADO", SumarPorDivisa(facturadoPorUsuario, divisas), divisa => ColorFacturado);
            AgregarGrupoTarjetas("POR FACTURAR", SumarPorDivisa(pendientePorUsuario, divisas), divisa => ColorPendiente);
            AgregarGrupoTarjetas("SIN ASIGNAR", SumarPorDivisa(sinAsignar, divisas), divisa => ColorSinAsignar);

            // Sin separación extra tras el último grupo.
            Control ultima = flpKpis.Controls[flpKpis.Controls.Count - 1];
            ultima.Margin = new Padding(0, 0, 0, 0);

            AjustarAnchoTarjetas();
        }

        /// <summary>
        /// Reparte el ancho disponible de la franja de KPIs entre todas
        /// las tarjetas (respetando sus márgenes), para que quepan
        /// todas a la vista sin barra de desplazamiento, dentro de los
        /// límites <see cref="AnchoTarjetaMinimo"/> y
        /// <see cref="AnchoTarjetaMaximo"/>. Si ni con el ancho mínimo
        /// caben (ventana muy estrecha), la franja muestra barra de
        /// desplazamiento. Se llama tras pintar las tarjetas y al
        /// redimensionar la ventana.
        /// </summary>
        private void AjustarAnchoTarjetas()
        {
            int numeroTarjetas = flpKpis.Controls.Count;

            if (numeroTarjetas == 0)
                return;

            int margenes = 0;

            foreach (Control tarjeta in flpKpis.Controls)
            {
                margenes += tarjeta.Margin.Horizontal;
            }

            // Pequeña holgura para que el FlowLayoutPanel no saque
            // barra de desplazamiento por un píxel de redondeo.
            int disponible = flpKpis.ClientSize.Width - margenes - 4;

            int ancho = Math.Max(AnchoTarjetaMinimo, Math.Min(AnchoTarjetaMaximo, disponible / numeroTarjetas));

            Font fuenteValor = ancho < AnchoTarjetaCompacta ? FuenteValorTarjetaCompacta : FuenteValorTarjeta;

            flpKpis.SuspendLayout();

            foreach (Control tarjeta in flpKpis.Controls)
            {
                tarjeta.Width = ancho;

                Control[] valores = tarjeta.Controls.Find(NombreEtiquetaValor, false);

                if (valores.Length > 0)
                {
                    valores[0].Font = fuenteValor;
                }
            }

            flpKpis.ResumeLayout();
        }

        /// <summary>
        /// Añade a la franja de KPIs un grupo de tarjetas: una por cada
        /// divisa de <paramref name="totalesPorDivisa"/> (título
        /// "<paramref name="titulo"/> &lt;divisa&gt;", importe y número
        /// de documentos). La última tarjeta del grupo lleva un margen
        /// derecho mayor, para separar visualmente un grupo del
        /// siguiente.
        /// </summary>
        private void AgregarGrupoTarjetas(
            string titulo, SortedDictionary<string, Acumulado> totalesPorDivisa, Func<string, Color> colorDeDivisa)
        {
            foreach (KeyValuePair<string, Acumulado> par in totalesPorDivisa)
            {
                flpKpis.Controls.Add(CrearTarjetaKpi(
                    titulo + " " + par.Key,
                    par.Value.Importe.ToString("N2", CulturaDecimal),
                    "N. Doc.: " + par.Value.NumDocumentos.ToString("N0", CulturaDecimal),
                    colorDeDivisa(par.Key)));
            }

            if (totalesPorDivisa.Count == 0)
            {
                // Solo si no hay ninguna divisa dada de alta.
                flpKpis.Controls.Add(CrearTarjetaKpi(
                    titulo,
                    "0,00",
                    "N. Doc.: 0",
                    Color.Gainsboro));
            }

            Control ultimaDelGrupo = flpKpis.Controls[flpKpis.Controls.Count - 1];
            ultimaDelGrupo.Margin = new Padding(0, 0, 35, 0);
        }

        /// <summary>
        /// Importe y número de documentos (tareas) acumulados de un
        /// grupo/divisa del cuadro de mando.
        /// </summary>
        private class Acumulado
        {
            public decimal Importe;

            public int NumDocumentos;

            /// <summary>
            /// Suma al acumulado el IMPORTE y el NUM_DOC de una fila de
            /// las consultas de ResumenPrincipalRepositorio.
            /// </summary>
            public void Sumar(DataRow fila)
            {
                Importe += Convert.ToDecimal(fila["IMPORTE"]);
                NumDocumentos += Convert.ToInt32(fila["NUM_DOC"]);
            }
        }

        /// <summary>
        /// Suma las columnas IMPORTE y NUM_DOC de
        /// <paramref name="datos"/> agrupando por la columna COD_DIV,
        /// ordenado por código de divisa. Incluye siempre todas las
        /// <paramref name="divisas"/> indicadas (a 0 si no tienen
        /// datos), más cualquier otra que aparezca en los datos.
        /// </summary>
        private SortedDictionary<string, Acumulado> SumarPorDivisa(DataTable datos, IEnumerable<string> divisas)
        {
            SortedDictionary<string, Acumulado> totalPorDivisa = new SortedDictionary<string, Acumulado>(StringComparer.Ordinal);

            foreach (string divisa in divisas)
            {
                totalPorDivisa[divisa] = new Acumulado();
            }

            foreach (DataRow fila in datos.Rows)
            {
                if (fila["IMPORTE"] == DBNull.Value)
                    continue;

                string divisa = fila["COD_DIV"].ToString();

                Acumulado acumulado;

                if (!totalPorDivisa.TryGetValue(divisa, out acumulado))
                {
                    acumulado = new Acumulado();
                    totalPorDivisa[divisa] = acumulado;
                }

                acumulado.Sumar(fila);
            }

            return totalPorDivisa;
        }

        /// <summary>
        /// Crea una tarjeta KPI: un panel blanco con una franja de
        /// color a la izquierda, un título pequeño en mayúsculas, el
        /// importe grande debajo y, al pie, el número de documentos,
        /// imitando el aspecto de los indicadores de un cuadro de
        /// mando.
        /// </summary>
        private Panel CrearTarjetaKpi(string titulo, string valor, string detalle, Color colorAcento)
        {
            Panel tarjeta = new Panel
            {
                Size = new Size(230, 110),
                Margin = new Padding(0, 0, 15, 0),
                BackColor = Color.White
            };

            Panel acento = new Panel
            {
                Dock = DockStyle.Left,
                Width = 6,
                BackColor = colorAcento
            };

            // Las etiquetas se anclan a izquierda y derecha para
            // estirarse/encogerse con la tarjeta (ver AjustarAnchoTarjetas).
            AnchorStyles anclaje = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;

            Label lblTitulo = new Label
            {
                AutoSize = false,
                Location = new Point(21, 16),
                Size = new Size(200, 18),
                Anchor = anclaje,
                AutoEllipsis = true,
                Font = FuenteTituloTarjeta,
                ForeColor = Color.Gray,
                Text = titulo
            };

            Label lblValor = new Label
            {
                Name = NombreEtiquetaValor,
                AutoSize = false,
                Location = new Point(18, 38),
                Size = new Size(205, 36),
                Anchor = anclaje,
                Font = FuenteValorTarjeta,
                ForeColor = Color.FromArgb(33, 33, 33),
                Text = valor
            };

            Label lblDetalle = new Label
            {
                AutoSize = false,
                Location = new Point(21, 80),
                Size = new Size(200, 18),
                Anchor = anclaje,
                Font = FuenteDetalleTarjeta,
                ForeColor = Color.DimGray,
                Text = detalle
            };

            tarjeta.Controls.Add(lblDetalle);
            tarjeta.Controls.Add(lblValor);
            tarjeta.Controls.Add(lblTitulo);
            tarjeta.Controls.Add(acento);

            return tarjeta;
        }

        /// <summary>Nombres de las columnas de importe y número de documentos de las rejillas de desglose.</summary>
        private const string ColDocFacturado = "N. Doc. Fact.";
        private const string ColFacturado = "Facturado";
        private const string ColDocPendiente = "N. Doc. Pend.";
        private const string ColPendiente = "Pendiente";

        /// <summary>
        /// Combina los importes facturados y pendientes (ambos con
        /// columnas <paramref name="columnaGrupo"/>, COD_DIV, IMPORTE y
        /// NUM_DOC) en una tabla de desglose con una fila por cada
        /// combinación grupo + divisa que aparezca en cualquiera de
        /// los dos orígenes. Columnas: <paramref name="cabeceraGrupo"/>,
        /// Divisa, N. Doc. Fact., Facturado, N. Doc. Pend. y Pendiente
        /// (DBNull si ese grupo no tiene importe en esa divisa).
        /// Ordenada por grupo y, dentro de cada grupo, por divisa.
        /// </summary>
        private DataTable CrearTablaDesglose(
            DataTable facturado, DataTable pendiente, string columnaGrupo, string cabeceraGrupo)
        {
            // grupo -> divisa -> [facturado, pendiente] (null = sin datos)
            SortedDictionary<string, SortedDictionary<string, Acumulado[]>> importes =
                new SortedDictionary<string, SortedDictionary<string, Acumulado[]>>(StringComparer.CurrentCultureIgnoreCase);

            AcumularDesglose(importes, facturado, columnaGrupo, 0);
            AcumularDesglose(importes, pendiente, columnaGrupo, 1);

            DataTable tabla = new DataTable();
            tabla.Columns.Add(cabeceraGrupo, typeof(string));
            tabla.Columns.Add("Divisa", typeof(string));
            tabla.Columns.Add(ColDocFacturado, typeof(int));
            tabla.Columns.Add(ColFacturado, typeof(decimal));
            tabla.Columns.Add(ColDocPendiente, typeof(int));
            tabla.Columns.Add(ColPendiente, typeof(decimal));

            foreach (KeyValuePair<string, SortedDictionary<string, Acumulado[]>> grupo in importes)
            {
                foreach (KeyValuePair<string, Acumulado[]> divisa in grupo.Value)
                {
                    Acumulado fac = divisa.Value[0];
                    Acumulado pen = divisa.Value[1];

                    tabla.Rows.Add(
                        grupo.Key,
                        divisa.Key,
                        fac != null ? (object)fac.NumDocumentos : DBNull.Value,
                        fac != null ? (object)fac.Importe : DBNull.Value,
                        pen != null ? (object)pen.NumDocumentos : DBNull.Value,
                        pen != null ? (object)pen.Importe : DBNull.Value);
                }
            }

            return tabla;
        }

        /// <summary>
        /// Suma los importes y documentos de <paramref name="datos"/>
        /// en la posición <paramref name="indice"/> (0 = facturado,
        /// 1 = pendiente) de la entrada grupo + divisa correspondiente
        /// de <paramref name="importes"/>, creándola si no existe.
        /// </summary>
        private void AcumularDesglose(
            SortedDictionary<string, SortedDictionary<string, Acumulado[]>> importes,
            DataTable datos, string columnaGrupo, int indice)
        {
            foreach (DataRow fila in datos.Rows)
            {
                if (fila["IMPORTE"] == DBNull.Value)
                    continue;

                string grupo = fila[columnaGrupo].ToString();
                string divisa = fila["COD_DIV"].ToString();

                SortedDictionary<string, Acumulado[]> porDivisa;

                if (!importes.TryGetValue(grupo, out porDivisa))
                {
                    porDivisa = new SortedDictionary<string, Acumulado[]>(StringComparer.Ordinal);
                    importes[grupo] = porDivisa;
                }

                Acumulado[] valores;

                if (!porDivisa.TryGetValue(divisa, out valores))
                {
                    valores = new Acumulado[2];
                    porDivisa[divisa] = valores;
                }

                if (valores[indice] == null)
                    valores[indice] = new Acumulado();

                valores[indice].Sumar(fila);
            }
        }

        /// <summary>
        /// Enlaza una tabla de desglose (ver CrearTablaDesglose) a su
        /// rejilla y le da formato: importes con dos decimales
        /// alineados a la derecha, número de documentos centrado ("-"
        /// donde no hay datos), columna Divisa estrecha y centrada, lo
        /// pendiente en naranja, y sin ordenación por columnas (para
        /// que las filas de un mismo grupo sigan juntas: ver
        /// GridDesglose_CellFormatting).
        /// </summary>
        private void PintarDesglose(DataGridView grid, string cabeceraGrupo, DataTable tabla)
        {
            grid.DataSource = tabla;

            if (grid.Columns.Count == 0)
                return;

            foreach (DataGridViewColumn columna in grid.Columns)
            {
                columna.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            grid.Columns[cabeceraGrupo].FillWeight = 140;
            grid.Columns[cabeceraGrupo].DefaultCellStyle.Font = grid.ColumnHeadersDefaultCellStyle.Font;
            grid.Columns[cabeceraGrupo].HeaderCell.Style.ForeColor = ColorTextoCabeceraGrupo;
            grid.Columns[cabeceraGrupo].HeaderCell.Style.SelectionForeColor = ColorTextoCabeceraGrupo;

            grid.Columns["Divisa"].FillWeight = 50;
            grid.Columns["Divisa"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grid.Columns["Divisa"].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;

            foreach (string nombre in new[] { ColFacturado, ColPendiente })
            {
                DataGridViewColumn columna = grid.Columns[nombre];

                columna.DefaultCellStyle.Format = "N2";
                columna.DefaultCellStyle.FormatProvider = CulturaDecimal;
                columna.DefaultCellStyle.NullValue = "-";
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleRight;
            }

            foreach (string nombre in new[] { ColDocFacturado, ColDocPendiente })
            {
                DataGridViewColumn columna = grid.Columns[nombre];

                columna.FillWeight = 60;
                columna.DefaultCellStyle.Format = "N0";
                columna.DefaultCellStyle.FormatProvider = CulturaDecimal;
                columna.DefaultCellStyle.NullValue = "-";
                columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                columna.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            foreach (string nombre in new[] { ColDocPendiente, ColPendiente })
            {
                grid.Columns[nombre].DefaultCellStyle.ForeColor = ColorPendiente;
                grid.Columns[nombre].DefaultCellStyle.SelectionForeColor = ColorPendiente;
            }

            grid.ClearSelection();
        }

        /// <summary>
        /// En las rejillas de desglose, deja en blanco el nombre del
        /// grupo (usuario/segmento/tipo de documento) cuando se repite respecto a la
        /// fila anterior, para que las filas de cada divisa se lean
        /// como un bloque bajo su grupo, como en un informe.
        /// </summary>
        private void GridDesglose_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex != 0 || e.RowIndex <= 0)
                return;

            DataGridView grid = (DataGridView)sender;

            object anterior = grid.Rows[e.RowIndex - 1].Cells[0].Value;

            if (Equals(anterior, e.Value))
            {
                e.Value = string.Empty;
                e.FormattingApplied = true;
            }
        }

        /// <summary>
        /// Muestra las etiquetas numéricas del eje Y de la gráfica
        /// mensual abreviadas (p. ej. 250 K, 1,5 M), para que se lean
        /// de un vistazo aunque los importes sean grandes.
        /// </summary>
        private void chartMensual_FormatNumber(object sender, FormatNumberEventArgs e)
        {
            if (e.ElementType != ChartElementType.AxisLabels)
                return;

            Axis eje = sender as Axis;

            if (eje == null || eje.AxisName != AxisName.Y)
                return;

            e.LocalizedValue = AbreviarImporte(e.Value);
        }

        /// <summary>
        /// Abrevia un importe con sufijos: K (miles), M (millones) y
        /// MM (miles de millones), con un decimal como máximo (p. ej.
        /// 1.250 → "1,3 K", 2.500.000 → "2,5 M"). Por debajo de mil
        /// se muestra sin decimales y sin sufijo.
        /// </summary>
        private static string AbreviarImporte(double valor)
        {
            double absoluto = Math.Abs(valor);

            if (absoluto >= 1e9)
                return (valor / 1e9).ToString("0.#", CulturaDecimal) + " MM";

            if (absoluto >= 1e6)
                return (valor / 1e6).ToString("0.#", CulturaDecimal) + " M";

            if (absoluto >= 1e3)
                return (valor / 1e3).ToString("0.#", CulturaDecimal) + " K";

            return valor.ToString("0", CulturaDecimal);
        }

        /// <summary>
        /// Vuelca el resultado (MES, COD_DIV, IMPORTE) sobre la
        /// gráfica de barras: una serie por cada divisa activa (y por
        /// cualquier otra con datos ese año), cada una con su color de
        /// PaletaDivisas, con un punto
        /// por cada uno de los 12 meses (0 donde no haya importe),
        /// para que las barras de las distintas divisas se agrupen
        /// mes a mes. Cada barra con importe lleva encima su valor
        /// abreviado (ver AbreviarImporte) y, al pasar el ratón, el
        /// importe completo. El eje Y se reescala a los datos en cada
        /// recarga (ver AjustarEjeY).
        /// </summary>
        private void PintarGraficoMensual(DataTable facturadoMensual, IEnumerable<string> divisasActivas)
        {
            chartMensual.Series.Clear();

            SortedSet<string> divisas = new SortedSet<string>(divisasActivas, StringComparer.Ordinal);
            Dictionary<string, decimal> valores = new Dictionary<string, decimal>();
            decimal maximo = 0m;

            foreach (DataRow fila in facturadoMensual.Rows)
            {
                int mes = Convert.ToInt32(fila["MES"]);
                string divisa = fila["COD_DIV"].ToString();
                decimal importe = Convert.ToDecimal(fila["IMPORTE"]);

                divisas.Add(divisa);
                valores[divisa + "|" + mes] = importe;
                maximo = Math.Max(maximo, importe);
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

                    int indice = serie.Points.AddXY(etiquetaMes, importe);

                    if (importe != 0m)
                    {
                        DataPoint punto = serie.Points[indice];

                        punto.Label = AbreviarImporte((double)importe);
                        punto.ToolTip = etiquetaMes + ": " + importe.ToString("N2", CulturaDecimal) + " " + divisa;
                    }
                }

                serie.Font = FuenteEtiquetasGrafica;
                serie.LabelForeColor = Color.FromArgb(66, 66, 66);

                chartMensual.Series.Add(serie);
            }

            AjustarEjeY((double)maximo);
        }

        /// <summary>
        /// Ajusta la escala del eje Y de la gráfica mensual al mayor
        /// importe mostrado: desde 0 hasta un valor "redondo" algo
        /// superior al máximo (con margen para que quepa la etiqueta
        /// encima de la barra más alta), dividido en unas 5 marcas.
        /// Se fija explícitamente en cada recarga en vez de dejarlo en
        /// automático, porque el control no siempre recalcula la
        /// escala al sustituir las series y una barra nueva mucho
        /// mayor que las anteriores podía quedar fuera del área.
        /// </summary>
        private void AjustarEjeY(double maximo)
        {
            Axis ejeY = chartMensual.ChartAreas["AreaPrincipal"].AxisY;

            const int numeroMarcas = 5;
            const double margenSuperior = 1.15;

            double tope = maximo > 0 ? maximo * margenSuperior : 1000;

            // Intervalo "redondo" (1, 2, 2,5 o 5 por potencia de 10).
            double bruto = tope / numeroMarcas;
            double magnitud = Math.Pow(10, Math.Floor(Math.Log10(bruto)));
            double relativo = bruto / magnitud;

            double paso = relativo <= 1 ? 1
                : relativo <= 2 ? 2
                : relativo <= 2.5 ? 2.5
                : relativo <= 5 ? 5
                : 10;

            double intervalo = paso * magnitud;

            ejeY.Minimum = 0;
            ejeY.Maximum = Math.Ceiling(tope / intervalo) * intervalo;
            ejeY.Interval = intervalo;

            chartMensual.Invalidate();
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

        /// <summary>
        /// Abre, como diálogo modal, el informe de motivos de bloqueo.
        /// </summary>
        private void motivosBloqueoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeMotivosBloqueo formulario = new FrmInformeMotivosBloqueo();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de antigüedad de
        /// tareas abiertas.
        /// </summary>
        private void antiguedadTareasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeAntiguedadTareas formulario = new FrmInformeAntiguedadTareas();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de ranking de usuarios.
        /// </summary>
        private void rankingUsuariosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeRankingUsuarios formulario = new FrmInformeRankingUsuarios();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de facturación por
        /// proyecto/segmento.
        /// </summary>
        private void facturacionProyectoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeFacturacionProyecto formulario = new FrmInformeFacturacionProyecto();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de desviación entre
        /// importe estimado e importe facturado.
        /// </summary>
        private void desviacionEstimadoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeDesviacion formulario = new FrmInformeDesviacion();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de trazabilidad de una
        /// tarea.
        /// </summary>
        private void trazabilidadTareaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeTrazabilidad formulario = new FrmInformeTrazabilidad();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, el informe de cancelaciones.
        /// </summary>
        private void cancelacionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInformeCancelaciones formulario = new FrmInformeCancelaciones();

            formulario.ShowDialog();
        }
    }
}
