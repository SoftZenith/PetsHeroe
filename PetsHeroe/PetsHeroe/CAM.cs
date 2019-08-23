using System;
using System.Collections.Generic;

namespace PetsHeroe
{
    public class CAM
    {
        public string nombre { get; set; }
        public string sucursal { get; set; }
        public string codigopostal { get; set; }
        public string telefono { get; set; }
        public string ciudad { get; set; }

        public CAM()
        {

        }


        public List<CAM> getCAMS() {

            List<CAM> listaCAMs = new List<CAM>();
            listaCAMs.Add(
                new CAM() {
                    nombre = "Acutus Medicina Veterinaria",
                    sucursal = "Rodrigo Reinoso Av.Gobernadores 404 - 4 Av.Gobernadores 404 - 4",
                    codigopostal = "64380",
                    telefono = "(81)83150001, (81)83150001",
                    ciudad = "Monterrey, Nuevo León. México"
                });
            listaCAMs.Add(
                new CAM()
                {
                    nombre = "AG Veterinaria",
                    sucursal = "Dirección principal",
                    codigopostal = "64385",
                    telefono = "(81)82140024, (81)8213045",
                    ciudad = "Monterrey, Nuevo León. México"
                });


            return listaCAMs;

        }

    }
}
