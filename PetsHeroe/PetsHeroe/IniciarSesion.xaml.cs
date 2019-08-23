using System;
using System.Collections.Generic;

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
                DisplayAlert("¿Olvidaste tu contraseña?", "Te enviamos un correo", "OK");
            };
            lblOlvide.GestureRecognizers.Add(forgetPassword_tap);
        }

        async void onEntrar(object sender, EventArgs args)
        {
            try
            {
                user = txtUsuario.Text.ToString();
                pass = txtPassword.Text.ToString();
            }
            catch (Exception ex) {
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
                    await DisplayAlert("Alert", "Usuario " + DependencyService.Get<IIOS>().nombre, "OK");
                    //Navigation.PushModalAsync(new menu_general());
                }
                else {
                    await DisplayAlert("Alert", "Usuario incorrecto", "OK");
                }
            }else if (Device.RuntimePlatform == Device.Android) {
                if (DependencyService.Get<IAndroid>().getValidaUsuario(user, pass))
                {
                    await DisplayAlert("Alert", "Usuario "+DependencyService.Get<IAndroid>().nombre, "OK");
                }
                else {
                    await DisplayAlert("Alert", "Usuario incorrecto", "OK");
                }
            }
        }

    }
}
