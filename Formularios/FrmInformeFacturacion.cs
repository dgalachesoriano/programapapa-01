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
    /// Informe de Facturación (menú Informes &gt; Facturación):
    /// gráfica de líneas con la evolución mensual de tres importes -
    /// facturado, pendiente de facturar (estimado de las tareas "En
    /// Proceso") y bloqueado (estimado de las tareas "Bloqueado") -,
    /// filtrable por rango de fechas y usuario.
    /// </summary>
    public partial class FrmInformeFacturacion : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly UsuarioRepositorio usuarioRepositorio = new UsuarioRepositorio();

        /// <summary>
        /// Valor usado en el combo de filtro de usuario para
        /// representar "no filtrar por este campo".
        /// </summary>
        private const int IdTodos = 0;

        // Nombres de serie: deben coincidir con la columna SERIE que
        // devuelve InformeRepositorio.ObtenerResumenFacturacion.
        private const string SerieFacturado = "FACTURADO";
        private const string SeriePendiente = "PENDIENTE";
        private const string SerieBloqueado = "BLOQUEADO";

        public FrmInformeFacturacion()
        {
            InitializeComponent();

            ConfigurarGrafica();
            CargarFiltroUsuarios();

            GenerarGrafica();
        }

        /// <summary>
        /// Prepara el área de la gráfica y da de alta las tres
        /// series (una línea por cada importe), sin puntos todavía.
        /// </summary>
        private void ConfigurarGrafica()
        {
            chartFacturacion.Series.Clear();
            chartFacturacion.ChartAreas.Clear();
            chartFacturacion.Legends.Clear();

            ChartArea area = new ChartArea("AreaPrincipal");
            area.AxisX.Title = "Mes";
            area.AxisY.Title = "Importe (€)";
            area.AxisX.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisY.MajorGrid.LineColor = Color.Gainsboro;
            area.AxisX.Interval = 1;

            chartFacturacion.ChartAreas.Add(area);
            chartFacturacion.Legends.Add(new Legend("Leyenda"));

            AgregarSerie(SerieFacturado, "Facturado", Color.FromArgb(76, 175, 80));
            AgregarSerie(SeriePendiente, "Pendiente de Facturar", Color.FromArgb(33, 150, 243));
            AgregarSerie(SerieBloqueado, "Bloqueado", Color.FromArgb(244, 67, 54));
        }

        /// <summary>
        /// Da de alta una serie de tipo línea en la gráfica.
        /// </summary>
        private void AgregarSerie(string nombre, string etiquetaLeyenda, Color color)
        {
            Series serie = new Series(nombre)
            {
                ChartType = SeriesChartType.Line,
                BorderWidth = 3,
                Color = color,
                LegendText = etiquetaLeyenda,
                ChartArea = "AreaPrincipal"
            };

            chartFacturacion.Series.Add(serie);
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
                    "FrmInformeFacturacion.CargarFiltroUsuarios",
                    "No se han podido cargar los usuarios.",
                    ex);
            }
        }

        /// <summary>
        /// Relanza la generación de la gráfica aplicando los filtros
        /// actuales de pantalla.
        /// </summary>
        private void btnAplicarFiltros_Click(object sender, EventArgs e)
        {
            GenerarGrafica();
        }

        /// <summary>
        /// Restablece todos los filtros a "sin filtrar" y vuelve a
        /// generar la gráfica.
        /// </summary>
        private void btnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            dtpFechaDesde.Checked = false;
            dtpFechaHasta.Checked = false;
            cboFiltroUsuario.SelectedIndex = 0;

            GenerarGrafica();
        }

        /// <summary>
        /// Construye el filtro a partir de los controles de pantalla,
        /// consulta el resumen mensual y lo vuelca sobre la gráfica.
        /// </summary>
        private void GenerarGrafica()
        {
            try
            {
                DateTime? fechaDesde = dtpFechaDesde.Checked ? (DateTime?)dtpFechaDesde.Value.Date : null;
                DateTime? fechaHasta = dtpFechaHasta.Checked ? (DateTime?)dtpFechaHasta.Value.Date : null;

                int idUsuario = cboFiltroUsuario.SelectedValue != null
                    ? Convert.ToInt32(cboFiltroUsuario.SelectedValue)
                    : IdTodos;

                DataTable resumen = informeRepositorio.ObtenerResumenFacturacion(
                    fechaDesde, fechaHasta, idUsuario);

                PintarGrafica(resumen);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeFacturacion.GenerarGrafica",
                    "No se ha podido generar la gráfica.",
                    ex);
            }
        }

        /// <summary>
        /// Vuelca el resultado (PERIODO, SERIE, IMPORTE) sobre las
        /// tres series de la gráfica. Calcula primero el conjunto de
        /// todos los meses presentes en cualquiera de las series, y
        /// añade un punto por cada mes en las tres (con 0 donde no
        /// haya importe), para que las tres compartan el mismo eje X
        /// y las líneas queden alineadas.
        /// </summary>
        private void PintarGrafica(DataTable resumen)
        {
            foreach (Series serie in chartFacturacion.Series)
            {
                serie.Points.Clear();
            }

            SortedSet<string> periodos = new SortedSet<string>(StringComparer.Ordinal);
            Dictionary<string, decimal> valores = new Dictionary<string, decimal>();

            foreach (DataRow fila in resumen.Rows)
            {
                string periodo = fila["PERIODO"].ToString();
                string nombreSerie = fila["SERIE"].ToString();

                decimal importe = fila["IMPORTE"] == DBNull.Value
                    ? 0m
                    : Convert.ToDecimal(fila["IMPORTE"]);

                periodos.Add(periodo);
                valores[nombreSerie + "|" + periodo] = importe;
            }

            if (periodos.Count == 0)
                return;

            foreach (string periodo in periodos)
            {
                string etiqueta = FormatearPeriodo(periodo);

                foreach (Series serie in chartFacturacion.Series)
                {
                    decimal importe;

                    valores.TryGetValue(serie.Name + "|" + periodo, out importe);

                    serie.Points.AddXY(etiqueta, importe);
                }
            }
        }

        /// <summary>
        /// Convierte un periodo "yyyy-MM" en una etiqueta legible
        /// para el eje X (p. ej. "sept. 2026"). Si por algún motivo
        /// el formato no se puede interpretar, devuelve el propio
        /// periodo tal cual.
        /// </summary>
        private string FormatearPeriodo(string periodoIso)
        {
            DateTime fecha;

            bool valido = DateTime.TryParseExact(
                periodoIso,
                "yyyy-MM",
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out fecha);

            if (!valido)
                return periodoIso;

            return fecha.ToString("MMM yyyy", new CultureInfo("es-ES"));
        }

        /// <summary>
        /// Cierra la pantalla del informe.
        /// </summary>
        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
