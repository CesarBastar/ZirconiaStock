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
            rs.Close();
            return discoZirconias;
        }

        public List<DiscoZirconia> BuscarZirconia(string texto)
        {
            List<DiscoZirconia> lista = new List<DiscoZirconia>();
            string query = "SELECT id, nombre, tipo, tamaño, color, cantidad, stock_minimo FROM zirconia " +
                           "WHERE nombre LIKE $t OR tipo LIKE $t OR color LIKE $t OR CAST(tamaño AS TEXT) LIKE $t";
            var rs = conn.ExecuteReader(query, ("$t", "%" + texto + "%"));
            while (rs.Read())
            {
                lista.Add(new DiscoZirconia(
                    rs.GetInt("id"), rs.GetString("nombre"), rs.GetString("tipo"),
                    rs.GetInt("tamaño"), rs.GetString("color"),
                    rs.GetInt("cantidad"), rs.GetInt("stock_minimo")));
            }
            return lista;
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

        public void EliminarZirconia(int id)
        {
            conn.ExecuteNonQuery("DELETE FROM zirconia WHERE id=$id", ("$id", id));
        }

        public void RegistrarHistorial(string producto, string accion, int antes, int despues)
        {
            string query = "INSERT INTO historial (producto, accion, cantidad_anterior, cantidad_nueva, fecha) VALUES ($producto, $accion, $antes, $despues, $fecha)";

            conn.ExecuteNonQuery(query,
                ("$producto", producto),
                ("$accion", accion),
                ("$antes", antes),
                ("$despues", despues),
                ("$fecha", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
        }
        public List<Historial> ObtenerHistorial()
        {
            List<Historial> lista = new List<Historial>();
            string query = "SELECT id, producto, accion, cantidad_anterior, cantidad_nueva, fecha FROM historial ORDER BY id DESC";
            var rs = conn.ExecuteReader(query);
            while (rs.Read())
            {
                lista.Add(new Historial(
                    rs.GetInt("id"),
                    rs.GetString("producto"),
                    rs.GetString("accion"),
                    rs.GetInt("cantidad_anterior"),
                    rs.GetInt("cantidad_nueva"),
                    rs.GetString("fecha")));
            }
            return lista;
        }
        public string ObtenerMasUsado()
        {
            string query = "SELECT producto, COUNT(*) AS veces FROM historial WHERE accion = 'Disminuir' GROUP BY producto ORDER BY veces DESC LIMIT 1";
                          
            var rs = conn.ExecuteReader(query);
            if (rs.Read())
                return rs.GetString("producto") + " (" + rs.GetInt("veces") + " veces)";
            return "Sin datos aún";
        }

        // Metodos implementados por el UNION ALL de la tabla de resinas

        public List<ProductoVista> ObtenerTodo()
        {
            List<ProductoVista> lista = new List<ProductoVista>();

            string query =
                "SELECT id, 'Zirconia' AS categoria, nombre, tipo, tamaño, color, cantidad, stock_minimo FROM zirconia  UNION ALL SELECT id, 'Resina', nombre, 'Resina', 0, '-', cantidad, stock_minimo FROM resinas"; 
             
               

            var rs = conn.ExecuteReader(query);
            while (rs.Read())
            {
                lista.Add(new ProductoVista
                {
                    Id = rs.GetInt("id"),
                    Categoria = rs.GetString("categoria"),
                    Nombre = rs.GetString("nombre"),
                    Tipo = rs.GetString("tipo"),
                    Tamaño = rs.GetInt("tamaño"),
                    Color = rs.GetString("color"),
                    Cantidad = rs.GetInt("cantidad"),
                    StockMinimo = rs.GetInt("stock_minimo")
                });
            }
            return lista;
        }

        public void ActualizarCantidadZirconia(int id, int cantidad)
        {
            conn.ExecuteNonQuery("UPDATE zirconia SET cantidad=$c WHERE id=$id", ("$c", cantidad), ("$id", id));
        }

        public void ActualizarCantidadResina(int id, int cantidad)
        {
            conn.ExecuteNonQuery("UPDATE resinas SET cantidad=$c WHERE id=$id", ("$c", cantidad), ("$id", id));
        }
        public List<Resina> ObtenerResinas()
        {
            List<Resina> lista = new List<Resina>();
            string query = "SELECT id, nombre, cantidad, stock_minimo FROM resinas";
            var rs = conn.ExecuteReader(query);
            while (rs.Read())
            {
                lista.Add(new Resina(
                    rs.GetInt("id"),
                    rs.GetString("nombre"),
                    rs.GetInt("cantidad"),
                    rs.GetInt("stock_minimo")));
            }

            rs.Close();
            return lista;
        }

        public void AgregarResina(Resina r)
        {
            string query = "INSERT INTO resinas (nombre, cantidad, stock_minimo) VALUES ($nombre, $cantidad, $stock_minimo)";
            conn.ExecuteNonQuery(query,
                ("$nombre", r.Nombre), ("$cantidad", r.Cantidad), ("$stock_minimo", r.StockMinimo));
        }

        public void EditarResina(Resina r)
        {
            string query = "UPDATE resinas SET nombre=$nombre, cantidad=$cantidad, stock_minimo=$stock_minimo WHERE id=$id";
            conn.ExecuteNonQuery(query,
                ("$nombre", r.Nombre), ("$cantidad", r.Cantidad),
                ("$stock_minimo", r.StockMinimo), ("$id", r.Id));
        }

        public void EliminarResina(int id)
        {
            conn.ExecuteNonQuery("DELETE FROM resinas WHERE id=$id", ("$id", id));
        }
    }

}


