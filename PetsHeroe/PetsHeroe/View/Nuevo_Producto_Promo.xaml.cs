using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace PetsHeroe.View
{
    public partial class Nuevo_Producto_Promo : ContentPage
    {
        IQRScanning scanningDepen;
        private Dictionary<string, int> tipoProductoDic = new Dictionary<string, int>();
        private Dictionary<string, int> marcaProductoDic = new Dictionary<string, int>();
        private Dictionary<string, int> ProductosDic = new Dictionary<string, int>();
        private int idAsociado = -1;
        private int idTipoProducto = -1;
        private int idMarcaProducto = -1;

        public Nuevo_Producto_Promo(bool isEdit, Promocion promocionEdit)
        {
            InitializeComponent();

            idAsociado = Preferences.Get("idAsociado", -1);

            scanningDepen = DependencyService.Get<IQRScanning>();

            DataTable tipoProducto = new DataTable();

            tipoProducto = DependencyService.Get<IWebService>().getTipoProducto_Busca();

            pkrTipo.Items.Clear();
            tipoProductoDic.Clear();
            foreach (DataRow dr in tipoProducto.Rows)
            {
                pkrTipo.Items.Add(dr[2].ToString());
                tipoProductoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrTipo.SelectedIndexChanged += tipoProductoSeleccionado;

            if (isEdit) {
                try
                {
                    pkrTipo.SelectedItem = promocionEdit.tipo;
                    //int indexTipo = tipoProductoDic[promocionEdit.tipo];
                    //pkrTipo.SelectedIndex = indexTipo;
                    pkrMarcar.SelectedItem = promocionEdit.marca;
                    //int indexMarca = marcaProductoDic[promocionEdit.marca];
                    //pkrMarcar.SelectedIndex = indexMarca;
                    pkrProducto.SelectedItem = promocionEdit.nombre;
                    //int indexProdcuto = ProductosDic[promocionEdit.nombre];
                    //pkrProducto.SelectedIndex = indexProdcuto;
                }
                catch (Exception ex) {

                }
                txtNombrePromo.Text = promocionEdit.nombre.ToString();
                txtPrecio.Text = promocionEdit.precio.ToString();
                txtDineroElectr.Text = promocionEdit.puntos.ToString();
                txtBuscarE.Text = promocionEdit.UPC;
                dpkrAPartir.Date = Convert.ToDateTime(promocionEdit.inicia);
                dpkrHasta.Date = Convert.ToDateTime(promocionEdit.vigencia);
            }

        }

        private void tipoProductoSeleccionado(object sender, EventArgs e)
        {

            idTipoProducto = tipoProductoDic[pkrTipo.SelectedItem.ToString()];

            DataTable marcaProducto = new DataTable();
            marcaProducto = DependencyService.Get<IWebService>().getMarcaProducto_Busca();

            pkrMarcar.Items.Clear();
            marcaProductoDic.Clear();

            foreach (DataRow dr in marcaProducto.Rows)
            {
                pkrMarcar.Items.Add(dr[2].ToString());
                marcaProductoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
            }

            pkrMarcar.SelectedIndexChanged += PkrMarcar_SelectedIndexChanged;

        }

        private void PkrMarcar_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                idMarcaProducto = marcaProductoDic[pkrMarcar.SelectedItem.ToString()];
            }
            catch (Exception ex) {
                idMarcaProducto = -1;
            }

            DataTable productos = new DataTable();
            productos = DependencyService.Get<IWebService>().getProducto_Busca(idAsociado, idTipoProducto, idMarcaProducto);

            pkrProducto.Items.Clear();
            ProductosDic.Clear();

            foreach (DataRow dr in productos.Rows)
            {
                pkrProducto.Items.Add(dr[11].ToString());
                ProductosDic.Add(dr[11].ToString(), Convert.ToInt32(dr[2]));
            }

            pkrProducto.SelectedIndexChanged += PkrProducto_SelectedIndexChanged;

        }

        private void PkrProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        public async void onEscanear(object sender, EventArgs args) {
            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    txtBuscarE.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtBuscarE.Text = "";
            }
        }

        public async void onAgregar(object sender, EventArgs args) {

            bool status = DependencyService.Get<IWebService>().promoProductos_Agrega(-1, -1, "", 0, 0, DateTime.Now, DateTime.Now, 0, 0);
            if (status) {
                await DisplayAlert("OK","Se agrego correctamente","Ok");
                pkrTipo.SelectedIndex = 0;
                pkrMarcar.SelectedIndex = 0;
                pkrProducto.SelectedIndex = 0;
            }
            else
            {

            }
            
        }
    }
}
