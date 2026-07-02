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
    public partial class AgregarProducto : Form
    {
        private Inventario inventario;
        private DiscoZirconia discoEditar = null;
        public AgregarProducto(Inventario inv)
        {
            InitializeComponent();
            inventario = inv;
            CargarOpciones();

            nudCantidad.Minimum = 0;
            nudCantidad.Maximum = 100;
            btnEliminar.Visible = false;
        }
        public AgregarProducto(Inventario inv, DiscoZirconia z) : this(inv)
        {
            discoEditar = z;
            this.Text = "Editar Producto";
            cmbNombre.Text = z.Nombre;
            cmbTipo.Text = z.Tipo;
            cmbTamaño.Text = z.Tamaño.ToString();
            cmbColor.Text = z.Color;
            nudCantidad.Value = z.Cantidad;
            btnEliminar.Visible = true;
        }

        private void CargarOpciones()
        {
            List<DiscoZirconia> discos = inventario.ObtenerZirconia();

            cmbNombre.Items.AddRange(discos.Select(z => z.Nombre).Distinct().ToArray());
            cmbTipo.Items.AddRange(discos.Select(z => z.Tipo).Distinct().ToArray());
            cmbTamaño.Items.AddRange(discos.Select(z => z.Tamaño.ToString()).Distinct().ToArray());
            cmbColor.Items.AddRange(discos.Select(z => z.Color).Distinct().ToArray());
        }


        private void btnGuardar_Click(object sender, EventArgs e)
        {
            


            if (cmbNombre.Text == "")
            {
                MessageBox.Show("Ingrese un nombre", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cmbTipo.Text == "")
            {
                MessageBox.Show("Ingrese un tipo", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int tamaño;
            if (!int.TryParse(cmbTamaño.Text, out tamaño))
            {
                MessageBox.Show("El tamaño debe ser un número", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cmbColor.Text == "")
            {
                MessageBox.Show("Ingrese un color", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (nudCantidad.Value == 0)
            {
                MessageBox.Show("Ingrese la cantidad", "Status", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int cantidad = (int)nudCantidad.Value;
            if (discoEditar == null)
            {
                // Modo AGREGAR: revisa duplicado y guarda nuevo
                bool existe = inventario.ObtenerZirconia().Any(d =>
                    d.Nombre == cmbNombre.Text && d.Tipo == cmbTipo.Text &&
                    d.Tamaño == tamaño && d.Color == cmbColor.Text);


                MessageBox.Show("Producto agregado exitosamente", "Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (existe)
                {
                    MessageBox.Show("Ya existe un producto igual.", "Status",
                        MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                DiscoZirconia z = new DiscoZirconia(0, cmbNombre.Text, cmbTipo.Text,
                    tamaño, cmbColor.Text, cantidad, 3);
              
                inventario.AgregarZirconia(z);

                string nombreA = z.Nombre + " " + z.Tipo + " " + z.Tamaño + "mm " + z.Color;

                inventario.RegistrarHistorial(nombreA, "Agregar", 0, z.Cantidad);

                MessageBox.Show("Producto agregado exitosamente", "Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Modo EDITAR: actualiza el disco seleccionado
                int cantidadAntes = discoEditar.Cantidad;

                discoEditar.Nombre = cmbNombre.Text;
                discoEditar.Tipo = cmbTipo.Text;
                discoEditar.Tamaño = tamaño;
                discoEditar.Color = cmbColor.Text;
                discoEditar.Cantidad = cantidad;
                inventario.EditarZirconia(discoEditar);

                string nombreE = discoEditar.Nombre + " " + discoEditar.Tipo + " " + discoEditar.Tamaño + "mm " + discoEditar.Color;
                inventario.RegistrarHistorial(nombreE, "Editar", cantidadAntes, discoEditar.Cantidad);

                MessageBox.Show("Producto editado exitosamente", "Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {

            DialogResult respuesta = MessageBox.Show(
           "¿Estás seguro que quieres cancelar?",
           "Confirmar",
           MessageBoxButtons.YesNo,
           MessageBoxIcon.Question);

            if (respuesta == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (discoEditar == null) return;

            DialogResult r = MessageBox.Show(
                "¿Seguro que quieres eliminar este producto?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (r == DialogResult.Yes)
            {
                string nombreX = discoEditar.Nombre + " " + discoEditar.Tipo + " " + discoEditar.Tamaño + "mm " + discoEditar.Color;
                inventario.RegistrarHistorial(nombreX, "Eliminar", discoEditar.Cantidad, 0);

                inventario.EliminarZirconia(discoEditar.Id);
                MessageBox.Show("Producto eliminado", "Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }
    }
}
