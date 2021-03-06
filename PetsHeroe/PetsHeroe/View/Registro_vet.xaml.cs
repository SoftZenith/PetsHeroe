﻿using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;
using PetsHeroe.Model;
using System.Linq;
using PetsHeroe.Services;
using System.Text.RegularExpressions;
using Xamarin.Forms.Xaml;
using Plugin.Connectivity;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Registro_vet : ContentPage
    {
        Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Dictionary<string, int> tipoAsociadoDic = new Dictionary<string, int>();
        int idTipoAsociado = -1;
        int sexo = -1;
        bool status;

        public Registro_vet()
        {
            InitializeComponent();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            DataTable tipoAsociado = new DataTable();

            DependencyService.Get<IWebService>().getTipoAsociado_Busca();
            tipoAsociado = DependencyService.Get<IWebService>().TipoAsociado_Busca;

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

            if (!CrossConnectivity.Current.IsConnected)
            {
                await DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

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

                    if (idTipoAsociado < 0) {
                        await DisplayAlert("Error", "Selecciona el tipo de asociado", "OK");
                        return;
                    }

                    asociado.nombreComerial = txtNombreComercial.Text;
                    asociado.nombre = txtNombre.Text;
                    asociado.apellidoPaterno = txtApellidoP.Text;
                    asociado.apellidoMaterno = txtApellidoM.Text == null ? "" : txtApellidoM.Text;
                    asociado.correo = txtCorreo.Text.ToLower();
                    asociado.contrasena = txtContrasena.Text;
                    asociado.sexo = sexo;

                    string[] textos = { asociado.nombreComerial, asociado.nombre, asociado.apellidoPaterno, asociado.correo, asociado.contrasena };
                    int[] enteros = { asociado.idAsociado, asociado.tipoAsociado, asociado.sexo };

                    if (textos.Any(item => item.Length <= 0)) {
                        await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                        return;
                    }

                    if (asociado.contrasena.Length < 6) {
                        await DisplayAlert("Error", "Tu contraseña debe ser igual o mayor a 6 digitos", "OK");
                        return;
                    }

                    if (enteros.Any(item => item <= -1)) {
                        await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                        return;
                    }

                    if (!ValidateEmail(txtCorreo.Text)) {
                        await DisplayAlert("Error", "Correo invalido", "OK");
                        return;
                    }

                    Retorno res = DependencyService.Get<IWebService>().setVeterinario_Registro(asociado);

                    if (res.Resultado) {
                        await DisplayAlert("OK", "Registro exitoso, te enviamos un enlace a tu correo para poder activar tu cuenta","OK");
                        await Navigation.PushAsync(new MainPage());
                    } else {
                        await DisplayAlert("ERROR", res.Mensaje, "OK");
                    }

                }
                catch {
                    await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                }
            }
            else {
                await DisplayAlert("Error", "Para poder registrarte es necesario que aceptes los términos y condiciones", "OK");
            }
        }

        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email.ToLower());
        }

    }
}
