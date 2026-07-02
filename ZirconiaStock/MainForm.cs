using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ClosedXML.Excel;


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

            string opcion = cmbFiltro.SelectedItem.ToString();
            if (opcion == "Stock bajo")
            {
                lista = lista.Where(z => z.StockBajo).ToList();
            }
            else if (opcion != "Todos los tipos")
            {
                lista = lista.Where(z => z.Tipo == opcion).ToList();
            }

            dgvZirconia.DataSource = null;
            dgvZirconia.DataSource = lista;
            dgvZirconia.Columns["StockMinimo"].Visible = false;
            dgvZirconia.Columns["Id"].DisplayIndex = 0;
            dgvZirconia.Columns["Nombre"].DisplayIndex = 1;
            lblMasUsado.Text = "Producto más usado: " + inventario.ObtenerMasUsado();
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

        /*
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
        */
        private void btnAumentar_Click(object sender, EventArgs e)
        {
            DiscoZirconia z = DiscoSeleccionado();
            if (z == null) { MessageBox.Show("Selecciona un disco de la tabla."); return; }

            int id = z.Id;
            int antes = z.Cantidad;
            z.Cantidad = z.Cantidad + 1;
            inventario.EditarZirconia(z);

            string nombre = z.Nombre + " " + z.Tipo + " " + z.Tamaño + "mm " + z.Color;
            inventario.RegistrarHistorial(nombre, "Aumentar", antes, z.Cantidad);

            RefrescarTabla();
            SeleccionarPorId(id);
        }

        private void btnDisminuir_Click(object sender, EventArgs e)
        {
            DiscoZirconia z = DiscoSeleccionado();
            if (z == null) { MessageBox.Show("Selecciona un disco de la tabla."); return; }

            int id = z.Id;
            int antes = z.Cantidad;                        //  guarda la cantidad actual
            if (z.Cantidad > 0) z.Cantidad = z.Cantidad - 1;  //  resta una sola vez
            inventario.EditarZirconia(z);                  //guarda en la base

            string nombre = z.Nombre + " " + z.Tipo + " " + z.Tamaño + "mm " + z.Color;
            inventario.RegistrarHistorial(nombre, "Disminuir", antes, z.Cantidad);  // 4) registra

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
            cmbFiltro.Items.Add("Stock bajo");
            foreach (string t in inventario.ObtenerZirconia().Select(z => z.Tipo).Distinct())
                cmbFiltro.Items.Add(t);
            cmbFiltro.SelectedIndex = 0;   // empieza mostrando todos
        }

        private void cmbFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefrescarTabla();
        }

        private void btnEditarProducto_Click(object sender, EventArgs e)
        {

            DiscoZirconia z = DiscoSeleccionado();
            if (z == null) { MessageBox.Show("Selecciona un disco de la tabla."); return; }

            AgregarProducto f = new AgregarProducto(inventario, z);   // abre en modo editar
            f.ShowDialog();
            RefrescarTabla();

        }

        private void btnHistorial_Click(object sender, EventArgs e)
        {
            HistorialForm f = new HistorialForm(inventario);
            f.ShowDialog();
        }


        /*
        private void btnExportar_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Archivo CSV (*.csv)|*.csv";
            guardar.FileName = "zirconia.csv";

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(guardar.FileName, false, System.Text.Encoding.UTF8))
                {
                    // Encabezados (solo columnas visibles)
                    List<string> encabezados = new List<string>();
                    foreach (DataGridViewColumn col in dgvZirconia.Columns)
                        if (col.Visible) encabezados.Add(col.HeaderText);
                    sw.WriteLine(string.Join(",", encabezados));

                    // Filas
                    foreach (DataGridViewRow fila in dgvZirconia.Rows)
                    {
                        List<string> valores = new List<string>();
                        foreach (DataGridViewColumn col in dgvZirconia.Columns)
                            if (col.Visible)
                            {
                                object valor = fila.Cells[col.Index].Value;
                                valores.Add(valor == null ? "" : valor.ToString());
                            }
                        sw.WriteLine(string.Join(",", valores));
                    }
                }

                MessageBox.Show("Tabla exportada correctamente.", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        */

        private void btnExportar_Click(object sender, EventArgs e)
        {
            SaveFileDialog guardar = new SaveFileDialog();
            guardar.Filter = "Excel (*.xlsx)|*.xlsx";
            guardar.FileName = "zirconia.xlsx";

            if (guardar.ShowDialog() == DialogResult.OK)
            {
                using (XLWorkbook libro = new XLWorkbook())
                {
                    var hoja = libro.Worksheets.Add("Zirconia");

                    // Encabezados (fila 1)
                    hoja.Cell(1, 1).Value = "Id";
                    hoja.Cell(1, 2).Value = "Nombre";
                    hoja.Cell(1, 3).Value = "Tipo";
                    hoja.Cell(1, 4).Value = "Tamaño";
                    hoja.Cell(1, 5).Value = "Color";
                    hoja.Cell(1, 6).Value = "Cantidad";

                    // Formato de los encabezados: negrita, fondo azul, letra blanca
                    var encabezado = hoja.Row(1);
                    encabezado.Style.Font.Bold = true;
                    encabezado.Style.Fill.BackgroundColor = XLColor.FromArgb(23, 42, 69);
                    encabezado.Style.Font.FontColor = XLColor.White;

                    // Datos (desde la fila 2)
                    int fila = 2;
                  //  foreach (DiscoZirconia z in inventario.ObtenerZirconia().OrderBy(z => z.Cantidad))
                    
                    foreach (DataGridViewRow filaGrid in dgvZirconia.Rows)
                    {
                        DiscoZirconia z = filaGrid.DataBoundItem as DiscoZirconia;
                        if (z == null) continue;

                            hoja.Cell(fila, 1).Value = z.Id;
                        hoja.Cell(fila, 2).Value = z.Nombre;
                        hoja.Cell(fila, 3).Value = z.Tipo;
                        hoja.Cell(fila, 4).Value = z.Tamaño;
                        hoja.Cell(fila, 5).Value = z.Color;
                        hoja.Cell(fila, 6).Value = z.Cantidad;

                        // Si tiene stock bajo, la cantidad en rojo
                        if (z.StockBajo)
                            hoja.Cell(fila, 6).Style.Font.FontColor = XLColor.Red;

                        fila++;
                    }

                    hoja.Columns().AdjustToContents();   // ajusta el ancho de las columnas
                    libro.SaveAs(guardar.FileName);
                }

                MessageBox.Show("Exportado a Excel correctamente.", "Exportar",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
    }

