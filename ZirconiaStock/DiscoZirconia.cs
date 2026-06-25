using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class DiscoZirconia : Producto
    {
        public string Tipo { get; set; }   
        public int Tamaño { get; set; }      
        public string Color { get; set; }    

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
