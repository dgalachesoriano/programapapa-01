namespace GestionFacturas.Formularios
{
    partial class FrmConsultasSql
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado

        /// <summary>
        /// Todos los controles de esta pantalla se crean por código
        /// (ver FrmConsultasSql.ConstruirInterfaz), no desde el
        /// diseñador: los campos de parámetros se generan en tiempo de
        /// ejecución según la consulta seleccionada, así que es más
        /// simple construir toda la pantalla en una sola pasada de
        /// código. Aquí solo se fijan las propiedades del propio
        /// formulario.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // FrmConsultasSql
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1100, 700);
            this.MinimumSize = new System.Drawing.Size(900, 550);
            this.Name = "FrmConsultasSql";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Datos · Consultas SQL";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
