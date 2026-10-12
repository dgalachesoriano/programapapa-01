namespace GestionFacturas.Formularios
{
    partial class FrmInformeMotivosBloqueo
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
        /// (ver FrmInformeMotivosBloqueo.ConstruirInterfaz), no desde
        /// el diseñador: al tratarse de un informe con secciones
        /// dinámicas (tarjetas KPI cuyo número no varía pero cuyo
        /// contenido sí, gráfica y rejilla enlazadas a consultas), es
        /// más simple y menos propenso a errores construirlo en una
        /// sola pasada de código que mantener sincronizados dos
        /// archivos. Aquí solo se fijan las propiedades del propio
        /// formulario.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            //
            // FrmInformeMotivosBloqueo
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.MinimumSize = new System.Drawing.Size(1000, 650);
            this.Name = "FrmInformeMotivosBloqueo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Informe · Motivos de Bloqueo";
            this.ResumeLayout(false);
        }

        #endregion
    }
}
