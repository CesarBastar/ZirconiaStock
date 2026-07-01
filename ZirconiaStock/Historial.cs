using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class Historial
    {
        public int Id { get; set; }
        public string Producto { get; set; }
        public string Accion { get; set; }
        public int CantidadAnterior { get; set; }
        public int CantidadNueva { get; set; }
        public string Fecha { get; set; }

        public Historial(int id, string producto, string accion, int antes, int despues, string fecha)
        {
            Id = id;
            Producto = producto;
            Accion = accion;
            CantidadAnterior = antes;
            CantidadNueva = despues;
            Fecha = fecha;
        }
    }
}
