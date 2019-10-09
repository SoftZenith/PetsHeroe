using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
using Xamarin.Forms;

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

        public List<CAM> getCAMS(Double latitud, Double longitud) {

            List<CAM> listaCAMs = new List<CAM>();
            DataTable lista_CAM = new DataTable();

            try
            {
                DependencyService.Get<IWebService>().getCAM_busca(latitud, longitud, 50);
                lista_CAM = DependencyService.Get<IWebService>().CAM_Busca;
            }catch (Exception ex)
            {
                Console.WriteLine("Error al obtener CAMs: "+ex);
            }

            foreach (DataRow dr in lista_CAM.Rows)
            {
                listaCAMs.Add(
                    new CAM()
                    {
                        nombre = dr["BusinessName"].ToString(),
                        sucursal = dr["Address1"].ToString(),
                        codigopostal = dr["ZipCode"].ToString(),
                        telefono = dr["Phone1"] + ", " + dr["Phone2"],
                        ciudad = dr["City"] + ", " + dr["State"] + ", " + dr["Country"]
                    });
            }

            return listaCAMs;

        }

    }
}
