using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using dotMorten.Xamarin.Forms;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
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
        private double ahorroServicios = 0;
        private int idProducto = -1;
        private int idServicio = -1;
        private int idTicket = -1;
        private int idVenta = -1;
        private int tipo = 1;
        private Venta ventaSelected = null;
        private int idAsociado = -1;
        private int idSucursal = -1;
        private int idMascota = -1;
        private string UPCG = "";
        private DataTable productosEntryComplete;
        public Dictionary<String,String> productosEntryDic;
        public List<Promocion> productosEntry;
        public string txtProductoEntry;
        private bool UPCEncontrado = false;

        public Caja_Ventas()
        {
            InitializeComponent();
            idAsociado = Preferences.Get("idAsociado", -1);
            idSucursal = DependencyService.Get<IWebService>().getSucursal(idAsociado);
            btnScanUPC.IsEnabled = true;
            lsvCarrito.ItemsSource = listaCarrito;
            lblTotal.Text = total.ToString();
            lblPuntos.Text = puntos.ToString();
            scanningDepen = DependencyService.Get<IQRScanning>();
            pkrTipo.SelectedIndexChanged += PkrTipo_SelectedIndexChanged;
            //listaCarrito.CollectionChanged += ListaCarrito_CollectionChanged;
            productosEntryComplete = DependencyService.Get<IWebService>().getPromoProductos_Busca(idAsociado);
            productosEntryDic = new Dictionary<string, string>();
            productosEntry = new List<Promocion>();
            foreach (DataRow dr in productosEntryComplete.Rows) {
                try
                {
                    productosEntryDic.Add(dr["UPC"].ToString(), dr["Name"].ToString());
                    Promocion promoTemp = new Promocion()
                    {
                        nombre = dr["Name"].ToString(),
                        UPC = dr["UPC"].ToString(),
                        idPromocion = Convert.ToInt32(dr["IDProduct"].ToString()),
                        isProduct = true
                    };
                    productosEntry.Add(promoTemp);
                }
                catch (Exception ex) {

                }
            }
            //txtUPC.ItemsSource = productosEntry;

        }

        protected override void OnAppearing()
        {
            //lsvMascotas.BeginRefresh();
            listaCarrito.Clear();
            total = 0;
            puntos = 0;
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";

            productosEntryComplete = DependencyService.Get<IWebService>().getPromoProductos_Busca(idAsociado);
            productosEntryDic = new Dictionary<string, string>();
            productosEntry = new List<Promocion>();
            foreach (DataRow dr in productosEntryComplete.Rows)
            {
                try
                {
                    productosEntryDic.Add(dr["UPC"].ToString(), dr["Name"].ToString());
                    Promocion promoTemp = new Promocion()
                    {
                        nombre = dr["Name"].ToString(),
                        UPC = dr["UPC"].ToString(),
                        idPromocion = Convert.ToInt32(dr["IDProduct"].ToString()),
                        isProduct = true
                    };
                    productosEntry.Add(promoTemp);
                }
                catch (Exception ex)
                {

                }
            }

            base.OnAppearing();

        }

        async void LsvCarrito_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView list = sender as ListView;
            ventaSelected = list.SelectedItem as Venta;
            //await DisplayAlert("Ok", "Se selecciono elemento de la lista", "OK");
            pkrCantidad.Focus();
        }

        private void pkrCantidadSelected(object sender, EventArgs e)
        {
            int cantidadSelected = pkrCantidad.SelectedIndex;
            if (cantidadSelected == 0)
            {
                Retorno resultado = DependencyService.Get<IWebService>().ventaCancela(ventaSelected.idVenta);
                if (resultado.Resultado)
                {
                    DisplayAlert("Ok", "Se eliminó del carrito correctamente", "Ok");
                    llenarCarrito();//Metodo para refrescar carrito
                }
                else
                {
                    DisplayAlert("Error", resultado.Mensaje, "Ok");
                }
            }
            else {
                Retorno retorno = DependencyService.Get<IWebService>().ventaCambia(ventaSelected.idVenta, cantidadSelected, (decimal)ventaSelected.precio);
                if (retorno.Resultado)
                {
                    llenarCarrito();//Metodo para refrescar carrito
                }
                else
                {
                    DisplayAlert("Error", retorno.Mensaje, "Ok");
                }
            }
        }

        private void PkrTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pkrTipo.SelectedIndex == 0)
            {
                imgTipo.Source = "collar_big.png";
                btnScanUPC.IsEnabled = true;
                productosEntry.Clear();
                productosEntryDic.Clear();
                productosEntryComplete = DependencyService.Get<IWebService>().getPromoProductos_Busca(idAsociado);
                foreach (DataRow dr in productosEntryComplete.Rows)
                {
                    try
                    {
                        productosEntryDic.Add(dr["UPC"].ToString(), dr["Name"].ToString());
                        Promocion promoTemp = new Promocion()
                        {
                            nombre = dr["Name"].ToString(),
                            UPC = dr["UPC"].ToString(),
                            idPromocion = Convert.ToInt32(dr["IDProduct"].ToString()),
                            isProduct = true
                        };
                        productosEntry.Add(promoTemp);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                txtUPC.Text = "";
            }
            else {
                imgTipo.Source = "shower_big.png";
                productosEntry.Clear();
                productosEntryDic.Clear();
                btnScanUPC.IsEnabled = false;
                productosEntryComplete = DependencyService.Get<IWebService>().getPromoServicios_Busca(idAsociado);
                foreach (DataRow dr in productosEntryComplete.Rows)
                {
                    try
                    {
                        productosEntryDic.Add(dr["IDPartnerService"].ToString(), dr["Name"].ToString());
                        Promocion promoTemp = new Promocion()
                        {
                            nombre = dr["Name"].ToString(),
                            idPromocion = Convert.ToInt32(dr["IDServiceType"].ToString()),
                            isProduct = false
                        };
                        productosEntry.Add(promoTemp);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                txtUPC.Text = "";
            }
        }

        async void scanCode(object sender, EventArgs args) {

            var status = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "La app no tiene permisos para utilizar la camara", "OK");
                return;
            }

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

            var status = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "La app no tiene permisos para utilizar la camara", "OK");
                return;
            }
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

        async void onAgregar(object sender, EventArgs args)
        {

            if (txtCodigoMascota.Text == "" || txtCodigoMascota.Text == null)
            {
                await DisplayAlert("Error", "Ingresa un código de mascota", "Ok");
                return;
            }

            DependencyService.Get<IWebService>().getCodigo_Valida(txtCodigoMascota.Text);
            bool Esvalido = DependencyService.Get<IWebService>().Codigo_Valida;
            if (!Esvalido) {
                await DisplayAlert("Error", "Código de mascota invalido", "Ok");
                return;
            }
            codigoMascota = txtCodigoMascota.Text;
            idMascota = DependencyService.Get<IWebService>().getIdMascota_Busca(txtCodigoMascota.Text);

            //obtener idProducto, isProduct de productosEntry
            if (pkrTipo.SelectedIndex == 0)//Si esta seleccionado el tipo producto buscar por UPC o descripción
            {
                Promocion promocion = productosEntry.Find(x => x.nombre == txtUPC.Text || x.UPC == txtUPC.Text);
                if (promocion != null)//Promoción de producto encontrada
                {
                    DependencyService.Get<IWebService>().agregar_venta(idTicket, idMascota, idSucursal, promocion.idPromocion, -1, 1, 0, out int idTicketOut, out int ventaResult);
                    idTicket = idTicketOut;
                } else { //Promoción de producto no encontrada descripción o UPC no existe
                    //Agregar como "otro"
                    if (txtMonto.Text == "" || txtMonto.Text == null)
                    {
                        await DisplayAlert("Error", "Ingrese un monto", "Ok");
                        return;
                    }
                    try
                    {
                        Convert.ToDouble(txtMonto.Text);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "Monto invalido", "Ok");
                        return;
                    }
                    DependencyService.Get<IWebService>().agregar_venta(idTicket, idMascota, idSucursal, -1, -1, 1, Convert.ToDouble(txtMonto.Text), out int idTicketOut, out int ventaResult);
                    idTicket = idTicketOut;
                }
            }
            else { //si esta seleccionado el tipo servicio buscar por descripción de promoción
                Promocion promocion = productosEntry.Find(x => x.nombre == txtUPC.Text);
                if (promocion != null)
                { //promoción de servicio encontrada
                    DependencyService.Get<IWebService>().agregar_venta(idTicket, idMascota, idSucursal, -1, promocion.idPromocion, 1, 0, out int idTicketOut, out int ventaResult);
                    idTicket = idTicketOut;
                }
                else {//promoción de servicio no encontrada descripción no existe
                    //agregar como "otro"
                    if (txtMonto.Text == "" || txtMonto.Text == null)
                    {
                        await DisplayAlert("Error", "Ingrese un monto", "Ok");
                        return;
                    }
                    try
                    {
                        Convert.ToDouble(txtMonto.Text);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("Error", "Monto invalido", "Ok");
                        return;
                    }
                    DependencyService.Get<IWebService>().agregar_venta(idTicket, idMascota, idSucursal, -1, -1, 1, Convert.ToDouble(txtMonto.Text), out int idTicketOut, out int ventaResult);
                }
            }

            //llamar a ticketCarga
            txtUPC.Text = "";
            llenarCarrito();
            /*
            DataTable listaTicketCarga = new DataTable();
            listaTicketCarga = DependencyService.Get<IWebService>().ticketCarga(idTicket, idMascota, idSucursal);
            listaCarrito.Clear();//Limpiar carrito para refrescar con productos/servicios agregados
            foreach (DataRow dr in listaTicketCarga.Rows) {

                try
                {
                    int idServicioTmp = Convert.ToInt32(dr["IDServiceType"].ToString());
                    Venta ticket = new Venta()
                    {
                        idServicio = Convert.ToInt32(dr["IDSale"].ToString()),
                        nombre = dr["RowDesc"].ToString(),
                        precio = Convert.ToDouble(dr["PriceU"].ToString()),
                        cantidad = Convert.ToInt32(dr["Units"].ToString()),
                        puntos = Convert.ToDouble(dr["PointsGain"].ToString()),
                        actual = Convert.ToInt32(dr["UnitsToDate"].ToString()),
                        isProduct = false,
                        isService = true,
                        imagen = "shower_big.png"
                    };
                    ticket.textoMostrar = "Tienes " + ticket.actual + " de ";
                    listaCarrito.Add(ticket);
                }
                catch (Exception ex) {
                    Venta ticket = new Venta()
                    {
                        idProducto = Convert.ToInt32(dr["IDSale"].ToString()),
                        nombre = dr["RowDesc"].ToString(),
                        precio = Convert.ToDouble(dr["PriceU"].ToString()),
                        cantidad = Convert.ToInt32(dr["Units"].ToString()),
                        puntos = Convert.ToDouble(dr["PointsGain"].ToString()),
                        isProduct = true,
                        isService = false,
                        imagen = "collar_big.png"
                    };
                    ticket.textoMostrar = ticket.puntos + " PTS";
                    listaCarrito.Add(ticket);
                }
            }*/
        }

        async void onSiguiente(object sender, EventArgs args) {

            //DependencyService.Get<IWebService>().agregar_venta(-1, 113, 16, 7, -1, 2, 29.93, out int idTicketOut, out int ventaResult);

            if (listaCarrito.Count > 0){
                await Navigation.PushAsync(new Pago_Venta(idSucursal, codigoMascota, total, idTicket));
            } else {
                await DisplayAlert("Error","No hay ningún producto/servicio en tu listado de compra","Ok");
            }
        }


        private void llenarCarrito() {
            total = 0;
            puntos = 0;
            DataTable listaTicketCarga = new DataTable();
            listaTicketCarga = DependencyService.Get<IWebService>().ticketCarga(idTicket, idMascota, idSucursal);
            listaCarrito.Clear();//Limpiar carrito para refrescar con productos/servicios agregados
            foreach (DataRow dr in listaTicketCarga.Rows)
            {

                try
                {
                    int idServicioTmp = Convert.ToInt32(dr["IDServiceType"].ToString());
                    total += Convert.ToDouble(dr["TotalRow"].ToString());
                    Venta ticket = new Venta()
                    {
                        idVenta = Convert.ToInt32(dr["IDSale"].ToString()),
                        nombre = dr["RowDesc"].ToString(),
                        precio = Convert.ToDouble(dr["PriceU"].ToString()),
                        cantidad = Convert.ToInt32(dr["Units"].ToString()),
                        puntos = Convert.ToDouble(dr["PointsGain"].ToString()),
                        actual = Convert.ToInt32(dr["UnitsToDate"].ToString()),
                        total = Convert.ToInt32(dr["PromoUnits"].ToString()),
                        isProduct = false,
                        isService = true,
                        imagen = "shower_big.png"
                    };
                    ticket.textoMostrar = "Tienes " + ticket.actual + " de ";
                    listaCarrito.Add(ticket);
                }
                catch (Exception ex)
                {
                    total += Convert.ToDouble(dr["TotalRow"].ToString());
                    puntos += Convert.ToDouble(dr["PointsGain"].ToString());
                    Venta ticket = new Venta()
                    {
                        idVenta = Convert.ToInt32(dr["IDSale"].ToString()),
                        nombre = dr["RowDesc"].ToString(),
                        precio = Convert.ToDouble(dr["PriceU"].ToString()),
                        cantidad = Convert.ToInt32(dr["Units"].ToString()),
                        puntos = Convert.ToDouble(dr["PointsGain"].ToString()),
                        isProduct = true,
                        isService = false,
                        imagen = "collar_big.png"
                    };
                    ticket.textoMostrar = ticket.puntos + " PTS";
                    listaCarrito.Add(ticket);
                }
            }
            lblPuntos.Text = puntos.ToString() + "  PTS";
            lblTotal.Text = total.ToString() + " MXN";
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
