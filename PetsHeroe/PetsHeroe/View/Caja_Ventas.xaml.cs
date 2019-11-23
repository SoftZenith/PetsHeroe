using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using dotMorten.Xamarin.Forms;
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
        private string UPCG = "";
        private DataTable productosEntryComplete;
        public Dictionary<String,String> productosEntryDic;
        public List<Promocion> productosEntry;
        public string txtProductoEntry;
        private bool UPCEncontrado = false;

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
            productosEntryDic = new Dictionary<string, string>();
            productosEntry = new List<Promocion>();
            foreach (DataRow dr in productosEntryComplete.Rows) {
                try
                {
                    productosEntryDic.Add(dr["UPC"].ToString(), dr["Product"].ToString());
                    Promocion promoTemp = new Promocion()
                    {
                        nombre = dr["Product"].ToString(),
                        UPC = dr["UPC"].ToString()
                    };
                    productosEntry.Add(promoTemp);
                }
                catch (Exception ex) {

                }
            }

            //txtUPC.ItemsSource = productosEntry;

        }
        
        private void pkrCantidadSelected(object sender, EventArgs e)
        {
            Picker pkr = sender as Picker;
            int idElemento = Convert.ToInt32(pkr.BindingContext);

            int cantidadPkr = pkr.SelectedIndex + 1;

            if (listaCarrito.Count <= 0) {
                return;
            }

            for (int i = 0; i < listaCarrito.Count; i++)
            {
                if (listaCarrito[i].idItem == idElemento)
                {
                    Venta ventaTmp = new Venta
                    {
                        idItem = listaCarrito[i].idItem,
                        idProducto = listaCarrito[i].idProducto,
                        cantidad = cantidadPkr,
                        nombre = listaCarrito[i].nombre,
                        precio = listaCarrito[i].precio,
                        puntos = listaCarrito[i].puntos
                    };

                    listaCarrito.RemoveAt(i);
                    listaCarrito.Add(ventaTmp);
                }
            }

            puntos = 0;
            total = 0;
            foreach (Venta venta in listaCarrito)
            {
                puntos += (venta.puntos * venta.cantidad);
                total += (venta.precio * venta.cantidad);
            }
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";

        }

        private void PkrTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTipo.SelectedIndex == 0)
            {
                imgTipo.Source = "collar_big.png";
                productosEntry.Clear();
                productosEntryDic.Clear();
                productosEntryComplete = DependencyService.Get<IWebService>().getProducto_Busca(-1, "");
                foreach (DataRow dr in productosEntryComplete.Rows)
                {
                    try
                    {
                        productosEntryDic.Add(dr["UPC"].ToString(), dr["Product"].ToString());
                        Promocion promoTemp = new Promocion()
                        {
                            nombre = dr["Product"].ToString(),
                            UPC = dr["UPC"].ToString()
                        };
                        productosEntry.Add(promoTemp);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            else {
                imgTipo.Source = "shower_big.png";
                productosEntry.Clear();
                productosEntryComplete = DependencyService.Get<IWebService>().getServicio_Busca(-1);
                foreach (DataRow dr in productosEntryComplete.Rows)
                {
                    try
                    {
                        //productosEntryDic.Add(dr["UPC"].ToString(), dr["Product"].ToString());
                        Promocion promoTemp = new Promocion()
                        {

                            nombre = dr["name"].ToString()
                            //UPC = dr["UPC"].ToString()
                        };
                        productosEntry.Add(promoTemp);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
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
            puntos = 0;
            total = 0;
            foreach (Venta venta in listaCarrito)
            {
                puntos += (venta.puntos * venta.cantidad);
                total += (venta.precio * venta.cantidad);
            }
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";
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
                        cantidad = listaCarrito[i].cantidad - 1 < 0 ? 0 : listaCarrito[i].cantidad - 1, //VALIDACION PARA NO TENER NEGATIVOS
                        nombre = listaCarrito[i].nombre,
                        precio = listaCarrito[i].precio,
                        puntos = listaCarrito[i].puntos
                    };
                    listaCarrito.RemoveAt(i);
                    listaCarrito.Add(ventaTmp);
                }
            }
            puntos = 0;
            total = 0;
            foreach (Venta venta in listaCarrito)
            {
                puntos += (venta.puntos * venta.cantidad);
                total += (venta.precio * venta.cantidad);
            }
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";

        }

        private void cantidad_TextChanged(object sender, TextChangedEventArgs e)
        {
            Entry cantidadEntry = sender as Entry;
            int idElemento = Convert.ToInt32(cantidadEntry.BindingContext);
            int cantidadX = 0;
            try
            {
                cantidadX = Convert.ToInt32(cantidadEntry.Text);
            }
            catch (Exception ex) {
                return;
            }

            for (int i = 0; i < listaCarrito.Count; i++)
            {
                if (listaCarrito[i].idItem == idElemento)
                {
                    Venta ventaTmp = new Venta
                    {
                        idItem = listaCarrito[i].idItem,
                        idProducto = listaCarrito[i].idProducto,
                        cantidad = cantidadX,
                        nombre = listaCarrito[i].nombre,
                        precio = listaCarrito[i].precio,
                        puntos = listaCarrito[i].puntos
                    };

                    listaCarrito.RemoveAt(i);
                    listaCarrito.Add(ventaTmp);
                }
            }

            puntos = 0;
            total = 0;
            foreach (Venta venta in listaCarrito)
            {
                puntos += (venta.puntos * venta.cantidad);
                total += (venta.precio * venta.cantidad);
            }
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";
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
                    //txtProducto.Text = result;
                    //txtProductoEntry = result;
                    //txtUPC.Text = result;
                    try
                    {
                        txtUPC.Text = productosEntryDic[result];
                        UPCG = result;
                    }
                    catch (Exception ex) {
                        await DisplayAlert("Error","Producto no existente","OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", "Error al leer código de barras intentelo nuevamente", "OK");
            }
        }

        async void onAgregar(object sender, EventArgs args) {
            
            if (txtCodigoMascota.Text == "" || txtCodigoMascota.Text == null)
            {
                await DisplayAlert("Error", "Ingresa un código de mascota", "Ok");
                return;
                
            }/*else if (txtProducto.Text == "" || txtProducto.Text == null) {
                await DisplayAlert("Error", "Ingrese nombre o código del producto", "Ok");
                return;
                
            }*/ else if (txtMonto.Text == "" || txtMonto.Text == null) {
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
            UPCEncontrado = false;
            foreach (Promocion promo in productosEntry) {
                if (promo.nombre == txtUPC.Text || promo.UPC == txtUPC.Text) {
                    UPCG = promo.UPC;
                    UPCEncontrado = true;
                }
            }

            DataTable productosEncontrados = DependencyService.Get<IWebService>().getProducto_Busca(-1, UPCG);

            DataTable serviciosEncontrados = DependencyService.Get<IWebService>().getServicio_Busca(-1);

            if (!DependencyService.Get<IWebService>().Codigo_Valida) {
                await DisplayAlert("Error", "Código de mascota invalido", "Ok");
                return;
            }
            else if (UPCEncontrado && pkrTipo.SelectedIndex == 0)
            {

                if (productosEncontrados.Rows.Count <= 0) {
                    return;
                }

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
                    puntos += (venta.puntos * venta.cantidad);
                    total += (venta.precio * venta.cantidad);
                }
                lblTotal.Text = "$ "+total.ToString() + " MXN";
                lblPuntos.Text = puntos.ToString() + " PTS";
                codigoMascota = txtCodigoMascota.Text;
                await DisplayAlert("Correcto", "Se agregó correctamente", "Ok");
                txtUPC.Text = "";
                txtMonto.Text = "";
                //txtCodigoMascota.Text = "";
            }else if (pkrTipo.SelectedIndex == 1)
            {

                if (serviciosEncontrados.Rows.Count <= 0) {
                    return;
                }

                string productName = "";
                Double productPoints = 0.0;

                foreach (DataRow dr in serviciosEncontrados.Rows)
                {
                    if (dr["Name"].ToString() == txtUPC.Text)
                    {
                        productName = dr["Name"].ToString();
                        productPoints = Convert.ToDouble(dr["Points"].ToString());
                    }
                }
                idItemPointer += 1;
                listaCarrito.Add(new Venta() { idItem = idItemPointer, cantidad = 1, nombre = productName, precio = Convert.ToDouble(txtMonto.Text), puntos = productPoints });
                total = 0;
                puntos = 0;
                foreach (Venta venta in listaCarrito)
                {
                    puntos += venta.puntos;
                    total += venta.precio;
                }
                lblTotal.Text = "$ " + total.ToString() + " MXN";
                lblPuntos.Text = puntos.ToString() + " PTS";
                codigoMascota = txtCodigoMascota.Text;
                await DisplayAlert("Correcto", "Se agregó correctamente", "Ok");
                txtUPC.Text = "";
                txtMonto.Text = "";
                //txtCodigoMascota.Text = "";
            }else {
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


        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                
                sender.ItemsSource = filtrar(sender.Text);
            }
        }

        private void AutoSuggestBox_TextChangedCant(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing, 
            // otherwise assume the value got filled in by TextMemberPath 
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                List<int> cantidades = new List<int>();
                cantidades.Add(1);
                cantidades.Add(2);
                cantidades.Add(3);
                cantidades.Add(4);
                cantidades.Add(5);
                cantidades.Add(6);
                cantidades.Add(7);
                cantidades.Add(8);
                cantidades.Add(9);
                sender.ItemsSource = cantidades;
            }
        }


        public List<String> filtrar(string texto) {

            List<String> listaFiltrada = new List<String>();

            foreach (Promocion producto in productosEntry) {
                if (producto.nombre.ToUpper().Contains(texto.ToUpper())) {
                    listaFiltrada.Add(producto.nombre);
                }
            }

            return listaFiltrada;
        }

        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            //sender.Text = args.SelectedItem.ToString();
        }

        private void AutoSuggestBox_SuggestionChosenCant(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
            //sender.Text = args.SelectedItem.ToString();
        }

        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }

        private void AutoSuggestBox_QuerySubmittedCant(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // User hit Enter from the search box. Use args.QueryText to determine what to do.
            }
        }

    }
}
