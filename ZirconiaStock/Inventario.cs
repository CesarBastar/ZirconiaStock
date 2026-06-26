using Microsoft.Data.Sqlite;
using SQLiteUtil;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ZirconiaStock
{
    public class Inventario
    {
        private SqliteConnection conn;
        public Inventario()
        {
            conn = new SqliteConnection("Data Source=odontec.db");
            conn.Open();
        }

        public List<DiscoZirconia> ObtenerZirconia()
        {
            List<DiscoZirconia> discoZirconias = new List<DiscoZirconia>();

            string query = "SELECT id, nombre, tipo, tamaño, color, cantidad, stock_minimo FROM zirconia";
            var rs = conn.ExecuteReader(query);
            while (rs.Read())
            {
                discoZirconias.Add(new DiscoZirconia(
                    rs.GetInt("id"),
                    rs.GetString("nombre"),
                    rs.GetString("tipo"),
                    rs.GetInt("tamaño"),
                    rs.GetString("color"),
                    rs.GetInt("cantidad"),
                    rs.GetInt("stock_minimo")

                    ));
            }
            return discoZirconias;
        }
         
        public void AgregarZirconia(DiscoZirconia z)
        {
            string query = "INSERT INTO zirconia (nombre, tipo, tamaño, color, cantidad, stock_minimo) VALUES ($nombre, $tipo, $tamaño, $color, $cantidad, $stock_minimo)";
            conn.ExecuteNonQuery(query,
                ("$nombre", z.Nombre),
                ("$tipo", z.Tipo),
                ("$tamaño", z.Tamaño),
                ("$color", z.Color),
                ("$cantidad", z.Cantidad),
                ("$stock_minimo", z.StockMinimo)
                );
        }


        public void EditarZirconia(DiscoZirconia z)
        {
            string query = "UPDATE zirconia SET nombre=$nombre, tipo=$tipo, tamaño=$tamaño, " +
                           "color=$color, cantidad=$cantidad, stock_minimo=$stock_minimo WHERE id=$id";
            conn.ExecuteNonQuery(query,
                ("$nombre", z.Nombre), ("$tipo", z.Tipo), ("$tamaño", z.Tamaño),
                ("$color", z.Color), ("$cantidad", z.Cantidad),
                ("$stock_minimo", z.StockMinimo), ("$id", z.Id));
        }
    }

}


