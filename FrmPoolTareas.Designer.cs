namespace GestionFacturas
{
    partial class FrmPoolTareas
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
            this.lblEstado = new System.Windows.Forms.Label();
            this.cboFiltroEstado = new System.Windows.Forms.ComboBox();
            this.lblFiltroProyecto = new System.Windows.Forms.Label();
            this.cboFiltroProyecto = new System.Windows.Forms.ComboBox();
            this.lblFiltroUsuario = new System.Windows.Forms.Label();
            this.cboFiltroUsuario = new System.Windows.Forms.ComboBox();
            this.lblFechaDesde = new System.Windows.Forms.Label();
            this.dtpFechaDesde = new System.Windows.Forms.DateTimePicker();
            this.lblFechaHasta = new System.Windows.Forms.Label();
            this.dtpFechaHasta = new System.Windows.Forms.DateTimePicker();
            this.pnlBotonesFiltro = new System.Windows.Forms.Panel();
            this.btnLimpiarFiltros = new System.Windows.Forms.Button();
            this.btnAplicarFiltros = new System.Windows.Forms.Button();
            this.splitPrincipal = new System.Windows.Forms.SplitContainer();
            this.grpTareas = new System.Windows.Forms.GroupBox();
            this.dgvTareas = new System.Windows.Forms.DataGridView();
            this.grpDetalle = new System.Windows.Forms.GroupBox();
            this.dgvDetalle = new System.Windows.Forms.DataGridView();
            this.grpAsignacion = new System.Windows.Forms.GroupBox();
            this.lblUsuarioAsignado = new System.Windows.Forms.Label();
            this.cboUsuarioAsignado = new System.Windows.Forms.ComboBox();
            this.btnAsignarTarea = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnAbrirTarea = new System.Windows.Forms.Button();
            this.lblNuevoEstado = new System.Windows.Forms.Label();
            this.cboNuevoEstado = new System.Windows.Forms.ComboBox();
            this.btnCambiarEstado = new System.Windows.Forms.Button();
            this.grpFiltros.SuspendLayout();
            this.tblFiltros.SuspendLayout();
            this.pnlBotonesFiltro.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPrincipal)).BeginInit();
            this.splitPrincipal.Panel1.SuspendLayout();
            this.splitPrincipal.Panel2.SuspendLayout();
            this.splitPrincipal.SuspendLayout();
            this.grpTareas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).BeginInit();
            this.grpDetalle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).BeginInit();
            this.grpAsignacion.SuspendLayout();
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
            this.tblFiltros.ColumnCount = 6;
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 55F));
            this.tblFiltros.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
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
            // Fila 0: Estado / Proyecto / Usuario
            //
            this.lblEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblEstado.AutoSize = true;
            this.lblEstado.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblEstado.Name = "lblEstado";
            this.lblEstado.Size = new System.Drawing.Size(43, 13);
            this.lblEstado.TabIndex = 0;
            this.lblEstado.Text = "Estado:";
            this.tblFiltros.Controls.Add(this.lblEstado, 0, 0);
            //
            this.cboFiltroEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFiltroEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroEstado.FormattingEnabled = true;
            this.cboFiltroEstado.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFiltroEstado.Name = "cboFiltroEstado";
            this.cboFiltroEstado.Size = new System.Drawing.Size(130, 21);
            this.cboFiltroEstado.TabIndex = 1;
            this.tblFiltros.Controls.Add(this.cboFiltroEstado, 1, 0);
            //
            this.lblFiltroProyecto.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltroProyecto.AutoSize = true;
            this.lblFiltroProyecto.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFiltroProyecto.Name = "lblFiltroProyecto";
            this.lblFiltroProyecto.Size = new System.Drawing.Size(52, 13);
            this.lblFiltroProyecto.TabIndex = 2;
            this.lblFiltroProyecto.Text = "Proyecto:";
            this.tblFiltros.Controls.Add(this.lblFiltroProyecto, 2, 0);
            //
            this.cboFiltroProyecto.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFiltroProyecto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroProyecto.FormattingEnabled = true;
            this.cboFiltroProyecto.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFiltroProyecto.Name = "cboFiltroProyecto";
            this.cboFiltroProyecto.Size = new System.Drawing.Size(130, 21);
            this.cboFiltroProyecto.TabIndex = 3;
            this.tblFiltros.Controls.Add(this.cboFiltroProyecto, 3, 0);
            //
            this.lblFiltroUsuario.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFiltroUsuario.AutoSize = true;
            this.lblFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFiltroUsuario.Name = "lblFiltroUsuario";
            this.lblFiltroUsuario.Size = new System.Drawing.Size(46, 13);
            this.lblFiltroUsuario.TabIndex = 4;
            this.lblFiltroUsuario.Text = "Usuario:";
            this.tblFiltros.Controls.Add(this.lblFiltroUsuario, 4, 0);
            //
            this.cboFiltroUsuario.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboFiltroUsuario.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFiltroUsuario.FormattingEnabled = true;
            this.cboFiltroUsuario.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboFiltroUsuario.Name = "cboFiltroUsuario";
            this.cboFiltroUsuario.Size = new System.Drawing.Size(130, 21);
            this.cboFiltroUsuario.TabIndex = 5;
            this.tblFiltros.Controls.Add(this.cboFiltroUsuario, 5, 0);
            //
            // Fila 1: Desde / Hasta
            //
            this.lblFechaDesde.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaDesde.Name = "lblFechaDesde";
            this.lblFechaDesde.Size = new System.Drawing.Size(41, 13);
            this.lblFechaDesde.TabIndex = 6;
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
            this.dtpFechaDesde.TabIndex = 7;
            this.tblFiltros.Controls.Add(this.dtpFechaDesde, 1, 1);
            //
            this.lblFechaHasta.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Margin = new System.Windows.Forms.Padding(3, 0, 3, 0);
            this.lblFechaHasta.Name = "lblFechaHasta";
            this.lblFechaHasta.Size = new System.Drawing.Size(38, 13);
            this.lblFechaHasta.TabIndex = 8;
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
            this.dtpFechaHasta.TabIndex = 9;
            this.tblFiltros.Controls.Add(this.dtpFechaHasta, 3, 1);
            //
            // Fila 2: barra de botones de filtro (ocupa todo el ancho)
            //
            this.tblFiltros.SetColumnSpan(this.pnlBotonesFiltro, 6);
            this.pnlBotonesFiltro.Controls.Add(this.btnLimpiarFiltros);
            this.pnlBotonesFiltro.Controls.Add(this.btnAplicarFiltros);
            this.pnlBotonesFiltro.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBotonesFiltro.Location = new System.Drawing.Point(0, 64);
            this.pnlBotonesFiltro.Margin = new System.Windows.Forms.Padding(0);
            this.pnlBotonesFiltro.Name = "pnlBotonesFiltro";
            this.pnlBotonesFiltro.Size = new System.Drawing.Size(1164, 46);
            this.pnlBotonesFiltro.TabIndex = 10;
            this.tblFiltros.Controls.Add(this.pnlBotonesFiltro, 0, 2);
            //
            // btnAplicarFiltros
            //
            this.btnAplicarFiltros.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAplicarFiltros.Location = new System.Drawing.Point(946, 6);
            this.btnAplicarFiltros.Name = "btnAplicarFiltros";
            this.btnAplicarFiltros.Size = new System.Drawing.Size(99, 36);
            this.btnAplicarFiltros.TabIndex = 10;
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
            this.btnLimpiarFiltros.TabIndex = 11;
            this.btnLimpiarFiltros.Text = "Limpiar";
            this.btnLimpiarFiltros.UseVisualStyleBackColor = true;
            this.btnLimpiarFiltros.Click += new System.EventHandler(this.btnLimpiarFiltros_Click);
            //
            // splitPrincipal
            //
            // Divide el espacio central entre la rejilla de tareas y la
            // de detalle; el usuario puede arrastrar el separador y
            // ambos paneles se adaptan al tamaño de la ventana.
            this.splitPrincipal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitPrincipal.FixedPanel = System.Windows.Forms.FixedPanel.None;
            this.splitPrincipal.Location = new System.Drawing.Point(0, 150);
            this.splitPrincipal.Name = "splitPrincipal";
            this.splitPrincipal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            //
            // splitPrincipal.Panel1
            //
            this.splitPrincipal.Panel1.Controls.Add(this.grpTareas);
            this.splitPrincipal.Panel1MinSize = 120;
            //
            // splitPrincipal.Panel2
            //
            this.splitPrincipal.Panel2.Controls.Add(this.grpDetalle);
            this.splitPrincipal.Panel2MinSize = 120;
            this.splitPrincipal.Size = new System.Drawing.Size(1184, 500);
            this.splitPrincipal.SplitterDistance = 245;
            this.splitPrincipal.TabIndex = 1;
            //
            // grpTareas
            //
            this.grpTareas.Controls.Add(this.dgvTareas);
            this.grpTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpTareas.Location = new System.Drawing.Point(0, 0);
            this.grpTareas.Name = "grpTareas";
            this.grpTareas.Padding = new System.Windows.Forms.Padding(10);
            this.grpTareas.Size = new System.Drawing.Size(1184, 245);
            this.grpTareas.TabIndex = 0;
            this.grpTareas.TabStop = false;
            this.grpTareas.Text = "Tareas";
            //
            // dgvTareas
            //
            this.dgvTareas.AllowUserToAddRows = false;
            this.dgvTareas.AllowUserToDeleteRows = false;
            this.dgvTareas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTareas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTareas.Location = new System.Drawing.Point(10, 23);
            this.dgvTareas.MultiSelect = true;
            this.dgvTareas.Name = "dgvTareas";
            this.dgvTareas.ReadOnly = true;
            this.dgvTareas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTareas.Size = new System.Drawing.Size(1164, 212);
            this.dgvTareas.TabIndex = 0;
            this.dgvTareas.SelectionChanged += new System.EventHandler(this.dgvTareas_SelectionChanged);
            //
            // grpDetalle
            //
            this.grpDetalle.Controls.Add(this.dgvDetalle);
            this.grpDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpDetalle.Location = new System.Drawing.Point(0, 0);
            this.grpDetalle.Name = "grpDetalle";
            this.grpDetalle.Padding = new System.Windows.Forms.Padding(10);
            this.grpDetalle.Size = new System.Drawing.Size(1184, 251);
            this.grpDetalle.TabIndex = 0;
            this.grpDetalle.TabStop = false;
            this.grpDetalle.Text = "Detalle de factura";
            //
            // dgvDetalle
            //
            this.dgvDetalle.AllowUserToAddRows = false;
            this.dgvDetalle.AllowUserToDeleteRows = false;
            this.dgvDetalle.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDetalle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDetalle.Location = new System.Drawing.Point(10, 23);
            this.dgvDetalle.MultiSelect = false;
            this.dgvDetalle.Name = "dgvDetalle";
            this.dgvDetalle.ReadOnly = true;
            this.dgvDetalle.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDetalle.Size = new System.Drawing.Size(1164, 218);
            this.dgvDetalle.TabIndex = 0;
            //
            // grpAsignacion
            //
            // Anclado al fondo del formulario, con alto fijo; los
            // botones se anclan a la derecha para seguir siempre ahí.
            // Dos filas de acciones en bloque sobre la selección
            // actual de la rejilla de tareas: asignar usuario (fila
            // 1) y cambiar estado (fila 2).
            this.grpAsignacion.Controls.Add(this.btnAbrirTarea);
            this.grpAsignacion.Controls.Add(this.btnActualizar);
            this.grpAsignacion.Controls.Add(this.btnAsignarTarea);
            this.grpAsignacion.Controls.Add(this.cboUsuarioAsignado);
            this.grpAsignacion.Controls.Add(this.lblUsuarioAsignado);
            this.grpAsignacion.Controls.Add(this.btnCambiarEstado);
            this.grpAsignacion.Controls.Add(this.cboNuevoEstado);
            this.grpAsignacion.Controls.Add(this.lblNuevoEstado);
            this.grpAsignacion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpAsignacion.Location = new System.Drawing.Point(0, 610);
            this.grpAsignacion.Name = "grpAsignacion";
            this.grpAsignacion.Size = new System.Drawing.Size(1184, 140);
            this.grpAsignacion.TabIndex = 3;
            this.grpAsignacion.TabStop = false;
            this.grpAsignacion.Text = "Asignación y acciones (sobre las tareas seleccionadas)";
            //
            // lblUsuarioAsignado
            //
            this.lblUsuarioAsignado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblUsuarioAsignado.AutoSize = true;
            this.lblUsuarioAsignado.Location = new System.Drawing.Point(12, 40);
            this.lblUsuarioAsignado.Name = "lblUsuarioAsignado";
            this.lblUsuarioAsignado.Size = new System.Drawing.Size(46, 13);
            this.lblUsuarioAsignado.TabIndex = 0;
            this.lblUsuarioAsignado.Text = "Usuario:";
            //
            // cboUsuarioAsignado
            //
            this.cboUsuarioAsignado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboUsuarioAsignado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUsuarioAsignado.FormattingEnabled = true;
            this.cboUsuarioAsignado.Location = new System.Drawing.Point(64, 37);
            this.cboUsuarioAsignado.Name = "cboUsuarioAsignado";
            this.cboUsuarioAsignado.Size = new System.Drawing.Size(760, 21);
            this.cboUsuarioAsignado.TabIndex = 1;
            //
            // btnAsignarTarea
            //
            this.btnAsignarTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAsignarTarea.Location = new System.Drawing.Point(836, 30);
            this.btnAsignarTarea.Name = "btnAsignarTarea";
            this.btnAsignarTarea.Size = new System.Drawing.Size(99, 38);
            this.btnAsignarTarea.TabIndex = 2;
            this.btnAsignarTarea.Text = "Asignar tarea";
            this.btnAsignarTarea.UseVisualStyleBackColor = true;
            this.btnAsignarTarea.Click += new System.EventHandler(this.btnAsignarTarea_Click);
            //
            // btnAbrirTarea
            //
            this.btnAbrirTarea.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAbrirTarea.Location = new System.Drawing.Point(941, 30);
            this.btnAbrirTarea.Name = "btnAbrirTarea";
            this.btnAbrirTarea.Size = new System.Drawing.Size(99, 38);
            this.btnAbrirTarea.TabIndex = 4;
            this.btnAbrirTarea.Text = "Abrir tarea";
            this.btnAbrirTarea.UseVisualStyleBackColor = true;
            this.btnAbrirTarea.Click += new System.EventHandler(this.btnAbrirTarea_Click);
            //
            // btnActualizar
            //
            this.btnActualizar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnActualizar.Location = new System.Drawing.Point(1046, 30);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(99, 38);
            this.btnActualizar.TabIndex = 3;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            //
            // lblNuevoEstado
            //
            this.lblNuevoEstado.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblNuevoEstado.AutoSize = true;
            this.lblNuevoEstado.Location = new System.Drawing.Point(12, 90);
            this.lblNuevoEstado.Name = "lblNuevoEstado";
            this.lblNuevoEstado.Size = new System.Drawing.Size(78, 13);
            this.lblNuevoEstado.TabIndex = 5;
            this.lblNuevoEstado.Text = "Nuevo estado:";
            //
            // cboNuevoEstado
            //
            this.cboNuevoEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cboNuevoEstado.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNuevoEstado.FormattingEnabled = true;
            this.cboNuevoEstado.Location = new System.Drawing.Point(96, 87);
            this.cboNuevoEstado.Name = "cboNuevoEstado";
            this.cboNuevoEstado.Size = new System.Drawing.Size(728, 21);
            this.cboNuevoEstado.TabIndex = 6;
            //
            // btnCambiarEstado
            //
            this.btnCambiarEstado.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCambiarEstado.Location = new System.Drawing.Point(836, 80);
            this.btnCambiarEstado.Name = "btnCambiarEstado";
            this.btnCambiarEstado.Size = new System.Drawing.Size(204, 38);
            this.btnCambiarEstado.TabIndex = 7;
            this.btnCambiarEstado.Text = "Cambiar estado";
            this.btnCambiarEstado.UseVisualStyleBackColor = true;
            this.btnCambiarEstado.Click += new System.EventHandler(this.btnCambiarEstado_Click);
            //
            // FrmPoolTareas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 800);
            this.Controls.Add(this.splitPrincipal);
            this.Controls.Add(this.grpAsignacion);
            this.Controls.Add(this.grpFiltros);
            this.MinimumSize = new System.Drawing.Size(900, 590);
            this.Name = "FrmPoolTareas";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pool de Tareas";
            this.grpFiltros.ResumeLayout(false);
            this.tblFiltros.ResumeLayout(false);
            this.tblFiltros.PerformLayout();
            this.pnlBotonesFiltro.ResumeLayout(false);
            this.splitPrincipal.Panel1.ResumeLayout(false);
            this.splitPrincipal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPrincipal)).EndInit();
            this.splitPrincipal.ResumeLayout(false);
            this.grpTareas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTareas)).EndInit();
            this.grpDetalle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDetalle)).EndInit();
            this.grpAsignacion.ResumeLayout(false);
            this.grpAsignacion.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpFiltros;
        private System.Windows.Forms.TableLayoutPanel tblFiltros;
        private System.Windows.Forms.ComboBox cboFiltroEstado;
        private System.Windows.Forms.Label lblEstado;
        private System.Windows.Forms.Label lblFechaDesde;
        private System.Windows.Forms.ComboBox cboFiltroUsuario;
        private System.Windows.Forms.Label lblFiltroUsuario;
        private System.Windows.Forms.ComboBox cboFiltroProyecto;
        private System.Windows.Forms.Label lblFiltroProyecto;
        private System.Windows.Forms.Panel pnlBotonesFiltro;
        private System.Windows.Forms.Button btnLimpiarFiltros;
        private System.Windows.Forms.Button btnAplicarFiltros;
        private System.Windows.Forms.DateTimePicker dtpFechaHasta;
        private System.Windows.Forms.Label lblFechaHasta;
        private System.Windows.Forms.DateTimePicker dtpFechaDesde;
        private System.Windows.Forms.SplitContainer splitPrincipal;
        private System.Windows.Forms.GroupBox grpTareas;
        private System.Windows.Forms.DataGridView dgvTareas;
        private System.Windows.Forms.GroupBox grpDetalle;
        private System.Windows.Forms.DataGridView dgvDetalle;
        private System.Windows.Forms.GroupBox grpAsignacion;
        private System.Windows.Forms.Button btnAbrirTarea;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnAsignarTarea;
        private System.Windows.Forms.ComboBox cboUsuarioAsignado;
        private System.Windows.Forms.Label lblUsuarioAsignado;
        private System.Windows.Forms.Label lblNuevoEstado;
        private System.Windows.Forms.ComboBox cboNuevoEstado;
        private System.Windows.Forms.Button btnCambiarEstado;
    }
}
