using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Tomar_Nota : ContentPage
    {
        private DataTable estados = new DataTable();
        private DataTable ciudades = new DataTable();
        int idEstado = -1;
        int idCiudad = -1;
        private string codigo_pre = "";
        private string nombre = "";
        private string correo = "";
        private string telefono = "";
        private string localizacion = "";
        private string notas = "";
        private double latitud = -1;
        private double longitud = -1;
        //Dictionarios para guardar nombre - id
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Tomar_Nota(string codigo)
        {
            InitializeComponent();
            codigo_pre = codigo;

            DependencyService.Get<IWebService>().getEstado_Busca();
            estados = DependencyService.Get<IWebService>().Estado_Busca;

            estadoDic.Clear();
            pkrEstado.Items.Clear();
            foreach (DataRow dr in estados.Rows)
            {
                pkrEstado.Items.Add(dr["Name"].ToString());
                estadoDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDState"]));
            }

            pkrEstado.SelectedIndexChanged += pkrEstadoSeleccionado;

        }

        private void pkrEstadoSeleccionado(object sender, EventArgs e)
        {
            idEstado = estadoDic[pkrEstado.SelectedItem.ToString()];
            try
            {
                cargarCiudades(idEstado);
                /*
                ciudadDic.Clear();
                pkrMunicipio.SelectedIndex = -1;
                pkrMunicipio.Items.Clear();
                if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IAndroid>().getCiudad_Busca(idEstado);
                    ciudades = DependencyService.Get<IAndroid>().Ciudad_Busca;
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    DependencyService.Get<IIOS>().getCiudad_Busca(idEstado);
                    ciudades = DependencyService.Get<IIOS>().Ciudad_Busca;
                }
                foreach (DataRow dr in ciudades.Rows)
                {
                    pkrMunicipio.Items.Add(dr["Name"].ToString());
                    ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                }
                pkrMunicipio.SelectedIndex = 0;
                pkrMunicipio.SelectedIndexChanged += (object senderM, EventArgs args) =>
                {
                    DisplayAlert("Ciudad seleccionada", "Ciudad: ", "Ok");
                    idCiudad = ciudadDic[pkrMunicipio.SelectedItem.ToString()];
                };*/
            }
            catch (Exception) {
                /*DisplayAlert("Error", "Fallo carga ciudades", "OK");
                ciudadDic.Clear();
                pkrMunicipio.SelectedIndex = 0;
                pkrMunicipio.Items.Clear();
                if (Device.RuntimePlatform == Device.Android)
                {
                    DependencyService.Get<IAndroid>().getCiudad_Busca(idEstado);
                    ciudades = DependencyService.Get<IAndroid>().Ciudad_Busca;
                }
                else if (Device.RuntimePlatform == Device.iOS)
                {
                    DependencyService.Get<IIOS>().getCiudad_Busca(idEstado);
                    ciudades = DependencyService.Get<IIOS>().Ciudad_Busca;
                }
                foreach (DataRow dr in ciudades.Rows)
                {
                    pkrMunicipio.Items.Add(dr["Name"].ToString());
                    ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                }
                pkrMunicipio.SelectedIndex = 0;
                pkrMunicipio.SelectedIndexChanged += (object senderM, EventArgs args) =>
                {
                    DisplayAlert("Ciudad seleccionada", "Ciudad: ", "Ok");
                    idCiudad = ciudadDic[pkrMunicipio.SelectedItem.ToString()];
                };*/
            }

        }

        private void cargarCiudades(int idEstado) {
            try
            {
                ciudadDic.Clear();
                pkrMunicipio.SelectedIndex = -1;
                pkrMunicipio.Items.Clear();

                DependencyService.Get<IWebService>().getCiudad_Busca(idEstado);
                ciudades = DependencyService.Get<IWebService>().Ciudad_Busca;

                foreach (DataRow dr in ciudades.Rows)
                {
                    pkrMunicipio.Items.Add(dr["Name"].ToString());
                    ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                }
                pkrMunicipio.SelectedIndex = 0;
                pkrMunicipio.SelectedIndexChanged += (object senderM, EventArgs args) =>
                {
                    DisplayAlert("Ciudad seleccionada", "Ciudad: ", "Ok");
                    idCiudad = ciudadDic[pkrMunicipio.SelectedItem.ToString()];
                };
            }
            catch (Exception) {
                cargarCiudades(idEstado);
            }
        }

        async void onAviso(object sender, EventArgs args) {

            bool estatus = false;

            try
            {
                localizacion = txtLocalizacion.Text;
                notas = txtNotas.Text;
                nombre = txtNombre.Text;
                correo = txtCorreo.Text;
                telefono = txtTelefono.Text;

                int[] enteros = { idEstado, idCiudad };
                string[] textos = { codigo_pre, localizacion, notas };

                if (enteros.Any(item => item < 0)) {
                    await DisplayAlert("Error","Faltan campos por llenar enteros, idEstado: "+idEstado+" idCiudad: "+idCiudad,"OK");
                    return;
                }

                if (textos.Any(item => item.Trim().Length == 0)) {
                    await DisplayAlert("Error", "Faltan campos por llenar textos", "OK");
                    return;
                }

                estatus = DependencyService.Get<IWebService>().setEntrega_Localizacion(new MensajeDueno()
                {
                    codigo = codigo_pre,
                    nombre = nombre,
                    correo = correo,
                    telefono = telefono,
                    localizacion = localizacion,
                    notas = notas,
                    latitud = latitud,
                    longitud = longitud,
                    idCiudad = idCiudad
                });

                if (estatus)
                {
                    if (!Preferences.Get("logged", false, "usuarioLogeado")) { await Navigation.PushAsync(new MainPage()); }
                    if (Preferences.Get("userType", 0, "tipoUsuario") == 1) { await Navigation.PushAsync(new Menu_dueno()); }
                    if (Preferences.Get("userType", 0, "tipoUsuario") == 2) { await Navigation.PushAsync(new Menu_veterinario()); }
                }
                else {
                    await DisplayAlert("ERROR", "Algo salio mal enviando tu localización", "OK");
                }

            }catch (Exception ex){
                await DisplayAlert("Error","Faltan campos por llenar","OK");
                Console.WriteLine("Error: " + ex);
                return;
            }
        }
    }
}
