using System;
using System.Linq;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Mensaje_Dueno : ContentPage
    {
        string codigo_pre; 

        public Mensaje_Dueno(string codigo)
        {
            InitializeComponent();
            codigo_pre = codigo;
        }

        async void onMensaje(object sender, EventArgs args) {
            bool status = false;
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

            }
            catch (Exception ex) {
                await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                Console.WriteLine("Error: "+ex);
                return;
            }
            if (Device.RuntimePlatform == Device.Android)
            {
                status = DependencyService.Get<IAndroid>().setEntrega_SoloMensaje(new Model.MensajeDueno()
                {
                    codigo = codigo,
                    correo = correo,
                    nombre = nombre,
                    telefono = telefono,
                    mensaje = mensaje,
                    latitud = 0.0000,
                    longitud = 0.0000
                });
            } else if (Device.RuntimePlatform == Device.iOS) {
                status = DependencyService.Get<IIOS>().setEntrega_SoloMensaje(new Model.MensajeDueno()
                {
                    codigo = codigo,
                    correo = correo,
                    nombre = nombre,
                    telefono = telefono,
                    mensaje = mensaje,
                    latitud = 0.0000,
                    longitud = 0.0000
                });
            }

            if (status){
                await DisplayAlert("OK", "Se envio correctamente tu mensaje al dueño", "OK");
            } else {
                await DisplayAlert("Error", "Hubo un error al enviar el mensaje", "OK");
            }

        }

    }
}
