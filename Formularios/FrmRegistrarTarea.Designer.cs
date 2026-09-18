namespace GestionFacturas.Formularios
{
    partial class FrmRegistrarTarea
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
            this.grpDetalle = new System.Windows.Forms.GroupBox();
            this.pnlDetalleBorde = new System.Windows.Forms.Panel();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpCabecera.SuspendLayout();
            this.tblCabecera.SuspendLayout();
            this.pnlDocumentoBorde.SuspendLayout();
            this.pnlOrgVentasBorde.SuspendLayout();
            this.pnlProyectoBorde.SuspendLayout();
            this.pnlSegmentoBorde.SuspendLayout();
            this.pnlImporteEstimadoBorde.SuspendLayout();
            this.grpDetalle.SuspendLayout();
            this.pnlDetalleBorde.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.pnlBotones.SuspendLayout();
            this.SuspendLayout();
            //
            // grpCabecera
            //
            // Anclado arriba y estirado a lo ancho del formulario: al
            // redimensionar la ventana, la cabecera siempre ocupa todo
            // el ancho disponible mientras conserva su alto fijo.
            this.grpCabecera.Controls.Add(this.tblCabecera);
            this.grpCabecera.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpCabecera.Location = new System.Drawing.Point(0, 0);
            this.grpCabecera.Name = "grpCabecera";
            this.grpCabecera.Padding = new System.Windows.Forms.Padding(10);
            this.grpCabecera.Size = new System.Drawing.Size(1184, 183);
            this.grpCabecera.TabIndex = 0;
            this.grpCabecera.TabStop = false;
            this.grpCabecera.Text = "Datos de la Tarea";
            //
            // tblCabecera
            //
            // Tabla de 4 columnas (etiqueta/control x2) que reparte el
            // ancho disponible entre las dos columnas de controles, de
            // forma que se ensanchan o encogen junto con la ventana.
            // Los campos obligatorios no van directamente en la celda,
            // sino envueltos en un Panel con relleno (pnlXxxBorde):
            // ese panel es el que cambia de color (ver
            // FrmRegistrarTarea.MarcarCampo) para simular un borde
            // rojo alrededor del control cuando falta por rellenar.
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
            this.tblCabecera.Size = new System.Drawing.Size(1164, 150);
            this.tblCabecera.TabIndex = 0;
            //
            // Fila 0: Documento (ocupa las 3 columnas de control)
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
            // grpDetalle
            //
            // Ocupa todo el espacio central que quede libre entre la
            // cabecera (arriba) y la barra de botones (abajo), tanto
            // al maximizar como al redimensionar la ventana.
            this.grpDetalle.Controls.Add(this.pnlDetalleBorde);
            this.grpDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalle.Location = new System.Drawing.Point(0, 183);
            this.grpDetalle.Name = "grpDetalle";
            this.grpDetalle.Padding = new System.Windows.Forms.Padding(10);
            this.grpDetalle.Size = new System.Drawing.Size(1184, 428);
            this.grpDetalle.TabIndex = 1;
            this.grpDetalle.TabStop = false;
            this.grpDetalle.Text = "Detalle";
            //
            // pnlDetalleBorde
            //
            // Igual que los pnlXxxBorde de la cabecera: cambia de
            // color para simular un borde rojo alrededor de todo el
            // grid cuando no hay ninguna línea de detalle completa.
            this.pnlDetalleBorde.BackColor = System.Drawing.SystemColors.Control;
            this.pnlDetalleBorde.Controls.Add(this.dgvDetalle);
            this.pnlDetalleBorde.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDetalleBorde.Location = new System.Drawing.Point(10, 23);
            this.pnlDetalleBorde.Name = "pnlDetalleBorde";
            this.pnlDetalleBorde.Padding = new System.Windows.Forms.Padding(2);
            this.pnlDetalleBorde.Size = new System.Drawing.Size(1164, 395);
            this.pnlDetalleBorde.TabIndex = 0;
            //
            // dgvDetalle
            //
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalle.Location = new System.Drawing.Point(2, 2);
            this.dgvDetalle.Margin = new System.Windows.Forms.Padding(0);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.Size = new System.Drawing.Size(1160, 391);
            this.dgvDetalle.TabIndex = 0;
            //
            // pnlBotones
            //
            // Franja inferior de altura fija anclada al fondo del
            // formulario; los botones se anclan a su esquina
            // superior derecha para seguir siempre ahí al redimensionar.
            this.pnlBotones.Controls.Add(this.btnCancelar);
            this.pnlBotones.Controls.Add(this.btnGrabar);
            this.pnlBotones.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotones.Location = new System.Drawing.Point(0, 611);
            this.pnlBotones.Name = "pnlBotones";
            this.pnlBotones.Padding = new System.Windows.Forms.Padding(0, 8, 12, 8);
            this.pnlBotones.Size = new System.Drawing.Size(1184, 50);
            this.pnlBotones.TabIndex = 2;
            //
            // btnGrabar
            //
            this.btnGrabar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGrabar.Location = new System.Drawing.Point(953, 8);
            this.btnGrabar.Name = "btnGrabar";
            this.btnGrabar.Size = new System.Drawing.Size(109, 34);
            this.btnGrabar.TabIndex = 8;
            this.btnGrabar.Text = "Grabar";
            this.btnGrabar.UseVisualStyleBackColor = true;
            this.btnGrabar.Click += new System.EventHandler(this.btnGrabar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelar.Location = new System.Drawing.Point(1068, 8);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(109, 34);
            this.btnCancelar.TabIndex = 9;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            //
            // FrmRegistrarTarea
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 661);
            this.Controls.Add(this.grpDetalle);
            this.Controls.Add(this.pnlBotones);
            this.Controls.Add(this.grpCabecera);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(820, 480);
            this.Name = "FrmRegistrarTarea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Registrar Tarea";
            this.Load += new System.EventHandler(this.FrmRegistrarTarea_Load);
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
            this.grpDetalle.ResumeLayout(false);
            this.pnlDetalleBorde.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCabecera;
        private System.Windows.Forms.TableLayoutPanel tblCabecera;
        private System.Windows.Forms.DateTimePicker dtpFEntCalidad;
        private System.Windows.Forms.Label lblFEntCalidad;
        private System.Windows.Forms.Panel pnlDocumentoBorde;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.Panel pnlOrgVentasBorde;
        private System.Windows.Forms.TextBox txtOrgVentas;
        private System.Windows.Forms.Label lblOrgVentas;
        private System.Windows.Forms.DateTimePicker dtpFRegistro;
        private System.Windows.Forms.Label lblFRegistro;
        private System.Windows.Forms.Label lblImporteEstimado;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.Panel pnlProyectoBorde;
        private System.Windows.Forms.ComboBox cboProyecto;
        private System.Windows.Forms.GroupBox grpDetalle;
        private System.Windows.Forms.Panel pnlDetalleBorde;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Panel pnlImporteEstimadoBorde;
        private System.Windows.Forms.TextBox txtImporteEstimado;
        private System.Windows.Forms.Label lblSegmento;
        private System.Windows.Forms.Panel pnlSegmentoBorde;
        private System.Windows.Forms.ComboBox cboSegmento;
    }
}
