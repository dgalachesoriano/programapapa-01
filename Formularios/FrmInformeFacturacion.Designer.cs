namespace GestionFacturas.Formularios
{
    partial class FrmInformeFacturacion
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
            this.lblFechaDesde = new System.Windows.Forms.Label();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.lblFechaHasta = new System.Windows.Forms.Label();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.lblFiltroUsuario = new System.Windows.Forms.Label();
            this.cboFiltroUsuario = new System.Windows.Forms.ComboBox();
            this.pnlBotonesFiltro = new System.Windows.Forms.Panel();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.btnLimpiarFiltros = new System.Windows.Forms.Button();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.grpGrafica = new System.Windows.Forms.GroupBox();
            this.chartFacturacion = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.grpFiltros.SuspendLayout();
            this.tblFiltros.SuspendLayout();
            this.pnlBotonesFiltro.SuspendLayout();
            this.grpGrafica.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartFacturacion)).BeginInit();
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
            this.grpFiltros.Text = "Filtros";
            //
            // tblFiltros
            //
            this.tblFiltros.ColumnCount = 4;
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
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
            // Fila 0: Desde / Hasta
            //
            this.lblFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaDesde.Name = "lblFechaDesde";
            this.lblFechaDesde.Size = new System.Drawing.Size(41, 13);
            this.lblFechaDesde.TabIndex = 0;
            this.lblFechaDesde.Text = "Desde:";
            this.tblFiltros.Controls.Add(this.lblFechaDesde, 0, 0);
            //
            this.dtpFechaDesde.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFechaDesde.Checked = false;
            this.dtpFechaDesde.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaDesde.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFechaDesde.Name = "dtpFechaDesde";
            this.dtpFechaDesde.ShowCheckBox = true;
            this.dtpFechaDesde.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaDesde.TabIndex = 1;
            this.tblFiltros.Controls.Add(this.dtpFechaDesde, 1, 0);
            //
            this.lblFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaHasta.Name = "lblFechaHasta";
            this.lblFechaHasta.Size = new System.Drawing.Size(38, 13);
            this.lblFechaHasta.TabIndex = 2;
            this.lblFechaHasta.Text = "Hasta:";
            this.tblFiltros.Controls.Add(this.lblFechaHasta, 2, 0);
            //
            this.dtpFechaHasta.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpFechaHasta.Checked = false;
            this.dtpFechaHasta.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFechaHasta.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dtpFechaHasta.Name = "dtpFechaHasta";
            this.dtpFechaHasta.ShowCheckBox = true;
            this.dtpFechaHasta.Size = new System.Drawing.Size(150, 20);
            this.dtpFechaHasta.TabIndex = 3;
            this.tblFiltros.Controls.Add(this.dtpFechaHasta, 3, 0);
            //
            // Fila 1: Usuario
            //
            this.lblFiltroUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltroUsuario.AutoSize = true;
            this.lblFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFiltroUsuario.Name = "lblFiltroUsuario";
            this.lblFiltroUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblFiltroUsuario.TabIndex = 4;
            this.lblFiltroUsuario.Text = "Usuario:";
            this.tblFiltros.Controls.Add(this.lblFiltroUsuario, 0, 1);
            //
            this.cboFiltroUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFiltroUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroUsuario.FormattingEnabled = true;
            this.cboFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFiltroUsuario.Name = "cboFiltroUsuario";
            this.cboFiltroUsuario.Size = new System.Drawing.Size(150, 21);
            this.cboFiltroUsuario.TabIndex = 5;
            this.tblFiltros.Controls.Add(this.cboFiltroUsuario, 1, 1);
            //
            // Fila 2: barra de botones (ocupa todo el ancho)
            //
            this.tblFiltros.SetColumnSpan(this.pnlBotonesFiltro, 4);
            this.pnlBotonesFiltro.Controls.Add(this.btnCerrar);
            this.pnlBotonesFiltro.Controls.Add(this.btnLimpiarFiltros);
            this.pnlBotonesFiltro.Controls.Add(this.btnAplicarFiltros);
            this.pnlBotonesFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBotonesFiltro.Location = new System.Drawing.Point(0, 64);
            this.pnlBotonesFiltro.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBotonesFiltro.Name = "pnlBotonesFiltro";
            this.pnlBotonesFiltro.Size = new System.Drawing.Size(1164, 46);
            this.pnlBotonesFiltro.TabIndex = 6;
            this.tblFiltros.Controls.Add(this.pnlBotonesFiltro, 0, 2);
            //
            // btnAplicarFiltros
            //
            this.btnAplicarFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicarFiltros.Location = new System.Drawing.Point(829, 6);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(99, 36);
            this.btnAplicarFiltros.TabIndex = 6;
            this.btnAplicarFiltros.Text = "Aplicar filtros";
            this.btnAplicarFiltros.UseVisualStyleBackColor = true;
            this.btnAplicarFiltros.Click += new System.EventHandler(this.btnAplicarFiltros_Click);
            //
            // btnLimpiarFiltros
            //
            this.btnLimpiarFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiarFiltros.Location = new System.Drawing.Point(946, 6);
            this.btnLimpiarFiltros.Name = "btnLimpiarFiltros";
            this.btnLimpiarFiltros.Size = new System.Drawing.Size(99, 36);
            this.btnLimpiarFiltros.TabIndex = 7;
            this.btnLimpiarFiltros.Text = "Limpiar";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            this.btnLimpiarFiltros.Click += new System.EventHandler(this.btnLimpiarFiltros_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1063, 6);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(99, 36);
            this.btnCerrar.TabIndex = 8;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // grpGrafica
            //
            // Ocupa todo el espacio disponible por debajo de los
            // filtros.
            this.grpGrafica.Controls.Add(this.chartFacturacion);
            this.grpGrafica.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpGrafica.Location = new System.Drawing.Point(0, 150);
            this.grpGrafica.Name = "grpGrafica";
            this.grpGrafica.Padding = new System.Windows.Forms.Padding(10);
            this.grpGrafica.Size = new System.Drawing.Size(1184, 470);
            this.grpGrafica.TabIndex = 1;
            this.grpGrafica.TabStop = false;
            this.grpGrafica.Text = "Evolución mensual";
            //
            // chartFacturacion
            //
            this.chartFacturacion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chartFacturacion.Location = new System.Drawing.Point(10, 23);
            this.chartFacturacion.Name = "chartFacturacion";
            this.chartFacturacion.Size = new System.Drawing.Size(1164, 437);
            this.chartFacturacion.TabIndex = 0;
            this.chartFacturacion.Text = "chartFacturacion";
            //
            // FrmInformeFacturacion
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 620);
            this.Controls.Add(this.grpGrafica);
            this.Controls.Add(this.grpFiltros);
            this.MinimumSize = new System.Drawing.Size(900, 480);
            this.Name = "FrmInformeFacturacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Informe de Facturación";
            this.grpFiltros.ResumeLayout(false);
            this.tblFiltros.ResumeLayout(false);
            this.tblFiltros.PerformLayout();
            this.pnlBotonesFiltro.ResumeLayout(false);
            this.grpGrafica.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartFacturacion)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.TableLayoutPanel tblFiltros;
        private System.Windows.Forms.Label lblFechaDesde;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.Label lblFechaHasta;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Label lblFiltroUsuario;
        private System.Windows.Forms.ComboBox cboFiltroUsuario;
        private System.Windows.Forms.Panel pnlBotonesFiltro;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.Button btnLimpiarFiltros;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.GroupBox grpGrafica;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartFacturacion;
    }
}
