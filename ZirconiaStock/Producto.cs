namespace ZirconiaStock
{
    public abstract class Producto                
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Cantidad { get; set; }
        public int StockMinimo { get; set; }
        public bool StockBajo
        
            =>  Cantidad < StockMinimo;
        
    }
}