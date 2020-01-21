using PetsHeroe.Model;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.Xaml;
using PetsHeroe.Services;
using Plugin.Connectivity;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mis_Clientes : ContentPage
    {

        public Mis_Clientes()
        {
            InitializeComponent();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }

            Cliente cliente = new Cliente();

            lsvClientes.ItemsSource = cliente.getListaClientes(Preferences.Get("idAsociado", -1));

        }
    }
}
