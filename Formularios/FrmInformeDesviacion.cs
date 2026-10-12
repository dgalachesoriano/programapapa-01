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
    /// Informe "Desviación Estimado vs. Facturado" (menú Informes):
    /// para las tareas ya facturadas con Importe Estimado informado,
    /// compara ese importe con el finalmente facturado, para detectar
    /// estimaciones poco fiables. El Importe Estimado no tiene divisa
    /// propia (solo la factura la tiene): la desviación compara
    /// magnitudes, no hace conversión de cambio, así que hay que
    /// interpretarla con cautela cuando la factura no está en la
    /// divisa "habitual". Rejilla ordenable y coloreada por magnitud
    /// de la desviación.
    /// </summary>
    public partial class FrmInformeDesviacion : Form
    {
        private readonly InformeRepositorio informeRepositorio = new InformeRepositorio();
        private readonly ProyectoRepositorio proyectoRepositorio = new ProyectoRepositorio();

        private static readonly CultureInfo CulturaDecimal = new CultureInfo("es-ES");
        private const int IdTodos = 0;

        private static readonly Color ColorDesviacionBaja = Color.FromArgb(200, 230, 201);
        private static readonly Color ColorDesviacionMedia = Color.FromArgb(255, 245, 157);
        private static readonly Color ColorDesviacionAlta = Color.FromArgb(255, 205, 210);

        private DateTimePicker dtpFechaDesde;
        private DateTimePicker dtpFechaHasta;
        private ComboBox cboFiltroProyecto;
        private FlowLayoutPanel flpKpis;
        private DataGridView dgvDesviacion;

        public FrmInformeDesviacion()
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
            GroupBox grpGrid = ConstruirPanelGrid();

            pnlCanvas.Controls.Add(grpGrid);
            pnlCanvas.Controls.Add(pnlKpis);
            pnlCanvas.Controls.Add(grpFiltros);

            this.Controls.Add(pnlCanvas);
        }

        private GroupBox ConstruirPanelFiltros()
        {
            GroupBox grupo = new GroupBox { Dock = DockStyle.Top, Height = 70, Text = "Filtros (sobre la fecha de factura)" };

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

        private GroupBox ConstruirPanelGrid()
        {
            GroupBox grupo = new GroupBox
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(12),
                Text = "Detalle por tarea (ordenado por mayor desviación)"
            };

            dgvDesviacion = new DataGridView
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

            EstiloDashboard.EstilizarGrid(dgvDesviacion);

            dgvDesviacion.DataBindingComplete += dgvDesviacion_DataBindingComplete;
            dgvDesviacion.CellDoubleClick += dgvDesviacion_CellDoubleClick;

            grupo.Controls.Add(dgvDesviacion);

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
                    "FrmInformeDesviacion.CargarFiltroProyectos",
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

                DataTable tabla = informeRepositorio.ObtenerDesviacionEstimadoFacturado(desde, hasta, idProyecto);

                PintarKpis(tabla);

                dgvDesviacion.DataSource = tabla;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmInformeDesviacion.Generar",
                    "No se ha podido generar el informe.",
                    ex);
            }
        }

        private void PintarKpis(DataTable tabla)
        {
            flpKpis.Controls.Clear();

            int numTareas = tabla.Rows.Count;
            decimal sumaPct = 0m;
            decimal maxPctAbsoluto = 0m;
            int conPorcentaje = 0;

            foreach (DataRow fila in tabla.Rows)
            {
                if (fila["DIFERENCIA_PCT"] == DBNull.Value)
                    continue;

                decimal pct = Convert.ToDecimal(fila["DIFERENCIA_PCT"]);

                sumaPct += pct;
                conPorcentaje++;

                if (Math.Abs(pct) > maxPctAbsoluto)
                    maxPctAbsoluto = Math.Abs(pct);
            }

            decimal mediaPct = conPorcentaje > 0 ? sumaPct / conPorcentaje : 0m;

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "TAREAS ANALIZADAS", numTareas.ToString(CulturaDecimal), EstiloDashboard.Paleta[0]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "DESVIACIÓN MEDIA", mediaPct.ToString("N1", CulturaDecimal) + " %",
                mediaPct < 0 ? EstiloDashboard.Paleta[5] : EstiloDashboard.Paleta[1]));

            flpKpis.Controls.Add(EstiloDashboard.CrearTarjetaKpi(
                this.Font, "MAYOR DESVIACIÓN (ABS.)", maxPctAbsoluto.ToString("N1", CulturaDecimal) + " %",
                EstiloDashboard.Paleta[2]));
        }

        /// <summary>
        /// Da formato a la rejilla y colorea cada fila según la
        /// magnitud (valor absoluto) de la desviación porcentual:
        /// verde por debajo del 5%, amarillo entre 5% y 15%, rojo por
        /// encima del 15%.
        /// </summary>
        private void dgvDesviacion_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvDesviacion.Columns.Count == 0)
                return;

            dgvDesviacion.Columns["ID_TAREA"].Visible = false;
            dgvDesviacion.Columns["DES_DOC"].HeaderText = "Documento";
            dgvDesviacion.Columns["DES_PROYECTO"].HeaderText = "Proyecto";
            dgvDesviacion.Columns["NOMBRE_USUARIO"].HeaderText = "Usuario";
            dgvDesviacion.Columns["IMP_ESTIMADO"].HeaderText = "Importe Estimado";
            dgvDesviacion.Columns["IMP_FACT"].HeaderText = "Importe Facturado";
            dgvDesviacion.Columns["COD_DIV"].HeaderText = "Divisa";
            dgvDesviacion.Columns["FEC_FACT"].HeaderText = "F. Factura";
            dgvDesviacion.Columns["DIFERENCIA"].HeaderText = "Diferencia";
            dgvDesviacion.Columns["DIFERENCIA_PCT"].HeaderText = "Diferencia %";

            dgvDesviacion.Columns["IMP_ESTIMADO"].DefaultCellStyle.Format = "N2";
            dgvDesviacion.Columns["IMP_ESTIMADO"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDesviacion.Columns["IMP_FACT"].DefaultCellStyle.Format = "N2";
            dgvDesviacion.Columns["IMP_FACT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDesviacion.Columns["DIFERENCIA"].DefaultCellStyle.Format = "N2";
            dgvDesviacion.Columns["DIFERENCIA"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDesviacion.Columns["DIFERENCIA_PCT"].DefaultCellStyle.Format = "N1";
            dgvDesviacion.Columns["DIFERENCIA_PCT"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvDesviacion.Columns["FEC_FACT"].DefaultCellStyle.Format = "dd/MM/yyyy";

            foreach (DataGridViewRow fila in dgvDesviacion.Rows)
            {
                object valorPct = fila.Cells["DIFERENCIA_PCT"].Value;

                if (valorPct == null || valorPct == DBNull.Value)
                    continue;

                decimal pct = Math.Abs(Convert.ToDecimal(valorPct));

                fila.DefaultCellStyle.BackColor = pct < 5m
                    ? ColorDesviacionBaja
                    : (pct <= 15m ? ColorDesviacionMedia : ColorDesviacionAlta);
            }
        }

        private void dgvDesviacion_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            object valor = dgvDesviacion.Rows[e.RowIndex].Cells["ID_TAREA"].Value;

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
