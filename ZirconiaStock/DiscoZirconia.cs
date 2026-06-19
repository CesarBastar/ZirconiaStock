using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class DiscoZirconia : Producto
    {
        public string Tipo { get; set; }    // MONO-LAYER, 4D-PRO, STML
        public int Tamaño { get; set; }      // 14, 16, 18, 20, 22 
        public string Color { get; set; }    // HT, A1, A2, A3

        public DiscoZirconia() { }

        public DiscoZirconia(int id, string nombre, string tipo, int tamaño, string color, int cantidad, int stockMinimo)
        {
            Id = id;
            Nombre = nombre;
            Tipo = tipo;
            Tamaño = tamaño;
            Color = color;
            Cantidad = cantidad;
            StockMinimo = stockMinimo;
        }
    }
}
