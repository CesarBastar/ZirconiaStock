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
            dgvZirconia.DataSource = inventario.ObtenerZirconia();
        }

        private void dgvZirconia_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
