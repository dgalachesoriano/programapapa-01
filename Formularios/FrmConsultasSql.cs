using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Pantalla "Consultas SQL" (menú Datos): permite elegir una de
    /// las consultas predefinidas de TBL_CONSULTAS (con buscador por
    /// nombre), rellenar sus parámetros (:Nombre), previsualizar los
    /// 10 primeros registros y exportar el resultado completo a
    /// Excel. Las consultas se dan de alta a mano en la BBDD (ver
    /// ScriptsBBDD/v2/AltaConsulta.sql); esta pantalla solo las lanza.
    /// </summary>
    public partial class FrmConsultasSql : Form
    {
        private readonly ConsultaRepositorio consultaRepositorio = new ConsultaRepositorio();

        private const int FilasPrevisualizacion = 10;

        private TextBox txtBuscar;
        private ComboBox cboConsultas;
        private Label lblDescripcion;
        private GroupBox grpParametros;
        private FlowLayoutPanel flpParametros;
        private Button btnEjecutar;
        private GroupBox grpPrevisualizacion;
        private DataGridView dgvPrevisualizacion;
        private Button btnExportar;

        private List<Consulta> todasLasConsultas = new List<Consulta>();
        private Consulta consultaActual;

        /// <summary>Caja de texto de cada parámetro de la consulta actual, por nombre.</summary>
        private readonly Dictionary<string, TextBox> camposParametros =
            new Dictionary<string, TextBox>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Evita reaccionar al SelectedIndexChanged que lanza el propio
        /// ComboBox al volver a enlazarlo tras filtrar.
        /// </summary>
        private bool refrescandoLista;

        public FrmConsultasSql()
        {
            InitializeComponent();

            ConstruirInterfaz();
            CargarConsultas();
        }

        /// <summary>
        /// Construye toda la interfaz por código: selección de consulta,
        /// parámetros, rejilla de previsualización y botón de
        /// exportación (esquina inferior derecha).
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

            GroupBox grpConsulta = ConstruirPanelConsulta();
            grpParametros = ConstruirPanelParametros();
            grpPrevisualizacion = ConstruirPanelPrevisualizacion();
            FlowLayoutPanel pnlBotones = ConstruirPanelBotones();

            // Orden de adición: el Dock=Fill va primero para que ocupe
            // lo que dejan libre los demás, y los Dock=Top en orden
            // inverso al visual (los últimos añadidos quedan arriba).
            pnlCanvas.Controls.Add(grpPrevisualizacion);
            pnlCanvas.Controls.Add(pnlBotones);
            pnlCanvas.Controls.Add(grpParametros);
            pnlCanvas.Controls.Add(grpConsulta);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelConsulta()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Top,
                Height = 100,
                Text = "Consulta"
            };

            Label lblBuscar = new Label { AutoSize = true, Location = new Point(12, 33), Text = "Buscar:" };
            txtBuscar = new TextBox
            {
                Location = new Point(70, 30),
                Size = new Size(220, 21)
            };
            txtBuscar.TextChanged += (s, e) => FiltrarConsultas();

            Label lblConsulta = new Label { AutoSize = true, Location = new Point(315, 33), Text = "Consulta:" };
            cboConsultas = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Location = new Point(380, 30),
                Size = new Size(420, 21)
            };
            cboConsultas.SelectedIndexChanged += cboConsultas_SelectedIndexChanged;

            Button btnCerrar = new Button
            {
                Text = "Cerrar",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(960, 25),
                Size = new Size(100, 30)
            };
            btnCerrar.Click += (s, e) => Close();

            lblDescripcion = new Label
            {
                AutoSize = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                Location = new Point(12, 62),
                Size = new Size(1040, 30),
                Font = new Font(this.Font, FontStyle.Italic),
                ForeColor = Color.Gray,
                Text = string.Empty
            };

            grupo.Controls.Add(lblBuscar);
            grupo.Controls.Add(txtBuscar);
            grupo.Controls.Add(lblConsulta);
            grupo.Controls.Add(cboConsultas);
            grupo.Controls.Add(btnCerrar);
            grupo.Controls.Add(lblDescripcion);

            return grupo;
        }

        /// <summary>
        /// Panel con una etiqueta + caja de texto por cada parámetro
        /// (:Nombre) de la consulta seleccionada y el botón Ejecutar.
        /// Solo se muestra cuando la consulta tiene parámetros.
        /// </summary>
        private GroupBox ConstruirPanelParametros()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Top,
                Height = 100,
                Padding = new Padding(12, 8, 12, 8),
                Text = "Parámetros",
                Visible = false
            };

            Panel pnlEjecutar = new Panel { Dock = DockStyle.Right, Width = 130 };

            btnEjecutar = new Button
            {
                Text = "Ejecutar",
                Location = new Point(10, 4),
                Size = new Size(120, 30)
            };
            btnEjecutar.Click += (s, e) => Previsualizar();

            pnlEjecutar.Controls.Add(btnEjecutar);

            flpParametros = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = true,
                AutoScroll = true
            };

            grupo.Controls.Add(flpParametros);
            grupo.Controls.Add(pnlEjecutar);

            return grupo;
        }

        private GroupBox ConstruirPanelPrevisualizacion()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                Text = "Previsualización (primeros " + FilasPrevisualizacion + " registros)"
            };

            dgvPrevisualizacion = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoGenerateColumns = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells,
                Enabled = false
            };

            EstiloDashboard.EstilizarGrid(dgvPrevisualizacion);

            dgvPrevisualizacion.DataBindingComplete += (s, e) => FormatearColumnasPrevisualizacion();

            grupo.Controls.Add(dgvPrevisualizacion);

            return grupo;
        }

        private FlowLayoutPanel ConstruirPanelBotones()
        {
            FlowLayoutPanel pnl = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                Height = 45,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(0, 10, 0, 0)
            };

            btnExportar = new Button
            {
                Text = "Exportar a Excel",
                Size = new Size(140, 30),
                Margin = new Padding(0),
                Enabled = false
            };
            btnExportar.Click += (s, e) => ExportarExcel();

            pnl.Controls.Add(btnExportar);

            return pnl;
        }

        /// <summary>
        /// Carga las consultas activas de TBL_CONSULTAS y las muestra
        /// en el desplegable, sin ninguna seleccionada.
        /// </summary>
        private void CargarConsultas()
        {
            try
            {
                todasLasConsultas = consultaRepositorio.ObtenerActivas();
            }
            catch (Exception ex)
            {
                todasLasConsultas = new List<Consulta>();

                Dialogos.MostrarError(
                    "FrmConsultasSql.CargarConsultas",
                    "No se han podido cargar las consultas.",
                    ex);
            }

            FiltrarConsultas();
        }

        /// <summary>
        /// Rellena el desplegable con las consultas cuyo nombre
        /// contiene el texto del buscador (sin distinguir mayúsculas
        /// ni tildes). Si la consulta seleccionada sigue en la lista,
        /// se mantiene; si no, se deja sin selección.
        /// </summary>
        private void FiltrarConsultas()
        {
            string texto = txtBuscar.Text.Trim();
            CompareInfo comparador = CultureInfo.CurrentCulture.CompareInfo;
            const CompareOptions opciones = CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace;

            List<Consulta> filtradas = todasLasConsultas
                .Where(c => texto.Length == 0 || comparador.IndexOf(c.Nombre, texto, opciones) >= 0)
                .ToList();

            refrescandoLista = true;

            try
            {
                cboConsultas.DataSource = filtradas;
                cboConsultas.DisplayMember = "Nombre";
                cboConsultas.ValueMember = "Id";

                int indice = consultaActual == null ? -1 : filtradas.FindIndex(c => c.Id == consultaActual.Id);
                cboConsultas.SelectedIndex = indice;
            }
            finally
            {
                refrescandoLista = false;
            }

            if (cboConsultas.SelectedIndex < 0 && consultaActual != null)
                SeleccionarConsulta(null);
        }

        private void cboConsultas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (refrescandoLista)
                return;

            SeleccionarConsulta(cboConsultas.SelectedItem as Consulta);
        }

        /// <summary>
        /// Prepara la pantalla para la consulta elegida: descripción,
        /// campos de parámetros y rejilla vacía. Si la consulta no
        /// tiene parámetros se previsualiza directamente; si los
        /// tiene, se espera a que el usuario los rellene y pulse
        /// Ejecutar.
        /// </summary>
        private void SeleccionarConsulta(Consulta consulta)
        {
            consultaActual = consulta;

            lblDescripcion.Text = consulta == null ? string.Empty : consulta.Descripcion;
            btnExportar.Enabled = consulta != null;

            LimpiarPrevisualizacion();

            List<string> parametros = consulta == null
                ? new List<string>()
                : ParametrosConsultaServicio.ExtraerParametros(consulta.TextoSql);

            ConstruirCamposParametros(parametros);

            if (consulta != null && parametros.Count == 0)
                Previsualizar();
        }

        private void ConstruirCamposParametros(List<string> parametros)
        {
            flpParametros.SuspendLayout();

            foreach (Control control in flpParametros.Controls.Cast<Control>().ToList())
                control.Dispose();

            flpParametros.Controls.Clear();
            camposParametros.Clear();

            foreach (string nombre in parametros)
            {
                Panel pnlParametro = new Panel { Size = new Size(290, 30), Margin = new Padding(0, 0, 10, 0) };

                Label lbl = new Label
                {
                    AutoSize = false,
                    Location = new Point(0, 6),
                    Size = new Size(120, 18),
                    TextAlign = ContentAlignment.MiddleRight,
                    AutoEllipsis = true,
                    Text = nombre + ":"
                };

                TextBox txt = new TextBox
                {
                    Location = new Point(125, 3),
                    Size = new Size(160, 21)
                };

                pnlParametro.Controls.Add(lbl);
                pnlParametro.Controls.Add(txt);
                flpParametros.Controls.Add(pnlParametro);

                camposParametros[nombre] = txt;
            }

            flpParametros.ResumeLayout();

            grpParametros.Visible = parametros.Count > 0;

            // Con parámetros, Intro en cualquier caja lanza la consulta.
            this.AcceptButton = parametros.Count > 0 ? btnEjecutar : null;

            if (parametros.Count > 0)
                camposParametros[parametros[0]].Focus();
        }

        private Dictionary<string, string> LeerValoresParametros()
        {
            return camposParametros.ToDictionary(
                par => par.Key,
                par => par.Value.Text.Trim(),
                StringComparer.OrdinalIgnoreCase);
        }

        private void LimpiarPrevisualizacion()
        {
            dgvPrevisualizacion.DataSource = null;
            dgvPrevisualizacion.Enabled = false;
        }

        /// <summary>
        /// Ejecuta la consulta actual con los valores de parámetros
        /// introducidos y muestra sus primeros registros en la rejilla.
        /// </summary>
        private void Previsualizar()
        {
            if (consultaActual == null)
                return;

            Cursor cursorAnterior = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable tabla = consultaRepositorio.Ejecutar(
                    consultaActual, LeerValoresParametros(), FilasPrevisualizacion);

                dgvPrevisualizacion.DataSource = tabla;
                dgvPrevisualizacion.Enabled = true;
            }
            catch (Exception ex)
            {
                LimpiarPrevisualizacion();

                Dialogos.MostrarError(
                    "FrmConsultasSql.Previsualizar",
                    "No se ha podido ejecutar la consulta '" + consultaActual.Nombre + "'.",
                    ex);
            }
            finally
            {
                this.Cursor = cursorAnterior;
            }
        }

        /// <summary>
        /// Ejecuta la consulta completa (sin límite de filas) con los
        /// valores de parámetros actuales y la abre en Excel como Tabla.
        /// </summary>
        private void ExportarExcel()
        {
            if (consultaActual == null)
                return;

            Cursor cursorAnterior = this.Cursor;

            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable tabla = consultaRepositorio.Ejecutar(consultaActual, LeerValoresParametros(), null);

                if (tabla.Rows.Count == 0)
                {
                    Dialogos.MostrarInformacion("La consulta no ha devuelto ningún registro.", "Exportar a Excel");
                    return;
                }

                ExportadorExcel.Exportar(tabla, consultaActual.Nombre);
            }
            catch (InvalidOperationException ex)
            {
                // Avisos previstos de ExportadorExcel (sin Excel instalado, demasiadas filas...).
                Dialogos.MostrarAviso(ex.Message, "Exportar a Excel");
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmConsultasSql.ExportarExcel",
                    "No se ha podido exportar la consulta '" + consultaActual.Nombre + "' a Excel.",
                    ex);
            }
            finally
            {
                this.Cursor = cursorAnterior;
            }
        }

        /// <summary>
        /// Formato de las columnas según su tipo, ya que la consulta
        /// es arbitraria y no hay columnas conocidas de antemano:
        /// importes con 2 decimales alineados a la derecha y fechas sin
        /// hora cuando no la tienen.
        /// </summary>
        private void FormatearColumnasPrevisualizacion()
        {
            foreach (DataGridViewColumn columna in dgvPrevisualizacion.Columns)
            {
                Type tipo = columna.ValueType;

                if (tipo == typeof(decimal) || tipo == typeof(double) || tipo == typeof(float))
                {
                    columna.DefaultCellStyle.Format = "N2";
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (tipo == typeof(int) || tipo == typeof(long) || tipo == typeof(short) || tipo == typeof(byte))
                {
                    columna.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (tipo == typeof(DateTime) && !ColumnaTieneHora(columna))
                {
                    columna.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }
        }

        private bool ColumnaTieneHora(DataGridViewColumn columna)
        {
            return dgvPrevisualizacion.Rows
                .Cast<DataGridViewRow>()
                .Select(fila => fila.Cells[columna.Index].Value)
                .OfType<DateTime>()
                .Any(fecha => fecha.TimeOfDay != TimeSpan.Zero);
        }
    }
}
