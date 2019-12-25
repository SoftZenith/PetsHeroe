using System;
namespace PetsHeroe.Model
{
    public class Venta
    {
        public int idItem { get; set; }
        public int idProducto { get; set; }
        public int idServicio { get; set; }
        public string nombre { get; set; }
        public int cantidad { get; set; }
        public double precio { get; set; }
        public double puntos { get; set; }
        public int tipo { get; set; } //1.- Producto, 2.- Servicio
        public string imagen { get; set; }
        public int actual { get; set; }
        public int total { get; set; }  
        public bool isService { get; set; }
        public bool isProduct { get; set; }
        public string textoMostrar { get; set; }
    }
}