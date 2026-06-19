using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class Resina : Producto
    {
        public Resina() { }

        public Resina(int id, string nombre, int cantidad, int stockMinimo)
        {
            Id = id;
            Nombre = nombre;
            Cantidad = cantidad;
            StockMinimo = stockMinimo;
        }
    }

}
