namespace GestionFacturas
{
    partial class FrmPrincipal
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.archivoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salirDeLaAplicaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tareasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registrarTareaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.poolDeTareasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.datosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.consultasSQLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.informesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturaciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.splitInferior = new System.Windows.Forms.SplitContainer();
            this.grpPorUsuario = new System.Windows.Forms.GroupBox();
            this.dgvPorUsuario = new System.Windows.Forms.DataGridView();
            this.grpGraficoMensual = new System.Windows.Forms.GroupBox();
            this.chartMensual = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.pnlKpis = new System.Windows.Forms.Panel();
            this.flpKpis = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTituloResumen = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.pnlCanvas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitInferior)).BeginInit();
            this.splitInferior.Panel1.SuspendLayout();
            this.splitInferior.Panel2.SuspendLayout();
            this.splitInferior.SuspendLayout();
            this.grpPorUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorUsuario)).BeginInit();
            this.grpGraficoMensual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartMensual)).BeginInit();
            this.pnlKpis.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.archivoToolStripMenuItem,
            this.tareasToolStripMenuItem,
            this.datosToolStripMenuItem,
            this.informesToolStripMenuItem});
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.Top;
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(800, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            //
            // archivoToolStripMenuItem
            //
            this.archivoToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.salirDeLaAplicaciónToolStripMenuItem});
            this.archivoToolStripMenuItem.Name = "archivoToolStripMenuItem";
            this.archivoToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.archivoToolStripMenuItem.Text = "Archivo";
            //
            // salirDeLaAplicaciónToolStripMenuItem
            //
            this.salirDeLaAplicaciónToolStripMenuItem.Name = "salirDeLaAplicaciónToolStripMenuItem";
            this.salirDeLaAplicaciónToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            this.salirDeLaAplicaciónToolStripMenuItem.Text = "Salir de la Aplicación";
            //
            // tareasToolStripMenuItem
            //
            this.tareasToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registrarTareaToolStripMenuItem,
            this.poolDeTareasToolStripMenuItem,
            this.facturarToolStripMenuItem});
            this.tareasToolStripMenuItem.Name = "tareasToolStripMenuItem";
            this.tareasToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.tareasToolStripMenuItem.Text = "Tareas";
            //
            // registrarTareaToolStripMenuItem
            //
            this.registrarTareaToolStripMenuItem.Name = "registrarTareaToolStripMenuItem";
            this.registrarTareaToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.registrarTareaToolStripMenuItem.Text = "Registrar Tarea";
            this.registrarTareaToolStripMenuItem.Click += new System.EventHandler(this.registrarTareaToolStripMenuItem_Click);
            //
            // poolDeTareasToolStripMenuItem
            //
            this.poolDeTareasToolStripMenuItem.Name = "poolDeTareasToolStripMenuItem";
            this.poolDeTareasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.poolDeTareasToolStripMenuItem.Text = "Pool de Tareas";
            this.poolDeTareasToolStripMenuItem.Click += new System.EventHandler(this.poolDeTareasToolStripMenuItem_Click);
            //
            // facturarToolStripMenuItem
            //
            this.facturarToolStripMenuItem.Name = "facturarToolStripMenuItem";
            this.facturarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.facturarToolStripMenuItem.Text = "Facturar";
            this.facturarToolStripMenuItem.Click += new System.EventHandler(this.facturarToolStripMenuItem_Click);
            //
            // datosToolStripMenuItem
            //
            this.datosToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.consultasSQLToolStripMenuItem});
            this.datosToolStripMenuItem.Name = "datosToolStripMenuItem";
            this.datosToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.datosToolStripMenuItem.Text = "Datos";
            //
            // consultasSQLToolStripMenuItem
            //
            this.consultasSQLToolStripMenuItem.Name = "consultasSQLToolStripMenuItem";
            this.consultasSQLToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.consultasSQLToolStripMenuItem.Text = "Consultas SQL";
            //
            // informesToolStripMenuItem
            //
            this.informesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facturaciónToolStripMenuItem});
            this.informesToolStripMenuItem.Name = "informesToolStripMenuItem";
            this.informesToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.informesToolStripMenuItem.Text = "Informes";
            //
            // facturaciónToolStripMenuItem
            //
            this.facturaciónToolStripMenuItem.Name = "facturaciónToolStripMenuItem";
            this.facturaciónToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.facturaciónToolStripMenuItem.Text = "Facturación";
            this.facturaciónToolStripMenuItem.Click += new System.EventHandler(this.facturaciónToolStripMenuItem_Click);
            //
            // pnlCanvas
            //
            // Lienzo de fondo del cuadro de mando (gris muy claro),
            // sobre el que resaltan las tarjetas KPI (blancas) y la
            // tabla/gráfica de abajo. Contiene, de arriba a abajo,
            // la franja de tarjetas KPI (alto fijo, ajustado a su
            // contenido) y el área inferior partida en dos.
            this.pnlCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.pnlCanvas.Controls.Add(this.splitInferior);
            this.pnlCanvas.Controls.Add(this.pnlKpis);
            this.pnlCanvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlCanvas.Location = new System.Drawing.Point(0, 24);
            this.pnlCanvas.Name = "pnlCanvas";
            this.pnlCanvas.Padding = new System.Windows.Forms.Padding(15);
            this.pnlCanvas.Size = new System.Drawing.Size(1284, 776);
            this.pnlCanvas.TabIndex = 1;
            //
            // pnlKpis
            //
            // Franja superior con las tarjetas KPI (una por divisa
            // facturada este mes, más "Pendiente de Facturar"),
            // generadas dinámicamente en FrmPrincipal.PintarTarjetasKpi
            // según los datos reales: su número y contenido varían,
            // por eso el alto de esta franja es fijo pero pequeño
            // (ajustado a lo que necesita una tarjeta), no a toda la
            // altura de la ventana.
            this.pnlKpis.Controls.Add(this.flpKpis);
            this.pnlKpis.Controls.Add(this.lblTituloResumen);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Location = new System.Drawing.Point(15, 15);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.Size = new System.Drawing.Size(1254, 130);
            this.pnlKpis.TabIndex = 0;
            //
            // lblTituloResumen
            //
            this.lblTituloResumen.AutoSize = true;
            this.lblTituloResumen.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTituloResumen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.lblTituloResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblTituloResumen.Location = new System.Drawing.Point(0, 0);
            this.lblTituloResumen.Margin = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTituloResumen.Name = "lblTituloResumen";
            this.lblTituloResumen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.lblTituloResumen.Size = new System.Drawing.Size(146, 29);
            this.lblTituloResumen.TabIndex = 1;
            this.lblTituloResumen.Text = "Resumen del mes";
            //
            // flpKpis
            //
            this.flpKpis.AutoScroll = true;
            this.flpKpis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpKpis.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight;
            this.flpKpis.Location = new System.Drawing.Point(0, 29);
            this.flpKpis.Name = "flpKpis";
            this.flpKpis.Size = new System.Drawing.Size(1254, 101);
            this.flpKpis.TabIndex = 0;
            this.flpKpis.WrapContents = false;
            //
            // splitInferior
            //
            // Divide el espacio restante entre la tabla por usuario
            // (izquierda, más estrecha) y la gráfica mensual (derecha,
            // más ancha porque necesita sitio para los 12 meses).
            this.splitInferior.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitInferior.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitInferior.Location = new System.Drawing.Point(15, 145);
            this.splitInferior.Name = "splitInferior";
            //
            // splitInferior.Panel1
            //
            this.splitInferior.Panel1.Controls.Add(this.grpPorUsuario);
            this.splitInferior.Panel1MinSize = 260;
            //
            // splitInferior.Panel2
            //
            this.splitInferior.Panel2.Controls.Add(this.grpGraficoMensual);
            this.splitInferior.Panel2MinSize = 320;
            this.splitInferior.Size = new System.Drawing.Size(1254, 616);
            this.splitInferior.SplitterDistance = 460;
            this.splitInferior.SplitterWidth = 10;
            this.splitInferior.TabIndex = 1;
            //
            // grpPorUsuario
            //
            this.grpPorUsuario.Controls.Add(this.dgvPorUsuario);
            this.grpPorUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpPorUsuario.Location = new System.Drawing.Point(0, 0);
            this.grpPorUsuario.Name = "grpPorUsuario";
            this.grpPorUsuario.Padding = new System.Windows.Forms.Padding(12);
            this.grpPorUsuario.Size = new System.Drawing.Size(460, 616);
            this.grpPorUsuario.TabIndex = 0;
            this.grpPorUsuario.TabStop = false;
            this.grpPorUsuario.Text = "Por usuario (mes actual)";
            //
            // dgvPorUsuario
            //
            this.dgvPorUsuario.AllowUserToAddRows = false;
            this.dgvPorUsuario.AllowUserToDeleteRows = false;
            this.dgvPorUsuario.BackgroundColor = System.Drawing.Color.White;
            this.dgvPorUsuario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPorUsuario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPorUsuario.EnableHeadersVisualStyles = false;
            this.dgvPorUsuario.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvPorUsuario.Location = new System.Drawing.Point(12, 30);
            this.dgvPorUsuario.Name = "dgvPorUsuario";
            this.dgvPorUsuario.ReadOnly = true;
            this.dgvPorUsuario.RowHeadersVisible = false;
            this.dgvPorUsuario.RowTemplate.Height = 28;
            this.dgvPorUsuario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorUsuario.Size = new System.Drawing.Size(436, 574);
            this.dgvPorUsuario.TabIndex = 0;
            //
            // grpGraficoMensual
            //
            this.grpGraficoMensual.Controls.Add(this.chartMensual);
            this.grpGraficoMensual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGraficoMensual.Location = new System.Drawing.Point(0, 0);
            this.grpGraficoMensual.Name = "grpGraficoMensual";
            this.grpGraficoMensual.Padding = new System.Windows.Forms.Padding(12);
            this.grpGraficoMensual.Size = new System.Drawing.Size(784, 616);
            this.grpGraficoMensual.TabIndex = 0;
            this.grpGraficoMensual.TabStop = false;
            this.grpGraficoMensual.Text = "Facturado por mes (año actual)";
            //
            // chartMensual
            //
            this.chartMensual.BackColor = System.Drawing.Color.White;
            this.chartMensual.BorderlineColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.chartMensual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMensual.Location = new System.Drawing.Point(12, 30);
            this.chartMensual.Name = "chartMensual";
            this.chartMensual.Size = new System.Drawing.Size(760, 574);
            this.chartMensual.TabIndex = 0;
            this.chartMensual.Text = "chartMensual";
            //
            // FrmPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.ClientSize = new System.Drawing.Size(1284, 800);
            this.Controls.Add(this.pnlCanvas);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FrmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Facturas";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmPrincipal_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlCanvas.ResumeLayout(false);
            this.splitInferior.Panel1.ResumeLayout(false);
            this.splitInferior.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitInferior)).EndInit();
            this.splitInferior.ResumeLayout(false);
            this.grpPorUsuario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorUsuario)).EndInit();
            this.grpGraficoMensual.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMensual)).EndInit();
            this.pnlKpis.ResumeLayout(false);
            this.pnlKpis.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem archivoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salirDeLaAplicaciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tareasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registrarTareaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem poolDeTareasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facturarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem datosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem consultasSQLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem informesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facturaciónToolStripMenuItem;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Panel pnlKpis;
        private System.Windows.Forms.Label lblTituloResumen;
        private System.Windows.Forms.FlowLayoutPanel flpKpis;
        private System.Windows.Forms.SplitContainer splitInferior;
        private System.Windows.Forms.GroupBox grpPorUsuario;
        private System.Windows.Forms.DataGridView dgvPorUsuario;
        private System.Windows.Forms.GroupBox grpGraficoMensual;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMensual;
    }
}
