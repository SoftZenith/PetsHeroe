using System;
using System.Collections.Generic;

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

        public List<Mascota> getMascotaList() {

            List<Mascota> mascotas = new List<Mascota>() {
                new Mascota(){
                    idMascota = 0,
                    nombre = "Firulais",
                    codigo = "098774839",
                    estatus = "Quo",
                    suscripcion = "Anual",
                    veterinario = "Dr. Carlos Perez",
                    alta = "20/06/2019",
                    expira = "20/06/2020",
                    perdida = false,
                    robada = false },
                new Mascota(){
                    idMascota = 1,
                    nombre = "El prieto",
                    codigo = "98667302",
                    estatus = "Quo",
                    suscripcion = "Anual",
                    veterinario = "Dra. Diana Laura",
                    alta = "02/01/2019",
                    expira = "02/01/2020",
                    perdida = false,
                    robada = false }
            };

            return mascotas;
        }

    }
}