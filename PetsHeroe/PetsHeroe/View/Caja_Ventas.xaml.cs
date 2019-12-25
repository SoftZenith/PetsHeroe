using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using dotMorten.Xamarin.Forms;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
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
        private double ahorroServicios = 0;
        private int idProducto = -1;
        private int idServicio = -1;
        private int idTicket = -1;
        private int tipo = 1;
        private int idElemento = 1;
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
            productosEntryComplete = DependencyService.Get<IWebService>().getProducto_Busca(idAsociado, "");
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

        protected override void OnAppearing()
        {
            //lsvMascotas.BeginRefresh();
            listaCarrito.Clear();
            total = 0;
            puntos = 0;
            lblTotal.Text = "$ " + total.ToString() + " MXN";
            lblPuntos.Text = puntos.ToString() + " PTS";

            productosEntryComplete = DependencyService.Get<IWebService>().getProducto_Busca(idAsociado, "");
            productosEntryDic = new Dictionary<string, string>();
            productosEntry = new List<Promocion>();
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

            base.OnAppearing();

        }

        async void LsvCarrito_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView list = sender as ListView;
            Venta item = list.SelectedItem as Venta;
            idElemento = item.idItem;
            //await DisplayAlert("Ok", "Se selecciono elemento de la lista", "OK");
            pkrCantidad.Focus();
        }

        private void pkrCantidadSelected(object sender, EventArgs e)
        {
            Picker pkr = sender as Picker;

            if (pkr.SelectedIndex == 0) {
                for (int i = 0; i < listaCarrito.Count; i++)
                {
                    if (listaCarrito[i].idItem == idElemento)
                    {
                        Venta ventaTmp = new Venta
                        {
                            idItem = listaCarrito[i].idItem,
                            idProducto = listaCarrito[i].idProducto,
                            cantidad = 0,
                            nombre = listaCarrito[i].nombre,
                            precio = listaCarrito[i].precio,
                            puntos = listaCarrito[i].puntos,
                            imagen = listaCarrito[i].imagen
                        };
                        listaCarrito.RemoveAt(i);
                    }
                }
                puntos = 0;
                total = 0;
                foreach (Venta venta in listaCarrito)
                {
                    puntos += (venta.puntos * venta.cantidad);
                    total += (venta.precio * venta.cantidad);
                }
                if (ahorroServicios > 0)
                {
                    lblAhorro.IsVisible = true;
                    lblAhorroTotal.IsVisible = true;
                    lblAhorroTotal.Text = "$" + ahorroServicios + " MXN";
                }
                else
                {
                    lblAhorro.IsVisible = false;
                    lblAhorroTotal.IsVisible = false;
                }
                lblTotal.Text = "$ " + total.ToString() + " MXN";
                lblPuntos.Text = puntos.ToString() + " PTS";
                return;
            }

            int cantidadPkr = pkr.SelectedIndex;

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
                        puntos = listaCarrito[i].puntos,
                        imagen = listaCarrito[i].imagen
                    };
                    listaCarrito.RemoveAt(i);
                    listaCarrito.Insert(i, ventaTmp);
                    //listaCarrito.Add(ventaTmp);
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
                btnScanUPC.IsEnabled = true;
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
                //txtUPC.ItemsSource = filtrar("");

            }
            else {
                imgTipo.Source = "shower_big.png";
                productosEntry.Clear();
                btnScanUPC.IsEnabled = false;
                productosEntryComplete = DependencyService.Get<IWebService>().getPromoServicios_Busca(idAsociado);
                foreach (DataRow dr in productosEntryComplete.Rows)
                {
                    try
                    {
                        //productosEntryDic.Add(dr["UPC"].ToString(), dr["Product"].ToString());
                        Promocion promoTemp = new Promocion()
                        {
                            nombre = dr["ServiceType"].ToString()
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

            DataTable serviciosEncontrados = DependencyService.Get<IWebService>().getPromoServicios_Busca(idAsociado);

            DataTable mascotasTbl = new DataTable();

            bool status = DependencyService.Get<IWebService>().getIdMascota_Busca(txtCodigoMascota.Text);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

                foreach (DataRow dr in mascotasTbl.Rows)
                {
                    idMascota = Convert.ToInt32(dr["IDPet"].ToString());
                    //idMiembro = Convert.ToInt32(dr["IDMember"].ToString());
                }
            }


            //idMascota = DependencyService.Get<IWebService>().getIdMascota_Busca(txtCodigoMascota.Text);

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
                string promoNameUPC = txtUPC.Text;
                foreach (DataRow dr in productosEncontrados.Rows)
                {
                    if (dr[9].ToString() == promoNameUPC) {
                        productName = dr["Name"].ToString();
                        productPoints = Convert.ToDouble(dr["Points"].ToString());
                        idProducto = Convert.ToInt32(dr["IDProduct"].ToString());
                    }
                }
                idItemPointer += 1;
                Venta ventaTmp = new Venta() { idProducto = idProducto, idServicio = -1, idItem = idItemPointer, cantidad = 1, imagen = "collar_big.png", nombre = productName, precio = Convert.ToDouble(txtMonto.Text), puntos = productPoints };
                ventaTmp.textoMostrar = ventaTmp.puntos + "PTS";
                listaCarrito.Add(ventaTmp);
                total = 0;
                puntos = 0;
                foreach (Venta venta in listaCarrito) {
                    puntos += (venta.puntos * venta.cantidad);
                    total += (venta.precio * venta.cantidad);
                }
                lblTotal.Text = "$ " + total.ToString() + " MXN";
                lblPuntos.Text = puntos.ToString() + " PTS";
                codigoMascota = txtCodigoMascota.Text;
                await DisplayAlert("Correcto", "Se agregó correctamente", "Ok");
                txtUPC.Text = "";
                txtMonto.Text = "";
                //txtCodigoMascota.Text = "";
            } else if (pkrTipo.SelectedIndex == 1 && txtUPC.Text != "")  //Agregar servicio
            {

                if (serviciosEncontrados.Rows.Count <= 0) {
                    return;
                }

                string productName = "";
                Double productPoints = 0.0;
                int servicesTotal = 0;

                foreach (DataRow dr in serviciosEncontrados.Rows)
                {
                    if (dr["ServiceType"].ToString() == txtUPC.Text)
                    {
                        productName = dr["Name"].ToString();
                        //productPoints = Convert.ToDouble(dr["Points"].ToString());
                        idServicio = Convert.ToInt32(dr["IDServiceType"].ToString());
                        servicesTotal = Convert.ToInt32(dr["Units"].ToString());
                    }
                }
                idItemPointer += 1;
                //listaCarrito.Add(new Venta() {idServicio = idServicio, idProducto = -1, idItem = idItemPointer, cantidad = 1, imagen = "shower_big.png", nombre = productName, precio = Convert.ToDouble(txtMonto.Text), puntos = productPoints });
                //llamar a ticketCarga
                DependencyService.Get<IWebService>().agregar_venta(idTicket, idMascota, idSucursal, -1, idServicio, 1, Convert.ToDouble(txtMonto.Text), out int idTicketOut, out int ventaResulta);
                idTicket = idTicketOut;
                DataTable listaTicketCarga = new DataTable();
                listaTicketCarga = DependencyService.Get<IWebService>().ticketCarga(idTicket, idMascota, idSucursal);
                foreach (DataRow dr in listaTicketCarga.Rows) {
                    if (Convert.ToInt32(dr["Units"].ToString()) == Convert.ToInt32(dr["UnitsToDate"].ToString())+1) {
                        //ahorroServicios += Convert.ToDouble(txtMonto.Text);
                    }
                    Venta ventaTmp = new Venta()
                    {
                        idServicio = idServicio,
                        idProducto = -1,
                        idItem = idItemPointer,
                        cantidad = 1,
                        imagen = "shower_big.png",
                        nombre = dr["RowDesc"].ToString(),
                        precio = Convert.ToInt32(dr["Units"].ToString()) == Convert.ToInt32(dr["UnitsToDate"].ToString())+1 ? 0 : Convert.ToDouble(txtMonto.Text),
                        actual = Convert.ToInt32(dr["Units"].ToString()),
                        //total = Convert.ToInt32(dr["UnitsToDate"].ToString()),
                        total = servicesTotal,
                        puntos = 0
                    };
                    int tmpIdServicio = -1;
                    try
                    {
                        tmpIdServicio = Convert.ToInt16(dr[6].ToString());
                        if (tmpIdServicio == idServicio)
                        {
                            ventaTmp.textoMostrar = "Tienes " + ventaTmp.actual + " de " + ventaTmp.total;
                            listaCarrito.Add(ventaTmp);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                //
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
                txtUPC.ItemsSource = filtrar("");
                if (ahorroServicios > 0)
                {
                    lblAhorro.IsVisible = true;
                    lblAhorroTotal.IsVisible = true;
                    lblAhorroTotal.Text = "$" + ahorroServicios + " MXN";
                }
                else {
                    lblAhorro.IsVisible = false;
                    lblAhorroTotal.IsVisible = false;
                }
            } else {
                if (pkrTipo.SelectedIndex == 0)
                {

                    idItemPointer += 1;
                    Venta ventaTmp = new Venta() { idProducto = -1, idServicio = -1, idItem = idItemPointer, cantidad = 1, imagen = "other_big.png", nombre = "Otro", precio = Convert.ToDouble(txtMonto.Text), puntos = 0 };
                    ventaTmp.textoMostrar = ventaTmp.puntos + "PTS";
                    listaCarrito.Add(ventaTmp);
                    total = 0;
                    puntos = 0;
                    foreach (Venta venta in listaCarrito)
                    {
                        puntos += (venta.puntos * venta.cantidad);
                        total += (venta.precio * venta.cantidad);
                    }
                    lblTotal.Text = "$ " + total.ToString() + " MXN";
                    lblPuntos.Text = puntos.ToString() + " PTS";
                    codigoMascota = txtCodigoMascota.Text;
                    await DisplayAlert("Correcto", "Se agregó correctamente", "Ok");
                    txtUPC.Text = "";
                    txtMonto.Text = "";

                    /*Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Producto no encontrado", "¿Deseas agregarlo?", "Si", "No");
                        if (result)
                        {
                           await Navigation.PushAsync(new Nuevo_Producto_Venta());
                        }
                    });*/
                } else {

                    idItemPointer += 1;
                    Venta ventaTmp = new Venta() { idProducto = -1, idServicio = -1, idItem = idItemPointer, cantidad = 1, imagen = "other_big.png", nombre = "Otro", precio = Convert.ToDouble(txtMonto.Text), puntos = 0 };
                    ventaTmp.textoMostrar = ventaTmp.puntos + "PTS";
                    listaCarrito.Add(ventaTmp);
                    total = 0;
                    puntos = 0;
                    foreach (Venta venta in listaCarrito)
                    {
                        puntos += (venta.puntos * venta.cantidad);
                        total += (venta.precio * venta.cantidad);
                    }
                    lblTotal.Text = "$ " + total.ToString() + " MXN";
                    lblPuntos.Text = puntos.ToString() + " PTS";
                    codigoMascota = txtCodigoMascota.Text;
                    await DisplayAlert("Correcto", "Se agregó correctamente", "Ok");
                    txtUPC.Text = "";
                    txtMonto.Text = "";

                    /*Device.BeginInvokeOnMainThread(async () =>
                    {
                        var result = await this.DisplayAlert("Servicio no encontrado", "¿Deseas agregarlo?", "Si", "No");
                        if (result)
                        {
                            await Navigation.PushAsync(new Nuevo_Servicio_Venta());
                        }
                    });*/
                }
            }
        }

        async void onSiguiente(object sender, EventArgs args) {

            //DependencyService.Get<IWebService>().agregar_venta(-1, 113, 16, 7, -1, 2, 29.93, out int idTicketOut, out int ventaResult);

            if (listaCarrito.Count > 0){
                await Navigation.PushAsync(new Pago_Venta(idSucursal, codigoMascota, total, listaCarrito));
            } else {
                await DisplayAlert("Error","No hay ningún producto/servicio en tu listado de compra","Ok");
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
