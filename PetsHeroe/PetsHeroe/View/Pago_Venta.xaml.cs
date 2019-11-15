using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Pago_Venta : ContentPage
    {

        private int idMascota = -1;
        private int idMiembro = -1;
        private double puntos = 0;
        
        public Pago_Venta(string codigoMascota, Double costo, ObservableCollection<Venta> listaCarrito)
        {
            InitializeComponent();

            DataTable mascotasTbl = new DataTable();
            DataTable promocionesDueno = new DataTable();

            bool status = DependencyService.Get<IWebService>().getIdMascota_Busca(codigoMascota);
            if (status)
            {
                mascotasTbl = DependencyService.Get<IWebService>().Mascota_Busca;

                foreach (DataRow dr in mascotasTbl.Rows)
                {
                    idMascota = Convert.ToInt32(dr["IDPet"].ToString());
                    idMiembro = Convert.ToInt32(dr["IDMember"].ToString());
                }

            }


            promocionesDueno = DependencyService.Get<IWebService>().setPuntosPromociones_Busca(idMiembro, -1, -1);

            foreach (DataRow dr in promocionesDueno.Rows)
            {
                if (Convert.ToBoolean(dr["IsPoints"]))
                {
                    puntos += Convert.ToInt32(dr["PointsActive"]);
                }
            }

            lblTotal.Text = costo.ToString();
            lblPuntos.Text = puntos.ToString();

        }

        async void onAplicar(object sender, EventArgs args)
        {
            try {
                Convert.ToDouble(txtPuntosAplicar.Text);
            }
            catch (Exception ex) {
                await DisplayAlert("Error","Valor invalido","Ok");
                return;
            }

            if (Convert.ToDouble(txtPuntosAplicar.Text) > puntos) {
                await DisplayAlert("Error", "Valor invalido", "Ok");
                return;
            }
        }

        async void onPagar(object sender, EventArgs args) {
            bool status = DependencyService.Get<IWebService>().ticketPaga(idMascota, 20, -1, Convert.ToDecimal(txtPuntosAplicar.Text));
            if (status)
            {
                DisplayAlert("Ok","Pago procesado correctamente","Ok");
            }
            else {
                DisplayAlert("Ok", "Error al procesar pago", "Ok");
            }
        }

    }
}
