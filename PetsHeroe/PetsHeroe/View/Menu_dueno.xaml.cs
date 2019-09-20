using System;
using PetsHeroe.Services;
using PetsHeroe.View;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Menu_dueno : ContentPage
    {
        public Menu_dueno()
        {
            InitializeComponent();
        }

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