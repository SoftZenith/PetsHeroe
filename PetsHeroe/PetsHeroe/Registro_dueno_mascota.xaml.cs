using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Registro_dueno_mascota : ContentPage
    {
        int idTipoMascota = -1;
        int idRazaMascota = -1;
        int idColorMascota = -1;
        char sexoDuenoC;
        char sexoMascotaC;
        //Diccionarios para guardar nombre - id
        Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>(); 
        Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();

        public Registro_dueno_mascota()
        {
            InitializeComponent();

            DataTable tipoMascota = new DataTable();

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IIOS>().MascotaTipo_Busca;
            } else if (Device.RuntimePlatform == Device.Android) {
                DependencyService.Get<IAndroid>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IAndroid>().MascotaTipo_Busca;
            }

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows) {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(),Convert.ToInt32(dr[0]));
            }

            pkrTipoMascota.SelectedIndexChanged += tipoMascotaSeleccionado;

            pkrSexoMascota.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                int sexoSelected = pkrSexoMascota.SelectedIndex;
                if (sexoSelected == 0)
                {
                    sexoMascotaC = (char)77;
                } else if (sexoSelected == 1) {
                    sexoMascotaC = (char)72;
                }
            };

            pkrSexoDueno.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                int sexoSelected = pkrSexoDueno.SelectedIndex;
                if (sexoSelected == 0) {
                    sexoDuenoC = (char)77;
                } else if (sexoSelected == 1) {
                    sexoDuenoC = (char)70;
                }
            };

        }//Constructor

        public void tipoMascotaSeleccionado(object sender, EventArgs args){
            idTipoMascota = tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()];
            DataTable razaMascota = new DataTable();
            DataTable colorMascota = new DataTable();

            if(Device.RuntimePlatform == Device.iOS){
                DependencyService.Get<IIOS>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IIOS>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IIOS>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IIOS>().MascotaRaza_Busca;
            }else if(Device.RuntimePlatform == Device.Android){
                DependencyService.Get<IAndroid>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IAndroid>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IAndroid>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IAndroid>().MascotaRaza_Busca;
            }

            pkrRazaMascota.Items.Clear();
            razaMascotaDic.Clear();
            foreach(DataRow dr in razaMascota.Rows){
                pkrRazaMascota.Items.Add(dr[5].ToString());
                razaMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrColorMascota.Items.Clear();
            colorMascotaDic.Clear();
            foreach(DataRow dr in colorMascota.Rows){
                pkrColorMascota.Items.Add(dr[5].ToString());
                colorMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrRazaMascota.SelectedIndexChanged += razaMascotaSeleccionado;
            pkrColorMascota.SelectedIndexChanged += colorMascotaSeleccionado;
        }

        public void razaMascotaSeleccionado(object sender, EventArgs args) {
            try
            {
                idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
            }
            catch (Exception) {
                
            }
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args) {
            try
            {
                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception)
            {
                
            }
        }

        async void onReg_dueno(object sender, EventArgs args) {
            string codigoMascota = "", nombreMascota = "", nombreDueno = "", apellidoP = "", apellidoM = "", correo = "", contrasena = "";
            int tipoMascota = -1, razaMascota = -1, colorMascota = -1, sexoMascota = -1, sexoDueno = -1, edadMascotaE = -1;

            try{
                codigoMascota = txtCodigoMascota.Text;
                tipoMascota = idTipoMascota;
                razaMascota = idRazaMascota;
                colorMascota = idColorMascota;
                //poner pickers
                sexoMascota = sexoMascotaC;
                sexoDueno = sexoDuenoC;
                //
                nombreMascota = txtnombreMascota.Text;
                edadMascotaE = Convert.ToInt32(txtedadMascota.Text);
                nombreDueno = txtnombreDueno.Text;
                apellidoP = txtApellidoPaterno.Text;
                apellidoM = txtApellidoMaterno.Text;
                correo = txtCorreo.Text;
                contrasena = txtContrasena.Text;

                string[] textos = { codigoMascota, nombreMascota, nombreDueno, apellidoP, apellidoM, correo, contrasena };
                int[] enteros = { tipoMascota, razaMascota, colorMascota, sexoMascota, sexoDueno, edadMascotaE };
                if (textos.Any(item => item.Length <= 0))
                {
                    await DisplayAlert("Error", "Faltan campos por llenar textos", "OK");
                    return;
                }
                else if (enteros.Any(item => item == -1)) {
                    await DisplayAlert("Error", "Faltan campos por llenar enteros", "OK");
                    return;
                }

                bool estatus = false;

                if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IAndroid>().getMascota_Registro(new Model.Dueno() {
                        idDueno = codigoMascota,
                        nombre = nombreDueno,
                        apellidoP = apellidoP,
                        apellidoM = apellidoM,
                        sexo = sexoDueno,
                        correo = correo,
                        contrasena = contrasena,
                        mascostaCodigo = codigoMascota,
                        sexoMascota = sexoMascota,
                        idTipoMascota = idTipoMascota,
                        idRazaMascota = idRazaMascota,
                        idColorMascota = idColorMascota,
                        edadMascota = edadMascotaE
                    });
                    estatus = DependencyService.Get<IAndroid>().Mascota_Registro;
                } else if (Device.RuntimePlatform == Device.iOS) {
                    DependencyService.Get<IIOS>().getMascota_Registro(new Model.Dueno() {
                        idDueno = codigoMascota,
                        nombre = nombreDueno,
                        apellidoP = apellidoP,
                        apellidoM = apellidoM,
                        sexo = sexoDueno,
                        correo = correo,
                        contrasena = contrasena,
                        mascostaCodigo = codigoMascota,
                        sexoMascota = sexoMascota,
                        idTipoMascota = idTipoMascota,
                        idRazaMascota = idRazaMascota,
                        idColorMascota = idColorMascota,
                        edadMascota = edadMascotaE
                    });
                    estatus = DependencyService.Get<IIOS>().Mascota_Registro;
                }
                if (estatus)
                {
                    await DisplayAlert("OK", "Se guardo registro", "OK");
                }
                else {
                    await DisplayAlert("Error", "Error al guardar registro", "OK");
                }

            }catch (Exception){
                await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                return;
            }
        }
    }
}
