using System;
using PetsHeroe.Services;
using PetsHeroe.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu_dueno : Xamarin.Forms.TabbedPage
    {

        public Menu_dueno(int tab)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            try
            {
                _ = On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom)
                 .SetBarItemColor(Color.White)
                 .SetBarSelectedItemColor(Color.OrangeRed);
                InitializeComponent();
                tbMenuDueno.CurrentPage = tbMenuDueno.Children[tab];
            }
            catch (Exception ex) {
                DisplayAlert("Error", "No estas conectado a internet utilizar una conexión WIFI o datos celulares", "OK");
                DependencyService.Get<IWebService>().CloseApp();
            }
        }

        private void onAppearingBeneficios(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            tbMenuDueno.Title = "Beneficios";
        }

        private void onAppearingLocMascota(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            tbMenuDueno.Title = "¿Encontraste unas mascota?";
        }

        private void onAppearingMascExisten(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            tbMenuDueno.Title = "Mis Mascotas";
        }

        private void onAppearingCAMS(object sender, EventArgs e)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            tbMenuDueno.Title = "Lista de CAMs";
        }

        /*
        async void onConsultaBene(object sender, EventArgs args) {
            await Navigation.PushAsync(new Consulta_bene_dueno());
        }

        async void onMascotaPerdida(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onRegistrarMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Registro_mascota());
        }

        async void onListaCAMs(object sender, EventArgs args) {
            await Navigation.PushAsync(new Consulta_CAMS());
        }

        async void verMascotas(object sender, EventArgs args) {
            await Navigation.PushAsync(new Masc_existentes());
        }

        async void onReportarMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Reg_Masc_Perdida());
        }*/

        async void onCerrarSesion(object sender, EventArgs args) {

            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Cerrar sesión", "¿Desea cerrar sesión?", "Si", "No");
                if (result)
                {
                    Preferences.Set("logged", false, "usuarioLogeado");
                    Preferences.Set("userType", 0, "tipoUsuario");
                    Preferences.Set("idAsociado", -1);
                    await Navigation.PushAsync(new MainPage());
                }
            });

        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () => {
                bool answer = await DisplayAlert("Salir", "¿Estás seguro que deseas salir de la aplicación?", "Si", "No");
                if (answer) await DependencyService.Get<IWebService>().CloseApp();
            });

            return true;
            /*
            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IWebService>().CloseApp();
            }
            return base.OnBackButtonPressed();*/
        }

    }
}