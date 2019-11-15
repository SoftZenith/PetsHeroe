using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Nuevo_Servicio_Promo : ContentPage
    {
        private Dictionary<string, int> tipoMascotaDic = new Dictionary<string, int>();
        private Dictionary<string, int> tipoServicioDic = new Dictionary<string, int>();
        private string nombre = "", fechaInicio = "", fechaFin = "";

        public Nuevo_Servicio_Promo()
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

        }

        private void PkrTipoMascota_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable servicios = new DataTable();

            servicios = DependencyService.Get<IWebService>().getServicio_Busca(tipoMascotaDic[pkrTipoMascota.SelectedItem.ToString()]);

            pkrServicio.Items.Clear();
            tipoMascotaDic.Clear();
            tipoServicioDic.Clear();
            foreach (DataRow dr in servicios.Rows) {
                pkrServicio.Items.Add(dr[7].ToString());
                tipoServicioDic.Add(dr[7].ToString(), Convert.ToInt32(dr[2]));
            }

        }

        public void onGuardarPromo(object sender, EventArgs args) {

        }

    }
}
