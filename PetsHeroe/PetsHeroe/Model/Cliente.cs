using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace PetsHeroe.Model
{
    public class Cliente : Dueno
    {
        public int idMiembro { get; set; }
        public string codigoMiembro { get; set; }
        public string telefono { get; set; }
        public int mascotas { get; set; }
        public List<MascotaCliente> mascotasList { get; set; }

        public List<Cliente> getListaClientes(int idAsociado) {

            DataTable clientesTbl = new DataTable();
            List<Cliente> clientes = new List<Cliente>();

            bool status = DependencyService.Get<IWebService>().getCliente_Busca(-1,idAsociado);

            if (status)
            {
                clientesTbl = DependencyService.Get<IWebService>().Cliente_Busca;
            }

            foreach (DataRow dr in clientesTbl.Rows)
            {
                var clientTmp = new Cliente()
                {
                    idMiembro = Convert.ToInt32(dr["IDMember"].ToString()),
                    idDueno = dr["IDMember"].ToString(),
                    codigoMiembro = dr["MemberCode"].ToString(),
                    nombre = dr["FullName"].ToString(),
                    correo = dr["EMail"].ToString(),
                    telefono = dr["PhoneCel"].ToString()
                };

                Mascota mascotasExistenCom = new Mascota();
                clientTmp.mascotasList = mascotasExistenCom.getMascotasByClient(clientTmp.idMiembro, idAsociado);

                clientes.Add(clientTmp);
            }

            return clientes;

        }

    }
}
