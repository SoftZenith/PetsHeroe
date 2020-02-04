using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Plugin.Connectivity;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nuevo_Servicio_Promo : ContentPage
    {
        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> tipoServicioDic = new Dictionary<string, int>();
        private string nombre = "", fechaInicio = "", fechaFin = "";
        private int IdServicio = -1;
        private int idAsociado = -1;
        private int idPromoServicio = -1;
        private bool isNewService = false;
        private bool isEditingPromo = false;

        protected override void OnAppearing()
        {

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            if (!isEditingPromo)
            {
                pkrTipoMascota.SelectedIndex = -1;
                pkrServicio.SelectedIndex = -1;
            }

        }

        public Nuevo_Servicio_Promo(bool isEdit, Promocion promocionEdit)
        {
            InitializeComponent();

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            isEditingPromo = isEdit;
            if (isEdit)
            {
                idPromoServicio = promocionEdit.idPromocion;
            }
            idAsociado = Preferences.Get("idAsociado", -1);

            DataTable tipoMascota = new DataTable();

            DependencyService.Get<IWebService>().getMascotaTipo_Busca();
            tipoMascota = DependencyService.Get<IWebService>().MascotaTipo_Busca;

            pkrTipoMascota.Items.Clear();
            tipoMascotaDic.Clear();
            foreach (DataRow dr in tipoMascota.Rows)
            {
                pkrTipoMascota.Items.Add(dr[2].ToString());
                tipoMascotaDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrTipoMascota.SelectedIndexChanged += PkrTipoMascota_SelectedIndexChanged;

            if (isEdit) {
                pkrTipoMascota.SelectedItem = promocionEdit.tipo;
                pkrTipoMascota.IsEnabled = false;
                pkrServicio.SelectedItem = promocionEdit.producto;
                pkrServicio.IsEnabled = false;
                txtAPartir.Date = DateTime.Now;
                txtHasta.Date = Convert.ToDateTime(promocionEdit.vigencia);
                txtNombrePromo.Text = promocionEdit.nombre;
                txtPrecio.Text = promocionEdit.precio.ToString();
                txtUnidades.Text = promocionEdit.compra.ToString();
            }

            if (isEdit)
            {
                this.Title = "Editar servicio";
                btnGuardar.Text = "Editar";
            }
            else
            {
                this.Title = "Agregar servicio";
                btnGuardar.Text = "Guardar";
            }

        }

        private void PkrTipoMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable servicios = new DataTable();

            try
            {
                servicios = DependencyService.Get<IWebService>().getServicio_Busca(-1, tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()]);
            }
            catch (Exception ex) {

            }
            pkrServicio.Items.Clear();
            tipoServicioDic.Clear();
            foreach (DataRow dr in servicios.Rows) {
                try
                {
                    tipoServicioDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDServiceType"]));
                    pkrServicio.Items.Add(dr["Name"].ToString());
                }
                catch (Exception ex) {
                    System.Console.Write(ex.ToString());
                }
            }
            pkrServicio.Items.Add("Agregar nuevo servicio");

            pkrServicio.SelectedIndexChanged += PkrServicio_SelectedIndexChanged;

        }

        private void PkrServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (pkrServicio.SelectedItem.ToString() == "Agregar nuevo servicio")
                {
                    if (!isNewService)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await DisplayAlert("Agregar", "¿Deseas agregarlo?", "Si", "No");
                            if (result)
                            {
                                isNewService = false;
                                await Navigation.PushAsync(new Nuevo_Servicio_Venta());
                            }
                            else {
                                isNewService = false;
                            }
                        });
                    }
                    isNewService = true;
                }
                else
                {
                    IdServicio = tipoServicioDic[pkrServicio.SelectedItem.ToString()];
                }
            }
            catch (Exception ex) {
                IdServicio = -1;
            }
        }

        public void onGuardarPromo(object sender, EventArgs args) {

            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            if (txtAPartir.Date < DateTime.Now.Date) {
                DisplayAlert("Error","Fecha de inicio menor a fecha actual","Ok");
                return;
            }

            if (txtHasta.Date < DateTime.Now.Date) {
                DisplayAlert("Error","Fecha final menor a fecha actual","Ok");
                return;
            }

            if (txtHasta.Date < txtAPartir.Date) {
                DisplayAlert("Error","Fecha final menor a fecha inicio","Ok");
                return;
            }

            if (isEditingPromo)
            {
                Retorno res = DependencyService.Get<IWebService>().promoServicio_Edita(idPromoServicio, txtNombrePromo.Text, Convert.ToDecimal(txtPrecio.Text), Convert.ToDecimal(txtPrecio.Text), txtAPartir.Date, txtHasta.Date, 0, Convert.ToInt32(txtUnidades.Text), chkActivo.IsChecked);
                if (res.Resultado)
                {
                    DisplayAlert("Ok", "Se edito correctamente", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Error", res.Mensaje, "Ok");
                }
            }
            else
            {

                Resultado res = DependencyService.Get<IWebService>().promoServicio_Agregar(idAsociado, IdServicio, txtNombrePromo.Text, Convert.ToDecimal(txtPrecio.Text), Convert.ToDecimal(txtPrecio.Text), txtAPartir.Date, txtHasta.Date, 0, Convert.ToInt32(txtUnidades.Text), chkActivo.IsChecked);
                if (res.status)
                {
                    DisplayAlert("Ok", "Se agrego correctamente", "Ok");
                    Navigation.PopAsync();
                }
                else
                {
                    DisplayAlert("Error", res.errorMessage, "Ok");
                }
            }

        }

    }
}
