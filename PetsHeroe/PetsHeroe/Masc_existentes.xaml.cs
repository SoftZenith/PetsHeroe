using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Masc_existentes : ContentPage
    {
        public Masc_existentes()
        {
            InitializeComponent();

            Mascota mascotasExisten = new Mascota();

            lsvMascotasExiste.ItemsSource = mascotasExisten.getMascotaList();
        }
    }
}
