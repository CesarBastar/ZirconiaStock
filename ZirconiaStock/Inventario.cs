using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using SQLiteUtil;

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
    }

}


