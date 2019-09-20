﻿using System;
using System.Collections.Generic;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Mascota_recuperada : ContentPage
    {
        private int idMascota;
        private int tipoRetorno = -1;
        private int condicion = -1;
        private int TIPO_INCIDENTE = 8;//Mascota encontrada directo

        public Mascota_recuperada(int idMascota)
        {
            InitializeComponent();
            this.idMascota = idMascota;

            pkrCondicion.SelectedIndexChanged += PkrCondicion_SelectedIndexChanged;
            pkrRetorno.SelectedIndexChanged += PkrRetorno_SelectedIndexChanged;

        }

        private void PkrRetorno_SelectedIndexChanged(object sender, EventArgs e)
        {
            tipoRetorno = pkrRetorno.SelectedIndex + 1;
        }

        private void PkrCondicion_SelectedIndexChanged(object sender, EventArgs e)
        {
            condicion = pkrCondicion.SelectedIndex + 1;
        }

        public void onRegistraMascota(object sender, EventArgs args) {

            if (tipoRetorno == -1) {
                DisplayAlert("Error", "Dinos como fue que regreso", "OK");
                return;
            }
            if (condicion == -1) {
                DisplayAlert("Error","Cuentamos en que condición regreso","OK");
                return;
            }

            bool estatus = DependencyService.Get<IWebService>().setMascota_Incidente(idMascota, TIPO_INCIDENTE, tipoRetorno, condicion, "");

            if (estatus)
            {
                DisplayAlert("OK", "Se cambio el estatus de tu mascota", "OK");
                Navigation.PopAsync();
            }
            else {
                DisplayAlert("Error", "Hubo un problema al cambiar el estatus de tu mascota", "OK");
            }

        }
    }
}
