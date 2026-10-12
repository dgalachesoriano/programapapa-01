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
            this.motivosBloqueoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.antiguedadTareasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rankingUsuariosToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.facturacionProyectoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.desviacionEstimadoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trazabilidadTareaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cancelacionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlCanvas = new System.Windows.Forms.Panel();
            this.tblDashboard = new System.Windows.Forms.TableLayoutPanel();
            this.pnlPorUsuario = new System.Windows.Forms.Panel();
            this.dgvPorUsuario = new System.Windows.Forms.DataGridView();
            this.lblPorUsuario = new System.Windows.Forms.Label();
            this.pnlPorSegmento = new System.Windows.Forms.Panel();
            this.dgvPorSegmento = new System.Windows.Forms.DataGridView();
            this.lblPorSegmento = new System.Windows.Forms.Label();
            this.pnlPorTipoDocumento = new System.Windows.Forms.Panel();
            this.dgvPorTipoDocumento = new System.Windows.Forms.DataGridView();
            this.lblPorTipoDocumento = new System.Windows.Forms.Label();
            this.pnlGraficoMensual = new System.Windows.Forms.Panel();
            this.chartMensual = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblGraficoMensual = new System.Windows.Forms.Label();
            this.pnlKpis = new System.Windows.Forms.Panel();
            this.flpKpis = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTituloResumen = new System.Windows.Forms.Label();
            this.pnlCabeceraResumen = new System.Windows.Forms.Panel();
            this.btnActualizarResumen = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.pnlCabeceraResumen.SuspendLayout();
            this.pnlCanvas.SuspendLayout();
            this.tblDashboard.SuspendLayout();
            this.pnlPorUsuario.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorUsuario)).BeginInit();
            this.pnlPorSegmento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorSegmento)).BeginInit();
            this.pnlPorTipoDocumento.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorTipoDocumento)).BeginInit();
            this.pnlGraficoMensual.SuspendLayout();
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
            this.consultasSQLToolStripMenuItem.Click += new System.EventHandler(this.consultasSQLToolStripMenuItem_Click);
            //
            // informesToolStripMenuItem
            //
            this.informesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.facturaciónToolStripMenuItem,
            this.motivosBloqueoToolStripMenuItem,
            this.antiguedadTareasToolStripMenuItem,
            this.rankingUsuariosToolStripMenuItem,
            this.facturacionProyectoToolStripMenuItem,
            this.desviacionEstimadoToolStripMenuItem,
            this.trazabilidadTareaToolStripMenuItem,
            this.cancelacionesToolStripMenuItem});
            this.informesToolStripMenuItem.Name = "informesToolStripMenuItem";
            this.informesToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.informesToolStripMenuItem.Text = "Informes";
            //
            // facturaciónToolStripMenuItem
            //
            this.facturaciónToolStripMenuItem.Name = "facturaciónToolStripMenuItem";
            this.facturaciónToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.facturaciónToolStripMenuItem.Text = "Facturación";
            this.facturaciónToolStripMenuItem.Click += new System.EventHandler(this.facturaciónToolStripMenuItem_Click);
            //
            // motivosBloqueoToolStripMenuItem
            //
            this.motivosBloqueoToolStripMenuItem.Name = "motivosBloqueoToolStripMenuItem";
            this.motivosBloqueoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.motivosBloqueoToolStripMenuItem.Text = "Motivos de Bloqueo";
            this.motivosBloqueoToolStripMenuItem.Click += new System.EventHandler(this.motivosBloqueoToolStripMenuItem_Click);
            //
            // antiguedadTareasToolStripMenuItem
            //
            this.antiguedadTareasToolStripMenuItem.Name = "antiguedadTareasToolStripMenuItem";
            this.antiguedadTareasToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.antiguedadTareasToolStripMenuItem.Text = "Antigüedad de Tareas";
            this.antiguedadTareasToolStripMenuItem.Click += new System.EventHandler(this.antiguedadTareasToolStripMenuItem_Click);
            //
            // rankingUsuariosToolStripMenuItem
            //
            this.rankingUsuariosToolStripMenuItem.Name = "rankingUsuariosToolStripMenuItem";
            this.rankingUsuariosToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.rankingUsuariosToolStripMenuItem.Text = "Ranking de Usuarios";
            this.rankingUsuariosToolStripMenuItem.Click += new System.EventHandler(this.rankingUsuariosToolStripMenuItem_Click);
            //
            // facturacionProyectoToolStripMenuItem
            //
            this.facturacionProyectoToolStripMenuItem.Name = "facturacionProyectoToolStripMenuItem";
            this.facturacionProyectoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.facturacionProyectoToolStripMenuItem.Text = "Facturación por Proyecto/Segmento";
            this.facturacionProyectoToolStripMenuItem.Click += new System.EventHandler(this.facturacionProyectoToolStripMenuItem_Click);
            //
            // desviacionEstimadoToolStripMenuItem
            //
            this.desviacionEstimadoToolStripMenuItem.Name = "desviacionEstimadoToolStripMenuItem";
            this.desviacionEstimadoToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.desviacionEstimadoToolStripMenuItem.Text = "Desviación Estimado vs. Facturado";
            this.desviacionEstimadoToolStripMenuItem.Click += new System.EventHandler(this.desviacionEstimadoToolStripMenuItem_Click);
            //
            // trazabilidadTareaToolStripMenuItem
            //
            this.trazabilidadTareaToolStripMenuItem.Name = "trazabilidadTareaToolStripMenuItem";
            this.trazabilidadTareaToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.trazabilidadTareaToolStripMenuItem.Text = "Trazabilidad de Tarea";
            this.trazabilidadTareaToolStripMenuItem.Click += new System.EventHandler(this.trazabilidadTareaToolStripMenuItem_Click);
            //
            // cancelacionesToolStripMenuItem
            //
            this.cancelacionesToolStripMenuItem.Name = "cancelacionesToolStripMenuItem";
            this.cancelacionesToolStripMenuItem.Size = new System.Drawing.Size(260, 22);
            this.cancelacionesToolStripMenuItem.Text = "Cancelaciones";
            this.cancelacionesToolStripMenuItem.Click += new System.EventHandler(this.cancelacionesToolStripMenuItem_Click);
            //
            // pnlCanvas
            //
            // Lienzo de fondo del cuadro de mando (gris muy claro),
            // sobre el que resaltan las tarjetas KPI (blancas) y la
            // tablas/gráfica de abajo. Contiene, de arriba a abajo,
            // la franja de tarjetas KPI (alto fijo, ajustado a su
            // contenido) y la rejilla de paneles del cuadro de mando.
            this.pnlCanvas.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(248)))));
            this.pnlCanvas.Controls.Add(this.tblDashboard);
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
            // Franja superior con las tarjetas KPI (una de "Facturado"
            // y otra de "Pendiente de Facturar" por cada divisa),
            // generadas dinámicamente en FrmPrincipal.PintarTarjetasKpi
            // según los datos reales: su número y contenido varían,
            // por eso el alto de esta franja es fijo pero pequeño
            // (ajustado a lo que necesita una tarjeta), no a toda la
            // altura de la ventana.
            this.pnlKpis.Controls.Add(this.flpKpis);
            this.pnlKpis.Controls.Add(this.pnlCabeceraResumen);
            this.pnlKpis.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlKpis.Location = new System.Drawing.Point(15, 15);
            this.pnlKpis.Name = "pnlKpis";
            this.pnlKpis.Size = new System.Drawing.Size(1254, 160);
            this.pnlKpis.TabIndex = 0;
            //
            // pnlCabeceraResumen
            //
            // Franja de título del cuadro de mando: el título (con la
            // hora de la última actualización) a la izquierda y el
            // botón Actualizar a la derecha.
            this.pnlCabeceraResumen.Controls.Add(this.lblTituloResumen);
            this.pnlCabeceraResumen.Controls.Add(this.btnActualizarResumen);
            this.pnlCabeceraResumen.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlCabeceraResumen.Location = new System.Drawing.Point(0, 0);
            this.pnlCabeceraResumen.Name = "pnlCabeceraResumen";
            this.pnlCabeceraResumen.Padding = new System.Windows.Forms.Padding(0, 0, 0, 8);
            this.pnlCabeceraResumen.Size = new System.Drawing.Size(1254, 40);
            this.pnlCabeceraResumen.TabIndex = 1;
            //
            // btnActualizarResumen
            //
            this.btnActualizarResumen.BackColor = System.Drawing.Color.White;
            this.btnActualizarResumen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnActualizarResumen.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnActualizarResumen.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(205)))), ((int)(((byte)(210)))));
            this.btnActualizarResumen.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(242)))), ((int)(((byte)(253)))));
            this.btnActualizarResumen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActualizarResumen.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.btnActualizarResumen.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnActualizarResumen.Name = "btnActualizarResumen";
            this.btnActualizarResumen.Padding = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.btnActualizarResumen.Size = new System.Drawing.Size(130, 32);
            this.btnActualizarResumen.TabIndex = 0;
            this.btnActualizarResumen.Text = "Actualizar (F5)";
            this.btnActualizarResumen.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnActualizarResumen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnActualizarResumen.UseVisualStyleBackColor = false;
            this.btnActualizarResumen.Click += new System.EventHandler(this.btnActualizarResumen_Click);
            //
            // lblTituloResumen
            //
            this.lblTituloResumen.AutoSize = true;
            this.lblTituloResumen.Dock = System.Windows.Forms.DockStyle.Left;
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
            this.flpKpis.Size = new System.Drawing.Size(1254, 121);
            this.flpKpis.TabIndex = 0;
            this.flpKpis.WrapContents = false;
            //
            // tblDashboard
            //
            // Rejilla de paneles del cuadro de mando bajo las tarjetas
            // KPI: arriba, los desgloses por usuario, por segmento y
            // por tipo de documento, a partes iguales; abajo, ocupando
            // todo el ancho, la gráfica mensual (necesita sitio para
            // los 12 meses). Todo se adapta al tamaño de la ventana.
            this.tblDashboard.ColumnCount = 3;
            this.tblDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tblDashboard.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tblDashboard.Controls.Add(this.pnlPorUsuario, 0, 0);
            this.tblDashboard.Controls.Add(this.pnlPorSegmento, 1, 0);
            this.tblDashboard.Controls.Add(this.pnlPorTipoDocumento, 2, 0);
            this.tblDashboard.Controls.Add(this.pnlGraficoMensual, 0, 1);
            this.tblDashboard.SetColumnSpan(this.pnlGraficoMensual, 3);
            this.tblDashboard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblDashboard.Location = new System.Drawing.Point(15, 145);
            this.tblDashboard.Name = "tblDashboard";
            this.tblDashboard.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.tblDashboard.RowCount = 2;
            this.tblDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDashboard.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblDashboard.Size = new System.Drawing.Size(1254, 616);
            this.tblDashboard.TabIndex = 1;
            //
            // pnlPorUsuario
            //
            // Panel blanco tipo "widget" de cuadro de mando: título
            // arriba y la rejilla ocupando el resto.
            this.pnlPorUsuario.BackColor = System.Drawing.Color.White;
            this.pnlPorUsuario.Controls.Add(this.dgvPorUsuario);
            this.pnlPorUsuario.Controls.Add(this.lblPorUsuario);
            this.pnlPorUsuario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPorUsuario.Margin = new System.Windows.Forms.Padding(0, 0, 8, 8);
            this.pnlPorUsuario.Name = "pnlPorUsuario";
            this.pnlPorUsuario.Padding = new System.Windows.Forms.Padding(14, 10, 14, 14);
            this.pnlPorUsuario.TabIndex = 0;
            //
            // lblPorUsuario
            //
            this.lblPorUsuario.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPorUsuario.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPorUsuario.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblPorUsuario.Name = "lblPorUsuario";
            this.lblPorUsuario.Size = new System.Drawing.Size(400, 30);
            this.lblPorUsuario.TabIndex = 1;
            this.lblPorUsuario.Text = "Por usuario  ·  facturado en el mes / pendiente total";
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
            this.dgvPorUsuario.Name = "dgvPorUsuario";
            this.dgvPorUsuario.ReadOnly = true;
            this.dgvPorUsuario.RowHeadersVisible = false;
            this.dgvPorUsuario.RowTemplate.Height = 28;
            this.dgvPorUsuario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorUsuario.TabIndex = 0;
            //
            // pnlPorSegmento
            //
            this.pnlPorSegmento.BackColor = System.Drawing.Color.White;
            this.pnlPorSegmento.Controls.Add(this.dgvPorSegmento);
            this.pnlPorSegmento.Controls.Add(this.lblPorSegmento);
            this.pnlPorSegmento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPorSegmento.Margin = new System.Windows.Forms.Padding(8, 0, 8, 8);
            this.pnlPorSegmento.Name = "pnlPorSegmento";
            this.pnlPorSegmento.Padding = new System.Windows.Forms.Padding(14, 10, 14, 14);
            this.pnlPorSegmento.TabIndex = 1;
            //
            // lblPorSegmento
            //
            this.lblPorSegmento.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPorSegmento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPorSegmento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblPorSegmento.Name = "lblPorSegmento";
            this.lblPorSegmento.Size = new System.Drawing.Size(400, 30);
            this.lblPorSegmento.TabIndex = 1;
            this.lblPorSegmento.Text = "Por segmento  ·  facturado en el mes / pendiente total";
            //
            // dgvPorSegmento
            //
            this.dgvPorSegmento.AllowUserToAddRows = false;
            this.dgvPorSegmento.AllowUserToDeleteRows = false;
            this.dgvPorSegmento.BackgroundColor = System.Drawing.Color.White;
            this.dgvPorSegmento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPorSegmento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorSegmento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPorSegmento.EnableHeadersVisualStyles = false;
            this.dgvPorSegmento.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvPorSegmento.Name = "dgvPorSegmento";
            this.dgvPorSegmento.ReadOnly = true;
            this.dgvPorSegmento.RowHeadersVisible = false;
            this.dgvPorSegmento.RowTemplate.Height = 28;
            this.dgvPorSegmento.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorSegmento.TabIndex = 0;
            //
            // pnlPorTipoDocumento
            //
            this.pnlPorTipoDocumento.BackColor = System.Drawing.Color.White;
            this.pnlPorTipoDocumento.Controls.Add(this.dgvPorTipoDocumento);
            this.pnlPorTipoDocumento.Controls.Add(this.lblPorTipoDocumento);
            this.pnlPorTipoDocumento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlPorTipoDocumento.Margin = new System.Windows.Forms.Padding(8, 0, 0, 8);
            this.pnlPorTipoDocumento.Name = "pnlPorTipoDocumento";
            this.pnlPorTipoDocumento.Padding = new System.Windows.Forms.Padding(14, 10, 14, 14);
            this.pnlPorTipoDocumento.TabIndex = 2;
            //
            // lblPorTipoDocumento
            //
            this.lblPorTipoDocumento.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblPorTipoDocumento.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblPorTipoDocumento.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblPorTipoDocumento.Name = "lblPorTipoDocumento";
            this.lblPorTipoDocumento.Size = new System.Drawing.Size(400, 30);
            this.lblPorTipoDocumento.TabIndex = 1;
            this.lblPorTipoDocumento.Text = "Por tipo de documento  ·  facturado en el mes / pendiente total";
            //
            // dgvPorTipoDocumento
            //
            this.dgvPorTipoDocumento.AllowUserToAddRows = false;
            this.dgvPorTipoDocumento.AllowUserToDeleteRows = false;
            this.dgvPorTipoDocumento.BackgroundColor = System.Drawing.Color.White;
            this.dgvPorTipoDocumento.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvPorTipoDocumento.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPorTipoDocumento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPorTipoDocumento.EnableHeadersVisualStyles = false;
            this.dgvPorTipoDocumento.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.dgvPorTipoDocumento.Name = "dgvPorTipoDocumento";
            this.dgvPorTipoDocumento.ReadOnly = true;
            this.dgvPorTipoDocumento.RowHeadersVisible = false;
            this.dgvPorTipoDocumento.RowTemplate.Height = 28;
            this.dgvPorTipoDocumento.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPorTipoDocumento.TabIndex = 0;
            //
            // pnlGraficoMensual
            //
            this.pnlGraficoMensual.BackColor = System.Drawing.Color.White;
            this.pnlGraficoMensual.Controls.Add(this.chartMensual);
            this.pnlGraficoMensual.Controls.Add(this.lblGraficoMensual);
            this.pnlGraficoMensual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGraficoMensual.Margin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.pnlGraficoMensual.Name = "pnlGraficoMensual";
            this.pnlGraficoMensual.Padding = new System.Windows.Forms.Padding(14, 10, 14, 14);
            this.pnlGraficoMensual.TabIndex = 2;
            //
            // lblGraficoMensual
            //
            this.lblGraficoMensual.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblGraficoMensual.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblGraficoMensual.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(55)))), ((int)(((byte)(71)))), ((int)(((byte)(79)))));
            this.lblGraficoMensual.Name = "lblGraficoMensual";
            this.lblGraficoMensual.Size = new System.Drawing.Size(400, 30);
            this.lblGraficoMensual.TabIndex = 1;
            this.lblGraficoMensual.Text = "Facturado por mes (año actual)   ·   K = miles, M = millones, MM = miles de millones";
            //
            // chartMensual
            //
            this.chartMensual.BackColor = System.Drawing.Color.White;
            this.chartMensual.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartMensual.Name = "chartMensual";
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
            this.tblDashboard.ResumeLayout(false);
            this.pnlPorUsuario.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorUsuario)).EndInit();
            this.pnlPorSegmento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorSegmento)).EndInit();
            this.pnlPorTipoDocumento.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPorTipoDocumento)).EndInit();
            this.pnlGraficoMensual.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartMensual)).EndInit();
            this.pnlCabeceraResumen.ResumeLayout(false);
            this.pnlCabeceraResumen.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem motivosBloqueoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem antiguedadTareasToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rankingUsuariosToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem facturacionProyectoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem desviacionEstimadoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trazabilidadTareaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cancelacionesToolStripMenuItem;
        private System.Windows.Forms.Panel pnlCanvas;
        private System.Windows.Forms.Panel pnlKpis;
        private System.Windows.Forms.Label lblTituloResumen;
        private System.Windows.Forms.Panel pnlCabeceraResumen;
        private System.Windows.Forms.Button btnActualizarResumen;
        private System.Windows.Forms.FlowLayoutPanel flpKpis;
        private System.Windows.Forms.TableLayoutPanel tblDashboard;
        private System.Windows.Forms.Panel pnlPorUsuario;
        private System.Windows.Forms.Label lblPorUsuario;
        private System.Windows.Forms.DataGridView dgvPorUsuario;
        private System.Windows.Forms.Panel pnlPorSegmento;
        private System.Windows.Forms.Label lblPorSegmento;
        private System.Windows.Forms.DataGridView dgvPorSegmento;
        private System.Windows.Forms.Panel pnlPorTipoDocumento;
        private System.Windows.Forms.Label lblPorTipoDocumento;
        private System.Windows.Forms.DataGridView dgvPorTipoDocumento;
        private System.Windows.Forms.Panel pnlGraficoMensual;
        private System.Windows.Forms.Label lblGraficoMensual;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartMensual;
    }
}
