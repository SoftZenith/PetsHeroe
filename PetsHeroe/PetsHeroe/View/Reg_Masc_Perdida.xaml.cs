using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Reg_Masc_Perdida : ContentPage
    {

        public void mascotaSelectedPerdida(object sender, EventArgs args) {
            Button button = (Button)sender;
            string nombre = button.CommandParameter.ToString();
            DisplayAlert("¿Reportar como perdido?", "Nombre: " + nombre, "OK");
            Navigation.PushAsync(new Mascota_recuperada());
        }

        public void mascotaSelectedRobada(object sender, EventArgs args) {
            Button button = (Button)sender;
            string nombre = button.CommandParameter.ToString();
            DisplayAlert("¿Reportar como robado?", "Nombre: " + nombre, "OK");
            Navigation.PushAsync(new Mascota_recuperada());
        }

        public Reg_Masc_Perdida()
        {
            InitializeComponent();

            Mascota listaMasc = new Mascota();
            lsvMascotas.ItemsSource = listaMasc.getMascotaList(Preferences.Get("idMiembro", -1));
        }
    }
}
