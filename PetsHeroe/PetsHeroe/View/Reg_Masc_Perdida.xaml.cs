using System;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reg_Masc_Perdida : ContentPage
    {
        protected override void OnAppearing()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }

            Mascota listaMasc = new Mascota();
            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
            lsvMascotas.IsRefreshing = true;
            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
            lsvMascotas.IsRefreshing = false;
            base.OnAppearing();
        }

        public void mascotaSelectedPerdida(object sender, EventArgs args) {
            Button button = (Button)sender;
            string texto = button.Text;
            int idMascota = Convert.ToInt32(button.CommandParameter);
            if (texto == "Reportar como perdida")
            {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await this.DisplayAlert("¿Reportar como perdido?", "¿Reportar como perdido?", "Si", "No");
                    if (result)
                    {
                        Retorno retorno = DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 1, -1, -1, "");
                        if (retorno.Resultado)
                        {
                            Mascota listaMasc = new Mascota();
                            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                        }
                        else
                        {
                            await DisplayAlert("Error",retorno.Mensaje,"Ok");
                        }
                    }
                });
            }
            else
            {
                Retorno retorno = DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 11, -1, -1, "");
                if (retorno.Resultado)
                {
                    DisplayAlert("OK", "Se cancelo el reporte", "OK");
                    Mascota listaMasc = new Mascota();
                    lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                }
                else
                {
                    DisplayAlert("Error", retorno.Mensaje, "Ok");
                }
            }
        }

        public void mascotaSelectedRobada(object sender, EventArgs args) {
            Button button = (Button)sender;
            string texto = button.Text;
            int idMascota = Convert.ToInt32(button.CommandParameter);
            if (texto == "Reportar como robada")
            {
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await this.DisplayAlert("¿Reportar como robada?", "¿Reportar como robada?", "Si", "No");
                    if (result)
                    {
                        Retorno retorno = DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 2, -1, -1, "");
                        if (retorno.Resultado)
                        {
                            Mascota listaMasc = new Mascota();
                            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                        }
                        else {
                            await DisplayAlert("Error", retorno.Mensaje, "Ok");
                        }
                    }
                });
            }
            else
            {
                Navigation.PushAsync(new Mascota_recuperada(idMascota));
            }
        }

        public Reg_Masc_Perdida(){
            InitializeComponent();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }

            Mascota listaMasc = new Mascota();
            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));

            lsvMascotas.RefreshCommand = new Command(() => {
                lsvMascotas.IsRefreshing = true;
                lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                lsvMascotas.IsRefreshing = false;
            });
        }
    }
}
