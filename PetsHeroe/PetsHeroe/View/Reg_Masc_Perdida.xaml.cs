using System;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Reg_Masc_Perdida : ContentPage
    {

        protected override void OnAppearing()
        {
            lsvMascotas.BeginRefresh();
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
                        DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 1, -1, -1, "");
                        Mascota listaMasc = new Mascota();
                        lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                    }
                });
            }
            else
            {
                bool status = DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 11, -1, -1, "");
                if (status)
                {
                    DisplayAlert("OK", "Se cancelo el reporte", "OK");
                    Mascota listaMasc = new Mascota();
                    lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
                }
                else
                {
                    DisplayAlert("Erro", "Hubo un error al cancelar el reporte", "Ok");
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
                        DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, 2, -1, -1, "");
                        Mascota listaMasc = new Mascota();
                        lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
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
