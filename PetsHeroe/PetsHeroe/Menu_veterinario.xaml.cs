using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Menu_veterinario : ContentPage
    {
        public Menu_veterinario()
        {
            InitializeComponent();
        }

        async void onLocMascota(object sender, EventArgs args) {
            await Navigation.PushAsync(new Loc_Mascota());
        }

        async void onCerrarSesion(object sender, EventArgs args) {
            await Navigation.PopAsync();
        }

    }
}
