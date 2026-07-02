using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class ProductoVista
    {
        public int Id { get; set; }
        public string Categoria { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int Tamaño { get; set; }
        public string Color { get; set; }
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
        public bool StockBajo => Cantidad < StockMinimo;
    }
}