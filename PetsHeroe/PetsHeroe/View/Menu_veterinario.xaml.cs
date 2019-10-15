using System;
using PetsHeroe.Services;
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

        async void onBeneficios(object sender, EventArgs args) {
            await Navigation.PushAsync(new Consulta_bene_vete());
        }

        async void onLocMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onVentas(object sender, EventArgs args) {
            await Navigation.PushAsync(new Caja_Ventas());
        }

        async void onCerrarSesion(object sender, EventArgs args) {
            Preferences.Set("logged", false, "usuarioLogeado");
            Preferences.Set("userType", 0, "tipoUsuario");
            Preferences.Set("idAsociado", -1);
            await Navigation.PushAsync(new MainPage());
        }

        protected override bool OnBackButtonPressed()
        {

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IWebService>().CloseApp();
            }
            return base.OnBackButtonPressed();
        }

    }
}
