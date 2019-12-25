using System;
using PetsHeroe.Services;
using PetsHeroe.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace PetsHeroe
{
    public partial class Menu_veterinario : Xamarin.Forms.TabbedPage
    {

        public Menu_veterinario(int tab)
        {
            try
            {
                InitializeComponent();

                On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
                 .SetBarItemColor(Color.White)
                 .SetBarSelectedItemColor(Color.OrangeRed);

                tbVete.CurrentPage = tbVete.Children[tab];
            }catch (Exception ex) {
                DisplayAlert("Error","No estas conectado a internet utilizar una conexión WIFI o datos celulares","OK");
                DependencyService.Get<IWebService>().CloseApp();
            }
        }

        private void onAppearingClients(object sender, EventArgs e)
        {
            tbVete.Title = "Mis Clientes";
        }

        private void onAppearingBenefics(object sender, EventArgs e)
        {
            tbVete.Title = "Consulta de beneficios";
        }

        private void onAppearingSales(object sender, EventArgs e)
        {
            tbVete.Title = "Caja de ventas";
        }

        private void onAppearLocation(object sender, EventArgs e)
        {
            tbVete.Title = "¿Encontraste una mascota?";
        }

        async void onCerrarSesion(object sender, EventArgs args) {

            var result = await this.DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión?", "Si", "No");
            if (result)
            {
                Preferences.Set("logged", false, "usuarioLogeado");
                Preferences.Set("userType", 0, "tipoUsuario");
                Preferences.Set("idAsociado", -1);
                await Navigation.PushAsync(new MainPage());
            }
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
