using System;
using System.Collections.Generic;
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

        public Consulta_bene_dueno()
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            Mascota mascotasExisten = new Mascota();

            try
            {
                int idMiembro = Preferences.Get("idMiembro", -1);
                promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);
            }
            catch(Exception)
            {
                promocionesDueno = null;
            }

            lsvDineroElect.ItemsSource = dataTableToListDinero();
            lsvServicios.ItemsSource = dataTableToListServicio();

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

        public void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            DisplayAlert("Error", "No tienes conexión a internet", "Ok");
        }

    }
}
