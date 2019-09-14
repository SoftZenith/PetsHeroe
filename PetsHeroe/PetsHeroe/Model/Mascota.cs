using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace PetsHeroe
{
    public class Mascota
    {
        public int idMascota { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string estatus { get; set; }
        public string suscripcion { get; set; }
        public string veterinario { get; set; }
        public string alta { get; set; }
        public string expira { get; set; }
        public bool perdida { get; set; }
        public bool robada { get; set; }

        public List<Mascota> getMascotaList(int idMiembro) {

            DataTable mascotasTbl = new DataTable();

            List<Mascota> mascotas = new List<Mascota>();

            if (Device.RuntimePlatform == Device.Android)
            {
                bool status = DependencyService.Get<IAndroid>().getMascota_Busca(idMiembro);
                if (status)
                {
                    mascotasTbl = DependencyService.Get<IAndroid>().Mascota_Busca;

                    foreach (DataRow dr in mascotasTbl.Rows)
                    {

                        var mascotaTmp = new Mascota()
                        {
                            idMascota = Convert.ToInt32(dr["IDPet"]),
                            nombre = dr["Name"].ToString(),
                            codigo = dr["Code"].ToString(),
                            estatus = dr["PetStatus"].ToString(),
                            suscripcion = dr["SubscriptionType"].ToString(),
                            veterinario = "",
                            alta = dr["DateActivated"].ToString(),
                            expira = dr["DateExpiration"].ToString(),
                            perdida = false,
                            robada = false
                        };

                        mascotas.Add(mascotaTmp);

                    }

                }
            } else if (Device.RuntimePlatform == Device.iOS) {
                bool status = DependencyService.Get<IIOS>().getMascota_Busca(idMiembro);
                if (status)
                {
                    mascotasTbl = DependencyService.Get<IIOS>().Mascota_Busca;

                    foreach (DataRow dr in mascotasTbl.Rows)
                    {

                        var mascotaTmp = new Mascota()
                        {
                            idMascota = Convert.ToInt32(dr["IDPet"]),
                            nombre = dr["Name"].ToString(),
                            codigo = dr["Code"].ToString(),
                            estatus = dr["PetStatus"].ToString(),
                            suscripcion = dr["SubscriptionType"].ToString(),
                            veterinario = "",
                            alta = dr["DateActivated"].ToString(),
                            expira = dr["DateExpiration"].ToString(),
                            perdida = false,
                            robada = false
                        };

                        mascotas.Add(mascotaTmp);

                    }

                }
            }

            return mascotas;
        }

    }
}