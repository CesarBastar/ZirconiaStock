using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZirconiaStock
{
    public partial class EditarResinaForm : Form
    {
        private Inventario inventario;
        private Resina resina;

        public EditarResinaForm(Inventario inv, Resina r)
        {
            InitializeComponent();
            inventario = inv;
            resina = r;

            nudCantidad.Minimum = 0;
            nudCantidad.Maximum = 10;

            txtNombre.Text = r.Nombre;
            nudCantidad.Value = r.Cantidad;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text.Trim() == "")
            {
                MessageBox.Show("Ingrese un nombre");
                return;
            }
            resina.Nombre = txtNombre.Text;
            resina.Cantidad = (int)nudCantidad.Value;
            inventario.EditarResina(resina);
            MessageBox.Show("Resina editada exitosamente");
            this.Close();

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("¿Eliminar esta resina?", "Confirmar",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                inventario.EliminarResina(resina.Id);
                MessageBox.Show("Resina eliminada");
                this.Close();
            }
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
