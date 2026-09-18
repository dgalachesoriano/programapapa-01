using GestionFacturas.Formularios;
using System;
using System.Windows.Forms;

namespace GestionFacturas
{
    /// <summary>
    /// Formulario principal (menú) de la aplicación. Desde aquí se
    /// abren el resto de pantallas como ventanas modales.
    /// </summary>
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Controlador de evento vacío generado por el diseñador al
        /// enganchar el evento Load del formulario; no requiere
        /// lógica adicional.
        /// </summary>
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla de registro de una
        /// tarea nueva.
        /// </summary>
        private void registrarTareaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRegistrarTarea formulario = new FrmRegistrarTarea();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla de facturación
        /// (paso 3 del flujo).
        /// </summary>
        private void facturarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFacturar formulario = new FrmFacturar();

            formulario.ShowDialog();
        }

        /// <summary>
        /// Abre, como diálogo modal, la pantalla del pool de tareas.
        /// </summary>
        private void poolDeTareasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPoolTareas formulario = new FrmPoolTareas();

            formulario.ShowDialog();
        }
    }
}
