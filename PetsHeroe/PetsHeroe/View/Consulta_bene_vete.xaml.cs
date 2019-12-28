using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
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
    public partial class Consulta_bene_vete : TabbedPage
    {
        ObservableCollection<Promocion> listaProductos = new ObservableCollection<Promocion>();
        public ObservableCollection<Promocion> ListaProductos { get { return listaProductos; } set { listaProductos = value; } }
        ObservableCollection<Promocion> listaServicios = new ObservableCollection<Promocion>();
        public ObservableCollection<Promocion> ListaServicios { get { return listaServicios; } set { listaServicios = value; } }
        ObservableCollection<Promocion> listaProductosCompleto = new ObservableCollection<Promocion>();
        ObservableCollection<Promocion> listaServiciosCompleto = new ObservableCollection<Promocion>();
        private int tipoBusqueda = -1, idMiembro = -1, puntos = 0;
        private string nombre = "", correo = "", codigo = "", usuario = "";
        private DataTable clientes = new DataTable();
        private Dictionary<string, int> MiembroDic = new Dictionary<string, int>();
        private List<Dueno> resultados = new List<Dueno>();
        private DataTable promocionesDueno = new DataTable();
        private DataTable promoProductosVet = new DataTable();
        private DataTable promoServiciosVet = new DataTable();
        IQRScanning scanningDepen;

        public Consulta_bene_vete()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }
            InitializeComponent();


            lsvProductos.RefreshCommand = new Command(() => {
                lsvProductos.IsRefreshing = true;
                listaProductosCompleto.Clear();
                listaProductos.Clear();
                promoProductosVet.Clear();
                promoProductosVet = DependencyService.Get<IWebService>().getPromoProductos_Busca(Preferences.Get("idAsociado", -1));
                listaProductosCompleto = dataTableToListProductos();
                lsvProductos.ItemsSource = listaProductos;
                lsvProductos.IsRefreshing = false;
            });

            lsvServicios.RefreshCommand = new Command(() => {
                lsvServicios.IsRefreshing = true;
                listaServiciosCompleto.Clear();
                ListaServicios.Clear();
                promoServiciosVet.Clear();
                promoServiciosVet = DependencyService.Get<IWebService>().getPromoServicios_Busca(Preferences.Get("idAsociado", -1));
                listaServiciosCompleto = dataTableToListServicios();
                lsvServicios.ItemsSource = listaServicios;
                lsvServicios.IsRefreshing = false;
            });

            tipoBusqueda = 2;
            pkrBuscarPor.SelectedIndex = tipoBusqueda;
            scanningDepen = DependencyService.Get<IQRScanning>();
            try
            {
                promoProductosVet = DependencyService.Get<IWebService>().getPromoProductos_Busca(Preferences.Get("idAsociado", -1));
                promoServiciosVet = DependencyService.Get<IWebService>().getPromoServicios_Busca(Preferences.Get("idAsociado", -1));

                listaProductosCompleto = dataTableToListProductos();
                lsvProductos.ItemsSource = listaProductos;
                //ListaProductos = dataTableToListProductos();
                listaServiciosCompleto = dataTableToListServicios();
                lsvServicios.ItemsSource = listaServicios;

            }
            catch (Exception ex) {
                promoProductosVet = null;
                promoServiciosVet = null;
            }

            pkrBuscarPor.SelectedIndexChanged += (object sender, EventArgs args) =>
            {
                tipoBusqueda = pkrBuscarPor.SelectedIndex;
            };

            lsvResultados.ItemSelected += (object sender, SelectedItemChangedEventArgs args) => {

                //DisplayAlert("OK","Se selecciono elemento de la lista","Ok");
                try
                {
                    var position = args.SelectedItemIndex;
                    var item = resultados[position] as Dueno;
                    idMiembro = Convert.ToInt32(item.idDueno);
                    usuario = item.nombre;


                    promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);

                    foreach (DataRow dr in promocionesDueno.Rows)
                    {
                        if (Convert.ToBoolean(dr["IsPoints"]))
                        {
                            puntos = Convert.ToInt32(dr["PointsActive"]);
                        }
                    }

                    txtNombre.IsVisible = true;
                    txtPuntos.IsVisible = true;
                    txtNombre.Text = " Nombre: " + usuario;
                    txtPuntos.Text = " Puntos: " + puntos;
                    puntos = 0;
                    lsvServiciosDueno.IsVisible = true;
                    lsvServiciosDueno.ItemsSource = dataTableToListServicioDueno();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: "+ex);
                    DisplayAlert("Error", "Hubo un problema al realizar la busqueda", "Ok");
                }

            };

            txtBuscarLsv.TextChanged += TxtBuscarLsv_TextChanged;
            txtBuscarServicios.TextChanged += TxtBuscarServicios_TextChanged;
        }

        private List<Promocion> dataTableToListServicioDueno()
        {

            List<Promocion> promociones = new List<Promocion>();

            foreach (DataRow dr in promocionesDueno.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    idPromocion = Convert.ToInt32(dr["ID"]),
                    partner = dr["Partner"].ToString(),
                    mascota = dr["Pet"].ToString(),
                    descripcion = dr["Descrip"].ToString(),
                    puntos = Convert.ToInt32(dr["PointsActive"]),
                    compra = Convert.ToInt32(dr["UnitsTotal"]),
                    gratis = Convert.ToInt32(dr["UnitsBought"]),
                    vigencia = dr["Vigency"].ToString(),
                    esDineroElectr = Convert.ToBoolean(dr["IsPoints"])
                };

                if (!promoTemp.esDineroElectr)
                {
                    promociones.Add(promoTemp);
                }
            }

            return promociones;

        }

        protected override void OnAppearing()
        {
            lsvProductos.BeginRefresh();
            lsvServicios.BeginRefresh();
            base.OnAppearing();
        }

        private void TxtBuscarServicios_TextChanged(object sender, TextChangedEventArgs e)
        {
            listaServicios = dataTableToListServicios(txtBuscarServicios.Text);
            lsvServicios.ItemsSource = listaServicios;
        }

        private void TxtBuscarLsv_TextChanged(object sender, TextChangedEventArgs e)
        {
            listaProductos = dataTableToListProductos(txtBuscarLsv.Text);
            lsvProductos.ItemsSource = listaProductos;
        }

        public void onBuscar(object sender, EventArgs args) {
            resultados.Clear();
            MiembroDic.Clear();
            txtNombre.IsVisible = false;
            txtPuntos.IsVisible = false;
            if (tipoBusqueda < 0) {
                DisplayAlert("Error", "Seleccionar el tipo de busqueda", "Ok");
                return;
            } else if (txtBuscar.Text is null || txtBuscar.Text.ToString() == "") {
                DisplayAlert("Error","Ingresa lo que deseas buscar","OK");
                return;
            }

            switch (tipoBusqueda)
            {
                case 0:
                    codigo = txtBuscar.Text;
                    correo = "";
                    nombre = "";
                    break;
                case 1:
                    correo = txtBuscar.Text;
                    codigo = "";
                    nombre = "";
                    break;
                case 2:
                    nombre = txtBuscar.Text;
                    correo = "";
                    codigo = "";
                    break;
            }

            try
            {
                var current = Connectivity.NetworkAccess;

                if (current != NetworkAccess.Internet)
                {
                    DisplayAlert("Ok","No estas conectado a internet","Ok");
                    return;
                }

                DependencyService.Get<IWebService>().getClientes_Busca(codigo, correo, nombre);
                clientes = DependencyService.Get<IWebService>().Cliente_Busca;

                txtNombre.Text = " Nombre: " + usuario;
                txtPuntos.Text = " Puntos: " + puntos;

                if (clientes.Rows.Count <= 0) {
                    lsvResultados.ItemsSource = null;
                    DisplayAlert("Error","No se encontro ningún cliente","Ok");
                    frmResultados.IsVisible = false;
                    frmHistorial.IsVisible = false;
                    lsvResultados.IsVisible = false;
                    lsvServiciosDueno.IsVisible = false;
                    return;
                }

                if (clientes.Rows.Count == 1)
                {
                    foreach (DataRow dr in clientes.Rows)
                    {
                        idMiembro = Convert.ToInt32(dr["IDMember"]);
                        usuario = dr["FullName"].ToString();
                    }
                    lsvResultados.ItemsSource = null;
                    DisplayAlert("Encontrado", "Un resultado de la busqueda", "Ok");
                    txtNombre.IsVisible = true;
                    txtPuntos.IsVisible = true;
                    frmResultados.IsVisible = false;
                    lsvResultados.IsVisible = false;
                    frmHistorial.IsVisible = true;
                    lsvServiciosDueno.IsVisible = true;
                }
                else {
                    MiembroDic.Clear();
                    lsvResultados.ItemsSource = null;
                    foreach (DataRow dr in clientes.Rows) {
                        MiembroDic.Add(dr["FullName"].ToString(), Convert.ToInt32(dr["IDMember"]));
                        resultados.Add(new Dueno() {
                            idDueno = dr["IDMember"].ToString(),
                            nombre = dr["FullName"].ToString(),
                            mascostaCodigo = dr["MemberCode"].ToString(),
                            correo = dr["EMail"].ToString()
                        });
                    }
                    frmHistorial.IsVisible = true;
                    frmResultados.IsVisible = true;
                    lsvResultados.IsVisible = true;
                    lsvResultados.ItemsSource = resultados;

                    DisplayAlert("Selecciona","Más de un resultado selecciona uno","Ok");
                    return;
                }

                promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);

                if (promocionesDueno.Rows.Count <= 0)
                {
                    frmHistorial.IsVisible = false;
                    lsvServiciosDueno.IsVisible = false;
                }
                else {
                    frmHistorial.IsVisible = true;
                    lsvServiciosDueno.IsVisible = true;
                }

                foreach (DataRow dr in promocionesDueno.Rows) {
                    if (Convert.ToBoolean(dr["IsPoints"]))
                    {
                        puntos = Convert.ToInt32(dr["PointsActive"]);
                    }
                }

                lsvServiciosDueno.ItemsSource = dataTableToListServicioDueno();

                txtNombre.Text = " Nombre: " + usuario;
                txtPuntos.Text = " Puntos: " + puntos;
                puntos = 0; 

            }
            catch(Exception ex) {
                DisplayAlert("Error","Hubo un error al realizar la busqueda","OK");
                return;
            }

            //DisplayAlert("OK","Puntos: "+puntos,"OK");

        }

        public async void onFiltrar(object sender, EventArgs args) {

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
                    txtBuscarLsv.Text = convertUPCtoName(result);
                }
            }
            catch (Exception ex)
            {
                txtBuscarLsv.Text = "";
            }
        }

        private ObservableCollection<Promocion> dataTableToListProductos()
        {

            ObservableCollection<Promocion> promociones = new ObservableCollection<Promocion>();

            foreach (DataRow dr in promoProductosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    idPromocion = Convert.ToInt32(dr["IDPartnerProduct"].ToString()),
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    tipo = dr["ProductType"].ToString(),
                    marca = dr["Brand"].ToString(),
                    producto = dr["Product"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    puntos = Convert.ToDouble(dr["Points"]),
                    UPC = dr["UPC"].ToString()
                };
                listaProductos.Add(promoTemp);
                promociones.Add(promoTemp);

            }

            return promociones;
        }

        private ObservableCollection<Promocion> dataTableToListProductos(string texto)
        {

            ObservableCollection<Promocion> promociones = new ObservableCollection<Promocion>();

            foreach (DataRow dr in promoProductosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    tipo = dr["ProductType"].ToString(),
                    marca = dr["Brand"].ToString(),
                    producto = dr["Product"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    puntos = Convert.ToDouble(dr["Points"]),
                    UPC = dr["UPC"].ToString()
                };

                if (promoTemp.nombre.ToUpper().Contains(texto.ToUpper())) {
                    promociones.Add(promoTemp);
                }
            }
            return promociones;
        }

        private string convertUPCtoName(string UPC) {

            foreach (DataRow dr in promoProductosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    tipo = dr["ProductType"].ToString(),
                    marca = dr["Brand"].ToString(),
                    producto = dr["Product"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    puntos = Convert.ToDouble(dr["Points"]),
                    UPC = dr["UPC"].ToString()
                };

                if (promoTemp.UPC == UPC)
                {
                    return promoTemp.nombre;
                }
            }

            return "";

        }

        private ObservableCollection<Promocion> dataTableToListServicios()
        {

            ObservableCollection<Promocion> promociones = new ObservableCollection<Promocion>();

            foreach (DataRow dr in promoServiciosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    idPromocion = Convert.ToInt32(dr["IDPartnerService"].ToString()),
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    tipo = dr["PetType"].ToString(),
                    producto = dr["ServiceType"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    compra = Convert.ToInt32(dr["Units"]),
                    PartnerPrice = Convert.ToDouble(dr["PartnerPrice"])
                };
                listaServicios.Add(promoTemp);
                promociones.Add(promoTemp);

            }

            return promociones;
        }

        private ObservableCollection<Promocion> dataTableToListServicios(string texto)
        {

            ObservableCollection<Promocion> promociones = new ObservableCollection<Promocion>();

            foreach (DataRow dr in promoServiciosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    tipo = dr["PetType"].ToString(),
                    producto = dr["ServiceType"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    compra = Convert.ToInt32(dr["Units"]),
                    PartnerPrice = Convert.ToDouble(dr["PartnerPrice"])
                };

                if (promoTemp.nombre.ToUpper().Contains(texto.ToUpper())) {
                    listaServicios.Add(promoTemp);
                    promociones.Add(promoTemp);
                }

            }

            return promociones;
        }

        public void onAgregarPromo(object sender, EventArgs args) {
            Navigation.PushAsync(new Nuevo_Servicio_Promo(false, null));
        }

        public void onAgregarPromoPrd(object sender, EventArgs args) {
            Navigation.PushAsync(new Nuevo_Producto_Promo(false, null));
        }

        public void ProductoSelectedEdit(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            Promocion promocion = button.CommandParameter as Promocion;
            Navigation.PushAsync(new Nuevo_Producto_Promo(true, promocion));
        }

        public void ProductoSelectedDelete(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            int idPromocion = Convert.ToInt32(button.CommandParameter);

            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Eliminar", "¿Eliminar promoción?", "Si", "No");
                if (result)
                {
                    Retorno status = DependencyService.Get<IWebService>().promoProducto_Eliminar(idPromocion);
                    if (status.Resultado) {
                        lsvProductos.IsRefreshing = true;
                        listaProductosCompleto.Clear();
                        listaProductos.Clear();
                        promoProductosVet.Clear();
                        promoProductosVet = DependencyService.Get<IWebService>().getPromoProductos_Busca(Preferences.Get("idAsociado", -1));
                        listaProductosCompleto = dataTableToListProductos();
                        lsvProductos.ItemsSource = listaProductos;
                        lsvProductos.IsRefreshing = false;
                        await DisplayAlert("Eliminado","Se elimino correctamente","Ok");
                    }
                    else {
                        await DisplayAlert("Error", status.Mensaje, "OK");
                    }
                }
            });
        }

        public void ServicioSelectedEdit(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            Promocion promocion = button.CommandParameter as Promocion;
            Navigation.PushAsync(new Nuevo_Servicio_Promo(true, promocion));
        }

        public void ServicioSelectedDelete(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            //int idPromocion = Convert.ToInt32(button.CommandParameter);
            Promocion promocion = button.CommandParameter as Promocion;

            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Eliminar", "¿Eliminar promoción?", "Si", "No");
                if (result)
                {
                    Retorno status = DependencyService.Get<IWebService>().promoServicio_Eliminar(promocion.idPromocion);
                    if (status.Resultado)
                    {
                        lsvServicios.IsRefreshing = true;
                        listaServiciosCompleto.Clear();
                        ListaServicios.Clear();
                        promoServiciosVet.Clear();
                        promoServiciosVet = DependencyService.Get<IWebService>().getPromoServicios_Busca(Preferences.Get("idAsociado", -1));
                        listaServiciosCompleto = dataTableToListServicios();
                        lsvServicios.ItemsSource = listaServicios;
                        lsvServicios.IsRefreshing = false;
                        await DisplayAlert("Eliminado", "Se elimino correctamente", "Ok");
                    }
                    else
                    {
                        await DisplayAlert("Error", status.Mensaje, "OK");
                    }
                }
            });
        }


    }
}
