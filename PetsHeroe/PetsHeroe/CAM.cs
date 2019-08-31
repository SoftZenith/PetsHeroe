using System;
using System.Collections.Generic;
using System.Data;
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


        public List<CAM> getCAMS() {

            List<CAM> listaCAMs = new List<CAM>();
            DataTable lista_CAM = new DataTable();

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getCAM_busca(25.708742, -100.344950, 100.0);
                lista_CAM = DependencyService.Get<IAndroid>().CAM_Busca;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getCAM_busca(25.708742, -100.344950, 100.0);
                lista_CAM = DependencyService.Get<IIOS>().CAM_Busca;
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
