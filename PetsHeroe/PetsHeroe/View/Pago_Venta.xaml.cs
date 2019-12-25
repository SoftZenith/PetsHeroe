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
        private int idSucursalG = -1;
        private double puntos = 0;
        private double costoCompra = 0;
        private double costoCompraG = 0;
        private double puntosG = 0;
        private int idTickerG = -1;
        private ObservableCollection<Venta> listaVenta;
        
        public Pago_Venta(int idSucursal, string codigoMascota, Double costo, ObservableCollection<Venta> listaCarrito)
        {
            InitializeComponent();

            DataTable mascotasTbl = new DataTable();
            DataTable promocionesDueno = new DataTable();
            idSucursalG = idSucursal;
            idTickerG = -1;
            listaVenta = listaCarrito;
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

            if (puntos > 0)
            {
                lblPuntos.IsVisible = true;
                lblSubTotal.IsVisible = true;
                txtPuntosAplicar.IsVisible = true;
                lblDeseaAplicar.IsVisible = true;
                lblLabelSubtotal.IsVisible = true;
                btnAplicar.IsVisible = true;
            } else {
                lblPuntos.IsVisible = false;
                lblSubTotal.IsVisible = false;
                txtPuntosAplicar.IsVisible = false;
                lblDeseaAplicar.IsVisible = false;
                lblLabelSubtotal.IsVisible = false;
                btnAplicar.IsVisible = false;
            }

            costoCompra = costo;
            costoCompraG = costo;
            puntosG = puntos;
            lblSubTotal.Text = costo.ToString();
            lblTotal.Text = costo.ToString();
            lblPuntos.Text = "Tienes "+puntos.ToString()+" puntos";
        }

        async void onAplicar(object sender, EventArgs args)
        {
            try {
                double montoAAplicar = Convert.ToDouble(txtPuntosAplicar.Text);
                if (montoAAplicar < 0) {
                    await DisplayAlert("Error", "Valor invalido", "Ok");
                    return;
                }
                
            }
            catch (Exception ex) {
                await DisplayAlert("Error","Valor invalido","Ok");
                return;
            }

            if (Convert.ToDouble(txtPuntosAplicar.Text) > puntos) {
                await DisplayAlert("Error", "No puedes aplicar más puntos de los que tienes", "Ok");
                return;
            }

            if (Convert.ToDouble(txtPuntosAplicar.Text) > costoCompra) {
                await DisplayAlert("Error", "No se puede aplicar una cantidad de puntos mayor al total", "Ok");
                return;
            }

            costoCompra = costoCompraG - Convert.ToDouble(txtPuntosAplicar.Text);
            puntos = puntosG - Convert.ToDouble(txtPuntosAplicar.Text);

            lblApplied.IsVisible = true;
            lblPuntosApplied.IsVisible = true;

            lblPuntosApplied.Text = txtPuntosAplicar.Text;
            lblTotal.Text = costoCompra.ToString();
            lblPuntos.Text = "Tienes " + puntos.ToString() + " puntos";
            lblSubTotal.TextDecorations = TextDecorations.Strikethrough;
            lblLabelSubtotal.TextDecorations = TextDecorations.Strikethrough;
        }

        async void onCancelPuntos(object sender, EventArgs args) {
            lblSubTotal.TextDecorations = TextDecorations.None;
            lblLabelSubtotal.TextDecorations = TextDecorations.None;

            lblSubTotal.Text = costoCompraG.ToString();
            lblTotal.Text = costoCompraG.ToString();
            costoCompra = costoCompraG;
            puntos = puntosG;
            lblApplied.IsVisible = false;
            lblPuntosApplied.IsVisible = false;
            lblPuntos.Text = "Tienes " + puntos.ToString() + " puntos";
        }
            
        async void onPagar(object sender, EventArgs args) {
            idTickerG = -1;
            foreach (Venta venta in listaVenta) {
                DependencyService.Get<IWebService>().agregar_venta(idTickerG, idMascota, idSucursalG, venta.idProducto, venta.idServicio, venta.cantidad, venta.precio, out int idTicketOut, out int ventaResult); //
                idTickerG = idTicketOut;
            }

            bool status = DependencyService.Get<IWebService>().ticketPaga(idMascota, idSucursalG, idTickerG, Convert.ToDecimal(txtPuntosAplicar.Text));
            if (status)
            {
                await DisplayAlert("Ok","Pago procesado correctamente","Ok");
                await Navigation.PopAsync();
            } else {
                await DisplayAlert("Ok", "Error al procesar pago", "Ok");
            }
        }

    }
}
