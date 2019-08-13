using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Registro_dueno_mascota : ContentPage
    {
        int idTipoMascota = 0;
        int idRazaMascota = 0;
        int idColorMascota = 0;
        //Diccionarios para guardar nombre - id
        Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>(); 
        Dictionary<string, int> razaMascotaDic = new Dictionary<string, int>();
        Dictionary<string, int> colorMascotaDic = new Dictionary<string, int>();

        public Registro_dueno_mascota()
        {
            InitializeComponent();

            DataTable tipoMascota = new DataTable();

            if (Device.RuntimePlatform == Device.iOS)
            {
                DependencyService.Get<IIOS>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IIOS>().MascotaTipo_Busca;
            } else if (Device.RuntimePlatform == Device.Android) {
                DependencyService.Get<IAndroid>().getMascotaTipo_Busca();
                tipoMascota = DependencyService.Get<IAndroid>().MascotaTipo_Busca;
            }

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows) {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(),Convert.ToInt32(dr[0]));
            }

            pkrTipoMascota.SelectedIndexChanged += tipoMascotaSeleccionado;

        }//Constructor

        public void tipoMascotaSeleccionado(object sender, EventArgs args){
            idTipoMascota = tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()];
            DataTable razaMascota = new DataTable();
            DataTable colorMascota = new DataTable();

            if(Device.RuntimePlatform == Device.iOS){
                DependencyService.Get<IIOS>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IIOS>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IIOS>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IIOS>().MascotaRaza_Busca;
            }else if(Device.RuntimePlatform == Device.Android){
                DependencyService.Get<IAndroid>().getMascotaRaza_Busca(idTipoMascota);
                DependencyService.Get<IAndroid>().getMascotaColor_Busca(idTipoMascota);
                colorMascota = DependencyService.Get<IAndroid>().MascotaColor_Busca;
                razaMascota = DependencyService.Get<IAndroid>().MascotaRaza_Busca;
            }

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
            idRazaMascota = razaMascotaDic[pkrRazaMascota.SelectedItem.ToString()];
        }

        public void colorMascotaSeleccionado(object sender, EventArgs args) {
            idColorMascota = colorMascotaDic[pkrColorMascota.SelectedItem.ToString()];
        }
    }
}
