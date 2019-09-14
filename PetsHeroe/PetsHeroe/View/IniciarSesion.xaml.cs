using System;
using System.Collections.Generic;
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

                if (Device.RuntimePlatform == Device.Android) {
                    DependencyService.Get<IAndroid>().getEnviaContrasena(correo);
                    if (DependencyService.Get<IAndroid>().EnviaContrasena)
                    {
                        DisplayAlert("Correcto", "Te enviamos un correo con tu contraseña", "OK");
                    } else {
                        DisplayAlert("Error", "Verifica que sea la dirección de correo con la que te registraste", "OK");
                    }
                } else if (Device.RuntimePlatform == Device.iOS) {
                    DependencyService.Get<IIOS>().getEnviaContrasena(correo);
                    if (DependencyService.Get<IIOS>().EnviaContrasena)
                    {
                        DisplayAlert("Correcto", "Te enviamos un correo con tu contraseña", "OK");
                    }
                    else
                    {
                        DisplayAlert("Error", "Verifica que sea la dirección de correo con la que te registraste", "OK");
                    }
                }
            };
            lblOlvide.GestureRecognizers.Add(forgetPassword_tap);
        }

        async void onEntrar(object sender, EventArgs args)
        {
            try{
                user = txtUsuario.Text.ToString();
                pass = txtPassword.Text.ToString();
            }catch (Exception ex) {
                await DisplayAlert("Campos faltantes","Llena todos los campos","OK");
                return;
            }

            if (user == "" || pass == "") {
                await DisplayAlert("Campos faltantes", "Llena todos los campos", "OK");
                return;
            }
            else if(Device.RuntimePlatform == Device.iOS)
            {
                if (DependencyService.Get<IIOS>().getValidaUsuario(user, pass))
                {

                    var asociado = DependencyService.Get<IIOS>().ValidaUsuario;
                    if (asociado.idMiembro > 0){
                        await DisplayAlert("Bienvenido", "Dueño "+asociado.nombre, "OK");
                        Preferences.Set("logged", true, "usuarioLogeado");
                        Preferences.Set("userType", 1, "tipoUsuario");
                        Preferences.Set("idMiembro", asociado.idMiembro);
                        await Navigation.PushAsync(new Menu_dueno());
                    }
                    else if(asociado.idAsociado > 0){
                        await DisplayAlert("Bienvenido", "Asociado "+asociado.nombre, "OK");
                        Preferences.Set("logged", true, "usuarioLogeado");
                        Preferences.Set("userType", 2, "tipoUsuario");
                        Preferences.Set("idAsociado", asociado.idAsociado);
                        await Navigation.PushAsync(new Menu_veterinario());
                    }
                    //Navigation.PushModalAsync(new menu_general());
                }
                else {
                    await DisplayAlert("Error", "Usuario y/o contraseña incorrecto", "OK");
                }
            }else if (Device.RuntimePlatform == Device.Android) {
                if (DependencyService.Get<IAndroid>().getValidaUsuario(user, pass))
                {

                    var asociado = DependencyService.Get<IAndroid>().ValidaUsuario;
                    if (asociado.idMiembro > 0)
                    {
                        await DisplayAlert("Bienvenido", "Dueño " + asociado.nombre, "OK");
                        Preferences.Set("logged", true, "usuarioLogeado");
                        Preferences.Set("userType", 1, "tipoUsuario");
                        Preferences.Set("idMiembro", asociado.idMiembro);
                        await Navigation.PushAsync(new Menu_dueno());
                    }
                    else if (asociado.idAsociado > 0)
                    {
                        await DisplayAlert("Bienvenido", "Asociado " + asociado.nombre, "OK");
                        Preferences.Set("logged", true, "usuarioLogeado");
                        Preferences.Set("userType", 2, "tipoUsuario");
                        Preferences.Set("idAsociado", asociado.idAsociado);
                        await Navigation.PushAsync(new Menu_veterinario());
                    }
                }
                else {
                    await DisplayAlert("Error", "Usuario y/o contraseña incorrectos", "OK");
                }
            }
        }

    }
}
