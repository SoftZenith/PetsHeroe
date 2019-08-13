using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class IniciarSesion : ContentPage
    {
        public IniciarSesion()
        {
            InitializeComponent();
        }

        async void onEntrar(object sender, EventArgs args)
        {
            string user = txtUsuario.Text;
            string pass = txtPassword.Text;

            if (Device.RuntimePlatform == Device.iOS)
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
