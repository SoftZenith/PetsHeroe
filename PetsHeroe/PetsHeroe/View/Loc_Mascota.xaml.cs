using System;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Loc_Mascota : ContentPage
    {
        private int opcion = 0;
        IAndroid wsDependency;
        IIOS wsDdependecyIOS;
        IQRScanning scanningDepen;

        public Loc_Mascota()
        {
            InitializeComponent();
            if (Device.RuntimePlatform == Device.Android)
            {
                wsDependency = DependencyService.Get<IAndroid>();
            } else if (Device.RuntimePlatform == Device.iOS) {
                wsDdependecyIOS = DependencyService.Get<IIOS>();
            }
            scanningDepen = DependencyService.Get<IQRScanning>();

            txtCodigo.Text = "";
            rdOpcion.SelectedItemChanged += onOpcionSeleccionada;
        }

        void onOpcionSeleccionada(object sender, EventArgs args) {
            opcion = rdOpcion.SelectedIndex;
        }

        async void onScan(object sender, EventArgs args) {
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtCodigo.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtCodigo.Text = ex.ToString();
            }
        }

        async void onSiguiente(object sender, EventArgs args) {

            string codigo;
            try
            {
                codigo = txtCodigo.Text;
            }catch (Exception){
                codigo = "";
            }

            bool valido = false;
            if (Device.RuntimePlatform == Device.Android) {
                wsDependency.getCodigo_Valida(codigo);
                valido = wsDependency.Codigo_Valida;
            } else if (Device.RuntimePlatform == Device.iOS) {
                wsDdependecyIOS.getCodigo_Valida(codigo);
                valido = wsDdependecyIOS.Codigo_Valida;
            }
            if (!valido) {
                await DisplayAlert("Código no valido", "Código no valido", "OK");
                return;
            }

            switch (opcion) {
                case 0:
                    _ = Navigation.PushAsync(new Llevar_centro(codigo));
                    break;
                case 1:
                    _ = Navigation.PushAsync(new Mensaje_Dueno(codigo));
                    break;
                case 2:
                    _ = Navigation.PushAsync(new Tomar_Nota(codigo));
                    break;
                default:
                    await DisplayAlert("Seleccionado", "Ninguno seleccionado", "OK");
                    break;
            }
        }
    }
}
