using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace PetsHeroe.Model
{
    public class Cliente : Dueno
    {
        public string codigoMiembro { get; set; }
        public string telefono { get; set; }
        public int mascotas { get; set; }

        public List<Cliente> getListaClientes(int idAsociado) {

            DataTable clientesTbl = new DataTable();
            List<Cliente> clientes = new List<Cliente>();

            if (Device.RuntimePlatform == Device.Android)
            {
                bool status = DependencyService.Get<IAndroid>().getCliente_Busca(idAsociado);

                if (status) {
                    clientesTbl = DependencyService.Get<IAndroid>().Cliente_Busca;
                }

                foreach (DataRow dr in clientesTbl.Rows) {
                    var clientTmp = new Cliente() {
                        idDueno = dr["IDMember"].ToString(),
                        codigoMiembro = dr["MemberCode"].ToString(),
                        nombre = dr["FullName"].ToString(),
                        correo = dr["EMail"].ToString(),
                        telefono = dr["PhoneCel"].ToString()
                    };

                    clientes.Add(clientTmp);
                }

            } else if (Device.RuntimePlatform == Device.iOS) {
                bool status = DependencyService.Get<IIOS>().getCliente_Busca(idAsociado);

                if (status)
                {
                    clientesTbl = DependencyService.Get<IIOS>().Cliente_Busca;
                }

                foreach (DataRow dr in clientesTbl.Rows)
                {
                    var clientTmp = new Cliente()
                    {
                        idDueno = dr["IDMember"].ToString(),
                        codigoMiembro = dr["MemberCode"].ToString(),
                        nombre = dr["FullName"].ToString(),
                        correo = dr["EMail"].ToString(),
                        telefono = dr["PhoneCel"].ToString()
                    };

                    clientes.Add(clientTmp);
                }
            }

            return clientes;

        }

    }
}
