using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nuevo_Producto_Venta : ContentPage
    {
        IQRScanning scanningDepen;
        private Dictionary<string, int> tipoProductoDic = new Dictionary<string, int>();
        private Dictionary<string, int> marcaProductoDic = new Dictionary<string, int>();

        private int idTipoProducto = -1;
        private int idMarcaProducto = -1;

        public Nuevo_Producto_Venta()
        {
            InitializeComponent();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            scanningDepen = DependencyService.Get<IQRScanning>();

            DataTable tipoProducto = new DataTable();

            tipoProducto = DependencyService.Get<IWebService>().getTipoProducto_Busca();

            pkrTipo.Items.Clear();
            tipoProductoDic.Clear();
            foreach (DataRow dr in tipoProducto.Rows)
            {
                pkrTipo.Items.Add(dr[2].ToString());
                tipoProductoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrTipo.SelectedIndexChanged += tipoProductoSeleccionado;

        }

        private void tipoProductoSeleccionado(object sender, EventArgs e)
        {

            idTipoProducto = tipoProductoDic[pkrTipo.SelectedItem.ToString()];

            DataTable marcaProducto = new DataTable();
            marcaProducto = DependencyService.Get<IWebService>().getMarcaProducto_Busca();

            pkrMarcar.Items.Clear();
            marcaProductoDic.Clear();

            foreach (DataRow dr in marcaProducto.Rows)
            {
                pkrMarcar.Items.Add(dr[2].ToString());
                marcaProductoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrMarcar.SelectedIndexChanged += PkrMarcar_SelectedIndexChanged;

        }

        private void PkrMarcar_SelectedIndexChanged(object sender, EventArgs e)
        {
            idMarcaProducto = marcaProductoDic[pkrMarcar.SelectedItem.ToString()];
        }

        async void onEscanear(object sender, EventArgs args)
        {
            var status = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "La app no tiene permisos para utilizar la camara", "OK");
                return;
            }
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtUPC.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtUPC.Text = "";
            }
        }

        async void onAgregar(object sender, EventArgs args)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            if (txtNombre.Text == null || txtNombre.Text == "") {
                await DisplayAlert("Error", "Ingresa un nombre para el producto", "Ok");
                return;
            }
            if (txtUPC.Text == null || txtUPC.Text == "") {
                await DisplayAlert("Error", "Ingresa un código UPC para el producto", "Ok");
                return;
            }
            if (idTipoProducto < 0) {
                await DisplayAlert("Error","Selecciona el tipo de producto","Ok");
                return;
            }
            if (idMarcaProducto < 0) {
                await DisplayAlert("Error","Selecciona una marca","Ok");
                return;
            }

            if (DependencyService.Get<IWebService>().producto_Agrega(idTipoProducto, idMarcaProducto, txtNombre.Text, txtUPC.Text) > 0)
            {
                await DisplayAlert("OK", "Se agrego correctamente", "Ok");
                await Navigation.PopAsync();
            }
            else {
                await DisplayAlert("Error","No se pudo agregar intenta con otro ","Ok");
            }

        }

    }
}
