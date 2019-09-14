using System;
using System.Collections.Generic;
using System.Data;
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

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IIOS>().MascotaTipo_Busca;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IAndroid>().MascotaTipo_Busca;
            }

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

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IIOS>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IIOS>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IIOS>().MascotaRaza_Busca;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IAndroid>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IAndroid>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IAndroid>().MascotaRaza_Busca;
            }

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
            if (Device.RuntimePlatform == Device.Android) {
                DependencyService.Get<IAndroid>().getMascota_Registro(new Model.Dueno()
                {
                    idDueno = "-1",
                    nombre = "-1",
                    apellidoP = "-1",
                    apellidoM = "-1",
                    sexo = -1,
                    correo = "-1",
                    contrasena = "-1",
                    mascostaCodigo = txtCodigo.Text,
                    sexoMascota = sexoMascotaC,
                    idTipoMascota = idTipoMascota,
                    idRazaMascota = idRazaMascota,
                    idColorMascota = idColorMascota,
                    edadMascota = Convert.ToInt32(txtedadMascota.Text)
                });
                estatus = DependencyService.Get<IAndroid>().Mascota_Registro;
            } else if (Device.RuntimePlatform == Device.iOS) {
                DependencyService.Get<IIOS>().getMascota_Registro(new Model.Dueno()
                {
                    idDueno = "-1",
                    nombre = "-1",
                    apellidoP = "-1",
                    apellidoM = "-1",
                    sexo = -1,
                    correo = "-1",
                    contrasena = "-1",
                    mascostaCodigo = txtCodigo.Text,
                    sexoMascota = sexoMascotaC,
                    idTipoMascota = idTipoMascota,
                    idRazaMascota = idRazaMascota,
                    idColorMascota = idColorMascota,
                    edadMascota = Convert.ToInt32(txtedadMascota.Text)
                });
                estatus = DependencyService.Get<IIOS>().Mascota_Registro;
            }
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
