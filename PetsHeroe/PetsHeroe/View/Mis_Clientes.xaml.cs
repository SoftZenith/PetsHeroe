using PetsHeroe.Model;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace PetsHeroe.View
{
    public partial class Mis_Clientes : ContentPage
    {
        public Mis_Clientes()
        {
            InitializeComponent();

            Cliente cliente = new Cliente();

            lsvClientes.ItemsSource = cliente.getListaClientes(Preferences.Get("idAsociado", -1));

        }
    }
}
