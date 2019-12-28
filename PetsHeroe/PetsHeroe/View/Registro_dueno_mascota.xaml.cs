using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetsHeroe.Services;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using PetsHeroe.Model;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro_dueno_mascota : ContentPage
    {
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        int idTipoMascota = -1;
        int idRazaMascota = -1;
        int idColorMascota = -1;
        char sexoDuenoC;
        char sexoMascotaC = (char)78;
        //Diccionarios para guardar nombre - id
        Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>(); 
        Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();

        public Registro_dueno_mascota()
        {
            InitializeComponent();

            pkrRazaMascota.IsEnabled = false;
            pkrColorMascota.IsEnabled = false;
            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;

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
                pkrColorMascota.IsEnabled = true;
            }catch (Exception) {
                
            }
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args) {
            try
            {
                if (idRazaMascota == -1) {
                    DisplayAlert("Advertencia","Es necesario seleccionar la raza","OK");
                }

                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception)
            {
                
            }
        }

        async void onReg_dueno(object sender, EventArgs args) {
            string codigoMascota = "", nombreMascota = "", nombreDueno = "", apellidoP = "", apellidoM = "", correo = "", contrasena = "";
            int tipoMascota = -1, razaMascota = -1, colorMascota = -1, sexoMascota = 0, sexoDueno = -1, edadMascotaE = -1;

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

                string[] textos = { codigoMascota, nombreMascota, nombreDueno, apellidoP, correo, contrasena };
                int[] enteros = { tipoMascota, razaMascota, colorMascota, sexoDueno, edadMascotaE };
                if (textos.Any(item => item.Length <= 0))
                {
                    await DisplayAlert("Error", "Faltan campos por llenar textos", "OK");
                    return;
                }
                if (enteros.Any(item => item == -1)) {
                    await DisplayAlert("Error", "Faltan campos por llenar enteros", "OK");
                    return;
                }

                if (!ValidateEmail(txtCorreo.Text)) {
                    await DisplayAlert("Error","Correo invalido","OK");
                    return;
                }

                bool estatus = false;

                Retorno retorno = DependencyService.Get<IWebService>().getMascota_Registro(new Model.Dueno()
                {
                    idDueno = codigoMascota,
                    nombre = nombreDueno,
                    apellidoP = apellidoP,
                    apellidoM = apellidoM,
                    sexo = sexoDueno,
                    correo = correo,
                    contrasena = contrasena,
                    mascostaCodigo = codigoMascota,
                    nombreMascota = nombreMascota,
                    sexoMascota = sexoMascota,
                    idTipoMascota = idTipoMascota,
                    idRazaMascota = idRazaMascota,
                    idColorMascota = idColorMascota,
                    edadMascota = edadMascotaE
                });
                estatus = DependencyService.Get<IWebService>().Mascota_Registro;

                if (retorno.Resultado){
                    await DisplayAlert("OK", "Se registro correctamente", "OK");
                    await Navigation.PushAsync(new MainPage());
                }else {
                    await DisplayAlert("Error", retorno.Mensaje, "OK");
                }

            }catch (Exception){
                await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                return;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                pkrTipoMascota.Focused += (object sender, FocusEventArgs eventHandler) =>
                {
                    Navigation.PopAsync();
                };
                pkrRazaMascota.IsVisible = false;
                pkrColorMascota.IsVisible = false;
            }
            return base.OnBackButtonPressed();
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }
    }
}
