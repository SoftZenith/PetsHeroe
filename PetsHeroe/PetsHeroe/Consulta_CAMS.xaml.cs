using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Consulta_CAMS : ContentPage
    {
        public Consulta_CAMS()
        {
            InitializeComponent();
            CAM lista = new CAM();
            DataTable lista_CAM = new DataTable();
            lsvCAMS.ItemsSource = lista.getCAMS();
            mapCAMS.MoveToRegion(new MapSpan(new Position(25.708742, -100.809230), 0.5, 0.5));

            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IAndroid>().getCAM_busca(25.708742, -100.344950, 100.0);
                    lista_CAM = DependencyService.Get<IAndroid>().CAM_Busca;
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    DependencyService.Get<IIOS>().getCAM_busca(25.708742, -100.344950, 100.0);
                    lista_CAM = DependencyService.Get<IIOS>().CAM_Busca;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.ToString());
            }

            List<Pin> listaPins = new List<Pin>();

            foreach (DataRow dr in lista_CAM.Rows)
            {
                Pin pinCAM = new Pin()
                {
                    Type = PinType.Place,
                    Label = dr["BusinessName"].ToString(),
                    Position = new Position(Convert.ToDouble(dr["GeoLat"].ToString()), Convert.ToDouble(dr["GeoLon"].ToString()))
                };
                /*
                pinCAM.Clicked += async (sender, e) => {
                    await DisplayAlert("Test", "Test", "test");
                };*/

                pinCAM.Clicked += (object sender, EventArgs e) => {
                    var pinClicked = sender as Pin;
                    DisplayAlert("CAM", "Coordenadas: " + pinClicked.Position.Latitude.ToString(), "OK");
                };
                mapCAMS.Pins.Add(pinCAM);
                //listaPins.Add(pinCAM);
            }

        }
    }
}