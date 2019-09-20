using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
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
        public bool perdida { get; set; } //EXT
        public bool robada { get; set; } //
        public string botonIzq { get; set; }
        public string botonDer { get; set; }
        //REG en casa con dueño

        public List<Mascota> getMascotaList(int idMiembro) {

            DataTable mascotasTbl = new DataTable();

            List<Mascota> mascotas = new List<Mascota>();

            bool status = DependencyService.Get<IWebService>().getMascota_Busca(idMiembro);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

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
                        perdida = (dr["PetStatusCode"].ToString() != "REG"),
                        robada = (dr["PetStatusCode"].ToString() != "REG"),
                        botonIzq = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como perdida" : "Cancelar reporte",
                        botonDer = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como robada" : "Reportar como encontrada"
                    };

                    mascotas.Add(mascotaTmp);
                }

            }

            return mascotas;
        }

    }
}