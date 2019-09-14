using System;
using PetsHeroe.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Menu_veterinario : ContentPage
    {
        public Menu_veterinario()
        {
            InitializeComponent();
        }

        async void onMisClientes(object sender, EventArgs args) {
            await Navigation.PushAsync(new Mis_Clientes());
        }

        async void onLocMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onCerrarSesion(object sender, EventArgs args) {
            Preferences.Clear();
            await Navigation.PushAsync(new MainPage());
        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().CloseApp();
            }
            return base.OnBackButtonPressed();
        }

    }
}
