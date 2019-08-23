using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Llevar_centro : ContentPage
    {
        public Llevar_centro()
        {
            InitializeComponent();

            var pin = new Pin() {
                Position = new Position(21.512188, -104.889230),
                Label = "Dotech Tepic",
                Address = "San Luis Nte 297"
            };

            pin.Clicked += (object sender, EventArgs e) => {
                var pinClicked = sender as Pin;
                DisplayAlert("CAM", "Coordenadas: " + pinClicked.Position.Latitude.ToString(), "OK");
            };

            mapLlevarCentro.Pins.Add(pin);
            mapLlevarCentro.MoveToRegion(new MapSpan(new Position(21.512188, -104.809230), 21.5121, -104.8092));

            _ = convertirUbicacion();
        }

        async Task convertirUbicacion()
        {
            var currentlocation = await Geolocation.GetLastKnownLocationAsync();

            var locations = await Geocoding.GetPlacemarksAsync(25.681411, -100.163098);

            var location = locations?.FirstOrDefault();

            Console.WriteLine("Current Latitud: " + currentlocation.Latitude.ToString() + " Longitud: "+currentlocation.Longitude.ToString());
        }
    }
}
