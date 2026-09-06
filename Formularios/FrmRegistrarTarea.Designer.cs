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
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblFEntCalidad = new System.Windows.Forms.Label();
            this.dtpFEntCalidad = new System.Windows.Forms.DateTimePicker();
            this.lblFRegistro = new System.Windows.Forms.Label();
            this.dtpFRegistro = new System.Windows.Forms.DateTimePicker();
            this.lblSociedad = new System.Windows.Forms.Label();
            this.txtSociedad = new System.Windows.Forms.TextBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.txtProyecto = new System.Windows.Forms.TextBox();
            this.lblSegmento = new System.Windows.Forms.Label();
            this.cboSegmento = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.cboUsuario = new System.Windows.Forms.ComboBox();
            this.lblImporteEstimado = new System.Windows.Forms.Label();
            this.txtImporteEstimado = new System.Windows.Forms.TextBox();
            this.grpDetalle = new System.Windows.Forms.GroupBox();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.pnlBotones = new System.Windows.Forms.Panel();
            this.btnGrabar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.grpCabecera.SuspendLayout();
            this.tblCabecera.SuspendLayout();
            this.grpDetalle.SuspendLayout();
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
            this.grpCabecera.Size = new System.Drawing.Size(1184, 215);
            this.grpCabecera.TabIndex = 0;
            this.grpCabecera.TabStop = false;
            this.grpCabecera.Text = "Datos de la RN/PL";
            //
            // tblCabecera
            //
            // Tabla de 4 columnas (etiqueta/control x2) que reparte el
            // ancho disponible entre las dos columnas de controles, de
            // forma que se ensanchan o encogen junto con la ventana.
            this.tblCabecera.ColumnCount = 4;
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tblCabecera.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblCabecera.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblCabecera.Location = new System.Drawing.Point(10, 23);
            this.tblCabecera.Name = "tblCabecera";
            this.tblCabecera.RowCount = 5;
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblCabecera.Size = new System.Drawing.Size(1164, 182);
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
            this.txtDocumento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumento.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(200, 20);
            this.txtDocumento.TabIndex = 0;
            this.tblCabecera.SetColumnSpan(this.txtDocumento, 3);
            this.tblCabecera.Controls.Add(this.txtDocumento, 1, 0);
            //
            // Fila 1: F. Ent. Calidad / F. Registro
            //
            this.lblFEntCalidad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFEntCalidad.AutoSize = true;
            this.lblFEntCalidad.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFEntCalidad.Name = "lblFEntCalidad";
            this.lblFEntCalidad.Size = new System.Drawing.Size(79, 13);
            this.lblFEntCalidad.TabIndex = 1;
            this.lblFEntCalidad.Text = "F. Ent. Calidad:";
            this.tblCabecera.Controls.Add(this.lblFEntCalidad, 0, 1);
            //
            this.dtpFEntCalidad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFEntCalidad.CustomFormat = "dd/MM/yyyy";
            this.dtpFEntCalidad.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFEntCalidad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFEntCalidad.Name = "dtpFEntCalidad";
            this.dtpFEntCalidad.Size = new System.Drawing.Size(150, 20);
            this.dtpFEntCalidad.TabIndex = 1;
            this.tblCabecera.Controls.Add(this.dtpFEntCalidad, 1, 1);
            //
            this.lblFRegistro.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFRegistro.AutoSize = true;
            this.lblFRegistro.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFRegistro.Name = "lblFRegistro";
            this.lblFRegistro.Size = new System.Drawing.Size(61, 13);
            this.lblFRegistro.TabIndex = 2;
            this.lblFRegistro.Text = "F. Registro:";
            this.tblCabecera.Controls.Add(this.lblFRegistro, 2, 1);
            //
            this.dtpFRegistro.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFRegistro.CustomFormat = "dd/MM/yyyy";
            this.dtpFRegistro.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpFRegistro.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFRegistro.Name = "dtpFRegistro";
            this.dtpFRegistro.Size = new System.Drawing.Size(150, 20);
            this.dtpFRegistro.TabIndex = 2;
            this.tblCabecera.Controls.Add(this.dtpFRegistro, 3, 1);
            //
            // Fila 2: Sociedad / Proyecto
            //
            this.lblSociedad.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblSociedad.AutoSize = true;
            this.lblSociedad.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblSociedad.Name = "lblSociedad";
            this.lblSociedad.Size = new System.Drawing.Size(55, 13);
            this.lblSociedad.TabIndex = 3;
            this.lblSociedad.Text = "Sociedad:";
            this.tblCabecera.Controls.Add(this.lblSociedad, 0, 2);
            //
            this.txtSociedad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSociedad.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSociedad.Name = "txtSociedad";
            this.txtSociedad.Size = new System.Drawing.Size(150, 20);
            this.txtSociedad.TabIndex = 3;
            this.tblCabecera.Controls.Add(this.txtSociedad, 1, 2);
            //
            this.lblProyecto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(52, 13);
            this.lblProyecto.TabIndex = 4;
            this.lblProyecto.Text = "Proyecto:";
            this.tblCabecera.Controls.Add(this.lblProyecto, 2, 2);
            //
            this.txtProyecto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProyecto.Name = "txtProyecto";
            this.txtProyecto.Size = new System.Drawing.Size(150, 20);
            this.txtProyecto.TabIndex = 4;
            this.tblCabecera.Controls.Add(this.txtProyecto, 3, 2);
            //
            // Fila 3: Segmento / Usuario
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
            this.cboSegmento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboSegmento.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSegmento.FormattingEnabled = true;
            this.cboSegmento.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboSegmento.Name = "cboSegmento";
            this.cboSegmento.Size = new System.Drawing.Size(150, 21);
            this.cboSegmento.TabIndex = 5;
            this.tblCabecera.Controls.Add(this.cboSegmento, 1, 3);
            //
            this.lblUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 14;
            this.lblUsuario.Text = "Usuario:";
            this.tblCabecera.Controls.Add(this.lblUsuario, 2, 3);
            //
            this.cboUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUsuario.FormattingEnabled = true;
            this.cboUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboUsuario.Name = "cboUsuario";
            this.cboUsuario.Size = new System.Drawing.Size(150, 21);
            this.cboUsuario.TabIndex = 7;
            this.tblCabecera.Controls.Add(this.cboUsuario, 3, 3);
            //
            // Fila 4: Importe Estimado
            //
            this.lblImporteEstimado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblImporteEstimado.AutoSize = true;
            this.lblImporteEstimado.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblImporteEstimado.Name = "lblImporteEstimado";
            this.lblImporteEstimado.Size = new System.Drawing.Size(91, 13);
            this.lblImporteEstimado.TabIndex = 12;
            this.lblImporteEstimado.Text = "Importe Estimado:";
            this.tblCabecera.Controls.Add(this.lblImporteEstimado, 0, 4);
            //
            this.txtImporteEstimado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtImporteEstimado.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtImporteEstimado.Name = "txtImporteEstimado";
            this.txtImporteEstimado.Size = new System.Drawing.Size(150, 20);
            this.txtImporteEstimado.TabIndex = 6;
            this.txtImporteEstimado.Leave += new System.EventHandler(this.txtImporteEstimado_Leave);
            this.tblCabecera.Controls.Add(this.txtImporteEstimado, 1, 4);
            //
            // grpDetalle
            //
            // Ocupa todo el espacio central que quede libre entre la
            // cabecera (arriba) y la barra de botones (abajo), tanto
            // al maximizar como al redimensionar la ventana.
            this.grpDetalle.Controls.Add(this.dgvDetalle);
            this.grpDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalle.Location = new System.Drawing.Point(0, 215);
            this.grpDetalle.Name = "grpDetalle";
            this.grpDetalle.Padding = new System.Windows.Forms.Padding(10);
            this.grpDetalle.Size = new System.Drawing.Size(1184, 396);
            this.grpDetalle.TabIndex = 1;
            this.grpDetalle.TabStop = false;
            this.grpDetalle.Text = "Detalle";
            //
            // dgvDetalle
            //
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalle.Location = new System.Drawing.Point(10, 23);
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.Size = new System.Drawing.Size(1164, 363);
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
            this.grpDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.pnlBotones.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpCabecera;
        private System.Windows.Forms.TableLayoutPanel tblCabecera;
        private System.Windows.Forms.DateTimePicker dtpFEntCalidad;
        private System.Windows.Forms.Label lblFEntCalidad;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.TextBox txtSociedad;
        private System.Windows.Forms.Label lblSociedad;
        private System.Windows.Forms.DateTimePicker dtpFRegistro;
        private System.Windows.Forms.Label lblFRegistro;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.Label lblImporteEstimado;
        private System.Windows.Forms.TextBox txtProyecto;
        private System.Windows.Forms.Label lblProyecto;
        private System.Windows.Forms.ComboBox cboUsuario;
        private System.Windows.Forms.GroupBox grpDetalle;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.Panel pnlBotones;
        private System.Windows.Forms.Button btnGrabar;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.TextBox txtImporteEstimado;
        private System.Windows.Forms.Label lblSegmento;
        private System.Windows.Forms.ComboBox cboSegmento;
    }
}
