namespace GestionFacturas
{
    partial class FrmTratarTarea
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
            this.lblDocumento = new System.Windows.Forms.Label();
            this.txtDocumento = new System.Windows.Forms.TextBox();
            this.lblEstado = new System.Windows.Forms.Label();
            this.cboEstado = new System.Windows.Forms.ComboBox();
            this.lblUsuario = new System.Windows.Forms.Label();
            this.cboUsuario = new System.Windows.Forms.ComboBox();
            this.lblProyecto = new System.Windows.Forms.Label();
            this.txtProyecto = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.grpResultados = new System.Windows.Forms.GroupBox();
            this.dgvTareas = new System.Windows.Forms.DataGridView();
            this.pnlBotonesResultados = new System.Windows.Forms.Panel();
            this.btnAbrir = new System.Windows.Forms.Button();
            this.btnCerrar = new System.Windows.Forms.Button();
            this.grpFiltros.SuspendLayout();
            this.tblFiltros.SuspendLayout();
            this.grpResultados.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).BeginInit();
            this.pnlBotonesResultados.SuspendLayout();
            this.SuspendLayout();
            //
            // grpFiltros
            //
            // Anclado arriba y estirado a lo ancho: siempre ocupa todo
            // el ancho disponible de la ventana, con alto fijo.
            this.grpFiltros.Controls.Add(this.tblFiltros);
            this.grpFiltros.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpFiltros.Location = new System.Drawing.Point(0, 0);
            this.grpFiltros.Name = "grpFiltros";
            this.grpFiltros.Padding = new System.Windows.Forms.Padding(10);
            this.grpFiltros.Size = new System.Drawing.Size(1484, 140);
            this.grpFiltros.TabIndex = 0;
            this.grpFiltros.TabStop = false;
            this.grpFiltros.Text = "Filtros";
            //
            // tblFiltros
            //
            this.tblFiltros.ColumnCount = 4;
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tblFiltros.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tblFiltros.Location = new System.Drawing.Point(10, 23);
            this.tblFiltros.Name = "tblFiltros";
            this.tblFiltros.RowCount = 3;
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tblFiltros.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tblFiltros.Size = new System.Drawing.Size(1464, 107);
            this.tblFiltros.TabIndex = 0;
            //
            // Fila 0: Documento (ocupa las 3 columnas de control)
            //
            this.lblDocumento.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblDocumento.AutoSize = true;
            this.lblDocumento.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblDocumento.Name = "lblDocumento";
            this.lblDocumento.Size = new System.Drawing.Size(65, 13);
            this.lblDocumento.TabIndex = 2;
            this.lblDocumento.Text = "Documento:";
            this.tblFiltros.Controls.Add(this.lblDocumento, 0, 0);
            //
            this.txtDocumento.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDocumento.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDocumento.Name = "txtDocumento";
            this.txtDocumento.Size = new System.Drawing.Size(200, 20);
            this.txtDocumento.TabIndex = 3;
            this.tblFiltros.SetColumnSpan(this.txtDocumento, 3);
            this.tblFiltros.Controls.Add(this.txtDocumento, 1, 0);
            //
            // Fila 1: Estado / Usuario
            //
            this.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstado.AutoSize = true;
            this.lblEstado.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(43, 13);
            this.lblEstado.TabIndex = 0;
            this.lblEstado.Text = "Estado:";
            this.tblFiltros.Controls.Add(this.lblEstado, 0, 1);
            //
            this.cboEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboEstado.FormattingEnabled = true;
            this.cboEstado.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboEstado.Name = "cboEstado";
            this.cboEstado.Size = new System.Drawing.Size(155, 21);
            this.cboEstado.TabIndex = 1;
            this.tblFiltros.Controls.Add(this.cboEstado, 1, 1);
            //
            this.lblUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUsuario.AutoSize = true;
            this.lblUsuario.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblUsuario.Name = "lblUsuario";
            this.lblUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblUsuario.TabIndex = 4;
            this.lblUsuario.Text = "Usuario:";
            this.tblFiltros.Controls.Add(this.lblUsuario, 2, 1);
            //
            this.cboUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuario.FormattingEnabled = true;
            this.cboUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboUsuario.Name = "cboUsuario";
            this.cboUsuario.Size = new System.Drawing.Size(155, 21);
            this.cboUsuario.TabIndex = 5;
            this.tblFiltros.Controls.Add(this.cboUsuario, 3, 1);
            //
            // Fila 2: Proyecto / Buscar
            //
            this.lblProyecto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblProyecto.AutoSize = true;
            this.lblProyecto.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblProyecto.Name = "lblProyecto";
            this.lblProyecto.Size = new System.Drawing.Size(52, 13);
            this.lblProyecto.TabIndex = 7;
            this.lblProyecto.Text = "Proyecto:";
            this.tblFiltros.Controls.Add(this.lblProyecto, 0, 2);
            //
            this.txtProyecto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProyecto.Name = "txtProyecto";
            this.txtProyecto.Size = new System.Drawing.Size(200, 20);
            this.txtProyecto.TabIndex = 8;
            this.tblFiltros.Controls.Add(this.txtProyecto, 1, 2);
            //
            this.btnBuscar.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(110, 34);
            this.btnBuscar.TabIndex = 6;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = true;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            this.tblFiltros.Controls.Add(this.btnBuscar, 3, 2);
            //
            // grpResultados
            //
            // Ocupa todo el espacio central que quede libre entre los
            // filtros (arriba) y la barra de acciones (abajo).
            this.grpResultados.Controls.Add(this.dgvTareas);
            this.grpResultados.Controls.Add(this.pnlBotonesResultados);
            this.grpResultados.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpResultados.Location = new System.Drawing.Point(0, 140);
            this.grpResultados.Name = "grpResultados";
            this.grpResultados.Padding = new System.Windows.Forms.Padding(10);
            this.grpResultados.Size = new System.Drawing.Size(1484, 621);
            this.grpResultados.TabIndex = 1;
            this.grpResultados.TabStop = false;
            this.grpResultados.Text = "Resultados";
            this.grpResultados.Enter += new System.EventHandler(this.grpResultados_Enter);
            //
            // dgvTareas
            //
            this.dgvTareas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTareas.Location = new System.Drawing.Point(10, 23);
            this.dgvTareas.Name = "dgvTareas";
            this.dgvTareas.Size = new System.Drawing.Size(1464, 538);
            this.dgvTareas.TabIndex = 0;
            //
            // pnlBotonesResultados
            //
            // Franja inferior de altura fija, anclada al fondo del
            // GroupBox; los botones se anclan a su esquina superior
            // derecha para seguir siempre ahí al redimensionar.
            this.pnlBotonesResultados.Controls.Add(this.btnCerrar);
            this.pnlBotonesResultados.Controls.Add(this.btnAbrir);
            this.pnlBotonesResultados.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBotonesResultados.Location = new System.Drawing.Point(10, 561);
            this.pnlBotonesResultados.Name = "pnlBotonesResultados";
            this.pnlBotonesResultados.Size = new System.Drawing.Size(1464, 50);
            this.pnlBotonesResultados.TabIndex = 1;
            //
            // btnAbrir
            //
            this.btnAbrir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrir.Location = new System.Drawing.Point(1264, 8);
            this.btnAbrir.Name = "btnAbrir";
            this.btnAbrir.Size = new System.Drawing.Size(90, 34);
            this.btnAbrir.TabIndex = 7;
            this.btnAbrir.Text = "Abrir tarea";
            this.btnAbrir.UseVisualStyleBackColor = true;
            this.btnAbrir.Click += new System.EventHandler(this.btnAbrir_Click);
            //
            // btnCerrar
            //
            this.btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCerrar.Location = new System.Drawing.Point(1364, 8);
            this.btnCerrar.Name = "btnCerrar";
            this.btnCerrar.Size = new System.Drawing.Size(90, 34);
            this.btnCerrar.TabIndex = 8;
            this.btnCerrar.Text = "Cerrar";
            this.btnCerrar.UseVisualStyleBackColor = true;
            this.btnCerrar.Click += new System.EventHandler(this.btnCerrar_Click);
            //
            // FrmTratarTarea
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1484, 761);
            this.Controls.Add(this.grpResultados);
            this.Controls.Add(this.grpFiltros);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "FrmTratarTarea";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tratar Tarea";
            this.Load += new System.EventHandler(this.FrmTratarTarea_Load);
            this.grpFiltros.ResumeLayout(false);
            this.tblFiltros.ResumeLayout(false);
            this.tblFiltros.PerformLayout();
            this.grpResultados.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).EndInit();
            this.pnlBotonesResultados.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.TableLayoutPanel tblFiltros;
        private System.Windows.Forms.TextBox txtDocumento;
        private System.Windows.Forms.Label lblDocumento;
        private System.Windows.Forms.ComboBox cboEstado;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.ComboBox cboUsuario;
        private System.Windows.Forms.Label lblUsuario;
        private System.Windows.Forms.GroupBox grpResultados;
        private System.Windows.Forms.DataGridView dgvTareas;
        private System.Windows.Forms.Panel pnlBotonesResultados;
        private System.Windows.Forms.Button btnCerrar;
        private System.Windows.Forms.Button btnAbrir;
        private System.Windows.Forms.TextBox txtProyecto;
        private System.Windows.Forms.Label lblProyecto;
    }
}
