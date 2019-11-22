﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
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
        ObservableCollection<Promocion> listaProductosCompleto = new ObservableCollection<Promocion>();
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

            InitializeComponent();
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
                lsvServicios.ItemsSource = dataTableToListServicios();

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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: "+ex);
                    DisplayAlert("Error", "Hubo un problema al realizar la busqueda", "Ok");
                }

            };

            txtBuscarLsv.TextChanged += TxtBuscarLsv_TextChanged;
        }

        private void TxtBuscarLsv_TextChanged(object sender, TextChangedEventArgs e)
        {
            listaProductos = dataTableToListProductos(txtBuscarLsv.Text);
            lsvProductos.ItemsSource = listaProductos;
        }

        public void onBuscar(object sender, EventArgs args) {
            resultados.Clear();
            MiembroDic.Clear();
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
                DependencyService.Get<IWebService>().getClientes_Busca(codigo, correo, nombre);
                clientes = DependencyService.Get<IWebService>().Cliente_Busca;

                txtNombre.Text = " Nombre: " + usuario;
                txtPuntos.Text = " Puntos: " + puntos;

                if (clientes.Rows.Count <= 0) {
                    lsvResultados.ItemsSource = null;
                    DisplayAlert("Error","No se encontro ningún cliente","Ok");
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

                    lsvResultados.ItemsSource = resultados;

                    DisplayAlert("Selecciona","Más de un resultado selecciona uno","Ok");
                    return;
                }

                promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);

                foreach (DataRow dr in promocionesDueno.Rows) {
                    if (Convert.ToBoolean(dr["IsPoints"]))
                    {
                        puntos = Convert.ToInt32(dr["PointsActive"]);
                    }
                }

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

                if (promoTemp.nombre.Contains(texto)) {
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

        private List<Promocion> dataTableToListServicios()
        {

            List<Promocion> promociones = new List<Promocion>();

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

                promociones.Add(promoTemp);

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
            int idPromocion = Convert.ToInt32(button.CommandParameter);

            Device.BeginInvokeOnMainThread(async () => {
                var result = await this.DisplayAlert("Eliminar", "¿Eliminar promoción?", "Si", "No");
                if (result)
                {

                }
            });
        }

    }
}
