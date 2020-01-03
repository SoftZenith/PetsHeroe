using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Tomar_Nota : ContentPage
    {

        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Regex PhoneRegex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");

        private int idEstado = -1;
        private int idCiudad = -1;
        private string codigo_pre = "";
        private string nombre = "";
        private string correo = "";
        private string telefono = "";
        private string localizacion = "";
        private string notas = "";
        private double latitud = -1;
        private double longitud = -1;
        private List<String> listaCiudades = new List<string>();
        //Dictionarios para guardar nombre - id
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Tomar_Nota(string codigo)
        {
            InitializeComponent();
            codigo_pre = codigo;

            DataTable estados = new DataTable();

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
            pkrMunicipio.SelectedIndexChanged += pkrMunicipioSeleccionado;
        }//Tomar_Nota

        public void pkrMunicipioSeleccionado(object sender, EventArgs e)
        {
            try
            {
                idCiudad = ciudadDic[pkrMunicipio.SelectedItem.ToString()];
            }
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex);
            }
            //DisplayAlert("Ciudad seleccionada", "Ciudad: "+idCiudad, "Ok");
        }

        public void pkrEstadoSeleccionado(object sender, EventArgs e)
        {
            idEstado = estadoDic[pkrEstado.SelectedItem.ToString()];
            try
            {
                cargarCiudades(idEstado);
            }
            catch (Exception) {
                
            }

        }

        public void cargarCiudades(int idEstadoP) {
            try
            {
                DataTable ciudades = new DataTable();
                ciudadDic.Clear();
                pkrMunicipio.Items.Clear();
                //listaCiudades.Clear();
                DependencyService.Get<IWebService>().getCiudad_Busca(idEstadoP);
                ciudades = DependencyService.Get<IWebService>().Ciudad_Busca;

                foreach (DataRow dr in ciudades.Rows)
                {
                    pkrMunicipio.Items.Add(dr["Name"].ToString());
                    listaCiudades.Add(dr["Name"].ToString());
                    ciudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                }
                //pkrMunicipio.ItemsSource = listaCiudades;
                //pkrMunicipio.SelectedIndexChanged += PkrMunicipio_SelectedIndexChanged;

            }
            catch (Exception ex) {
                Console.WriteLine("Error: "+ex);
                cargarCiudades(idEstadoP);
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
                    await DisplayAlert("Error","Faltan campos por llenar","OK");
                    return;
                }

                if (textos.Any(item => item.Trim().Length == 0)) {
                    await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                    return;
                }

                Retorno retorno = DependencyService.Get<IWebService>().setEntrega_Localizacion(new MensajeDueno()
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

                if (retorno.Resultado)
                {
                    await DisplayAlert("OK","Se enviaron tus notas al dueño","Ok");
                    if (!Preferences.Get("logged", false, "usuarioLogeado")) { await Navigation.PushAsync(new MainPage()); }
                    if (Preferences.Get("userType", 0, "tipoUsuario") == 1) { await Navigation.PushAsync(new Menu_dueno(1)); }
                    if (Preferences.Get("userType", 0, "tipoUsuario") == 2) { await Navigation.PushAsync(new Menu_veterinario(3)); }
                }
                else {
                    await DisplayAlert("ERROR", retorno.Mensaje, "OK");
                }

            }catch (Exception ex){
                await DisplayAlert("Error","Faltan campos por llenar","OK");
                Console.WriteLine("Error: " + ex);
                return;
            }
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        public bool IsPhoneNumber(string number)
        {
            number = number.Replace("-", "");
            number = number.Replace("(", "");
            number = number.Replace(")", "");
            if (number.Length <10)
            {
                return false;
            }
            return PhoneRegex.IsMatch(number);
        }
    }
}
