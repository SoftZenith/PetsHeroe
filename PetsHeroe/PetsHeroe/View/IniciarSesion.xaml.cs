using System;
using System.Collections.Generic;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class IniciarSesion : ContentPage
    {
        string user = "";
        string pass = "";

        public IniciarSesion()
        {
            InitializeComponent();

            var forgetPassword_tap = new TapGestureRecognizer();

            txtUsuario.Completed += TxtPassword_Completed;

            forgetPassword_tap.Tapped += (s, e) =>
            {
                var correo = "";
                try{
                    correo = txtUsuario.Text;
                    if (correo == "")
                    {
                        DisplayAlert("Error", "Ingresa tu correo", "OK");
                        return;
                    }
                }catch (Exception) {
                    correo = "";
                }


                DependencyService.Get<IWebService>().getEnviaContrasena(correo);
                if (DependencyService.Get<IWebService>().EnviaContrasena)
                {
                    DisplayAlert("Correcto", "Te enviamos un correo con tu contraseña", "OK");
                }
                else
                {
                    DisplayAlert("Error", "Verifica que sea la dirección de correo con la que te registraste", "OK");
                }
            };

            lblOlvide.GestureRecognizers.Add(forgetPassword_tap);
        }

        private void TxtPassword_Completed(object sender, EventArgs e)
        {

        }

        async void onEntrar(object sender, EventArgs args)
        {
            try{
                user = txtUsuario.Text.ToString();
                pass = txtPassword.Text.ToString();
            }catch (Exception) {
                await DisplayAlert("Campos faltantes","Llena todos los campos","OK");
                return;
            }

            if (user == "" || pass == "") {
                await DisplayAlert("Campos faltantes", "Llena todos los campos", "OK");
                return;
            }

            if (DependencyService.Get<IWebService>().getValidaUsuario(user, pass)) {
                var asociado = DependencyService.Get<IWebService>().ValidaUsuario;
                if (asociado.idMiembro > 0)
                {
                    await DisplayAlert("Bienvenido", "Dueño " + asociado.nombre, "OK");
                    Preferences.Set("logged", true, "usuarioLogeado");
                    Preferences.Set("userType", 1, "tipoUsuario");
                    Preferences.Set("idMiembro", asociado.idMiembro);
                    Preferences.Set("password", pass);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        Navigation.PushAsync(new Menu_dueno(0));
                    });
                }
                else if (asociado.idAsociado > 0)
                {
                    await DisplayAlert("Bienvenido", "Asociado " + asociado.nombre, "OK");
                    Preferences.Set("logged", true, "usuarioLogeado");
                    Preferences.Set("userType", 2, "tipoUsuario");
                    Preferences.Set("idAsociado", asociado.idAsociado);
                    Preferences.Set("password", pass);
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PushAsync(new Menu_veterinario(0));
                    });
                }

            } else {
                await DisplayAlert("Error", "Usuario y/o contraseña incorrecto", "OK");
            }
        }



    }
}