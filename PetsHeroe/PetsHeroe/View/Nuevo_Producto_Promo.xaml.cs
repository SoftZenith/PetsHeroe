using System;
using System.Collections.Generic;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Nuevo_Producto_Promo : ContentPage
    {
        IQRScanning scanningDepen;

        public Nuevo_Producto_Promo()
        {
            InitializeComponent();
            scanningDepen = DependencyService.Get<IQRScanning>();
        }

        public async void onEscanear(object sender, EventArgs args) {
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    result = result.Replace("=", "");
                    txtBuscarE.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtBuscarE.Text = "";
            }
        }
    }
}
