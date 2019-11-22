using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Nuevo_Servicio_Promo : ContentPage
    {
        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> tipoServicioDic = new Dictionary<string, int>();
        private string nombre = "", fechaInicio = "", fechaFin = "";
        private int IdServicio = -1;

        public Nuevo_Servicio_Promo(bool isEdit, Promocion promocionEdit)
        {
            InitializeComponent();

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
                pkrServicio.SelectedItem = promocionEdit.producto;
                txtAPartir.Date = Convert.ToDateTime(promocionEdit.inicia);
                txtHasta.Date = Convert.ToDateTime(promocionEdit.vigencia);
                txtNombrePromo.Text = promocionEdit.nombre;
                txtPrecio.Text = promocionEdit.precio.ToString();
                txtUnidades.Text = promocionEdit.compra.ToString();
            }

        }

        private void PkrTipoMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable servicios = new DataTable();

            servicios = DependencyService.Get<IWebService>().getServicio_Busca(tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()]);

            pkrServicio.Items.Clear();
            tipoServicioDic.Clear();
            foreach (DataRow dr in servicios.Rows) {
                pkrServicio.Items.Add(dr[7].ToString());
                tipoServicioDic.Add(dr[7].ToString(), Convert.ToInt32(dr[2]));
            }

            pkrServicio.SelectedIndexChanged += PkrServicio_SelectedIndexChanged;

        }

        private void PkrServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                IdServicio = tipoServicioDic[pkrServicio.SelectedItem.ToString()];
                pkrServicio.SelectedIndex = 0;
            }
            catch (Exception ex) {
                IdServicio = -1;
            }
        }

        public void onGuardarPromo(object sender, EventArgs args) {

            if (txtAPartir.Date < DateTime.Now) {
                DisplayAlert("Error","Fecha de inicio menor a fecha actual","Ok");
                return;
            }

            if (txtHasta.Date < DateTime.Now) {
                DisplayAlert("Error","Fecha final menor a fecha actual","Ok");
                return;
            }

            if (txtHasta.Date < txtAPartir.Date) {
                DisplayAlert("Error","Fecha final menor a fecha inicio","Ok");
                return;
            }

            bool status = DependencyService.Get<IWebService>().promoServicio_Agregar(-1, IdServicio, "", 0, 0, DateTime.Now, DateTime.Now, 0, 0);
            

        }

    }
}
