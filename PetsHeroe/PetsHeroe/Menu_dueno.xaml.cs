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

        async void onCerrarSesion(object sender, EventArgs args) {
            await Navigation.PopAsync();
        }
    }
}
