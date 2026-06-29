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
    public partial class MainForm : Form
    {
        private Inventario inventario; 
        public MainForm()
        {

            InitializeComponent();

            inventario = new Inventario();
            //dgvZirconia.DataSource = inventario.ObtenerZirconia();
            //dgvZirconia.Columns["StockMinimo"].Visible = false;
            //dgvZirconia.Columns["Id"].DisplayIndex = 0;
            //dgvZirconia.Columns["Nombre"].DisplayIndex = 1;

            CargarFiltro();
            RefrescarTabla();
         
        }
        

        /*
        private void RefrescarTabla()
        {
            List<DiscoZirconia> lista = inventario.ObtenerZirconia().OrderByDescending(z => z.Cantidad).ToList();
                    
           
            dgvZirconia.DataSource = null;
            dgvZirconia.DataSource = lista;
            dgvZirconia.Columns["StockMinimo"].Visible = false;
            dgvZirconia.Columns["Id"].DisplayIndex = 0;
            dgvZirconia.Columns["Nombre"].DisplayIndex = 1;
        }
        */

        private void RefrescarTabla()
        {
            List<DiscoZirconia> lista = inventario.BuscarZirconia(txtBuscar.Text).OrderByDescending(z => z.Cantidad).ToList();

            if (cmbFiltro.SelectedIndex > 0)
            {
                string tipo = cmbFiltro.SelectedItem.ToString();
                lista = lista.Where(z => z.Tipo == tipo).ToList();
            }

            dgvZirconia.DataSource = null;
            dgvZirconia.DataSource = lista;
            dgvZirconia.Columns["StockMinimo"].Visible = false;
            dgvZirconia.Columns["Id"].DisplayIndex = 0;
            dgvZirconia.Columns["Nombre"].DisplayIndex = 1;
        }

        private DiscoZirconia DiscoSeleccionado()
        {
            if (dgvZirconia.CurrentRow == null) return null;
            return dgvZirconia.CurrentRow.DataBoundItem as DiscoZirconia;
        }
       
        
        /*
        private void SeleccionarPorId(int id)
        {                                                                                 //Me movia el Index == 0 
            foreach (DataGridViewRow fila in dgvZirconia.Rows)
            {
                DiscoZirconia z = fila.DataBoundItem as DiscoZirconia;
                if (z != null && z.Id == id) { fila.Selected = true; break; }
            }
        }
        */

        private void SeleccionarPorId(int id)
        {
            foreach (DataGridViewRow fila in dgvZirconia.Rows)
            {
                DiscoZirconia z = fila.DataBoundItem as DiscoZirconia;
                if (z != null && z.Id == id)
                {
                    dgvZirconia.ClearSelection();
                    dgvZirconia.CurrentCell = fila.Cells["Id"];  // mueve la celda actual a esa fila
                    fila.Selected = true;
                    break;
                }
            }
        }

        private void dgvZirconia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            AgregarProducto r = new AgregarProducto(inventario);
            r.ShowDialog();
            RefrescarTabla();

        }

        private void btnAumentar_Click(object sender, EventArgs e)
        {
            DiscoZirconia z = DiscoSeleccionado();
            if (z == null) { MessageBox.Show("Selecciona un disco de la tabla."); return; }

            int id = z.Id;
            z.Cantidad = z.Cantidad + 1;
            inventario.EditarZirconia(z);
            RefrescarTabla();
            SeleccionarPorId(id);
        }

        private void btnDisminuir_Click(object sender, EventArgs e)
        {
            DiscoZirconia z = DiscoSeleccionado();
            if (z == null) { MessageBox.Show("Selecciona un disco de la tabla."); return; }

            int id = z.Id;
            if (z.Cantidad > 0) z.Cantidad = z.Cantidad - 1;
            inventario.EditarZirconia(z);
            RefrescarTabla();
            SeleccionarPorId(id);
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            RefrescarTabla();
        }

        private void CargarFiltro()
        {
            cmbFiltro.Items.Clear();
            cmbFiltro.Items.Add("Todos los tipos");
            foreach (string t in inventario.ObtenerZirconia().Select(z => z.Tipo).Distinct())
                cmbFiltro.Items.Add(t);
            cmbFiltro.SelectedIndex = 0;   // empieza mostrando todos
        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarTabla();
        }
    }
}
