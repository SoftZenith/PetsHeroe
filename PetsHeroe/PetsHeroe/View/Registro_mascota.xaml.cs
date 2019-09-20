using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Registro_mascota : ContentPage
    {
        int idTipoMascota = -1;
        int idRazaMascota = -1;
        int idColorMascota = -1;
        char sexoMascotaC = (char)78;
        //Diccionarios para guardar nombre - id
        Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();

        public Registro_mascota()
        {
            InitializeComponent();

            pkrRazaMascota.IsEnabled = false;
            pkrColorMascota.IsEnabled = false;
            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows)
            {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrTipoMascota.SelectedIndexChanged += tipoMascotaSeleccionado;

            pkrSexoMascota.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                int sexoSelected = pkrSexoMascota.SelectedIndex;
                if (sexoSelected == 0)
                {
                    sexoMascotaC = (char)77;
                }
                else if (sexoSelected == 1)
                {
                    sexoMascotaC = (char)72;
                }
            };

        }//Constructor

        public void tipoMascotaSeleccionado(object sender, EventArgs args)
        {
            idTipoMascota = tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()];
            pkrRazaMascota.IsEnabled = true;
            pkrColorMascota.IsEnabled = false;
            DataTable razaMascota = new DataTable();
            DataTable colorMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaRaza_Busca(idTipoMascota);
            DependencyService.Get<IWebService>().getMascotaColor_Busca(idTipoMascota);
            colorMascota = DependencyService.Get<IWebService>().MascotaColor_Busca;
            razaMascota = DependencyService.Get<IWebService>().MascotaRaza_Busca;

            pkrRazaMascota.Items.Clear();
            razaMascotaDic.Clear();
            foreach (DataRow dr in razaMascota.Rows)
            {
                pkrRazaMascota.Items.Add(dr[5].ToString());
                razaMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrColorMascota.Items.Clear();
            colorMascotaDic.Clear();
            foreach (DataRow dr in colorMascota.Rows)
            {
                pkrColorMascota.Items.Add(dr[5].ToString());
                colorMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrRazaMascota.SelectedIndexChanged += razaMascotaSeleccionado;
            pkrColorMascota.SelectedIndexChanged += colorMascotaSeleccionado;
        }

        public void razaMascotaSeleccionado(object sender, EventArgs args)
        {
            try
            {
                idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
                pkrColorMascota.IsEnabled = true;
            }
            catch (Exception)
            {

            }
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args)
        {
            try
            {
                if (idRazaMascota == -1)
                {
                    DisplayAlert("Advertencia", "Es necesario seleccionar la raza", "OK");
                }

                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception)
            {

            }
        }

        public void onRegistraMascota(object sender, EventArgs args) {
            bool estatus = false;

            DependencyService.Get<IWebService>().getMascota_Registro(new Model.Dueno()
            {
                idDueno = txtCodigo.Text,
                nombre = "-1", //Preferences.Get("name","")
                apellidoP = "-1", //Preferences.Get("lastNameP","")
                apellidoM = "-1", //Preferences.Get("lastNameM","")
                sexo = -1, //Preferences.Get("sex",'M')
                correo = "-1", //Preferences.Get("email",456)
                contrasena = "-1", //Preferences.Get("password", 456)
                mascostaCodigo = txtCodigo.Text,
                sexoMascota = sexoMascotaC,
                idTipoMascota = idTipoMascota,
                idRazaMascota = idRazaMascota,
                idColorMascota = idColorMascota,
                edadMascota = Convert.ToInt32(txtedadMascota.Text)
            }); 
            estatus = DependencyService.Get<IWebService>().Mascota_Registro;

            if (estatus)
            {
                DisplayAlert("Registro", "Se registro correctamente", "OK");
            }
            else {
                DisplayAlert("Error","Tipo: "+ idTipoMascota+" ,raza: "+ idRazaMascota+",color: "+ idColorMascota, "OK");
            }

        }

    }
}
