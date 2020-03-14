using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Input;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro_mascota : ContentPage
    {
        private int idTipoMascota = -1;
        private int idRazaMascota = -1;
        private int idColorMascota = -1;
        private char sexoMascotaC = (char)0;
        private int idMiembro = -1;
        private int idPais = -1;
        private int idEstado = -1;
        private int idCiudad = -1;
        private int idVeterinario = -1;

        private string nombre;
        private string apellidoP;
        private string apellidoM;
        private int sexo;
        private string correo;
        private string contrasena;

        private Mascota mascotaG;
        private bool isEditG = false;

        //Diccionarios para guardar tipo - id
        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> paisDic = new Dictionary<string, int>();
        private Dictionary<string, int> EstadoDic = new Dictionary<string, int>();
        private Dictionary<string, int> CiudadDic = new Dictionary<string, int>();
        private Dictionary<string, int> VeterinarioDic = new Dictionary<string, int>();
        private Dictionary<int, string> paisDicEdit = new Dictionary<int, string>();
        private Dictionary<int, string> EstadoDicEdit = new Dictionary<int, string>();
        private Dictionary<int, string> CiudadDicEdit = new Dictionary<int, string>();
        private Dictionary<int, string> VeterinarioDicEdit = new Dictionary<int, string>();
        //DataTable para datos del dueño
        private DataTable dtDueno = new DataTable();
        public Dueno dueno;

        //public ICommand TapCommand => new Command<string>(async (url) => await Launcher.OpenAsync(url));
        public ICommand TapCommand => new Command<string>((url) =>
        {
            
        });

        public void onTerminos(object sender, EventArgs eventArgs) {
            Launcher.OpenAsync("http://petshero.com.mx/avisoprivacidad.html");
        }

        public Registro_mascota(Mascota mascota, bool isEdit)
        {
            InitializeComponent();

            pkrRazaMascota.IsEnabled = false;
            pkrColorMascota.IsEnabled = false;

            if (isEdit) {
                mascotaG = mascota;
                isEditG = isEdit;
                txtCodigo.Text = mascota.codigo;
                txtNombre.Text = mascota.nombre;
                txtedadMascota.Text = mascota.edad.ToString();
                pkrSexoMascota.SelectedIndex = mascota.sexo == "M" ? 0 : 1;
                sexoMascotaC = pkrTipoMascota.SelectedIndex == 0 ? (char)77 : (char)72;
                chkTermino.IsChecked = true;
                //txtedadMascota.Text = mascota.edad.ToString();
            }

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;

            //get client data by memberId
            try
            {
                DependencyService.Get<IWebService>().getCliente_Busca(Preferences.Get("idMiembro", 0), -1);
                dtDueno = DependencyService.Get<IWebService>().Cliente_Busca;
                Console.WriteLine("Get idMiembro" + Preferences.Get("idMiembro", 0));
                Console.WriteLine("Rows: " + dtDueno.Rows.Count);
                foreach (DataRow dr in dtDueno.Rows) {
                    dueno = new Dueno() {
                        nombre = dr["Name"].ToString(),
                        apellidoP = dr["LastName"].ToString(),
                        apellidoM = dr["LastName2"].ToString(),
                        sexo = dr["Sex"].ToString() == "M" ? 77 : 70,
                        correo = dr["EMail"].ToString().Replace("\t","").Trim(),
                        contrasena = Preferences.Get("password", "")
                    };

                    Console.WriteLine("Nombre: " + nombre);
                    Console.WriteLine("Apellido P: " + apellidoP);
                    Console.WriteLine("Apellido M: " + apellidoM);
                    Console.WriteLine("Sexo: " + sexo);
                    Console.WriteLine("correo: " + correo);
                    Console.WriteLine("contrasena: " + contrasena);
                }
            }
            catch (Exception ex) {
                Console.WriteLine("Error linea 68: "+ex);
            }

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows)
            {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            

            pkrTipoMascota.SelectedIndexChanged += tipoMascotaSeleccionado;

            pkrSexoMascota.SelectedIndexChanged += (object sender, EventArgs eventArgs) =>
            {
                int sexoSelected = pkrSexoMascota.SelectedIndex;
                if (sexoSelected == 0)
                {
                    sexoMascotaC = (char)77;
                }
                else if (sexoSelected == 1)
                {
                    sexoMascotaC = (char)72;
                }
            };

            DataTable paises = new DataTable();
            DependencyService.Get<IWebService>().getPais_Busca();
            paises = DependencyService.Get<IWebService>().Pais_Busca;
            pkrPais.Items.Clear();
            paisDic.Clear();
            foreach (DataRow dr in paises.Rows) {
                pkrPais.Items.Add(dr["Name"].ToString());
                paisDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCountry"]));
                paisDicEdit.Add(Convert.ToInt32(dr["IDCountry"]), dr["Name"].ToString());
            }

            pkrPais.SelectedIndexChanged += PkrPais_SelectedIndexChanged;

            if (isEdit)
            {
                pkrTipoMascota.SelectedItem = mascota.tipo;
                pkrRazaMascota.IsEnabled = true;
                tipoMascotaSeleccionado(null, null);
                try
                {
                    pkrPais.SelectedItem = paisDicEdit[mascota.idPais];
                    PkrPais_SelectedIndexChanged(null, null);
                }
                catch (Exception) {

                }

            }

        }//Constructor

        private void PkrPais_SelectedIndexChanged(object sender, EventArgs e)
        {
            idPais = paisDic[pkrPais.SelectedItem.ToString()];
            idEstado = -1;
            DataTable estados = new DataTable();
            DependencyService.Get<IWebService>().getEstado_Busca(idPais);
            estados = DependencyService.Get<IWebService>().Estado_Busca;
            pkrEstado.Items.Clear();
            paisDic.Clear();
            EstadoDicEdit.Clear();
            foreach (DataRow dr in estados.Rows) {
                pkrEstado.Items.Add(dr["Name"].ToString());
                EstadoDic.Add(dr["Name"].ToString(),Convert.ToInt32(dr["IDState"]));
                EstadoDicEdit.Add(Convert.ToInt32(dr["IDState"]), dr["Name"].ToString());
            }

            pkrEstado.SelectedIndexChanged += PkrEstado_SelectedIndexChanged;

            if (isEditG)
            {
                try
                {
                    pkrEstado.SelectedItem = EstadoDicEdit[mascotaG.idEstado];
                    //PkrEstado_SelectedIndexChanged(null, null);
                }
                catch (Exception)
                {

                }
            }

        }

        private void PkrEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            idEstado = EstadoDic[pkrEstado.SelectedItem.ToString()];
            idCiudad = -1;
            DataTable ciudades = new DataTable();
            DependencyService.Get<IWebService>().getCiudad_Busca(idEstado);
            ciudades = DependencyService.Get<IWebService>().Ciudad_Busca;

            pkrVeterinario.Items.Clear();
            VeterinarioDic.Clear();

            pkrCiudad.Items.Clear();
            CiudadDic.Clear();
            CiudadDicEdit.Clear();
            foreach (DataRow dr in ciudades.Rows) {
                pkrCiudad.Items.Add(dr["Name"].ToString());
                CiudadDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDCity"]));
                CiudadDicEdit.Add(Convert.ToInt32(dr["IDCity"]), dr["Name"].ToString());
            }

            pkrCiudad.SelectedIndexChanged += PkrCiudad_SelectedIndexChanged;

            if (isEditG) {
                try
                {
                    pkrCiudad.SelectedItem = CiudadDicEdit[mascotaG.idCiudad];
                    //PkrCiudad_SelectedIndexChanged(null, null);
                }
                catch (Exception ex) {

                }
            }

        }

        private void PkrCiudad_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idCiudad = CiudadDic[pkrCiudad.SelectedItem.ToString()];
                idVeterinario = -1;
                DataTable CAMs = new DataTable();
                DependencyService.Get<IWebService>().getCAM_busca(idPais, idEstado, idCiudad);
                CAMs = DependencyService.Get<IWebService>().CAM_Busca;

                pkrVeterinario.Items.Clear();
                VeterinarioDic.Clear();
                VeterinarioDicEdit.Clear();
                foreach (DataRow dr in CAMs.Rows) {
                    pkrVeterinario.Items.Add(dr["BusinessName"].ToString());
                    VeterinarioDic.Add(dr["BusinessName"].ToString(), Convert.ToInt32(dr["IDPartnerLocation"]));
                    VeterinarioDicEdit.Add(Convert.ToInt32(dr["IDPartnerLocation"]), dr["BusinessName"].ToString());
                }

                pkrVeterinario.SelectedIndexChanged += PkrVeterinario_SelectedIndexChanged;

                if (isEditG) {
                    try
                    {
                        pkrVeterinario.SelectedItem = VeterinarioDicEdit[mascotaG.idSucursal];
                    }
                    catch (Exception) {

                    }

                }

            }
            catch (Exception) {

            }
        }

        private void PkrVeterinario_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idVeterinario = VeterinarioDic[pkrVeterinario.SelectedItem.ToString()];
            }
            catch (Exception) {

            }
        }

        public void tipoMascotaSeleccionado(object sender, EventArgs args)
        {
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
            foreach (DataRow dr in razaMascota.Rows)
            {
                pkrRazaMascota.Items.Add(dr[5].ToString());
                razaMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            try
            {
                pkrRazaMascota.SelectedItem = mascotaG.raza;
                idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
            }
            catch (Exception ex) {

            }

            pkrColorMascota.Items.Clear();
            colorMascotaDic.Clear();
            foreach (DataRow dr in colorMascota.Rows)
            {
                pkrColorMascota.Items.Add(dr[5].ToString());
                colorMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
            }

            try
            {
                pkrColorMascota.SelectedItem = mascotaG.color;
                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception ex) {

            }

            pkrRazaMascota.SelectedIndexChanged += razaMascotaSeleccionado;
            pkrColorMascota.SelectedIndexChanged += colorMascotaSeleccionado;
        }

        public void razaMascotaSeleccionado(object sender, EventArgs args)
        {
            try
            {
                idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
                pkrColorMascota.IsEnabled = true;
            }
            catch (Exception)
            {

            }
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args)
        {
            try
            {
                if (idRazaMascota == -1)
                {
                    DisplayAlert("Advertencia", "Es necesario seleccionar la raza", "OK");
                }

                idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
            }
            catch (Exception)
            {

            }
        }

        public void onRegistraMascota(object sender, EventArgs args) {

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            try
            {
                bool estatus = false;

                if (idTipoMascota < 0)
                {
                    DisplayAlert("Error", "Selecciona el tipo de mascota", "OK");
                    return;
                }
                else if (idRazaMascota < 0)
                {
                    DisplayAlert("Error", "Selecciona la raza", "OK");
                    return;
                }
                else if (idColorMascota < 0)
                {
                    DisplayAlert("Error", "Selecciona el color de tu mascota", "OK");
                    return;
                }
                else if (sexoMascotaC == 0)
                {
                    DisplayAlert("Error", "Selecciona el sexo de tu mascota", "OK");
                    return;
                }
                else if (txtCodigo.Text == "" || txtCodigo.Text == null)
                {
                    DisplayAlert("Error", "Ingresa el código de tu mascota", "OK");
                    return;
                }
                else if (txtNombre.Text == "" || txtNombre.Text == null)
                {
                    DisplayAlert("Error", "Ingresa un nombre de tu mascota", "OK");
                    return;
                }
                else if (txtedadMascota.Text == "" || txtedadMascota.Text == null)
                {
                    DisplayAlert("Error", "Ingresa la edad de tu mascota", "OK");
                    return;
                }

                if (idPais < 0) {
                    DisplayAlert("Error", "Selecciona tu país", "OK");
                    return;
                }

                if (idEstado < 0) {
                    DisplayAlert("Error", "Selecciona tu estado", "OK");
                    return;
                }

                if (idCiudad < 0) {
                    DisplayAlert("Error", "Selecciona tu ciudad", "OK");
                    return;
                }

                if (idVeterinario < 0) {
                    DisplayAlert("Error", "Selecciona tu veterinario", "OK");
                    return;
                }

                try
                {
                    Convert.ToInt32(txtedadMascota.Text);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", "Edad invalida", "Error");
                    return;
                }

                if (!chkTermino.IsChecked)
                {
                    DisplayAlert("Error", "Para registrar tu mascota es necesario aceptar los términos y condiciones", "Error");
                    return;
                }

                if (isEditG)
                {
                    Retorno retorno = DependencyService.Get<IWebService>().Mascota_Modifica(mascotaG.idMascota, txtNombre.Text, sexoMascotaC, idTipoMascota, idRazaMascota, idColorMascota, Convert.ToInt32(txtedadMascota.Text));
                    if (retorno.Resultado)
                    {

                        Retorno retornoCAM = DependencyService.Get<IWebService>().Mascota_AsignaCam(mascotaG.idMascota, idVeterinario);

                        if (retornoCAM.Resultado)
                        {
                            DisplayAlert("Registro", "Se editó correctamente", "OK");
                            Navigation.PushAsync(new Menu_dueno(2));
                        }
                        else {
                            DisplayAlert("Registro", "No se guardaron todos los datos", "OK");
                            Navigation.PushAsync(new Menu_dueno(2));
                        }
                    }
                    else
                    {
                        DisplayAlert("Error", retorno.Mensaje, "OK");
                    }
                }
                else
                {
                    Retorno retorno = DependencyService.Get<IWebService>().getMascota_Registro(new Model.Dueno()
                    {
                        idDueno = txtCodigo.Text,
                        nombre = dueno.nombre,
                        apellidoP = dueno.apellidoP,
                        apellidoM = dueno.apellidoM,
                        sexo = dueno.sexo,
                        correo = dueno.correo,
                        contrasena = dueno.contrasena,
                        mascostaCodigo = txtCodigo.Text,
                        sexoMascota = sexoMascotaC,
                        idTipoMascota = idTipoMascota,
                        nombreMascota = txtNombre.Text,
                        idRazaMascota = idRazaMascota,
                        idColorMascota = idColorMascota,
                        edadMascota = Convert.ToInt32(txtedadMascota.Text)
                    });

                    if (retorno.Resultado)
                    {
                        DisplayAlert("Registro", "Se registro correctamente", "OK");
                        Navigation.PushAsync(new Menu_dueno(2));
                    }
                    else
                    {
                        DisplayAlert("Error", retorno.Mensaje, "OK");
                    }
                }

            }
            catch (Exception ex) {
                Console.WriteLine("Error linea 230: "+ex);
            }
        }

    }
}
