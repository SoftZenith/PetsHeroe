using System;
namespace PetsHeroe.Model
{
    public class MensajeDueno
    {
        public string codigo { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string correo { get; set; }
        public string mensaje { get; set; }
        public string localizacion { get; set; }
        public string notas { get; set; }
        public double latitud { get; set; }
        public double longitud { get; set; }
        public int idCiudad { get; set; }
        public int idSucursal { get; set; }
    }
}