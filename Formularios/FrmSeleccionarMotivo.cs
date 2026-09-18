using GestionFacturas.Datos;
using GestionFacturas.Modelos;
using GestionFacturas.Servicios;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace GestionFacturas.Formularios
{
    /// <summary>
    /// Diálogo modal para elegir, de una lista desplegable obligatoria
    /// (sin texto libre), el motivo de bloqueo de una o varias tareas.
    /// Usado por FrmPoolTareas al pulsar "Bloquear tarea".
    /// </summary>
    public partial class FrmSeleccionarMotivo : Form
    {
        private readonly MotivoRepositorio motivoRepositorio = new MotivoRepositorio();

        /// <summary>
        /// Identificador del motivo elegido. Solo tiene un valor
        /// válido cuando <see cref="Form.DialogResult"/> es
        /// <see cref="DialogResult.OK"/>.
        /// </summary>
        public int IdMotivoSeleccionado { get; private set; }

        public FrmSeleccionarMotivo()
        {
            InitializeComponent();

            CargarMotivos();
        }

        /// <summary>
        /// Carga en el combo los motivos activos, sin ninguno
        /// preseleccionado: hay que elegir uno explícitamente.
        /// </summary>
        private void CargarMotivos()
        {
            try
            {
                List<Motivo> motivos = motivoRepositorio.ObtenerActivos();

                cboMotivo.DataSource = motivos;
                cboMotivo.DisplayMember = "Descripcion";
                cboMotivo.ValueMember = "Id";
                cboMotivo.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Dialogos.MostrarError(
                    "FrmSeleccionarMotivo.CargarMotivos",
                    "No se han podido cargar los motivos.",
                    ex);
            }
        }

        /// <summary>
        /// Valida que se haya elegido un motivo y, si es así, cierra
        /// el diálogo con resultado OK.
        /// </summary>
        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (cboMotivo.SelectedIndex == -1 || cboMotivo.SelectedValue == null)
            {
                Dialogos.MostrarAviso("Debe seleccionar un motivo.", "Validación");

                cboMotivo.Focus();

                return;
            }

            IdMotivoSeleccionado = Convert.ToInt32(cboMotivo.SelectedValue);

            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Cierra el diálogo sin seleccionar ningún motivo.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
