using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;
using PetsHeroe.Model;
using System.Linq;

namespace PetsHeroe
{
    public partial class Registro_vet : ContentPage
    {

        Dictionary<string, int> tipoAsociadoDic = new Dictionary<string, int>();
        int idTipoAsociado = -1;
        int sexo = -1;
        bool status;

        public Registro_vet()
        {
            InitializeComponent();

            DataTable tipoAsociado = new DataTable();

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getTipoAsociado_Busca();
                tipoAsociado = DependencyService.Get<IIOS>().TipoAsociado_Busca;
            }
            else if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getTipoAsociado_Busca();
                tipoAsociado = DependencyService.Get<IAndroid>().TipoAsociado_Busca;
            }

            pkrAsociado.Items.Clear();
            tipoAsociadoDic.Clear();
            foreach (DataRow dr in tipoAsociado.Rows)
            {
                pkrAsociado.Items.Add(dr[2].ToString());
                tipoAsociadoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrAsociado.SelectedIndexChanged += (object sender, EventArgs args) =>
            {
                idTipoAsociado = tipoAsociadoDic[pkrAsociado.SelectedItem.ToString()];
            };

            pkrSexo.SelectedIndexChanged += (object sender, EventArgs args) => {
                if (pkrSexo.SelectedIndex == 0)
                {
                    sexo = (char)77;
                }
                else if (pkrSexo.SelectedIndex == 1) {
                    sexo = (char)70;
                }
            };
        }

        async void onRegVet(object sender, EventArgs args) {
            if (chkTermino.IsChecked)
            {
                Asociado asociado = new Asociado();
                asociado.idAsociado = 0;
                asociado.tipoAsociado = -1;
                asociado.nombreComerial = "";
                asociado.nombre = "";
                asociado.apellidoPaterno = "";
                asociado.apellidoMaterno = "";
                asociado.correo = "";
                asociado.contrasena = "";
                asociado.sexo = -1;

                if (txtContrasena.Text != txtConConstrasena.Text) {
                    await DisplayAlert("Error", "Verifica que ambas contraseñas coincidan", "OK");
                    return;
                }

                try
                {
                    asociado.tipoAsociado = idTipoAsociado;
                    asociado.nombreComerial = txtNombreComercial.Text;
                    asociado.nombre = txtNombre.Text;
                    asociado.apellidoPaterno = txtApellidoP.Text;
                    asociado.apellidoMaterno = txtApellidoM.Text;
                    asociado.correo = txtCorreo.Text;
                    asociado.contrasena = txtContrasena.Text;
                    asociado.sexo = sexo;

                    string[] textos = { asociado.nombreComerial, asociado.nombre, asociado.apellidoPaterno, asociado.correo, asociado.contrasena };
                    int[] enteros = { asociado.idAsociado, asociado.tipoAsociado, asociado.sexo };

                    if (textos.Any(item => item.Length <= 0)) {
                        await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                        return;
                    }

                    if (enteros.Any(item => item <= -1)) {
                        await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                        return;
                    }

                    if (Device.RuntimePlatform == Device.iOS)
                    {
                       status = DependencyService.Get<IIOS>().setVeterinario_Registro(asociado);
                    }
                    else if (Device.RuntimePlatform == Device.Android)
                    {
                       status = DependencyService.Get<IAndroid>().setVeterinario_Registro(asociado);
                    }

                    if (status)
                    {
                       await DisplayAlert("OK", "Registro exitoso, te enviamos un enlace a tu correo para poder activar tu cuenta","OK");
                    }
                    else {
                        await DisplayAlert("ERROR", "Ups parece que hubo un error", "OK");
                    }

                }
                catch{
                    await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                }
            }
            else {
                await DisplayAlert("Error", "Para poder registrarte es necesario que aceptes los terminos y condiciones", "OK");
            }
        }
    }
}
