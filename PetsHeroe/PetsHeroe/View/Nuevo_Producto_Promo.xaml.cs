using System;
using System.Collections.Generic;
using System.Data;
using PetsHeroe.Model;
using PetsHeroe.Services;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Nuevo_Producto_Promo : ContentPage
    {
        IQRScanning scanningDepen;
        private Dictionary<string, int> tipoProductoDic = new Dictionary<string, int>();
        private Dictionary<string, int> marcaProductoDic = new Dictionary<string, int>();
        private Dictionary<string, int> ProductosDic = new Dictionary<string, int>();
        private int idAsociado = -1;
        private int idTipoProducto = -1;
        private int idMarcaProducto = -1;
        private int idProducto = -1;
        private bool isEditing = false;
        private bool isNewPRoduct = false;
        private Promocion promocionEditing;

        protected override void OnAppearing()
        {
            idAsociado = Preferences.Get("idAsociado", -1);

            scanningDepen = DependencyService.Get<IQRScanning>();

            DataTable tipoProducto = new DataTable();

            if (!isEditing)
            {

                tipoProducto = DependencyService.Get<IWebService>().getTipoProducto_Busca();

                pkrTipo.Items.Clear();
                tipoProductoDic.Clear();
                foreach (DataRow dr in tipoProducto.Rows)
                {
                    pkrTipo.Items.Add(dr[2].ToString());
                    tipoProductoDic.Add(dr[2].ToString(), Convert.ToInt32(dr[0]));
                }

                pkrTipo.SelectedIndexChanged += tipoProductoSeleccionado;
            }

        }

        public Nuevo_Producto_Promo(bool isEdit, Promocion promocionEdit)
        {
            InitializeComponent();
            idAsociado = Preferences.Get("idAsociado", -1);
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

            if (isEdit)
            {
                this.Title = "Editar promoción";
                btnGuardar.Text = "Editar";
            }
            else {
                this.Title = "Agregar promoción";
                btnGuardar.Text = "Guardar";
            }

            isEditing = isEdit;
            promocionEditing = promocionEdit;
            if (isEdit)
            {
                try
                {
                    pkrTipo.SelectedItem = promocionEdit.tipo;
                    pkrMarcar.SelectedItem = promocionEdit.marca;
                    pkrProducto.SelectedItem = promocionEdit.nombre;
                }
                catch (Exception ex)
                {

                }
                txtNombrePromo.Text = promocionEdit.nombre.ToString();
                txtPrecio.Text = promocionEdit.precio.ToString();
                txtDineroElectr.Text = promocionEdit.puntos.ToString();
                dpkrAPartir.Date = DateTime.Now;
                dpkrHasta.Date = Convert.ToDateTime(promocionEdit.vigencia);
            }
        }

        private void tipoProductoSeleccionado(object sender, EventArgs e)
        {
            try
            {
                idTipoProducto = tipoProductoDic[pkrTipo.SelectedItem.ToString()];
            }
            catch (Exception) {

            }
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
                pkrProducto.Items.Add(dr["Name"].ToString());
                try{
                    ProductosDic.Add(dr["Name"].ToString(), Convert.ToInt32(dr["IDProduct"]));
                }catch (Exception ex) {

                }
            }

            pkrProducto.Items.Add("Agregar nuevo producto");

            pkrProducto.SelectedIndexChanged += PkrProducto_SelectedIndexChanged;

        }

        private void PkrProducto_SelectedIndexChanged(object sender, EventArgs e)
        {
            try {
                if (pkrProducto.SelectedItem.ToString() == "Agregar nuevo producto")
                {
                    if (!isNewPRoduct)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var result = await DisplayAlert("Agregar", "¿Deseas agregarlo?", "Si", "No");
                            if (result)
                            {
                                isNewPRoduct = false;
                                await Navigation.PushAsync(new Nuevo_Producto_Venta());
                            }
                        });
                    }
                    isNewPRoduct = true;
                }
                else
                {
                    idProducto = ProductosDic[pkrProducto.SelectedItem.ToString()];
                }
            }
            catch (Exception) {
                idProducto = -1;
            }
        }

        public async void onAgregar(object sender, EventArgs args) {

            if (txtNombrePromo.Text == "" || txtNombrePromo.Text == null) {
                await DisplayAlert("Error", "Ingresa un nombre para la promoción", "Ok");
                return;
            }
            if (dpkrAPartir.Date.Day < DateTime.Now.Day) {
                await DisplayAlert("Error","Fecha \"A partir del\" es menor a fecha actual","Ok");
                return;
            }
            if (dpkrHasta.Date.Day < DateTime.Now.Day) {
                await DisplayAlert("Error","Fecha \"Hasta el\" es menor a la fecha actual","Ok");
                return;
            }
            if(dpkrHasta.Date.Day < dpkrAPartir.Date.Day)
            {
                await DisplayAlert("Error", "Fecha \"Hasta el\" superior a la fecha \"A partir del\"", "Ok");
                return;
            }

            if (idProducto < 0) {
                await DisplayAlert("Error", "Selecciona un producto", "Ok");
                return;
            }

            if (txtPrecio.Text == "" || txtPrecio == null)
            {
                await DisplayAlert("Error", "Ingresa el precio", "Ok");
                return;
            }
            else {
                try {
                    Convert.ToDecimal(txtPrecio.Text);
                }
                catch (Exception) {
                    await DisplayAlert("Error", "Precio invalido", "Ok");
                    return;
                }
            }

            if (txtDineroElectr.Text == "" || txtDineroElectr.Text == null)
            {
                await DisplayAlert("Error", "Ingresa valor de dinero electrónico", "Ok");
                return;
            }
            else {
                try
                {
                    Convert.ToInt32(txtDineroElectr.Text);
                }
                catch (Exception) {
                    await DisplayAlert("Error", "Valor de dinero electrónico invalido", "Ok");
                    return;
                }
            }
            if (isEditing)
            {
                bool status = DependencyService.Get<IWebService>().promoProducto_Edita(promocionEditing.idPromocion, txtNombrePromo.Text, Convert.ToDecimal(txtPrecio.Text), Convert.ToDecimal(txtPrecio.Text), dpkrAPartir.Date, dpkrHasta.Date, Convert.ToInt32(txtDineroElectr.Text), 1, chkActivo.IsChecked);
                if (status)
                {
                    await DisplayAlert("OK", "Se editó correctamente", "Ok");
                    txtNombrePromo.Text = "";
                    dpkrAPartir.Date = DateTime.Today;
                    dpkrHasta.Date = DateTime.Today;
                    txtPrecio.Text = "";
                    txtDineroElectr.Text = "";
                    pkrTipo.SelectedIndex = 0;
                    pkrMarcar.SelectedIndex = 0;
                    await Navigation.PopAsync();
                    //pkrProducto.SelectedIndex = 0;
                }
                else
                {
                    await DisplayAlert("Error", "Intente nuevamente", "Ok");
                }
            }
            else
            {
                Resultado res  = DependencyService.Get<IWebService>().promoProductos_Agrega(idAsociado, idProducto, txtNombrePromo.Text, Convert.ToDecimal(txtPrecio.Text), Convert.ToDecimal(txtPrecio.Text), dpkrAPartir.Date, dpkrHasta.Date, Convert.ToInt32(txtDineroElectr.Text), 1, chkActivo.IsChecked);
                if (res.status)
                {
                    await DisplayAlert("OK", "Se agregó correctamente", "Ok");
                    txtNombrePromo.Text = "";
                    dpkrAPartir.Date = DateTime.Today;
                    dpkrHasta.Date = DateTime.Today;
                    txtPrecio.Text = "";
                    txtDineroElectr.Text = "";
                    pkrTipo.SelectedIndex = 0;
                    pkrMarcar.SelectedIndex = 0;
                    await Navigation.PopAsync();
                    //pkrProducto.SelectedIndex = 0;
                }
                else
                {
                    await DisplayAlert("Error", res.errorMessage, "Ok");
                }
            }
        }
    }
}
