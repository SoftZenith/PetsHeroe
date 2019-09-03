using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace PetsHeroe
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {               
        public MainPage()
        {
            InitializeComponent();
        }//Constructor

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android) {
                DependencyService.Get<IAndroid>().CloseApp();
            }
            return base.OnBackButtonPressed();
        }

        async void onIniciar(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new IniciarSesion());
            //_ = Navigation.PushModalAsync(new IniciarSesion());
        }

        async void onRegDueno(object sender, EventArgs args)
        {

            await Navigation.PushAsync(new Registro_dueno_mascota());
        }

        async void onRegVet(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new Registro_vet());
            //_ = Navigation.PushModalAsync(new Registro_vet());
        }

        async void onMascEnc(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onListaCAM(object sender, EventArgs args) {
            //NavigationPage npMasc_existen = new NavigationPage(new Masc_existentes());
            //await Navigation.PushAsync(new Masc_existentes());
            //await Navigation.PushAsync(new Reg_Masc_Perdida());
            await Navigation.PushAsync(new Consulta_CAMS());
        }

    }
}