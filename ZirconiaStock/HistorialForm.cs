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
    public partial class HistorialForm : Form
    {
        private Inventario inventario;

        public HistorialForm(Inventario inv)
        {
            InitializeComponent();
            inventario = inv;
            dgvHistorial.DataSource = inventario.ObtenerHistorial();
        }
    }
}
