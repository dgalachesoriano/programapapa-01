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
    /// Informe "Trazabilidad de Tarea" (menú Informes): busca una
    /// tarea por documento y muestra, en orden cronológico, la línea
    /// de tiempo completa de sus eventos (TBL_CONTROL): alta,
    /// asignaciones, bloqueos con su motivo y facturación con sus
    /// datos. Es el único informe que explota directamente el
    /// histórico append-only: el resto de pantallas solo muestran el
    /// evento vigente de cada tarea.
    /// </summary>
    public partial class FrmInformeTrazabilidad : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly TareaRepositorio tareaRepositorio = new TareaRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");

        private TextBox txtBuscarDocumento;
        private DataGridView dgvResultados;
        private Label lblTareaSeleccionada;
        private Button btnAbrirTarea;
        private FlowLayoutPanel flpLineaTiempo;

        private int? idTareaSeleccionada;

        public FrmInformeTrazabilidad()
        {
            InitializeComponent();

            ConstruirInterfaz();
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

            GroupBox grpBusqueda = ConstruirPanelBusqueda();

            SplitContainer split = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 200,
                SplitterWidth = 8
            };

            GroupBox grpResultados = ConstruirPanelResultados();
            GroupBox grpLineaTiempo = ConstruirPanelLineaTiempo();

            split.Panel1.Controls.Add(grpResultados);
            split.Panel2.Controls.Add(grpLineaTiempo);

            pnlCanvas.Controls.Add(split);
            pnlCanvas.Controls.Add(grpBusqueda);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelBusqueda()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Top, Height = 70, Text = "Buscar tarea por documento" };

            Label lblDocumento = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Documento:" };

            txtBuscarDocumento = new TextBox { Location = new Point(90, 30), Size = new Size(300, 21) };
            txtBuscarDocumento.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Buscar();
                    e.SuppressKeyPress = true;
                }
            };

            Button btnBuscar = new Button { Text = "Buscar", Location = new Point(400, 27), Size = new Size(100, 30) };
            btnBuscar.Click += (s, e) => Buscar();

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(1000, 27),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

            grupo.Controls.Add(lblDocumento);
            grupo.Controls.Add(txtBuscarDocumento);
            grupo.Controls.Add(btnBuscar);
            grupo.Controls.Add(btnCerrar);

            return grupo;
        }

        private GroupBox ConstruirPanelResultados()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Resultados de la búsqueda" };

            dgvResultados = new DataGridView
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

            EstiloDashboard.EstilizarGrid(dgvResultados);

            dgvResultados.DataBindingComplete += (s, e) => FormateadorGridTareas.AplicarFormato(dgvResultados);
            dgvResultados.SelectionChanged += dgvResultados_SelectionChanged;

            grupo.Controls.Add(dgvResultados);

            return grupo;
        }

        private GroupBox ConstruirPanelLineaTiempo()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Fill, Padding = new Padding(12), Text = "Línea de tiempo" };

            Panel pnlCabecera = new Panel { Dock = DockStyle.Top, Height = 30 };

            lblTareaSeleccionada = new Label
            {
                AutoSize = true,
                Location = new Point(0, 7),
                Font = new Font(this.Font, FontStyle.Bold),
                Text = "Seleccione una tarea arriba para ver su historial."
            };

            btnAbrirTarea = new Button
            {
                Text = "Abrir tarea",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(890, 0),
                Size = new Size(100, 26),
                Enabled = false
            };
            btnAbrirTarea.Click += btnAbrirTarea_Click;

            pnlCabecera.Controls.Add(lblTareaSeleccionada);
            pnlCabecera.Controls.Add(btnAbrirTarea);

            Panel pnlScroll = new Panel { Dock = DockStyle.Fill, AutoScroll = true, BackColor = Color.White };

            flpLineaTiempo = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Padding = new Padding(10)
            };

            pnlScroll.Controls.Add(flpLineaTiempo);

            grupo.Controls.Add(pnlScroll);
            grupo.Controls.Add(pnlCabecera);

            return grupo;
        }

        private void Buscar()
        {
            try
            {
                DataTable resultado = tareaRepositorio.BuscarPorFiltro(new FiltroBusquedaTareas
                {
                    Documento = txtBuscarDocumento.Text.Trim()
                });

                dgvResultados.DataSource = resultado;

                if (resultado.Rows.Count == 0)
                {
                    Dialogos.MostrarInformacion(
                        "No se ha encontrado ninguna tarea con ese documento.",
                        "Trazabilidad de Tarea");
                }
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeTrazabilidad.Buscar",
                    "No se ha podido buscar la tarea.",
                    ex);
            }
        }

        private void dgvResultados_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvResultados.CurrentRow == null || dgvResultados.CurrentRow.Cells["ID_TAREA"].Value == null)
            {
                idTareaSeleccionada = null;
                btnAbrirTarea.Enabled = false;
                return;
            }

            int idTarea = Convert.ToInt32(dgvResultados.CurrentRow.Cells["ID_TAREA"].Value);
            string documento = dgvResultados.CurrentRow.Cells["DES_DOC"].Value.ToString();

            idTareaSeleccionada = idTarea;
            btnAbrirTarea.Enabled = true;

            CargarLineaTiempo(idTarea, documento);
        }

        private void CargarLineaTiempo(int idTarea, string documento)
        {
            try
            {
                List<EventoTarea> eventos = informeRepositorio.ObtenerTrazabilidad(idTarea);

                lblTareaSeleccionada.Text = "Historial de la tarea: " + documento + " (" + eventos.Count + " evento(s))";

                flpLineaTiempo.Controls.Clear();
                flpLineaTiempo.SuspendLayout();

                foreach (EventoTarea evento in eventos)
                {
                    flpLineaTiempo.Controls.Add(CrearTarjetaEvento(evento));
                }

                flpLineaTiempo.ResumeLayout(true);
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeTrazabilidad.CargarLineaTiempo",
                    "No se ha podido cargar el historial de la tarea.",
                    ex);
            }
        }

        /// <summary>
        /// Traduce el nombre del estado al color con que se muestra su
        /// franja de acento en la tarjeta de evento (misma familia de
        /// colores que el resto de la aplicación).
        /// </summary>
        private Color ColorDeEstado(string descripcionEstado)
        {
            switch (descripcionEstado)
            {
                case "Registrado": return Color.FromArgb(97, 97, 97);
                case "En proceso": return Color.FromArgb(21, 101, 192);
                case "Pendiente": return Color.FromArgb(245, 127, 23);
                case "Facturado": return Color.FromArgb(46, 125, 50);
                case "Cancelado": return Color.FromArgb(198, 40, 40);
                default: return Color.Gray;
            }
        }

        /// <summary>
        /// Crea una "tarjeta de evento" para la línea de tiempo: una
        /// franja de acento coloreada según el estado, la fecha y el
        /// estado en negrita, y el resto de detalles (usuario, motivo,
        /// factura) en líneas debajo.
        /// </summary>
        private Panel CrearTarjetaEvento(EventoTarea evento)
        {
            Panel tarjeta = new Panel
            {
                Width = 850,
                Height = 80,
                Margin = new Padding(0, 0, 0, 10),
                BackColor = Color.White
            };

            Panel acento = new Panel { Dock = DockStyle.Left, Width = 6, BackColor = ColorDeEstado(evento.DescripcionEstado) };

            Label lblFechaEstado = new Label
            {
                AutoSize = true,
                Location = new Point(21, 10),
                Font = new Font(this.Font.FontFamily, 10F, FontStyle.Bold),
                Text = evento.Fecha.ToString("dd/MM/yyyy", CulturaDecimal) + "  —  " + evento.DescripcionEstado
            };

            string detalle = ConstruirDetalleEvento(evento);

            Label lblDetalle = new Label
            {
                AutoSize = true,
                Location = new Point(21, 34),
                ForeColor = Color.DimGray,
                MaximumSize = new Size(800, 0),
                Text = detalle
            };

            tarjeta.Controls.Add(lblDetalle);
            tarjeta.Controls.Add(lblFechaEstado);
            tarjeta.Controls.Add(acento);

            return tarjeta;
        }

        private string ConstruirDetalleEvento(EventoTarea evento)
        {
            List<string> partes = new List<string>();

            partes.Add("Usuario: " + (evento.NombreUsuario ?? "(sin asignar)"));

            if (!string.IsNullOrEmpty(evento.DescripcionMotivo))
                partes.Add("Motivo: " + evento.DescripcionMotivo);

            if (!string.IsNullOrEmpty(evento.CodigoFactura))
            {
                string importe = evento.ImporteFactura.HasValue
                    ? evento.ImporteFactura.Value.ToString("N2", CulturaDecimal) + " " + evento.CodigoDivisa
                    : "";

                partes.Add("Factura: " + evento.CodigoFactura + (importe != "" ? " — " + importe : ""));
            }

            return string.Join("    ·    ", partes);
        }

        private void btnAbrirTarea_Click(object sender, EventArgs e)
        {
            if (!idTareaSeleccionada.HasValue)
                return;

            using (FrmDetalleTarea formulario = new FrmDetalleTarea(idTareaSeleccionada.Value))
            {
                formulario.ShowDialog();
            }

            CargarLineaTiempo(idTareaSeleccionada.Value, dgvResultados.CurrentRow.Cells["DES_DOC"].Value.ToString());
        }
    }
}
