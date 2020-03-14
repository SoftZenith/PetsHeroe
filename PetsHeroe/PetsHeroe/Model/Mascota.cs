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
        public int idAsociado { get; set; }
        public int idSucursal { get; set; }
        public int idPais { get; set; }
        public int idEstado { get; set; }
        public int idCiudad { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string estatus { get; set; }
        public string suscripcion { get; set; }
        public string veterinario { get; set; }
        public string sexo { get; set; }
        public string tipo { get; set; }
        public string raza { get; set; }
        public string color { get; set; }
        public string alta { get; set; }
        public string expira { get; set; }
        public bool perdida { get; set; } //EXT
        public bool robada { get; set; } //ROB
        public string botonIzq { get; set; }
        public string botonDer { get; set; }
        public int edad { get; set; }
        //REG en casa con dueño

        int years(DateTime start, DateTime end)
        {
            return (end.Year - start.Year - 1) +
                (((end.Month > start.Month) ||
                ((end.Month == start.Month) && (end.Day >= start.Day))) ? 1 : 0);
        }

        public List<Mascota> getMascotaList(int idMiembro) {

            DataTable mascotasTbl = new DataTable();

            List<Mascota> mascotas = new List<Mascota>();

            bool status = DependencyService.Get<IWebService>().getMascota_Busca(idMiembro,-1);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

                foreach (DataRow dr in mascotasTbl.Rows)
                {

                    int year = Convert.ToInt32(dr["YearBitrh"].ToString());
                    int month = Convert.ToInt32(dr["MonthBirth"].ToString());

                    int año = DateTime.Now.Year;
                    int mes = DateTime.Now.Month;

                    DateTime actual = new DateTime(año, mes, 1);
                    DateTime nacimiento = new DateTime(year, month, 1);



                    var mascotaTmp = new Mascota()
                    {
                        idMascota = Convert.ToInt32(dr["IDPet"]),
                        idPais = Convert.ToInt32(dr["IDCountryLoc"]),
                        idEstado = Convert.ToInt32(dr["IDStateLoc"]),
                        idCiudad = Convert.ToInt32(dr["IDCityLoc"]),
                        idSucursal = Convert.ToInt32(dr["IDPartnerLocation"]),
                        nombre = dr["Name"].ToString(),
                        codigo = dr["Code"].ToString(),
                        tipo = dr["PetType"].ToString(),
                        raza = dr["Breed"].ToString(),
                        color = dr["Color"].ToString(),
                        edad = years(nacimiento,actual), //No es el campo correcto
                        sexo = dr["Sex"].ToString(),
                        suscripcion = dr["SubscriptionType"].ToString(),
                        idAsociado = Convert.ToInt32(dr["IDPartner"].ToString()),
                        veterinario = dr["BusinessName"].ToString(),
                        alta = dr["DateActivated"].ToString(),
                        expira = dr["DateExpiration"].ToString(),
                        botonIzq = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como perdida" : "Cancelar reporte",
                        botonDer = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como robada" : "Reportar como encontrada"
                    };

                    Mascota incidenteMascota = getMascotaListPerdidas(mascotaTmp.idMascota, idMiembro);
                    if (incidenteMascota == null)
                    {
                        mascotaTmp.perdida = false;
                        mascotaTmp.robada = false;
                        mascotaTmp.estatus = "En casa con dueño";
                    } else {

                        mascotaTmp.perdida = incidenteMascota.perdida;
                        mascotaTmp.robada = incidenteMascota.robada;

                        if (mascotaTmp.perdida) mascotaTmp.estatus = "Reportada como extraviada";
                        if (mascotaTmp.robada) mascotaTmp.estatus = "Reportada como robada";
                    }

                    mascotas.Add(mascotaTmp);
                }

            }

            return mascotas;
        }

        public Mascota getMascotaListPerdidas(int idMascota, int idMiembro)
        {

            DataTable mascotasTbl = new DataTable();

            List<Mascota> mascotas = new List<Mascota>();
            Mascota mascotaTmp = null;

            mascotasTbl = DependencyService.Get<IWebService>().Mascota_IncidenteBusca(idMascota, idMiembro, -1, -1, -1, -1, -1, 1);

            foreach (DataRow dr in mascotasTbl.Rows)
            {

                mascotaTmp = new Mascota()
                {
                    idMascota = Convert.ToInt32(dr["IDPet"]),
                    nombre = dr["PetName"].ToString(),
                    //codigo = dr["Code"].ToString(),
                    //estatus = dr["PetStatus"].ToString(),
                    //suscripcion = dr["SubscriptionType"].ToString(),
                    //idAsociado = Convert.ToInt32(dr["IDPartner"].ToString()),
                    //veterinario = dr["BusinessName"].ToString(),
                    //alta = dr["DateActivated"].ToString(),
                    //expira = dr["DateExpiration"].ToString(),
                    perdida = (dr["IncidentTypeCode"].ToString() == "REXT"),
                    robada = (dr["IncidentTypeCode"].ToString() == "RROB"),
                    //botonIzq = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como perdida" : "Cancelar reporte",
                    //botonDer = (dr["PetStatusCode"].ToString() == "REG") ? "Reportar como robada" : "Reportar como encontrada"
                };

                //mascotas.Add(mascotaTmp);
            }


            return mascotaTmp;
        }

        public List<MascotaCliente> getMascotasByClient(int idMiembro, int idAsociado) {
            DataTable mascotasTbl = new DataTable();

            List<MascotaCliente> mascotas = new List<MascotaCliente>();

            bool status = DependencyService.Get<IWebService>().getMascota_Busca(idMiembro, idAsociado);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

                foreach (DataRow dr in mascotasTbl.Rows)
                {

                    MascotaCliente mascotaTmp = new MascotaCliente()
                    {
                        idMascota = Convert.ToInt32(dr["IDPet"]),
                        nombre = dr["Name"].ToString(),
                        codigo = dr["Code"].ToString(),
                        estatus = dr["PetStatus"].ToString(),
                    };

                    mascotas.Add(mascotaTmp);
                }
            }
            return mascotas;
        }

    }

    public class MascotaCliente {
        public int idMascota { get; set; }
        public string nombre { get; set; }
        public string codigo { get; set; }
        public string estatus { get; set; }
    }
}