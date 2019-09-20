using System;
namespace PetsHeroe.Model
{
    public class Promocion
    {
        public int idPromocion { get; set; }
        public string partner { get; set; }
        public string mascota { get; set; }
        public string descripcion { get; set; }
        public int puntos { get; set; }
        public int compra { get; set; }
        public int gratis { get; set; }
        public string vigencia { get; set; }
        public bool esDineroElectr { get; set; }


        public Promocion()
        {
        }
    }
}
