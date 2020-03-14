using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Consulta_bene_dueno : TabbedPage
    {
        private DataTable promocionesDueno = new DataTable();
        private DataTable promoProductosVet = new DataTable();
        private DataTable promoServiciosVet = new DataTable();
        ObservableCollection<Promocion> promociones = new ObservableCollection<Promocion>();
        ObservableCollection<Promocion> listaProductos = new ObservableCollection<Promocion>();
        public ObservableCollection<Promocion> ListaProductos { get { return listaProductos; } set { listaProductos = value; } }
        ObservableCollection<Promocion> listaServicios = new ObservableCollection<Promocion>();
        public ObservableCollection<Promocion> ListaServicios { get { return listaServicios; } set { listaServicios = value; } }
        private int idMiembroG = -1;

        public Consulta_bene_dueno()
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            Mascota mascotasExisten = new Mascota();

            try
            {
                int idMiembro = Preferences.Get("idMiembro", -1);
                idMiembroG = idMiembro;
                promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);
            }
            catch(Exception)
            {
                promocionesDueno = null;
            }

            lsvDineroElect.ItemsSource = dataTableToListDinero();
            lsvServicios.ItemsSource = dataTableToListServicio();
            List<int> listVet = getVeterinarioList(idMiembroG);
            foreach (int asoc in listVet) {
                dataTableToListProductos(asoc);
                dataTableToListServicios(asoc);
            }
            lsvPromosVet.ItemsSource = promociones;

            lsvPromosVet.RefreshCommand = new Command(() => {
                lsvPromosVet.IsRefreshing = true;
                lsvPromosVet.ItemsSource = promociones;
                lsvPromosVet.IsRefreshing = false;
             });
        }

        private List<Promocion> dataTableToListDinero() {

            List<Promocion> promociones = new List<Promocion>();

            foreach(DataRow dr in promocionesDueno.Rows) {
                Promocion promoTemp = new Promocion() {
                    idPromocion = Convert.ToInt32(dr["ID"]),
                    partner = dr["Partner"].ToString(),
                    mascota = dr["Pet"].ToString(),
                    descripcion = dr["Descrip"].ToString(),
                    puntos = Convert.ToInt32(dr["PointsActive"]),
                    vigencia = dr["Vigency"].ToString(),
                    esDineroElectr = Convert.ToBoolean(dr["IsPoints"])
                };

                if (promoTemp.esDineroElectr)
                {
                    promociones.Add(promoTemp);
                }
            }
            return promociones;
        }

        private List<Promocion> dataTableToListServicio()
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

        public List<int> getVeterinarioList(int idMiembro)
        {

            DataTable mascotasTbl = new DataTable();

            List<int> veterinarios = new List<int>();

            bool status = DependencyService.Get<IWebService>().getMascota_Busca(idMiembro, -1);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

                foreach (DataRow dr in mascotasTbl.Rows)
                {
                    Mascota mascotaTmp = new Mascota()
                    {
                        idMascota = Convert.ToInt32(dr["IDPet"]),
                        nombre = dr["Name"].ToString(),
                        codigo = dr["Code"].ToString(),
                        idAsociado = Convert.ToInt32(dr["IDPartner"].ToString()),
                        veterinario = dr["BusinessName"].ToString()
                    };
                    if (!veterinarios.Contains(mascotaTmp.idAsociado))
                    {
                        veterinarios.Add(mascotaTmp.idAsociado);
                    }
                }
            }
            return veterinarios;
        }

        private void dataTableToListProductos(int idAsociado)
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            promoProductosVet = DependencyService.Get<IWebService>().getPromoProductos_Busca(idAsociado);

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
                    isProduct = true,
                    isService = false,
                    producto = dr["Product"].ToString(),
                    partner = dr["Partner"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    puntos = Convert.ToDouble(dr["Points"]),
                    UPC = dr["UPC"].ToString()
                };
                //listaProductos.Add(promoTemp);
                promociones.Add(promoTemp);
            }

            //return promociones;
        }

        private void dataTableToListServicios(int idAsociado)
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            promoServiciosVet = DependencyService.Get<IWebService>().getPromoServicios_Busca(idAsociado);

            foreach (DataRow dr in promoServiciosVet.Rows)
            {
                Promocion promoTemp = new Promocion()
                {
                    idPromocion = Convert.ToInt32(dr["IDPartnerService"].ToString()),
                    nombre = dr["Name"].ToString(),
                    inicia = dr["StartDate"].ToString(),
                    vigencia = dr["EndDate"].ToString(),
                    isProduct = false,
                    isService = true,
                    tipo = dr["PetType"].ToString(),
                    producto = dr["ServiceType"].ToString(),
                    precio = Convert.ToDouble(dr["RegularPrice"]),
                    compra = Convert.ToInt32(dr["Units"]),
                    PartnerPrice = Convert.ToDouble(dr["PartnerPrice"]),
                    partner = dr["Partner"].ToString()
                };
                //listaServicios.Add(promoTemp);
                promociones.Add(promoTemp);
            }

            //return promociones;
        }

    }
}
