using System;
using System.Collections.Generic;
using PetsHeroe.Services;
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            InitializeComponent();

            lsvMascotasExiste.RefreshCommand = new Command(() => {
                lsvMascotasExiste.IsRefreshing = true;
                Mascota mascotasExistenCom = new Mascota();
                lsvMascotasExiste.ItemsSource = mascotasExistenCom.getMascotaList(Preferences.Get("idMiembro", -1));
                lsvMascotasExiste.IsRefreshing = false;
            });

            Mascota mascotasExisten = new Mascota();

            lsvMascotasExiste.ItemsSource = mascotasExisten.getMascotaList(Preferences.Get("idMiembro", -1));
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
