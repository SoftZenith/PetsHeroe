using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PetsHeroe
{
    public partial class Consulta_CAMS : ContentPage
    {

        bool locationGrant = false;
        DataTable estados = new DataTable();
        DataTable ciudades = new DataTable();
        Picker picker, pickerC;
        int idEstado = -1, idCiudad = -1;
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Consulta_CAMS()
        {
            InitializeComponent();

            getPermisoLocation();

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
            lsvCAMS.ItemsSource = lista.getCAMS();
            mapCAMS.MoveToRegion(new MapSpan(new Position(25.708742, -100.809230), 0.5, 0.5));

            try{
                if (Device.RuntimePlatform == Device.Android)
                {

                    if (!locationGrant)
                    {

                        DependencyService.Get<IAndroid>().getEstado_Busca();
                        estados = DependencyService.Get<IAndroid>().Estado_Busca;

                        estadoDic.Clear();
                        picker.SelectedIndex = -1;
                        picker.Items.Clear();
                        foreach (DataRow dr in estados.Rows)
                        {
                            picker.Items.Add(dr["Name"].ToString());
                            estadoDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDState"]));
                        }
                        picker.SelectedIndex = 0;
                        picker.SelectedIndexChanged += (object sender, EventArgs args) => {

                            idEstado = estadoDic[picker.SelectedItem.ToString()];

                            DependencyService.Get<IAndroid>().getCiudad_Busca(idEstado);
                            ciudades = DependencyService.Get<IAndroid>().Ciudad_Busca;

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


                    DependencyService.Get<IAndroid>().getCAM_busca(25.708742, -100.344950, 100.0);
                    lista_CAM = DependencyService.Get<IAndroid>().CAM_Busca;
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {

                    if (!locationGrant)
                    {

                        DependencyService.Get<IIOS>().getEstado_Busca();
                        estados = DependencyService.Get<IIOS>().Estado_Busca;

                        estadoDic.Clear();
                        picker.Items.Clear();
                        foreach (DataRow dr in estados.Rows)
                        {
                            picker.Items.Add(dr["Name"].ToString());
                            estadoDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDState"]));
                        }

                        picker.SelectedIndexChanged += (object sender, EventArgs args) => {

                            idEstado = estadoDic[picker.SelectedItem.ToString()];

                            DependencyService.Get<IIOS>().getCiudad_Busca(idEstado);
                            ciudades = DependencyService.Get<IIOS>().Ciudad_Busca;

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


                    DependencyService.Get<IIOS>().getCAM_busca(25.708742, -100.344950, 100.0);
                    lista_CAM = DependencyService.Get<IIOS>().CAM_Busca;
                }
            }catch (Exception ex){
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
                pinCAM.Clicked += (object sender, EventArgs e) => {
                    var pinClicked = sender as Pin;
                    DisplayAlert("CAM", "Coordenadas: " + pinClicked.Position.Latitude.ToString(), "OK");
                };
                mapCAMS.Pins.Add(pinCAM);
                //listaPins.Add(pinCAM);
            }

        }


        private void PickerC_SelectedIndexChanged(object sender, EventArgs e)
        {
            idCiudad = 0;
        }

        public async void getPermisoLocation() {

            if (Device.RuntimePlatform == Device.iOS)
            {
                locationGrant = await DependencyService.Get<IIOS>().getPermisoLocation();
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                locationGrant = await DependencyService.Get<IAndroid>().getPermisoLocation();
            }

        }

    }
}