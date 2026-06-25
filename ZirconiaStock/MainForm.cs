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
            RefrescarTabla();
        }
        private void RefrescarTabla()
        {
            dgvZirconia.DataSource = null;
            dgvZirconia.DataSource = inventario.ObtenerZirconia();
            dgvZirconia.Columns["StockMinimo"].Visible = false;
            dgvZirconia.Columns["Id"].DisplayIndex = 0;
            dgvZirconia.Columns["Nombre"].DisplayIndex = 1;
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
    }
}
