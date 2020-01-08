using PetsHeroe.Model;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using PetsHeroe.Services;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mis_Clientes : ContentPage
    {

        public Mis_Clientes()
        {
            InitializeComponent();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }

            Cliente cliente = new Cliente();

            lsvClientes.ItemsSource = cliente.getListaClientes(Preferences.Get("idAsociado", -1));

        }
    }
}
