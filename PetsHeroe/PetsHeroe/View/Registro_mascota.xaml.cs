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
    public partial class Registro_mascota : ContentPage
    {
        private int idTipoMascota = -1;
        private int idRazaMascota = -1;
        private int idColorMascota = -1;
        private char sexoMascotaC = (char)0;
        private int idMiembro = -1;

        private string nombre;
        private string apellidoP;
        private string apellidoM;
        private int sexo;
        private string correo;
        private string contrasena;

        //Diccionarios para guardar tipo - id
        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();
        //DataTable para datos del dueño
        private DataTable dtDueno = new DataTable();
        public Dueno dueno;
        public Registro_mascota()
        {
            InitializeComponent();

            pkrRazaMascota.IsEnabled = false;
            pkrColorMascota.IsEnabled = false;
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

        }//Constructor

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

            pkrColorMascota.Items.Clear();
            colorMascotaDic.Clear();
            foreach (DataRow dr in colorMascota.Rows)
            {
                pkrColorMascota.Items.Add(dr[5].ToString());
                colorMascotaDic.Add(dr[5].ToString(), Convert.ToInt32(dr[0]));
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

                try
                {
                    Convert.ToInt32(txtedadMascota.Text);
                }
                catch (Exception ex)
                {
                    DisplayAlert("Error", "Edad invalida", "Error");
                    return;
                }

                DependencyService.Get<IWebService>().getMascota_Registro(new Model.Dueno()
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
                estatus = DependencyService.Get<IWebService>().Mascota_Registro;

                Console.WriteLine("En la linea 220");

                if (estatus)
                {
                    DisplayAlert("Registro", "Se registro correctamente", "OK");
                    Navigation.PushAsync(new Menu_dueno(1));
                }
                else
                {
                    DisplayAlert("Error", "Hubo un error al registrar tu mascota", "OK");
                }

            }
            catch (Exception ex) {
                Console.WriteLine("Error linea 230: "+ex);
            }
        }

    }
}
