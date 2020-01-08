using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetsHeroe.Services;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using PetsHeroe.Model;
using Xamarin.Forms.Xaml;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro_dueno_mascota : ContentPage
    {
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        int idTipoMascota = -1;
        int idRazaMascota = -1;
        int idColorMascota = -1;
        int idSucursal = -1;
        char sexoDuenoC = (char)0;
        char sexoMascotaC = (char)78;
        int sexoSelectedD = -1;
        int sexoSelectedM = -1;
        IQRScanning scanningDepen;
        Location currentlocation;
        bool locationGrant = false;
        //Diccionarios para guardar nombre - id
        DataTable lista_CAM = new DataTable();
        Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>(); 
        Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> CAMDic = new Dictionary<string, int>();
        public Registro_dueno_mascota()
        {
            InitializeComponent();

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }

            _ = Plugin.Geolocator.CrossGeolocator.Current.GetPositionAsync(TimeSpan.FromMilliseconds(500), null, false);
            _ = getCurrentLocation();
            _ = getPermisoLocation();
            pkrRazaMascota.IsEnabled = false;
            pkrColorMascota.IsEnabled = false;
            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;
            scanningDepen = DependencyService.Get<IQRScanning>();
            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows) {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(),Convert.ToInt32(dr[0]));
            }

            pkrTipoMascota.SelectedIndexChanged += tipoMascotaSeleccionado;

            pkrSexoMascota.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                sexoSelectedM = pkrSexoMascota.SelectedIndex;
                if (sexoSelectedM == 0)
                {
                    sexoMascotaC = (char)77;
                } else if (sexoSelectedM == 1) {
                    sexoMascotaC = (char)72;
                }
            };

            pkrSexoDueno.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                sexoSelectedD = pkrSexoDueno.SelectedIndex;
                if (sexoSelectedD == 0) {
                    sexoDuenoC = (char)77;
                } else if (sexoSelectedD == 1) {
                    sexoDuenoC = (char)70;
                }
            };

            //obtener CAMs a 30kms o menos
            try
            {
                DependencyService.Get<IWebService>().getCAM_busca(currentlocation.Latitude, currentlocation.Longitude, 50); //call to get CAMs list
                lista_CAM = DependencyService.Get<IWebService>().CAM_Busca; //get DataTable with Cams list
            }
            catch (Exception)
            {
                DependencyService.Get<IWebService>().getCAM_busca(25.691288, -100.316775, 50); //call to get CAMs list
                lista_CAM = DependencyService.Get<IWebService>().CAM_Busca; //get DataTable with Cams list
            }
            CAMDic.Clear();
            pkrVeterinario.Items.Clear();
            foreach (DataRow dr in lista_CAM.Rows) {
                CAMDic.Add(dr["BusinessName"].ToString(), Convert.ToInt32(dr["IDPartnerLocation"].ToString()));
                pkrVeterinario.Items.Add(dr["BusinessName"].ToString());
            }

            pkrVeterinario.SelectedIndexChanged += PkrVeterinario_SelectedIndexChanged;

        }//Constructor

        private void PkrVeterinario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                idSucursal = CAMDic[pkrVeterinario.SelectedItem.ToString()];
            }
            catch (Exception exc) {
                idSucursal = -1;
            }
        }

        public void tipoMascotaSeleccionado(object sender, EventArgs args){
            idTipoMascota = tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()];
            pkrRazaMascota.IsEnabled = true;
            pkrColorMascota.IsEnabled = false;
            DataTable razaMascota = new DataTable();
            DataTable colorMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaRaza_Busca(idTipoMascota);
            DependencyService.Get<IWebService>().getMascotaColor_Busca(idTipoMascota);
            colorMascota = DependencyService.Get<IWebService>().MascotaColor_Busca;
            razaMascota = DependencyService.Get<IWebService>().MascotaRaza_Busca;

            pkrRazaMascota.Items.Clear();
            razaMascotaDic.Clear();
            foreach(DataRow dr in razaMascota.Rows){
                pkrRazaMascota.Items.Add(dr[5].ToString());
                razaMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrColorMascota.Items.Clear();
            colorMascotaDic.Clear();
            foreach(DataRow dr in colorMascota.Rows){
                pkrColorMascota.Items.Add(dr[5].ToString());
                colorMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrRazaMascota.SelectedIndexChanged += razaMascotaSeleccionado;
            pkrColorMascota.SelectedIndexChanged += colorMascotaSeleccionado;
        }

        public void razaMascotaSeleccionado(object sender, EventArgs args) {
            try
            {
                idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
                pkrColorMascota.IsEnabled = true;
            }catch (Exception) {
                
            }
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args) {
            try
            {
                if (idRazaMascota == -1) {
                    DisplayAlert("Advertencia","Es necesario seleccionar la raza","OK");
                }

                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception)
            {
                
            }
        }

        async void OnEscanear(object sender, EventArgs args) {
            var status = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != PermissionStatus.Granted)
            {
                await DisplayAlert("Error", "La app no tiene permisos para utilizar la camara", "OK");
                return;
            }

            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtCodigoMascota.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtCodigoMascota.Text = "";
            }
        }

        async void onReg_dueno(object sender, EventArgs args) {
            string codigoMascota = "", nombreMascota = "", nombreDueno = "", apellidoP = "", apellidoM = "", correo = "", contrasena = "";
            int tipoMascota = -1, razaMascota = -1, colorMascota = -1, sexoMascota = 0, sexoDueno = -1, edadMascotaE = -1;

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                Device.BeginInvokeOnMainThread(async () => {
                    await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                    await DependencyService.Get<IWebService>().CloseApp();
                });
            }

            try
            {
                codigoMascota = txtCodigoMascota.Text;
                tipoMascota = idTipoMascota;
                razaMascota = idRazaMascota;
                colorMascota = idColorMascota;
                //poner pickers
                sexoMascota = sexoMascotaC;
                sexoDueno = sexoDuenoC;
                //
                nombreMascota = txtnombreMascota.Text;
                edadMascotaE = Convert.ToInt32(txtedadMascota.Text);
                nombreDueno = txtnombreDueno.Text;
                apellidoP = txtApellidoPaterno.Text;
                apellidoM = txtApellidoMaterno.Text;
                correo = txtCorreo.Text;
                contrasena = txtContrasena.Text;

                string[] textos = { codigoMascota, nombreMascota, nombreDueno, apellidoP, correo, contrasena };
                int[] enteros = { tipoMascota, razaMascota, colorMascota, sexoDueno, edadMascotaE };


                if (txtCodigoMascota.Text == null || txtCodigoMascota.Text == "") {
                    await DisplayAlert("Error", "Ingresa un código de mascota", "OK");
                    return;
                }

                if (idTipoMascota == -1) {
                    await DisplayAlert("Error", "Selecciona un tipo de mascota", "OK");
                    return;
                }

                if (idRazaMascota == -1) {
                    await DisplayAlert("Error", "Selecciona la raza de tu mascota", "OK");
                    return;
                }

                if (idColorMascota == -1) {
                    await DisplayAlert("Error", "Selecciona el color de tu mascota", "OK");
                    return;
                }

                if (txtnombreMascota.Text == null || txtnombreMascota.Text == "") {
                    await DisplayAlert("Error", "Ingresa el nombre de tu mascota", "OK");
                    return;
                }

                if (sexoSelectedM == -1) {
                    await DisplayAlert("Error", "Ingresa el sexo de tu mascota", "OK");
                    return;
                }

                if (txtedadMascota.Text == null || txtedadMascota.Text == "") {
                    await DisplayAlert("Error", "Ingresa la edad de tu mascota", "OK");
                    return;
                }

                if (idSucursal == -1)
                {
                    await DisplayAlert("Error", "Selecciona un veterinario", "OK");
                    return;
                }

                if (txtnombreDueno.Text == null || txtnombreDueno.Text == "") {
                    await DisplayAlert("Error", "Ingresa el nombre del dueño", "OK");
                    return;
                }

                if (txtApellidoPaterno.Text == null || txtApellidoPaterno.Text == "") {
                    await DisplayAlert("Error", "Ingresa el apellido paterno del dueño", "OK");
                    return;
                }

                if (sexoSelectedD == -1) {
                    await DisplayAlert("Error", "Selecciona el sexo del dueño", "OK");
                    return;
                }

                if (txtCorreo.Text == null || txtCorreo.Text == "") {
                    await DisplayAlert("Error", "Ingresa un correo electrónico", "OK");
                    return;
                }

                if (!ValidateEmail(txtCorreo.Text))
                {
                    await DisplayAlert("Error", "Correo invalido", "OK");
                    return;
                }

                if (txtContrasena.Text == null || txtContrasena.Text == "") {
                    await DisplayAlert("Error", "Correo invalido", "OK");
                    return;
                }
                    
                if (textos.Any(item => item.Length <= 0))
                {
                    await DisplayAlert("Error", "Faltan campos por llenar textos", "OK");
                    return;
                }
                if (enteros.Any(item => item == -1)) {
                    await DisplayAlert("Error", "Faltan campos por llenar enteros", "OK");
                    return;
                }

                bool estatus = false;

                Retorno retorno = DependencyService.Get<IWebService>().getMascota_Registro(new Model.Dueno()
                {
                    idDueno = codigoMascota,
                    idSucursal = idSucursal,
                    nombre = nombreDueno,
                    apellidoP = apellidoP,
                    apellidoM = apellidoM,
                    sexo = sexoDueno,
                    correo = correo,
                    contrasena = contrasena,
                    mascostaCodigo = codigoMascota,
                    nombreMascota = nombreMascota,
                    sexoMascota = sexoMascota,
                    idTipoMascota = idTipoMascota,
                    idRazaMascota = idRazaMascota,
                    idColorMascota = idColorMascota,
                    edadMascota = edadMascotaE
                });
                estatus = DependencyService.Get<IWebService>().Mascota_Registro;

                if (retorno.Resultado){
                    await DisplayAlert("OK", "Se registro correctamente", "OK");
                    await Navigation.PushAsync(new MainPage());
                }else {
                    await DisplayAlert("Error", retorno.Mensaje, "OK");
                }

            }catch (Exception){
                await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                return;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                pkrTipoMascota.Focused += (object sender, FocusEventArgs eventHandler) =>
                {
                    Navigation.PopAsync();
                };
                pkrRazaMascota.IsVisible = false;
                pkrColorMascota.IsVisible = false;
            }
            return base.OnBackButtonPressed();
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
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

        private async Task getPermisoLocation()
        {
            locationGrant = await DependencyService.Get<IWebService>().getPermisoLocation();
        }
    }
}
