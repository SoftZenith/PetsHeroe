using System;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Reg_Masc_Perdida : ContentPage
    {

        public void mascotaSelectedPerdida(object sender, EventArgs args) {
            Button button = (Button)sender;
            string nombre = button.CommandParameter.ToString();
            DisplayAlert("¿Reportar como perdido?", "Nombre: " + nombre, "OK");
        }

        public void mascotaSelectedRobada(object sender, EventArgs args) {
            Button button = (Button)sender;
            string nombre = button.CommandParameter.ToString();
            DisplayAlert("¿Reportar como robado?", "Nombre: " + nombre, "OK");
        }

        public Reg_Masc_Perdida()
        {
            InitializeComponent();

            Mascota listaMasc = new Mascota();
            lsvMascotas.ItemsSource = listaMasc.getMascotaList();
        }
    }
}
