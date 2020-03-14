using System;
namespace PetsHeroe.Model
{
    public class Promocion
    {
        public int idPromocion { get; set; }
        public string nombre { get; set; }
        public string marca { get; set; }
        public string partner { get; set; }
        public string mascota { get; set; }
        public string descripcion { get; set; }
        public string tipo { get; set; }
        public double precio { get; set; }
        public double PartnerPrice { get; set; }
        public double puntos { get; set; }
        public string  producto { get; set; }
        public int compra { get; set; }
        public int gratis { get; set; }
        public string inicia { get; set; }
        public string vigencia { get; set; }
        public bool esDineroElectr { get; set; }
        public string UPC { get; set; }
        public bool isProduct { get; set; }
        public bool isService { get; set; }

        public Promocion()
        {
        }
    }
}
