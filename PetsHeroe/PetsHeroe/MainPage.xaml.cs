using System;
using System.ComponentModel;
using System.Data;
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
            return true;
        }

        async void onIniciar(object sender, EventArgs args)
        {
            _ = Navigation.PushModalAsync(new IniciarSesion());
        }

        async void onRegDueno(object sender, EventArgs args)
        {
            _ = Navigation.PushModalAsync(new Registro_dueno_mascota());
        }

        async void onRegVet(object sender, EventArgs args)
        {
            //_ = Navigation.PushModalAsync(new Registro_veterinario());
        }

        async void onMascEnc(object sender, EventArgs args) {
            _ = Navigation.PushModalAsync(new Loc_Mascota());
        }

    }
}