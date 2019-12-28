using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nuevo_Servicio_Venta : ContentPage
    {

        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private int idTipoMascota = -1;

        public Nuevo_Servicio_Venta()
        {
            InitializeComponent();

            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows)
            {
                pkrTipoMascota.Items.Add(dr["Name"].ToString());
                tipoMascotaDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDPetType"]));
            }

            pkrTipoMascota.SelectedIndexChanged += PkrTipoMascota_SelectedIndexChanged;

        }

        private void PkrTipoMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idTipoMascota = tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()];
            }
            catch (Exception ex) {
                idTipoMascota = -1;
            }
        }

        async void onAgregar(object sender, EventArgs args) {
            if (txtServicio.Text == null || txtServicio.Text == "")
            {
                await DisplayAlert("Error", "Agrega un nombre para el nuevo servicio", "Ok");
            }
            else if (txtCodigo.Text == null || txtCodigo.Text == "")
            {
                await DisplayAlert("Error", "Agrega un código para el nuevo servicio", "Ok");
            }
            else if (idTipoMascota < 0) {
                await DisplayAlert("Error", "Selecciona un tipo de mascota para el nuevo servicio", "Ok");
            }

            Retorno retorno = DependencyService.Get<IWebService>().setServicioAgrega(idTipoMascota, txtCodigo.Text, txtServicio.Text);

            if (retorno.Resultado) {
                await DisplayAlert("OK","Se agrego el nuevo servicio correctamente","Ok");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", "No se puedo agregar el nuevo servicio, intente nuevamente", "Ok");
            }
        }

    }
}
