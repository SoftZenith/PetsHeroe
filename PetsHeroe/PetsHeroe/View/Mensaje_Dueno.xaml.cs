using System;
using System.Linq;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using System.Text.RegularExpressions;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Mensaje_Dueno : ContentPage
    {
        string codigo_pre;
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Regex PhoneRegex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");

        public Mensaje_Dueno(string codigo)
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }
            codigo_pre = codigo;
        }

        async void onMensaje(object sender, EventArgs args) {

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estas conectado a internet", "Ok");
                return;
            }

            Retorno status = new Retorno();
            string codigo = "", nombre = "", correo = "", telefono = "", mensaje = "";
            try
            { 
                codigo = codigo_pre;
                correo = txtCorreo.Text;
                nombre = txtNombre.Text;
                telefono = txtTelefono.Text;
                mensaje = txtMensaje.Text;

                string[] textos = { codigo, nombre, correo, telefono, mensaje };

                if (textos.Any(item => item.Trim().Length <= 0))
                {
                    await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                    return;
                }

                if (!ValidateEmail(txtCorreo.Text)) {
                    await DisplayAlert("Error", "Correo invalido", "OK");
                    return;
                }

                if (!IsPhoneNumber(txtTelefono.Text)) {
                    await DisplayAlert("Error", "Telefono inválido", "OK");
                    return;
                }

            }
            catch (Exception ex) {
                await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                Console.WriteLine("Error: "+ex);
                return;
            }

            status = DependencyService.Get<IWebService>().setEntrega_SoloMensaje(new Model.MensajeDueno()
            {
                codigo = codigo,
                correo = correo,
                nombre = nombre,
                telefono = telefono,
                mensaje = mensaje,
                latitud = 0.0000,
                longitud = 0.0000
            });

            if (status.Resultado){
                await DisplayAlert("OK", "Se envio correctamente tu mensaje al dueño", "OK");
                if (!Preferences.Get("logged", false, "usuarioLogeado")) { await Navigation.PushAsync(new MainPage()); }
                if (Preferences.Get("userType", 0, "tipoUsuario") == 1) { await Navigation.PushAsync(new Menu_dueno(1)); }
                if (Preferences.Get("userType", 0, "tipoUsuario") == 2) { await Navigation.PushAsync(new Menu_veterinario(3)); }
            } else {
                await DisplayAlert("Error", "Esta mascota no ha sido reportada como extraviada o robada", "OK");
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
            if (number.Length < 10)
            {
                return false;
            }
            return PhoneRegex.IsMatch(number);
        }
    }
}
