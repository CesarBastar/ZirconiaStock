using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class DiscoZirconia
    {
        public int Id { get; set; }
        public string Color { get; set; }

        public int Grosor { get; set; }
        public string Tipo { get; set; }

        public int Cantidad { get; set; }

        public DiscoZirconia(int id, string color, int grosor, string tipo, int cantidad)
        {
            Id = id;

            Color = color;

            Grosor = grosor;

            Tipo = tipo;

            Cantidad = cantidad;
        }

    }

}
