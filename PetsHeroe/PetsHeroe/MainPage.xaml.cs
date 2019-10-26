using System;
using System.ComponentModel;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Location currentlocation;

        public MainPage()
        {
            InitializeComponent();
            getCurrentLocation();
           
            bool logged = Preferences.Get("logged", false, "usuarioLogeado"); 
            int userType = Preferences.Get("userType", 0, "tipoUsuario");
            
            if (logged && userType == 1)
            {
                Device.BeginInvokeOnMainThread(async () =>
                { // Code for navigation });
                    Navigation.PushAsync(new Menu_dueno());
                });
            }
            else if (logged && userType == 2) {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Navigation.PushAsync(new Menu_veterinario());
                });

            }

        }//Constructor

        async void getCurrentLocation()
        {
            try{
                currentlocation = await Geolocation.GetLastKnownLocationAsync();
            }catch (Exception) {

            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android) {
                DependencyService.Get<IWebService>().CloseApp();
            }
            return base.OnBackButtonPressed();
        }

        async void onIniciar(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new IniciarSesion());
        }

        async void onRegDueno(object sender, EventArgs args)
        {

            await Navigation.PushAsync(new Registro_dueno_mascota());
        }

        async void onRegVet(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Registro_vet());
        }

        async void onMascEnc(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onListaCAM(object sender, EventArgs args) {
            await Navigation.PushAsync(new Consulta_CAMS());
        }

    }
}