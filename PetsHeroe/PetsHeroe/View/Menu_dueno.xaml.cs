using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Menu_dueno : ContentPage
    {
        public Menu_dueno()
        {
            InitializeComponent();
        }

        async void onMascotaPerdida(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onRegistrarMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Registro_mascota());
        }

        async void onListaCAMs(object sender, EventArgs args) {
            await Navigation.PushAsync(new Consulta_CAMS());
        }

        async void verMascotas(object sender, EventArgs args) {
            await Navigation.PushAsync(new Masc_existentes());
        }

        async void onReportarMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Reg_Masc_Perdida());
        }

        async void onCerrarSesion(object sender, EventArgs args) {
            await Navigation.PopAsync();
        }
    }
}
