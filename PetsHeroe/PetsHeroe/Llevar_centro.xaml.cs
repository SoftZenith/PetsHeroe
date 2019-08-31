using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Llevar_centro : ContentPage
    {

        DataTable lista_CAM = new DataTable();
        Location currentlocation;

        public Llevar_centro(string codigo)
        {
            
            InitializeComponent();
            getCurrentLocation();
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
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex.ToString());
            }

            List <Pin> listaPins = new List<Pin>();
            
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
                mapLlevarCentro.Pins.Add(pinCAM);
                //listaPins.Add(pinCAM);
            }

            /*
            var pinVet = new CAMPin() {
                Position = new Position(25.708742, -100.344950)
            };

            var pin = new Pin() {
                Position = new Position(21.512188, -104.889230),
                Label = "Dotech Tepic",
                Address = "San Luis Nte 297"
            };
            
            pin.Clicked += (object sender, EventArgs e) => {
                var pinClicked = sender as CAMPin;
                DisplayAlert("CAM", "Coordenadas: " + pinClicked.Position.Latitude.ToString(), "OK");
            };*/

            /*
            foreach (var pinMap in listaPins)
            {
                pinMap.Clicked += (object sender, EventArgs e) => {
                    var pinClicked = sender as Pin;
                    DisplayAlert("CAM", "Coordenadas: " + pinClicked.Position.Latitude.ToString(), "OK");
                };
                mapLlevarCentro.Pins.Add(pinMap);
            }*/

            mapLlevarCentro.MoveToRegion(new MapSpan(new Position(25.8494, -100.3523), 0.5, 0.5));

        }

        async void getCurrentLocation() {
            currentlocation = await Geolocation.GetLastKnownLocationAsync();
        }
    }
}
