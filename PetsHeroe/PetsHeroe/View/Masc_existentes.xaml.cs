using System;
using System.Collections.Generic;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Masc_existentes : ContentPage
    {
        public Masc_existentes()
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }
            lsvMascotasExiste.RefreshCommand = new Command(() => {
                lsvMascotasExiste.IsRefreshing = true;
                Mascota mascotasExistenCom = new Mascota();
                lsvMascotasExiste.ItemsSource = mascotasExistenCom.getMascotaList(Preferences.Get("idMiembro", -1));
                lsvMascotasExiste.IsRefreshing = false;
            });

            lsvMascotasExiste.ItemTapped += LsvMascotasExiste_ItemTapped;

            Mascota mascotasExisten = new Mascota();

            lsvMascotasExiste.ItemsSource = mascotasExisten.getMascotaList(Preferences.Get("idMiembro", -1));
        }

        private void LsvMascotasExiste_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Mascota mascotaSelected = lsvMascotasExiste.SelectedItem as Mascota;
            Navigation.PushAsync(new Registro_mascota(mascotaSelected, true));
        }

        protected override void OnAppearing()
        {
            lsvMascotasExiste.IsRefreshing = true;
            Mascota mascotasExisten = new Mascota();
            lsvMascotasExiste.ItemsSource = mascotasExisten.getMascotaList(Preferences.Get("idMiembro", -1));
            lsvMascotasExiste.IsRefreshing = false;
        }

        async void onReportar(object sender, EventArgs args) {
            await Navigation.PushAsync(new Reg_Masc_Perdida());
        }

    }
}
