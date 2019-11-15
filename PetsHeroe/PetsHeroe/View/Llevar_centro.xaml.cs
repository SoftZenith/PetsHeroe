using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Llevar_centro : ContentPage
    {

        private string codigo = "";
        DataTable lista_CAM = new DataTable();
        DataTable estados = new DataTable();
        DataTable ciudades = new DataTable();
        double latitud = -1, longitud = -1;
        Location currentlocation;
        bool locationGrant = false;
        Picker picker, pickerC;
        int idEstado = -1, idCiudad = -1;
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Llevar_centro(string codigo)
        {
            this.codigo = codigo;
            InitializeComponent();
            _ = Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMilliseconds(500), null, false);
            _ = getCurrentLocation();
            _ = getPermisoLocation();

            if (!locationGrant)
            {
                picker = new Picker()
                {
                    Margin = new Thickness(8, 4, 8, 0),
                    Title = "Estado",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                pickerC = new Picker()
                {
                    Margin = new Thickness(0, 4, 8, 0),
                    Title = "Ciudad",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };
                gridContenedor.Children.Add(picker, 0, 0);
                gridContenedor.Children.Add(pickerC, 5, 0);
                Grid.SetColumnSpan(picker, 5);
                Grid.SetColumnSpan(pickerC, 5);
            }
            else
            {
                Grid.SetRow(frmMapa, 0);
                Grid.SetRowSpan(frmMapa, 5);
            }

            try
            {
                if (!locationGrant) //if hasn´t location permissions
                {

                    DependencyService.Get<IWebService>().getEstado_Busca(); //call to get states list
                    estados = DependencyService.Get<IWebService>().Estado_Busca; //get DataTable with states list

                    estadoDic.Clear();
                    picker.Items.Clear();
                    foreach (DataRow dr in estados.Rows)
                    {
                        picker.Items.Add(dr["Name"].ToString());
                        estadoDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDState"]));
                    }

                    picker.SelectedIndexChanged += (object sender, EventArgs args) => {

                        idEstado = estadoDic[picker.SelectedItem.ToString()];

                        DependencyService.Get<IWebService>().getCiudad_Busca(idEstado); //call to get cities list
                        ciudades = DependencyService.Get<IWebService>().Ciudad_Busca; //get DataTable with cities list

                        ciudadDic.Clear();
                        pickerC.Items.Clear();
                        foreach (DataRow dr in ciudades.Rows)
                        {
                            pickerC.Items.Add(dr["Name"].ToString());
                            ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                        }

                    };

                }
                try
                {
                    DependencyService.Get<IWebService>().getCAM_busca(currentlocation.Latitude, currentlocation.Longitude, 50); //call to get CAMs list
                    lista_CAM = DependencyService.Get<IWebService>().CAM_Busca; //get DataTable with Cams list
                }
                catch (Exception) {
                    DependencyService.Get<IWebService>().getCAM_busca(25.691288, -100.316775, 50); //call to get CAMs list
                    lista_CAM = DependencyService.Get<IWebService>().CAM_Busca; //get DataTable with Cams list
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
                
                pinCAM.Clicked += (object sender, EventArgs e) => {
                    var pinClicked = sender as Pin;
                    DisplayAlert("CAM", "Cam seleccionado: " + pinClicked.Label.ToString(), "OK");
                    latitud = pinClicked.Position.Latitude;
                    longitud = pinClicked.Position.Longitude;
                };
                mapLlevarCentro.Pins.Add(pinCAM);
                //listaPins.Add(pinCAM);
            }
            try
            {
                mapLlevarCentro.MoveToRegion(new MapSpan(new Position(currentlocation.Latitude, currentlocation.Longitude), 0.15, 0.15));
            }
            catch (Exception) {
                mapLlevarCentro.MoveToRegion(new MapSpan(new Position(25.691288, -100.316775), 0.15, 0.15));
            }

        }

        async void onDejarMascota(object sender, EventArgs args) {
            if (txtNotas.Text == "") {
                await DisplayAlert("Error","Agregar alguna nota","OK");
                return;
            }else if(latitud == -1 || longitud == -1){
                await DisplayAlert("Error","Selecciona un CAM","OK");
                return;
            }

            bool status = DependencyService.Get<IWebService>().setEntrega_CAM(codigo, txtNotas.Text, currentlocation.Longitude, currentlocation.Latitude);

            if (status) {

                await DisplayAlert("OK","Se envio un mensaje al dueño","OK");
                if (!Preferences.Get("logged", false, "usuarioLogeado")) { await Navigation.PushAsync(new MainPage()); }
                if (Preferences.Get("userType", 0, "tipoUsuario") == 1) { await Navigation.PushAsync(new Menu_dueno(1)); }
                if (Preferences.Get("userType", 0, "tipoUsuario") == 2) { await Navigation.PushAsync(new Menu_veterinario(3)); }
            }
            else
            {
                await DisplayAlert("Error","Hubo un error al enviar tu mensaje","OK");
            }

        }

        private async Task getCurrentLocation() {
            try {
                var geoLocation = await Plugin.Geolocator.CrossGeolocator.Current.GetLastKnownLocationAsync();
                if (geoLocation != null)
                {
                    currentlocation = new Location(geoLocation.Latitude, geoLocation.Longitude);
                }
                else {
                    currentlocation = new Location(25.691288, -100.316775);
                }
            }catch(Exception ex) {
                Console.WriteLine("Error al obtener location: "+ex);
                currentlocation = new Location(25.691288, -100.316775);
            }
        }

        private async Task getPermisoLocation()
        {
            locationGrant = await DependencyService.Get<IWebService>().getPermisoLocation();
        }
    }
}
