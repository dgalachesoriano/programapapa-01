namespace GestionFacturas.Formularios
{
    partial class FrmDetalleTarea
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
            this.pnlEstado = new System.Windows.Forms.GroupBox();
            this.lblMotivoValor = new System.Windows.Forms.Label();
            this.lblMotivoTitulo = new System.Windows.Forms.Label();
            this.cboUsuarioAsignado = new System.Windows.Forms.ComboBox();
            this.lblUsuarioTitulo = new System.Windows.Forms.Label();
            this.lblEstadoValor = new System.Windows.Forms.Label();
            this.lblEstadoTitulo = new System.Windows.Forms.Label();
            this.grpCabecera = new System.Windows.Forms.GroupBox();
            this.tblCabecera = new System.Windows.Forms.TableLayoutPanel();
            this.lblDocumento = new System.Windows.Forms.Label();
            this.pnlDocumentoBorde = new System.Windows.Forms.Panel();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblFEntCalidad = new System.Windows.Forms.Label();
            this.dtpFEntCalidad = new System.Windows.Forms.DateTimePicker();
            this.lblFRegistro = new System.Windows.Forms.Label();
            this.dtpFRegistro = new System.Windows.Forms.DateTimePicker();
            this.lblOrgVentas = new System.Windows.Forms.Label();
            this.pnlOrgVentasBorde = new System.Windows.Forms.Panel();
            this.txtOrgVentas = new System.Windows.Forms.TextBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.pnlProyectoBorde = new System.Windows.Forms.Panel();
            this.cboProyecto = new System.Windows.Forms.ComboBox();
            this.lblSegmento = new System.Windows.Forms.Label();
            this.pnlSegmentoBorde = new System.Windows.Forms.Panel();
            this.cboSegmento = new System.Windows.Forms.ComboBox();
            this.lblImporteEstimado = new System.Windows.Forms.Label();
            this.pnlImporteEstimadoBorde = new System.Windows.Forms.Panel();
            this.txtImporteEstimado = new System.Windows.Forms.TextBox();
            this.pnlDetalle = new System.Windows.Forms.Panel();
            this.pnlDetalleBorde = new System.Windows.Forms.Panel();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.pnlDetalleHeader = new System.Windows.Forms.Panel();
            this.lblDetalleHeader = new System.Windows.Forms.Label();
            this.btnColapsarDetalle = new System.Windows.Forms.Button();
            this.grpFacturacion = new System.Windows.Forms.GroupBox();
            this.lblEntidadSalida = new System.Windows.Forms.Label();
            this.pnlEntidadSalidaBorde = new System.Windows.Forms.Panel();
            this.txtEntidadSalida = new System.Windows.Forms.TextBox();
            this.lblFechaFactura = new System.Windows.Forms.Label();
            this.dtpFechaFactura = new System.Windows.Forms.DateTimePicker();
            this.lblCodigoFactura = new System.Windows.Forms.Label();
            this.pnlCodigoFacturaBorde = new System.Windows.Forms.Panel();
            this.txtCodigoFactura = new System.Windows.Forms.TextBox();
            this.lblImporteFactura = new System.Windows.Forms.Label();
            this.pnlImporteFacturaBorde = new System.Windows.Forms.Panel();
            this.txtImporteFactura = new System.Windows.Forms.TextBox();
            this.lblDivisa = new System.Windows.Forms.Label();
            this.pnlDivisaBorde = new System.Windows.Forms.Panel();
            this.cboDivisa = new System.Windows.Forms.ComboBox();
            this.lblTipoFactura = new System.Windows.Forms.Label();
            this.pnlTipoFacturaBorde = new System.Windows.Forms.Panel();
            this.cboTipoFactura = new System.Windows.Forms.ComboBox();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnCancelarTarea = new System.Windows.Forms.Button();
            this.btnDesbloquearTarea = new System.Windows.Forms.Button();
            this.btnBloquearTarea = new System.Windows.Forms.Button();
            this.pnlEstado.SuspendLayout();
            this.grpCabecera.SuspendLayout();
            this.tblCabecera.SuspendLayout();
            this.pnlDocumentoBorde.SuspendLayout();
            this.pnlOrgVentasBorde.SuspendLayout();
            this.pnlProyectoBorde.SuspendLayout();
            this.pnlSegmentoBorde.SuspendLayout();
            this.pnlImporteEstimadoBorde.SuspendLayout();
            this.pnlDetalle.SuspendLayout();
            this.pnlDetalleBorde.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.pnlDetalleHeader.SuspendLayout();
            this.grpFacturacion.SuspendLayout();
            this.pnlEntidadSalidaBorde.SuspendLayout();
            this.pnlCodigoFacturaBorde.SuspendLayout();
            this.pnlImporteFacturaBorde.SuspendLayout();
            this.pnlDivisaBorde.SuspendLayout();
            this.pnlTipoFacturaBorde.SuspendLayout();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // pnlEstado
            //
            // Franja superior con el estado actual (solo lectura,
            // coloreado) y el usuario asignado (editable); el motivo
            // de bloqueo solo se muestra cuando la tarea está
            // Bloqueada (ver FrmDetalleTarea.CargarEstadoYUsuario).
            this.pnlEstado.Controls.Add(this.lblMotivoValor);
            this.pnlEstado.Controls.Add(this.lblMotivoTitulo);
            this.pnlEstado.Controls.Add(this.cboUsuarioAsignado);
            this.pnlEstado.Controls.Add(this.lblUsuarioTitulo);
            this.pnlEstado.Controls.Add(this.lblEstadoValor);
            this.pnlEstado.Controls.Add(this.lblEstadoTitulo);
            this.pnlEstado.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEstado.Location = new System.Drawing.Point(0, 0);
            this.pnlEstado.Name = "pnlEstado";
            this.pnlEstado.Size = new System.Drawing.Size(1150, 90);
            this.pnlEstado.TabIndex = 0;
            this.pnlEstado.TabStop = false;
            this.pnlEstado.Text = "Estado de la tarea";
            //
            // lblEstadoTitulo
            //
            this.lblEstadoTitulo.AutoSize = true;
            this.lblEstadoTitulo.Location = new System.Drawing.Point(12, 30);
            this.lblEstadoTitulo.Name = "lblEstadoTitulo";
            this.lblEstadoTitulo.Size = new System.Drawing.Size(43, 13);
            this.lblEstadoTitulo.TabIndex = 0;
            this.lblEstadoTitulo.Text = "Estado:";
            //
            // lblEstadoValor
            //
            this.lblEstadoValor.AutoSize = true;
            this.lblEstadoValor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblEstadoValor.Location = new System.Drawing.Point(100, 28);
            this.lblEstadoValor.Name = "lblEstadoValor";
            this.lblEstadoValor.Size = new System.Drawing.Size(15, 15);
            this.lblEstadoValor.TabIndex = 1;
            this.lblEstadoValor.Text = "-";
            //
            // lblUsuarioTitulo
            //
            this.lblUsuarioTitulo.AutoSize = true;
            this.lblUsuarioTitulo.Location = new System.Drawing.Point(360, 30);
            this.lblUsuarioTitulo.Name = "lblUsuarioTitulo";
            this.lblUsuarioTitulo.Size = new System.Drawing.Size(96, 13);
            this.lblUsuarioTitulo.TabIndex = 2;
            this.lblUsuarioTitulo.Text = "Usuario asignado:";
            //
            // cboUsuarioAsignado
            //
            this.cboUsuarioAsignado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuarioAsignado.FormattingEnabled = true;
            this.cboUsuarioAsignado.Location = new System.Drawing.Point(500, 27);
            this.cboUsuarioAsignado.Name = "cboUsuarioAsignado";
            this.cboUsuarioAsignado.Size = new System.Drawing.Size(250, 21);
            this.cboUsuarioAsignado.TabIndex = 3;
            //
            // lblMotivoTitulo
            //
            this.lblMotivoTitulo.AutoSize = true;
            this.lblMotivoTitulo.Location = new System.Drawing.Point(12, 60);
            this.lblMotivoTitulo.Name = "lblMotivoTitulo";
            this.lblMotivoTitulo.Size = new System.Drawing.Size(93, 13);
            this.lblMotivoTitulo.TabIndex = 4;
            this.lblMotivoTitulo.Text = "Motivo bloqueo:";
            this.lblMotivoTitulo.Visible = false;
            //
            // lblMotivoValor
            //
            this.lblMotivoValor.AutoSize = true;
            this.lblMotivoValor.Location = new System.Drawing.Point(140, 60);
            this.lblMotivoValor.Name = "lblMotivoValor";
            this.lblMotivoValor.Size = new System.Drawing.Size(15, 13);
            this.lblMotivoValor.TabIndex = 5;
            this.lblMotivoValor.Text = "-";
            this.lblMotivoValor.Visible = false;
            //
            // grpCabecera
            //
            this.grpCabecera.Controls.Add(this.tblCabecera);
            this.grpCabecera.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCabecera.Location = new System.Drawing.Point(0, 90);
            this.grpCabecera.Name = "grpCabecera";
            this.grpCabecera.Padding = new System.Windows.Forms.Padding(10);
            this.grpCabecera.Size = new System.Drawing.Size(1150, 183);
            this.grpCabecera.TabIndex = 1;
            this.grpCabecera.TabStop = false;
            this.grpCabecera.Text = "Datos de la Tarea";
            //
            // tblCabecera
            //
            this.tblCabecera.ColumnCount = 4;
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCabecera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblCabecera.Location = new System.Drawing.Point(10, 23);
            this.tblCabecera.Name = "tblCabecera";
            this.tblCabecera.RowCount = 4;
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.Size = new System.Drawing.Size(1130, 150);
            this.tblCabecera.TabIndex = 0;
            //
            // Fila 0: Documento
            //
            this.lblDocumento.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(65, 13);
            this.lblDocumento.TabIndex = 0;
            this.lblDocumento.Text = "Documento:";
            this.tblCabecera.Controls.Add(this.lblDocumento, 0, 0);
            //
            // pnlDocumentoBorde
            //
            this.pnlDocumentoBorde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDocumentoBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDocumentoBorde.Controls.Add(this.txtDocumento);
            this.pnlDocumentoBorde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlDocumentoBorde.Name = "pnlDocumentoBorde";
            this.pnlDocumentoBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDocumentoBorde.Size = new System.Drawing.Size(200, 24);
            this.pnlDocumentoBorde.TabIndex = 1;
            this.tblCabecera.SetColumnSpan(this.pnlDocumentoBorde, 3);
            this.tblCabecera.Controls.Add(this.pnlDocumentoBorde, 1, 0);
            //
            this.txtDocumento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDocumento.Location = new System.Drawing.Point(2, 2);
            this.txtDocumento.Margin = new System.Windows.Forms.Padding(0);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(196, 20);
            this.txtDocumento.TabIndex = 0;
            //
            // Fila 1: F. Ent. Calidad / F. Registro
            //
            this.lblFEntCalidad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFEntCalidad.AutoSize = true;
            this.lblFEntCalidad.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFEntCalidad.Name = "lblFEntCalidad";
            this.lblFEntCalidad.Size = new System.Drawing.Size(79, 13);
            this.lblFEntCalidad.TabIndex = 2;
            this.lblFEntCalidad.Text = "F. Ent. Calidad:";
            this.tblCabecera.Controls.Add(this.lblFEntCalidad, 0, 1);
            //
            this.dtpFEntCalidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFEntCalidad.CustomFormat = "dd/MM/yyyy";
            this.dtpFEntCalidad.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFEntCalidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFEntCalidad.Name = "dtpFEntCalidad";
            this.dtpFEntCalidad.Size = new System.Drawing.Size(150, 20);
            this.dtpFEntCalidad.TabIndex = 3;
            this.tblCabecera.Controls.Add(this.dtpFEntCalidad, 1, 1);
            //
            this.lblFRegistro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFRegistro.AutoSize = true;
            this.lblFRegistro.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFRegistro.Name = "lblFRegistro";
            this.lblFRegistro.Size = new System.Drawing.Size(61, 13);
            this.lblFRegistro.TabIndex = 4;
            this.lblFRegistro.Text = "F. Registro:";
            this.tblCabecera.Controls.Add(this.lblFRegistro, 2, 1);
            //
            this.dtpFRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFRegistro.CustomFormat = "dd/MM/yyyy";
            this.dtpFRegistro.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFRegistro.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFRegistro.Name = "dtpFRegistro";
            this.dtpFRegistro.Size = new System.Drawing.Size(150, 20);
            this.dtpFRegistro.TabIndex = 5;
            this.tblCabecera.Controls.Add(this.dtpFRegistro, 3, 1);
            //
            // Fila 2: Organización de Ventas / Proyecto
            //
            this.lblOrgVentas.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblOrgVentas.AutoSize = true;
            this.lblOrgVentas.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblOrgVentas.Name = "lblOrgVentas";
            this.lblOrgVentas.Size = new System.Drawing.Size(130, 13);
            this.lblOrgVentas.TabIndex = 6;
            this.lblOrgVentas.Text = "Organización de Ventas:";
            this.tblCabecera.Controls.Add(this.lblOrgVentas, 0, 2);
            //
            // pnlOrgVentasBorde
            //
            this.pnlOrgVentasBorde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlOrgVentasBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlOrgVentasBorde.Controls.Add(this.txtOrgVentas);
            this.pnlOrgVentasBorde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlOrgVentasBorde.Name = "pnlOrgVentasBorde";
            this.pnlOrgVentasBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlOrgVentasBorde.Size = new System.Drawing.Size(150, 24);
            this.pnlOrgVentasBorde.TabIndex = 7;
            this.tblCabecera.Controls.Add(this.pnlOrgVentasBorde, 1, 2);
            //
            this.txtOrgVentas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOrgVentas.Location = new System.Drawing.Point(2, 2);
            this.txtOrgVentas.Margin = new System.Windows.Forms.Padding(0);
            this.txtOrgVentas.Name = "txtOrgVentas";
            this.txtOrgVentas.Size = new System.Drawing.Size(146, 20);
            this.txtOrgVentas.TabIndex = 0;
            //
            this.lblProyecto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(52, 13);
            this.lblProyecto.TabIndex = 8;
            this.lblProyecto.Text = "Proyecto:";
            this.tblCabecera.Controls.Add(this.lblProyecto, 2, 2);
            //
            // pnlProyectoBorde
            //
            this.pnlProyectoBorde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlProyectoBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlProyectoBorde.Controls.Add(this.cboProyecto);
            this.pnlProyectoBorde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlProyectoBorde.Name = "pnlProyectoBorde";
            this.pnlProyectoBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlProyectoBorde.Size = new System.Drawing.Size(150, 25);
            this.pnlProyectoBorde.TabIndex = 9;
            this.tblCabecera.Controls.Add(this.pnlProyectoBorde, 3, 2);
            //
            this.cboProyecto.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboProyecto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProyecto.FormattingEnabled = true;
            this.cboProyecto.Location = new System.Drawing.Point(2, 2);
            this.cboProyecto.Margin = new System.Windows.Forms.Padding(0);
            this.cboProyecto.Name = "cboProyecto";
            this.cboProyecto.Size = new System.Drawing.Size(146, 21);
            this.cboProyecto.TabIndex = 0;
            //
            // Fila 3: Segmento / Importe Estimado
            //
            this.lblSegmento.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSegmento.AutoSize = true;
            this.lblSegmento.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblSegmento.Name = "lblSegmento";
            this.lblSegmento.Size = new System.Drawing.Size(61, 13);
            this.lblSegmento.TabIndex = 10;
            this.lblSegmento.Text = "Segmento :";
            this.tblCabecera.Controls.Add(this.lblSegmento, 0, 3);
            //
            // pnlSegmentoBorde
            //
            this.pnlSegmentoBorde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSegmentoBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlSegmentoBorde.Controls.Add(this.cboSegmento);
            this.pnlSegmentoBorde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlSegmentoBorde.Name = "pnlSegmentoBorde";
            this.pnlSegmentoBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlSegmentoBorde.Size = new System.Drawing.Size(150, 25);
            this.pnlSegmentoBorde.TabIndex = 11;
            this.tblCabecera.Controls.Add(this.pnlSegmentoBorde, 1, 3);
            //
            this.cboSegmento.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboSegmento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSegmento.FormattingEnabled = true;
            this.cboSegmento.Location = new System.Drawing.Point(2, 2);
            this.cboSegmento.Margin = new System.Windows.Forms.Padding(0);
            this.cboSegmento.Name = "cboSegmento";
            this.cboSegmento.Size = new System.Drawing.Size(146, 21);
            this.cboSegmento.TabIndex = 0;
            //
            this.lblImporteEstimado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblImporteEstimado.AutoSize = true;
            this.lblImporteEstimado.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblImporteEstimado.Name = "lblImporteEstimado";
            this.lblImporteEstimado.Size = new System.Drawing.Size(91, 13);
            this.lblImporteEstimado.TabIndex = 12;
            this.lblImporteEstimado.Text = "Importe Estimado:";
            this.tblCabecera.Controls.Add(this.lblImporteEstimado, 2, 3);
            //
            // pnlImporteEstimadoBorde
            //
            this.pnlImporteEstimadoBorde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlImporteEstimadoBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlImporteEstimadoBorde.Controls.Add(this.txtImporteEstimado);
            this.pnlImporteEstimadoBorde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlImporteEstimadoBorde.Name = "pnlImporteEstimadoBorde";
            this.pnlImporteEstimadoBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlImporteEstimadoBorde.Size = new System.Drawing.Size(150, 24);
            this.pnlImporteEstimadoBorde.TabIndex = 13;
            this.tblCabecera.Controls.Add(this.pnlImporteEstimadoBorde, 3, 3);
            //
            this.txtImporteEstimado.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtImporteEstimado.Location = new System.Drawing.Point(2, 2);
            this.txtImporteEstimado.Margin = new System.Windows.Forms.Padding(0);
            this.txtImporteEstimado.Name = "txtImporteEstimado";
            this.txtImporteEstimado.Size = new System.Drawing.Size(146, 20);
            this.txtImporteEstimado.TabIndex = 0;
            this.txtImporteEstimado.Leave += new System.EventHandler(this.txtImporteEstimado_Leave);
            //
            // pnlDetalle
            //
            // Panel colapsable: su alto alterna entre el de la cabecera
            // (colapsado) y AltoDetalleExpandido (desplegado) al pulsar
            // btnColapsarDetalle. Empieza desplegado.
            this.pnlDetalle.Controls.Add(this.pnlDetalleBorde);
            this.pnlDetalle.Controls.Add(this.pnlDetalleHeader);
            this.pnlDetalle.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetalle.Location = new System.Drawing.Point(0, 273);
            this.pnlDetalle.Name = "pnlDetalle";
            this.pnlDetalle.Padding = new System.Windows.Forms.Padding(10, 0, 10, 10);
            this.pnlDetalle.Size = new System.Drawing.Size(1150, 200);
            this.pnlDetalle.TabIndex = 2;
            //
            // pnlDetalleBorde
            //
            this.pnlDetalleBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDetalleBorde.Controls.Add(this.dgvDetalle);
            this.pnlDetalleBorde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalleBorde.Location = new System.Drawing.Point(10, 28);
            this.pnlDetalleBorde.Name = "pnlDetalleBorde";
            this.pnlDetalleBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDetalleBorde.Size = new System.Drawing.Size(1130, 162);
            this.pnlDetalleBorde.TabIndex = 1;
            //
            // dgvDetalle
            //
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalle.Location = new System.Drawing.Point(2, 2);
            this.dgvDetalle.Margin = new System.Windows.Forms.Padding(0);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.Size = new System.Drawing.Size(1126, 158);
            this.dgvDetalle.TabIndex = 0;
            //
            // pnlDetalleHeader
            //
            this.pnlDetalleHeader.Controls.Add(this.btnColapsarDetalle);
            this.pnlDetalleHeader.Controls.Add(this.lblDetalleHeader);
            this.pnlDetalleHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlDetalleHeader.Location = new System.Drawing.Point(10, 0);
            this.pnlDetalleHeader.Name = "pnlDetalleHeader";
            this.pnlDetalleHeader.Size = new System.Drawing.Size(1130, 28);
            this.pnlDetalleHeader.TabIndex = 0;
            //
            // lblDetalleHeader
            //
            this.lblDetalleHeader.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDetalleHeader.AutoSize = true;
            this.lblDetalleHeader.Location = new System.Drawing.Point(0, 7);
            this.lblDetalleHeader.Name = "lblDetalleHeader";
            this.lblDetalleHeader.Size = new System.Drawing.Size(85, 13);
            this.lblDetalleHeader.TabIndex = 0;
            this.lblDetalleHeader.Text = "Detalle de tarea";
            //
            // btnColapsarDetalle
            //
            this.btnColapsarDetalle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColapsarDetalle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColapsarDetalle.Location = new System.Drawing.Point(970, 0);
            this.btnColapsarDetalle.Name = "btnColapsarDetalle";
            this.btnColapsarDetalle.Size = new System.Drawing.Size(160, 26);
            this.btnColapsarDetalle.TabIndex = 1;
            this.btnColapsarDetalle.Text = "▼ Ocultar detalle";
            this.btnColapsarDetalle.UseVisualStyleBackColor = true;
            this.btnColapsarDetalle.Click += new System.EventHandler(this.btnColapsarDetalle_Click);
            //
            // grpFacturacion
            //
            this.grpFacturacion.Controls.Add(this.lblEntidadSalida);
            this.grpFacturacion.Controls.Add(this.pnlEntidadSalidaBorde);
            this.grpFacturacion.Controls.Add(this.lblFechaFactura);
            this.grpFacturacion.Controls.Add(this.dtpFechaFactura);
            this.grpFacturacion.Controls.Add(this.lblCodigoFactura);
            this.grpFacturacion.Controls.Add(this.pnlCodigoFacturaBorde);
            this.grpFacturacion.Controls.Add(this.lblImporteFactura);
            this.grpFacturacion.Controls.Add(this.pnlImporteFacturaBorde);
            this.grpFacturacion.Controls.Add(this.lblDivisa);
            this.grpFacturacion.Controls.Add(this.pnlDivisaBorde);
            this.grpFacturacion.Controls.Add(this.lblTipoFactura);
            this.grpFacturacion.Controls.Add(this.pnlTipoFacturaBorde);
            this.grpFacturacion.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFacturacion.Location = new System.Drawing.Point(0, 473);
            this.grpFacturacion.Name = "grpFacturacion";
            this.grpFacturacion.Size = new System.Drawing.Size(1150, 140);
            this.grpFacturacion.TabIndex = 3;
            this.grpFacturacion.TabStop = false;
            this.grpFacturacion.Text = "Datos de facturación (vacíos si la tarea aún no se ha facturado)";
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
            // pnlEntidadSalidaBorde
            //
            this.pnlEntidadSalidaBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlEntidadSalidaBorde.Controls.Add(this.txtEntidadSalida);
            this.pnlEntidadSalidaBorde.Location = new System.Drawing.Point(140, 30);
            this.pnlEntidadSalidaBorde.Name = "pnlEntidadSalidaBorde";
            this.pnlEntidadSalidaBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlEntidadSalidaBorde.Size = new System.Drawing.Size(180, 24);
            this.pnlEntidadSalidaBorde.TabIndex = 1;
            //
            this.txtEntidadSalida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEntidadSalida.Location = new System.Drawing.Point(2, 2);
            this.txtEntidadSalida.Margin = new System.Windows.Forms.Padding(0);
            this.txtEntidadSalida.Name = "txtEntidadSalida";
            this.txtEntidadSalida.Size = new System.Drawing.Size(176, 20);
            this.txtEntidadSalida.TabIndex = 0;
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
            // pnlCodigoFacturaBorde
            //
            this.pnlCodigoFacturaBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlCodigoFacturaBorde.Controls.Add(this.txtCodigoFactura);
            this.pnlCodigoFacturaBorde.Location = new System.Drawing.Point(140, 65);
            this.pnlCodigoFacturaBorde.Name = "pnlCodigoFacturaBorde";
            this.pnlCodigoFacturaBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlCodigoFacturaBorde.Size = new System.Drawing.Size(180, 24);
            this.pnlCodigoFacturaBorde.TabIndex = 5;
            //
            this.txtCodigoFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCodigoFactura.Location = new System.Drawing.Point(2, 2);
            this.txtCodigoFactura.Margin = new System.Windows.Forms.Padding(0);
            this.txtCodigoFactura.Name = "txtCodigoFactura";
            this.txtCodigoFactura.Size = new System.Drawing.Size(176, 20);
            this.txtCodigoFactura.TabIndex = 0;
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
            // pnlImporteFacturaBorde
            //
            this.pnlImporteFacturaBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlImporteFacturaBorde.Controls.Add(this.txtImporteFactura);
            this.pnlImporteFacturaBorde.Location = new System.Drawing.Point(440, 65);
            this.pnlImporteFacturaBorde.Name = "pnlImporteFacturaBorde";
            this.pnlImporteFacturaBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlImporteFacturaBorde.Size = new System.Drawing.Size(150, 24);
            this.pnlImporteFacturaBorde.TabIndex = 7;
            //
            this.txtImporteFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtImporteFactura.Location = new System.Drawing.Point(2, 2);
            this.txtImporteFactura.Margin = new System.Windows.Forms.Padding(0);
            this.txtImporteFactura.Name = "txtImporteFactura";
            this.txtImporteFactura.Size = new System.Drawing.Size(146, 20);
            this.txtImporteFactura.TabIndex = 0;
            this.txtImporteFactura.Leave += new System.EventHandler(this.txtImporteFactura_Leave);
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
            // pnlDivisaBorde
            //
            this.pnlDivisaBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDivisaBorde.Controls.Add(this.cboDivisa);
            this.pnlDivisaBorde.Location = new System.Drawing.Point(140, 100);
            this.pnlDivisaBorde.Name = "pnlDivisaBorde";
            this.pnlDivisaBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDivisaBorde.Size = new System.Drawing.Size(120, 25);
            this.pnlDivisaBorde.TabIndex = 9;
            //
            this.cboDivisa.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboDivisa.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDivisa.FormattingEnabled = true;
            this.cboDivisa.Location = new System.Drawing.Point(2, 2);
            this.cboDivisa.Margin = new System.Windows.Forms.Padding(0);
            this.cboDivisa.Name = "cboDivisa";
            this.cboDivisa.Size = new System.Drawing.Size(116, 21);
            this.cboDivisa.TabIndex = 0;
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
            // pnlTipoFacturaBorde
            //
            this.pnlTipoFacturaBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlTipoFacturaBorde.Controls.Add(this.cboTipoFactura);
            this.pnlTipoFacturaBorde.Location = new System.Drawing.Point(440, 100);
            this.pnlTipoFacturaBorde.Name = "pnlTipoFacturaBorde";
            this.pnlTipoFacturaBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlTipoFacturaBorde.Size = new System.Drawing.Size(250, 25);
            this.pnlTipoFacturaBorde.TabIndex = 11;
            //
            this.cboTipoFactura.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cboTipoFactura.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoFactura.FormattingEnabled = true;
            this.cboTipoFactura.Location = new System.Drawing.Point(2, 2);
            this.cboTipoFactura.Margin = new System.Windows.Forms.Padding(0);
            this.cboTipoFactura.Name = "cboTipoFactura";
            this.cboTipoFactura.Size = new System.Drawing.Size(246, 21);
            this.cboTipoFactura.TabIndex = 0;
            //
            // pnlBotones
            //
            // Franja inferior de altura fija anclada al fondo del
            // formulario; los botones se anclan a su esquina superior
            // derecha. De izquierda a derecha: acciones de cambio de
            // estado (Bloquear/Desbloquear/Cancelar) y, al final,
            // Guardar/Cerrar.
            this.pnlBotones.Controls.Add(this.btnCerrar);
            this.pnlBotones.Controls.Add(this.btnGuardar);
            this.pnlBotones.Controls.Add(this.btnCancelarTarea);
            this.pnlBotones.Controls.Add(this.btnDesbloquearTarea);
            this.pnlBotones.Controls.Add(this.btnBloquearTarea);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 613);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Padding = new System.Windows.Forms.Padding(0, 8, 12, 8);
            this.pnlBotones.Size = new System.Drawing.Size(1150, 60);
            this.pnlBotones.TabIndex = 4;
            //
            // btnBloquearTarea
            //
            this.btnBloquearTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBloquearTarea.Location = new System.Drawing.Point(430, 13);
            this.btnBloquearTarea.Name = "btnBloquearTarea";
            this.btnBloquearTarea.Size = new System.Drawing.Size(150, 34);
            this.btnBloquearTarea.TabIndex = 0;
            this.btnBloquearTarea.Text = "Bloquear tarea";
            this.btnBloquearTarea.UseVisualStyleBackColor = true;
            this.btnBloquearTarea.Click += new System.EventHandler(this.btnBloquearTarea_Click);
            //
            // btnDesbloquearTarea
            //
            this.btnDesbloquearTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDesbloquearTarea.Location = new System.Drawing.Point(590, 13);
            this.btnDesbloquearTarea.Name = "btnDesbloquearTarea";
            this.btnDesbloquearTarea.Size = new System.Drawing.Size(150, 34);
            this.btnDesbloquearTarea.TabIndex = 1;
            this.btnDesbloquearTarea.Text = "Desbloquear tarea";
            this.btnDesbloquearTarea.UseVisualStyleBackColor = true;
            this.btnDesbloquearTarea.Click += new System.EventHandler(this.btnDesbloquearTarea_Click);
            //
            // btnCancelarTarea
            //
            this.btnCancelarTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelarTarea.Location = new System.Drawing.Point(750, 13);
            this.btnCancelarTarea.Name = "btnCancelarTarea";
            this.btnCancelarTarea.Size = new System.Drawing.Size(150, 34);
            this.btnCancelarTarea.TabIndex = 2;
            this.btnCancelarTarea.Text = "Cancelar tarea";
            this.btnCancelarTarea.UseVisualStyleBackColor = true;
            this.btnCancelarTarea.Click += new System.EventHandler(this.btnCancelarTarea_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGuardar.Location = new System.Drawing.Point(910, 13);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(109, 34);
            this.btnGuardar.TabIndex = 3;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1029, 13);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(109, 34);
            this.btnCerrar.TabIndex = 4;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FrmDetalleTarea
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1150, 673);
            this.Controls.Add(this.grpFacturacion);
            this.Controls.Add(this.pnlDetalle);
            this.Controls.Add(this.grpCabecera);
            this.Controls.Add(this.pnlEstado);
            this.Controls.Add(this.pnlBotones);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(950, 500);
            this.Name = "FrmDetalleTarea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tarea";
            this.pnlEstado.ResumeLayout(false);
            this.pnlEstado.PerformLayout();
            this.grpCabecera.ResumeLayout(false);
            this.tblCabecera.ResumeLayout(false);
            this.tblCabecera.PerformLayout();
            this.pnlDocumentoBorde.ResumeLayout(false);
            this.pnlDocumentoBorde.PerformLayout();
            this.pnlOrgVentasBorde.ResumeLayout(false);
            this.pnlOrgVentasBorde.PerformLayout();
            this.pnlProyectoBorde.ResumeLayout(false);
            this.pnlSegmentoBorde.ResumeLayout(false);
            this.pnlImporteEstimadoBorde.ResumeLayout(false);
            this.pnlImporteEstimadoBorde.PerformLayout();
            this.pnlDetalle.ResumeLayout(false);
            this.pnlDetalleBorde.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.pnlDetalleHeader.ResumeLayout(false);
            this.pnlDetalleHeader.PerformLayout();
            this.grpFacturacion.ResumeLayout(false);
            this.grpFacturacion.PerformLayout();
            this.pnlEntidadSalidaBorde.ResumeLayout(false);
            this.pnlEntidadSalidaBorde.PerformLayout();
            this.pnlCodigoFacturaBorde.ResumeLayout(false);
            this.pnlCodigoFacturaBorde.PerformLayout();
            this.pnlImporteFacturaBorde.ResumeLayout(false);
            this.pnlImporteFacturaBorde.PerformLayout();
            this.pnlDivisaBorde.ResumeLayout(false);
            this.pnlTipoFacturaBorde.ResumeLayout(false);
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pnlEstado;
        private System.Windows.Forms.Label lblEstadoTitulo;
        private System.Windows.Forms.Label lblEstadoValor;
        private System.Windows.Forms.Label lblUsuarioTitulo;
        private System.Windows.Forms.ComboBox cboUsuarioAsignado;
        private System.Windows.Forms.Label lblMotivoTitulo;
        private System.Windows.Forms.Label lblMotivoValor;
        private System.Windows.Forms.GroupBox grpCabecera;
        private System.Windows.Forms.TableLayoutPanel tblCabecera;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.Panel pnlDocumentoBorde;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblFEntCalidad;
        private System.Windows.Forms.DateTimePicker dtpFEntCalidad;
        private System.Windows.Forms.Label lblFRegistro;
        private System.Windows.Forms.DateTimePicker dtpFRegistro;
        private System.Windows.Forms.Label lblOrgVentas;
        private System.Windows.Forms.Panel pnlOrgVentasBorde;
        private System.Windows.Forms.TextBox txtOrgVentas;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.Panel pnlProyectoBorde;
        private System.Windows.Forms.ComboBox cboProyecto;
        private System.Windows.Forms.Label lblSegmento;
        private System.Windows.Forms.Panel pnlSegmentoBorde;
        private System.Windows.Forms.ComboBox cboSegmento;
        private System.Windows.Forms.Label lblImporteEstimado;
        private System.Windows.Forms.Panel pnlImporteEstimadoBorde;
        private System.Windows.Forms.TextBox txtImporteEstimado;
        private System.Windows.Forms.Panel pnlDetalle;
        private System.Windows.Forms.Panel pnlDetalleBorde;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Panel pnlDetalleHeader;
        private System.Windows.Forms.Label lblDetalleHeader;
        private System.Windows.Forms.Button btnColapsarDetalle;
        private System.Windows.Forms.GroupBox grpFacturacion;
        private System.Windows.Forms.Label lblEntidadSalida;
        private System.Windows.Forms.Panel pnlEntidadSalidaBorde;
        private System.Windows.Forms.TextBox txtEntidadSalida;
        private System.Windows.Forms.Label lblFechaFactura;
        private System.Windows.Forms.DateTimePicker dtpFechaFactura;
        private System.Windows.Forms.Label lblCodigoFactura;
        private System.Windows.Forms.Panel pnlCodigoFacturaBorde;
        private System.Windows.Forms.TextBox txtCodigoFactura;
        private System.Windows.Forms.Label lblImporteFactura;
        private System.Windows.Forms.Panel pnlImporteFacturaBorde;
        private System.Windows.Forms.TextBox txtImporteFactura;
        private System.Windows.Forms.Label lblDivisa;
        private System.Windows.Forms.Panel pnlDivisaBorde;
        private System.Windows.Forms.ComboBox cboDivisa;
        private System.Windows.Forms.Label lblTipoFactura;
        private System.Windows.Forms.Panel pnlTipoFacturaBorde;
        private System.Windows.Forms.ComboBox cboTipoFactura;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnBloquearTarea;
        private System.Windows.Forms.Button btnDesbloquearTarea;
        private System.Windows.Forms.Button btnCancelarTarea;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnCerrar;
    }
}
