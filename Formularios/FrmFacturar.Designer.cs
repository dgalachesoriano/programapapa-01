namespace GestionFacturas.Formularios
{
    partial class FrmFacturar
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.grpFiltros = new System.Windows.Forms.GroupBox();
            this.tblFiltros = new System.Windows.Forms.TableLayoutPanel();
            this.lblFiltroDocumento = new System.Windows.Forms.Label();
            this.txtFiltroDocumento = new System.Windows.Forms.TextBox();
            this.lblFiltroUsuario = new System.Windows.Forms.Label();
            this.cboFiltroUsuario = new System.Windows.Forms.ComboBox();
            this.lblFechaDesde = new System.Windows.Forms.Label();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.lblFechaHasta = new System.Windows.Forms.Label();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.pnlBotonesFiltro = new System.Windows.Forms.Panel();
            this.btnLimpiarFiltros = new System.Windows.Forms.Button();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.grpTareas = new System.Windows.Forms.GroupBox();
            this.dgvTareas = new System.Windows.Forms.DataGridView();
            this.pnlDetalle = new System.Windows.Forms.Panel();
            this.dgvDetalleTarea = new System.Windows.Forms.DataGridView();
            this.pnlDetalleHeader = new System.Windows.Forms.Panel();
            this.lblDetalleHeader = new System.Windows.Forms.Label();
            this.btnColapsarDetalle = new System.Windows.Forms.Button();
            this.grpFacturacion = new System.Windows.Forms.GroupBox();
            this.lblEntidadSalida = new System.Windows.Forms.Label();
            this.txtEntidadSalida = new System.Windows.Forms.TextBox();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.dtpFechaFactura = new System.Windows.Forms.DateTimePicker();
            this.lblCodigoFactura = new System.Windows.Forms.Label();
            this.txtCodigoFactura = new System.Windows.Forms.TextBox();
            this.lblImporteFactura = new System.Windows.Forms.Label();
            this.txtImporteFactura = new System.Windows.Forms.TextBox();
            this.lblDivisa = new System.Windows.Forms.Label();
            this.cboDivisa = new System.Windows.Forms.ComboBox();
            this.lblTipoFactura = new System.Windows.Forms.Label();
            this.cboTipoFactura = new System.Windows.Forms.ComboBox();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.grpFiltros.SuspendLayout();
            this.tblFiltros.SuspendLayout();
            this.pnlBotonesFiltro.SuspendLayout();
            this.grpTareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).BeginInit();
            this.pnlDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleTarea)).BeginInit();
            this.pnlDetalleHeader.SuspendLayout();
            this.grpFacturacion.SuspendLayout();
            this.SuspendLayout();
            //
            // grpFiltros
            //
            // Anclado arriba y estirado a lo ancho del formulario.
            this.grpFiltros.Controls.Add(this.tblFiltros);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Location = new System.Drawing.Point(0, 0);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Padding = new System.Windows.Forms.Padding(10);
            this.grpFiltros.Size = new System.Drawing.Size(1184, 150);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Filtros (tareas en estado \"En Proceso\")";
            //
            // tblFiltros
            //
            this.tblFiltros.ColumnCount = 4;
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFiltros.Location = new System.Drawing.Point(10, 23);
            this.tblFiltros.Name = "tblFiltros";
            this.tblFiltros.RowCount = 3;
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
            this.tblFiltros.Size = new System.Drawing.Size(1164, 117);
            this.tblFiltros.TabIndex = 0;
            //
            // Fila 0: Documento / Usuario
            //
            this.lblFiltroDocumento.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltroDocumento.AutoSize = true;
            this.lblFiltroDocumento.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFiltroDocumento.Name = "lblFiltroDocumento";
            this.lblFiltroDocumento.Size = new System.Drawing.Size(65, 13);
            this.lblFiltroDocumento.TabIndex = 0;
            this.lblFiltroDocumento.Text = "Documento:";
            this.tblFiltros.Controls.Add(this.lblFiltroDocumento, 0, 0);
            //
            this.txtFiltroDocumento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFiltroDocumento.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFiltroDocumento.Name = "txtFiltroDocumento";
            this.txtFiltroDocumento.Size = new System.Drawing.Size(150, 20);
            this.txtFiltroDocumento.TabIndex = 1;
            this.tblFiltros.Controls.Add(this.txtFiltroDocumento, 1, 0);
            //
            this.lblFiltroUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltroUsuario.AutoSize = true;
            this.lblFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFiltroUsuario.Name = "lblFiltroUsuario";
            this.lblFiltroUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblFiltroUsuario.TabIndex = 2;
            this.lblFiltroUsuario.Text = "Usuario:";
            this.tblFiltros.Controls.Add(this.lblFiltroUsuario, 2, 0);
            //
            this.cboFiltroUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFiltroUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroUsuario.FormattingEnabled = true;
            this.cboFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFiltroUsuario.Name = "cboFiltroUsuario";
            this.cboFiltroUsuario.Size = new System.Drawing.Size(130, 21);
            this.cboFiltroUsuario.TabIndex = 3;
            this.tblFiltros.Controls.Add(this.cboFiltroUsuario, 3, 0);
            //
            // Fila 1: Desde / Hasta
            //
            this.lblFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaDesde.Name = "lblFechaDesde";
            this.lblFechaDesde.Size = new System.Drawing.Size(41, 13);
            this.lblFechaDesde.TabIndex = 4;
            this.lblFechaDesde.Text = "Desde:";
            this.tblFiltros.Controls.Add(this.lblFechaDesde, 0, 1);
            //
            this.dtpFechaDesde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFechaDesde.Checked = false;
            this.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaDesde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFechaDesde.Name = "dtpFechaDesde";
            this.dtpFechaDesde.ShowCheckBox = true;
            this.dtpFechaDesde.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaDesde.TabIndex = 5;
            this.tblFiltros.Controls.Add(this.dtpFechaDesde, 1, 1);
            //
            this.lblFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaHasta.Name = "lblFechaHasta";
            this.lblFechaHasta.Size = new System.Drawing.Size(38, 13);
            this.lblFechaHasta.TabIndex = 6;
            this.lblFechaHasta.Text = "Hasta:";
            this.tblFiltros.Controls.Add(this.lblFechaHasta, 2, 1);
            //
            this.dtpFechaHasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFechaHasta.Checked = false;
            this.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaHasta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFechaHasta.Name = "dtpFechaHasta";
            this.dtpFechaHasta.ShowCheckBox = true;
            this.dtpFechaHasta.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaHasta.TabIndex = 7;
            this.tblFiltros.Controls.Add(this.dtpFechaHasta, 3, 1);
            //
            // Fila 2: barra de botones de filtro (ocupa todo el ancho)
            //
            this.tblFiltros.SetColumnSpan(this.pnlBotonesFiltro, 4);
            this.pnlBotonesFiltro.Controls.Add(this.btnLimpiarFiltros);
            this.pnlBotonesFiltro.Controls.Add(this.btnAplicarFiltros);
            this.pnlBotonesFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBotonesFiltro.Location = new System.Drawing.Point(0, 64);
            this.pnlBotonesFiltro.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBotonesFiltro.Name = "pnlBotonesFiltro";
            this.pnlBotonesFiltro.Size = new System.Drawing.Size(1164, 46);
            this.pnlBotonesFiltro.TabIndex = 8;
            this.tblFiltros.Controls.Add(this.pnlBotonesFiltro, 0, 2);
            //
            // btnAplicarFiltros
            //
            this.btnAplicarFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicarFiltros.Location = new System.Drawing.Point(946, 6);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(99, 36);
            this.btnAplicarFiltros.TabIndex = 8;
            this.btnAplicarFiltros.Text = "Aplicar filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = true;
            this.btnAplicarFiltros.Click += new System.EventHandler(this.btnAplicarFiltros_Click);
            //
            // btnLimpiarFiltros
            //
            this.btnLimpiarFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiarFiltros.Location = new System.Drawing.Point(1063, 6);
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Size = new System.Drawing.Size(99, 36);
            this.btnLimpiarFiltros.TabIndex = 9;
            this.btnLimpiarFiltros.Text = "Limpiar";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            this.btnLimpiarFiltros.Click += new System.EventHandler(this.btnLimpiarFiltros_Click);
            //
            // grpTareas
            //
            // Ocupa todo el espacio que quede libre entre los filtros
            // (arriba) y el panel de detalle + datos de facturación
            // (abajo).
            this.grpTareas.Controls.Add(this.dgvTareas);
            this.grpTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTareas.Location = new System.Drawing.Point(0, 150);
            this.grpTareas.Name = "grpTareas";
            this.grpTareas.Padding = new System.Windows.Forms.Padding(10);
            this.grpTareas.Size = new System.Drawing.Size(1184, 200);
            this.grpTareas.TabIndex = 1;
            this.grpTareas.TabStop = false;
            this.grpTareas.Text = "Tareas pendientes de facturar";
            //
            // dgvTareas
            //
            this.dgvTareas.AllowUserToAddRows = false;
            this.dgvTareas.AllowUserToDeleteRows = false;
            this.dgvTareas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTareas.Location = new System.Drawing.Point(10, 23);
            this.dgvTareas.MultiSelect = false;
            this.dgvTareas.Name = "dgvTareas";
            this.dgvTareas.ReadOnly = true;
            this.dgvTareas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTareas.Size = new System.Drawing.Size(1164, 167);
            this.dgvTareas.TabIndex = 0;
            this.dgvTareas.SelectionChanged += new System.EventHandler(this.dgvTareas_SelectionChanged);
            //
            // pnlDetalle
            //
            // Panel colapsable: su alto alterna entre el de la cabecera
            // (colapsado) y AltoDetalleExpandido (desplegado) al pulsar
            // btnColapsarDetalle. Empieza desplegado.
            this.pnlDetalle.Controls.Add(this.dgvDetalleTarea);
            this.pnlDetalle.Controls.Add(this.pnlDetalleHeader);
            this.pnlDetalle.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlDetalle.Location = new System.Drawing.Point(0, 350);
            this.pnlDetalle.Name = "pnlDetalle";
            this.pnlDetalle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlDetalle.Size = new System.Drawing.Size(1184, 220);
            this.pnlDetalle.TabIndex = 2;
            //
            // dgvDetalleTarea
            //
            this.dgvDetalleTarea.AllowUserToAddRows = false;
            this.dgvDetalleTarea.AllowUserToDeleteRows = false;
            this.dgvDetalleTarea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalleTarea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalleTarea.Location = new System.Drawing.Point(10, 28);
            this.dgvDetalleTarea.MultiSelect = false;
            this.dgvDetalleTarea.Name = "dgvDetalleTarea";
            this.dgvDetalleTarea.ReadOnly = true;
            this.dgvDetalleTarea.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalleTarea.Size = new System.Drawing.Size(1164, 182);
            this.dgvDetalleTarea.TabIndex = 1;
            //
            // pnlDetalleHeader
            //
            this.pnlDetalleHeader.Controls.Add(this.btnColapsarDetalle);
            this.pnlDetalleHeader.Controls.Add(this.lblDetalleHeader);
            this.pnlDetalleHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetalleHeader.Location = new System.Drawing.Point(10, 0);
            this.pnlDetalleHeader.Name = "pnlDetalleHeader";
            this.pnlDetalleHeader.Size = new System.Drawing.Size(1164, 28);
            this.pnlDetalleHeader.TabIndex = 0;
            //
            // lblDetalleHeader
            //
            this.lblDetalleHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDetalleHeader.AutoSize = true;
            this.lblDetalleHeader.Location = new System.Drawing.Point(0, 7);
            this.lblDetalleHeader.Name = "lblDetalleHeader";
            this.lblDetalleHeader.Size = new System.Drawing.Size(163, 13);
            this.lblDetalleHeader.TabIndex = 0;
            this.lblDetalleHeader.Text = "Detalle de la tarea seleccionada";
            //
            // btnColapsarDetalle
            //
            this.btnColapsarDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColapsarDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColapsarDetalle.Location = new System.Drawing.Point(1004, 0);
            this.btnColapsarDetalle.Name = "btnColapsarDetalle";
            this.btnColapsarDetalle.Size = new System.Drawing.Size(160, 26);
            this.btnColapsarDetalle.TabIndex = 1;
            this.btnColapsarDetalle.Text = "▼ Ocultar detalle";
            this.btnColapsarDetalle.UseVisualStyleBackColor = true;
            this.btnColapsarDetalle.Click += new System.EventHandler(this.btnColapsarDetalle_Click);
            //
            // grpFacturacion
            //
            // Anclado al fondo del formulario, con alto fijo; los
            // campos se posicionan de forma absoluta (igual que la
            // franja de acciones del Pool de Tareas).
            this.grpFacturacion.Controls.Add(this.lblEntidadSalida);
            this.grpFacturacion.Controls.Add(this.txtEntidadSalida);
            this.grpFacturacion.Controls.Add(this.lblFechaFactura);
            this.grpFacturacion.Controls.Add(this.dtpFechaFactura);
            this.grpFacturacion.Controls.Add(this.lblCodigoFactura);
            this.grpFacturacion.Controls.Add(this.txtCodigoFactura);
            this.grpFacturacion.Controls.Add(this.lblImporteFactura);
            this.grpFacturacion.Controls.Add(this.txtImporteFactura);
            this.grpFacturacion.Controls.Add(this.lblDivisa);
            this.grpFacturacion.Controls.Add(this.cboDivisa);
            this.grpFacturacion.Controls.Add(this.lblTipoFactura);
            this.grpFacturacion.Controls.Add(this.cboTipoFactura);
            this.grpFacturacion.Controls.Add(this.btnGrabar);
            this.grpFacturacion.Controls.Add(this.btnCerrar);
            this.grpFacturacion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpFacturacion.Location = new System.Drawing.Point(0, 570);
            this.grpFacturacion.Name = "grpFacturacion";
            this.grpFacturacion.Size = new System.Drawing.Size(1184, 200);
            this.grpFacturacion.TabIndex = 3;
            this.grpFacturacion.TabStop = false;
            this.grpFacturacion.Text = "Datos de facturación (de la tarea seleccionada arriba)";
            //
            // lblEntidadSalida
            //
            this.lblEntidadSalida.AutoSize = true;
            this.lblEntidadSalida.Location = new System.Drawing.Point(12, 33);
            this.lblEntidadSalida.Name = "lblEntidadSalida";
            this.lblEntidadSalida.Size = new System.Drawing.Size(87, 13);
            this.lblEntidadSalida.TabIndex = 0;
            this.lblEntidadSalida.Text = "Entidad de Salida:";
            //
            // txtEntidadSalida
            //
            this.txtEntidadSalida.Location = new System.Drawing.Point(140, 30);
            this.txtEntidadSalida.Name = "txtEntidadSalida";
            this.txtEntidadSalida.Size = new System.Drawing.Size(180, 20);
            this.txtEntidadSalida.TabIndex = 1;
            //
            // lblFechaFactura
            //
            this.lblFechaFactura.AutoSize = true;
            this.lblFechaFactura.Location = new System.Drawing.Point(360, 33);
            this.lblFechaFactura.Name = "lblFechaFactura";
            this.lblFechaFactura.Size = new System.Drawing.Size(72, 13);
            this.lblFechaFactura.TabIndex = 2;
            this.lblFechaFactura.Text = "F. Factura:";
            //
            // dtpFechaFactura
            //
            this.dtpFechaFactura.CustomFormat = "dd/MM/yyyy";
            this.dtpFechaFactura.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFechaFactura.Location = new System.Drawing.Point(440, 30);
            this.dtpFechaFactura.Name = "dtpFechaFactura";
            this.dtpFechaFactura.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaFactura.TabIndex = 3;
            //
            // lblCodigoFactura
            //
            this.lblCodigoFactura.AutoSize = true;
            this.lblCodigoFactura.Location = new System.Drawing.Point(12, 68);
            this.lblCodigoFactura.Name = "lblCodigoFactura";
            this.lblCodigoFactura.Size = new System.Drawing.Size(89, 13);
            this.lblCodigoFactura.TabIndex = 4;
            this.lblCodigoFactura.Text = "Código Factura:";
            //
            // txtCodigoFactura
            //
            this.txtCodigoFactura.Location = new System.Drawing.Point(140, 65);
            this.txtCodigoFactura.Name = "txtCodigoFactura";
            this.txtCodigoFactura.Size = new System.Drawing.Size(180, 20);
            this.txtCodigoFactura.TabIndex = 5;
            //
            // lblImporteFactura
            //
            this.lblImporteFactura.AutoSize = true;
            this.lblImporteFactura.Location = new System.Drawing.Point(360, 68);
            this.lblImporteFactura.Name = "lblImporteFactura";
            this.lblImporteFactura.Size = new System.Drawing.Size(79, 13);
            this.lblImporteFactura.TabIndex = 6;
            this.lblImporteFactura.Text = "Importe Fra.:";
            //
            // txtImporteFactura
            //
            this.txtImporteFactura.Location = new System.Drawing.Point(440, 65);
            this.txtImporteFactura.Name = "txtImporteFactura";
            this.txtImporteFactura.Size = new System.Drawing.Size(150, 20);
            this.txtImporteFactura.TabIndex = 7;
            //
            // lblDivisa
            //
            this.lblDivisa.AutoSize = true;
            this.lblDivisa.Location = new System.Drawing.Point(12, 103);
            this.lblDivisa.Name = "lblDivisa";
            this.lblDivisa.Size = new System.Drawing.Size(40, 13);
            this.lblDivisa.TabIndex = 8;
            this.lblDivisa.Text = "Divisa:";
            //
            // cboDivisa
            //
            this.cboDivisa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDivisa.FormattingEnabled = true;
            this.cboDivisa.Location = new System.Drawing.Point(140, 100);
            this.cboDivisa.Name = "cboDivisa";
            this.cboDivisa.Size = new System.Drawing.Size(120, 21);
            this.cboDivisa.TabIndex = 9;
            //
            // lblTipoFactura
            //
            this.lblTipoFactura.AutoSize = true;
            this.lblTipoFactura.Location = new System.Drawing.Point(360, 103);
            this.lblTipoFactura.Name = "lblTipoFactura";
            this.lblTipoFactura.Size = new System.Drawing.Size(75, 13);
            this.lblTipoFactura.TabIndex = 10;
            this.lblTipoFactura.Text = "Tipo Factura:";
            //
            // cboTipoFactura
            //
            this.cboTipoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoFactura.FormattingEnabled = true;
            this.cboTipoFactura.Location = new System.Drawing.Point(440, 100);
            this.cboTipoFactura.Name = "cboTipoFactura";
            this.cboTipoFactura.Size = new System.Drawing.Size(250, 21);
            this.cboTipoFactura.TabIndex = 11;
            //
            // btnGrabar
            //
            this.btnGrabar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGrabar.Location = new System.Drawing.Point(953, 140);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(109, 34);
            this.btnGrabar.TabIndex = 12;
            this.btnGrabar.Text = "Facturar";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1068, 140);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(109, 34);
            this.btnCerrar.TabIndex = 13;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FrmFacturar
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 770);
            this.Controls.Add(this.grpTareas);
            this.Controls.Add(this.pnlDetalle);
            this.Controls.Add(this.grpFacturacion);
            this.Controls.Add(this.grpFiltros);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.Name = "FrmFacturar";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Facturar";
            this.grpFiltros.ResumeLayout(false);
            this.tblFiltros.ResumeLayout(false);
            this.tblFiltros.PerformLayout();
            this.pnlBotonesFiltro.ResumeLayout(false);
            this.grpTareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).EndInit();
            this.pnlDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalleTarea)).EndInit();
            this.pnlDetalleHeader.ResumeLayout(false);
            this.pnlDetalleHeader.PerformLayout();
            this.grpFacturacion.ResumeLayout(false);
            this.grpFacturacion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.TableLayoutPanel tblFiltros;
        private System.Windows.Forms.Label lblFiltroDocumento;
        private System.Windows.Forms.TextBox txtFiltroDocumento;
        private System.Windows.Forms.Label lblFiltroUsuario;
        private System.Windows.Forms.ComboBox cboFiltroUsuario;
        private System.Windows.Forms.Label lblFechaDesde;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.Label lblFechaHasta;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Panel pnlBotonesFiltro;
        private System.Windows.Forms.Button btnLimpiarFiltros;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.GroupBox grpTareas;
        private System.Windows.Forms.DataGridView dgvTareas;
        private System.Windows.Forms.Panel pnlDetalle;
        private System.Windows.Forms.DataGridView dgvDetalleTarea;
        private System.Windows.Forms.Panel pnlDetalleHeader;
        private System.Windows.Forms.Label lblDetalleHeader;
        private System.Windows.Forms.Button btnColapsarDetalle;
        private System.Windows.Forms.GroupBox grpFacturacion;
        private System.Windows.Forms.Label lblEntidadSalida;
        private System.Windows.Forms.TextBox txtEntidadSalida;
        private System.Windows.Forms.Label lblFechaFactura;
        private System.Windows.Forms.DateTimePicker dtpFechaFactura;
        private System.Windows.Forms.Label lblCodigoFactura;
        private System.Windows.Forms.TextBox txtCodigoFactura;
        private System.Windows.Forms.Label lblImporteFactura;
        private System.Windows.Forms.TextBox txtImporteFactura;
        private System.Windows.Forms.Label lblDivisa;
        private System.Windows.Forms.ComboBox cboDivisa;
        private System.Windows.Forms.Label lblTipoFactura;
        private System.Windows.Forms.ComboBox cboTipoFactura;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCerrar;
    }
}
