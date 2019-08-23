using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Loc_Mascota : ContentPage
    {
        private int opcion = 0;

        public Loc_Mascota()
        {
            InitializeComponent();
            rdOpcion.SelectedItemChanged += onOpcionSeleccionada;
        }

        void onOpcionSeleccionada(object sender, EventArgs args) {
            opcion = rdOpcion.SelectedIndex;
        }

        async void onSiguiente(object sender, EventArgs args) {
            switch (opcion) {
                case 0:
                    _ = Navigation.PushAsync(new Llevar_centro());
                    break;
                case 1:
                    _ = Navigation.PushAsync(new Mensaje_Dueno());
                    break;
                case 2:
                    _ = Navigation.PushAsync(new Tomar_Nota());
                    break;
                default:
                    DisplayAlert("Seleccionado", "Ninguno seleccionado", "OK");
                    break;
            }
        }
    }
}
