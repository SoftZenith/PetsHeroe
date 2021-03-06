﻿using System;
using System.ComponentModel;
using PetsHeroe.Services;
using PetsHeroe.View;
using Plugin.Connectivity;
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
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }
            getCurrentLocation();
           
            bool logged = Preferences.Get("logged", false, "usuarioLogeado"); 
            int userType = Preferences.Get("userType", 0, "tipoUsuario");
            
            if (logged && userType == 1)
            {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    DisplayAlert("Error", "No estás conectado a internet", "Ok");
                    return;
                }
                Device.BeginInvokeOnMainThread(async () =>
                { // Code for navigation });
                   await Navigation.PushAsync(new Menu_dueno(0));
                });
            }
            else if (logged && userType == 2) {
                if (!CrossConnectivity.Current.IsConnected)
                {
                    DisplayAlert("Error", "No estás conectado a internet", "Ok");
                }
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PushAsync(new Menu_veterinario(0));
                });
            }

        }//Constructor

        async void getCurrentLocation()
        {
            try{
                if (!CrossConnectivity.Current.IsConnected)
                {
                    await DisplayAlert("Error", "No estás conectado a internet", "Ok");
                    return;
                }
                currentlocation = await Geolocation.GetLastKnownLocationAsync();
            }catch (Exception ex) {
                await DisplayAlert("Error", "No se tiene permiso para acceder a la localización de tu dispositivo", "Ok");
                return;
            }
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

        async void onIniciar(object sender, EventArgs args)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            await Navigation.PushAsync(new IniciarSesion());
        }

        async void onRegDueno(object sender, EventArgs args)
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            await Navigation.PushAsync(new Registro_dueno_mascota());
        }

        async void onRegVet(object sender, EventArgs args)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            await Navigation.PushAsync(new Registro_vet());
        }

        async void onMascEnc(object sender, EventArgs args) {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onListaCAM(object sender, EventArgs args) {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            await Navigation.PushAsync(new Consulta_CAMS());
        }

    }
}