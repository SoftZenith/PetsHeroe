using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Caja_Ventas : ContentPage
    {
        ObservableCollection<Venta> listaCarrito = new ObservableCollection<Venta>();
        public ObservableCollection<Venta> ListaCarrito { get { return listaCarrito; } }
        //private List<Venta> listaCarrito = new List<Venta>();
        IQRScanning scanningDepen;
        private string codigoMascota = string.Empty;
        private double precio = -1;
        private string upc = string.Empty;
        private double total = 0;
        private double puntos = 0;
        private int idItemPointer = 0;
        private int idProducto = -1;
        private int tipo = 1;
        private DataTable productosEntryComplete;
        public List<string> productosEntry;
        public string txtProductoEntry;

        public Caja_Ventas()
        {
            InitializeComponent();
            lsvCarrito.ItemsSource = listaCarrito;
            lblTotal.Text = total.ToString();
            lblPuntos.Text = puntos.ToString();
            scanningDepen = DependencyService.Get<IQRScanning>();
            pkrTipo.SelectedIndexChanged += PkrTipo_SelectedIndexChanged;
            //listaCarrito.CollectionChanged += ListaCarrito_CollectionChanged;

            productosEntryComplete = DependencyService.Get<IWebService>().getProducto_Busca(-1, "");
            productosEntry = new List<string>();
            foreach (DataRow dr in productosEntryComplete.Rows) {
                productosEntry.Add(dr["UPC"].ToString());
            }

            txtProducto.ItemsSource = productosEntry;

        }

        private void PkrTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipo = pkrTipo.SelectedIndex + 1;

        }

        /*
        private void ListaCarrito_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
           foreach (Venta venta in listaCarrito)
           {
               puntos += venta.puntos;
               total += venta.precio;
           }
           lblTotal.Text = total.ToString();
           lblPuntos.Text = puntos.ToString();
        }*/

        public void btnMas(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            int idItemC = Convert.ToInt32(button.CommandParameter);

            for (int i = 0; i < listaCarrito.Count; i++) {
                if (listaCarrito[i].idItem == idItemC) {
                    Venta ventaTmp = new Venta
                    {
                        idItem = listaCarrito[i].idItem,
                        idProducto = listaCarrito[i].idProducto,
                        cantidad = listaCarrito[i].cantidad + 1,
                        nombre = listaCarrito[i].nombre,
                        precio = listaCarrito[i].precio,
                        puntos = listaCarrito[i].puntos
                    };
                    listaCarrito.RemoveAt(i);
                    listaCarrito.Add(ventaTmp);
                }
            }
        }



        public void btnMenos(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            int idItemC = Convert.ToInt32(button.CommandParameter);

            for (int i = 0; i < listaCarrito.Count; i++)
            {
                if (listaCarrito[i].idItem == idItemC)
                {
                    Venta ventaTmp = new Venta
                    {
                        idItem = listaCarrito[i].idItem,
                        idProducto = listaCarrito[i].idProducto,
                        cantidad = listaCarrito[i].cantidad - 1, //VALIDACION PARA NO TENER NEGATIVOS
                        nombre = listaCarrito[i].nombre,
                        precio = listaCarrito[i].precio,
                        puntos = listaCarrito[i].puntos
                    };
                    listaCarrito.RemoveAt(i);
                    listaCarrito.Add(ventaTmp);
                }
            }
        }

        async void scanCode(object sender, EventArgs args) {
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtCodigoMascota.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtCodigoMascota.Text = "";
            }
        }

        async void onEscanear(object sender, EventArgs args) {
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtProducto.Text = result;
                    txtProductoEntry = result;
                }
            }
            catch (Exception ex)
            {
                txtProducto.Text = "";
            }
        }

        async void onAgregar(object sender, EventArgs args) {
            
            if (txtCodigoMascota.Text == "" || txtCodigoMascota.Text == null)
            {
                await DisplayAlert("Error", "Ingresa un código de mascota", "Ok");
                return;
                
            }else if (txtProducto.Text == "" || txtProducto.Text == null) {
                await DisplayAlert("Error", "Ingrese nombre o código del producto", "Ok");
                return;
                
            } else if (txtMonto.Text == "" || txtMonto.Text == null) {
                await DisplayAlert("Error", "Ingrese un monto para este producto", "Ok");
                return;
            }

            try {
                Convert.ToDouble(txtMonto.Text);
            }
            catch (Exception ex) {
                await DisplayAlert("Error", "Monto invalido", "Ok");
                return;
            }

            DependencyService.Get<IWebService>().getCodigo_Valida(txtCodigoMascota.Text);

            DataTable productosEncontrados = DependencyService.Get<IWebService>().getProducto_Busca(-1, txtProducto.Text);

            if (!DependencyService.Get<IWebService>().Codigo_Valida) {
                await DisplayAlert("Error", "Código de mascota invalido", "Ok");
                return;
            }
            else if (productosEncontrados.Rows.Count > 0){

                string productName = "";
                Double productPoints = 0.0;

                foreach (DataRow dr in productosEncontrados.Rows)
                {
                    productName = dr["Name"].ToString();
                    productPoints = Convert.ToDouble(dr["Points"].ToString());
                    idProducto = Convert.ToInt32(dr["IDProduct"].ToString());
                }
                idItemPointer += 1;
                listaCarrito.Add(new Venta() { idItem = idItemPointer, cantidad = 1, nombre = productName, precio = Convert.ToDouble(txtMonto.Text), puntos = productPoints });
                total = 0;
                puntos = 0;
                foreach (Venta venta in listaCarrito) {
                    puntos += venta.puntos;
                    total += venta.precio;
                }
                lblTotal.Text = total.ToString();
                lblPuntos.Text = puntos.ToString();
                codigoMascota = txtCodigoMascota.Text;
                await DisplayAlert("Correcto", "Se agrego correctamente", "Ok");
            }
            else {
                //await DisplayAlert("Error", "No se encontro el producto o servicio", "Ok");
                Device.BeginInvokeOnMainThread(async () => {
                    var result = await this.DisplayAlert("Producto no encontrado", "¿Deseas agregarlo?", "Si", "No");
                    if (result)
                    {
                        Navigation.PushAsync(new Nuevo_Producto_Venta());
                    }
                });
            }
        }

        async void onSiguiente(object sender, EventArgs args) {

            //DependencyService.Get<IWebService>().agregar_venta(-1, 113, 16, 7, -1, 2, 29.93, out int idTicketOut, out int ventaResult);

            if (listaCarrito.Count > 0){
                await Navigation.PushAsync(new Pago_Venta(codigoMascota, total, listaCarrito));
            } else {
                await DisplayAlert("Error","No hay ningún producto/servicio en tu carrito","Ok");
            }
        }

    }
}
