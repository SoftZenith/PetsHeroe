using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Consulta_CAMS : ContentPage
    {

        bool locationGrant = false;
        DataTable estados = new DataTable();
        DataTable ciudades = new DataTable();
        Picker picker, pickerC;
        Location currentlocation;
        int idEstado = -1, idCiudad = -1;
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Consulta_CAMS()
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            _ = Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMilliseconds(500), null, false);
            _ = getCurrentLocation();
            _ = getPermisoLocation();

            if (!locationGrant)
            {

                picker = new Picker()
                {
                    //Margin = new Thickness(8, 4, 8, 0),
                    Title = "Estado",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                Frame frmEstado = new Frame() {
                    BorderColor = Color.FromRgb(255, 113, 0),
                    CornerRadius = 8,
                    Content = picker,
                    Padding = 0,
                    Margin = new Thickness(8,4,8,0),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };


                pickerC = new Picker()
                {
                    //Margin = new Thickness(0, 4, 8, 0),
                    Title = "Ciudad",
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                Frame frmCiudad = new Frame() {
                    BorderColor = Color.FromRgb(255, 113, 0),
                    CornerRadius = 8,
                    Content = pickerC,
                    Padding = 0,
                    Margin = new Thickness(0, 4, 8, 0),
                    HorizontalOptions = LayoutOptions.FillAndExpand
                };

                gridContenedor.Children.Add(frmEstado, 0, 0);
                gridContenedor.Children.Add(frmCiudad, 1, 0);
            } else {
                Grid.SetRow(frmMapa, 0);
                Grid.SetRowSpan(frmMapa, 4);
            }


            CAM lista = new CAM();
            DataTable lista_CAM = new DataTable();
            if(currentlocation is null)
            {
                _ = getCurrentLocation();
            }
            try
            {
                lsvCAMS.ItemsSource = lista.getCAMS(currentlocation.Latitude, currentlocation.Longitude);
                mapCAMS.MoveToRegion(new MapSpan(new Position(currentlocation.Latitude, currentlocation.Longitude), 0.15, 0.15));
            }
            catch (Exception)
            {
                lsvCAMS.ItemsSource = lista.getCAMS(25.691288, -100.316775);
                mapCAMS.MoveToRegion(new MapSpan(new Position(25.691288, -100.316775), 0.15, 0.15));
            }

            try{

                if (!locationGrant)
                {

                    DependencyService.Get<IWebService>().getEstado_Busca(1);
                    estados = DependencyService.Get<IWebService>().Estado_Busca;

                    estadoDic.Clear();
                    picker.Items.Clear();
                    foreach (DataRow dr in estados.Rows)
                    {
                        picker.Items.Add(dr["Name"].ToString());
                        estadoDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDState"]));
                    }

                    picker.SelectedIndexChanged += (object sender, EventArgs args) => {

                        idEstado = estadoDic[picker.SelectedItem.ToString()];

                        DependencyService.Get<IWebService>().getCiudad_Busca(idEstado);
                        ciudades = DependencyService.Get<IWebService>().Ciudad_Busca;

                        ciudadDic.Clear();
                        pickerC.SelectedIndex = -1;
                        pickerC.Items.Clear();
                        foreach (DataRow dr in ciudades.Rows)
                        {
                            pickerC.Items.Add(dr["Name"].ToString());
                            ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                        }
                        pickerC.SelectedIndex = 0;
                    };

                    pickerC.SelectedIndexChanged += PickerC_SelectedIndexChanged;

                }

                if (currentlocation is null) {
                    _ = getCurrentLocation();
                }
                DependencyService.Get<IWebService>().getCAM_busca(currentlocation.Latitude, currentlocation.Longitude, 50);
                lista_CAM = DependencyService.Get<IWebService>().CAM_Busca;
            }
            catch (Exception ex){
                Console.WriteLine("Error: " + ex.ToString());
            }

            List<Pin> listaPins = new List<Pin>();

            foreach (DataRow dr in lista_CAM.Rows)
            {
                Pin pinCAM = new Pin()
                {
                    Type = PinType.Place,
                    Label = dr["BusinessName"].ToString(),
                    Position = new Position(Convert.ToDouble(dr["GeoLat"].ToString()), Convert.ToDouble(dr["GeoLon"].ToString())),
                    Address = dr["Address1"].ToString()
                };
                pinCAM.Clicked += (object sender, EventArgs e) => {
                    var pinClicked = sender as Pin;
                    DisplayAlert("CAM", "Dirección: " + pinClicked.Address, "OK");
                };
                mapCAMS.Pins.Add(pinCAM);
                //listaPins.Add(pinCAM);
            }

        }


        private void PickerC_SelectedIndexChanged(object sender, EventArgs e)
        {
            idCiudad = 0;
        }

        private async Task getCurrentLocation()
        {
            try
            {
                var geoLocation = await Plugin.Geolocator.CrossGeolocator.Current.GetLastKnownLocationAsync();
                if (geoLocation != null)
                {
                    currentlocation = new Location(geoLocation.Latitude, geoLocation.Longitude);
                }
                else
                {
                    currentlocation = new Location(25.691288, -100.316775);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al obtener location: " + ex);
                currentlocation = new Location(25.691288, -100.316775);
            }
        }

        public async Task getPermisoLocation() {
            locationGrant = await DependencyService.Get<IWebService>().getPermisoLocation();
        }

        public void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            DisplayAlert("Error", "No tienes conexión a internet", "Ok");
        }

    }
}