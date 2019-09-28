using System;
using System.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Consulta_bene_vete : TabbedPage
    {
        private int tipoBusqueda = -1;
        private DataTable clientes = new DataTable();

        public Consulta_bene_vete()
        {
            InitializeComponent();

            pkrBuscarPor.SelectedIndexChanged += (object sender, EventArgs args) =>
            {
                tipoBusqueda = pkrBuscarPor.SelectedIndex;
            };

            clientes.Clear();
            clientes.Columns.Add("codigo");
            clientes.Columns.Add("correo");
            clientes.Columns.Add("nombre");

            DataRow dr = clientes.NewRow();
            dr["codigo"] = "AF099403";
            dr["correo"] = "brayan.gutierrez@dotech.com.mx";
            dr["nombre"] = "Brayan Jesus Gutierrez Esparza";
            clientes.Rows.Add(dr);

            DataRow dr2 = clientes.NewRow();
            dr["codigo"] = "000KL984";
            dr["correo"] = "miguel.carreon@dotech.com.mx";
            dr["nombre"] = "Miguel Angel Carreon Viera";
            clientes.Rows.Add(dr2);
        }

        public void onBuscar(object sender, EventArgs args) {
            if (tipoBusqueda < 0) {
                DisplayAlert("Error", "Seleccionar el tipo de busqueda", "Ok");
                return;
            } else if (txtBuscar.Text is null || txtBuscar.Text.ToString() == "") {
                DisplayAlert("Error","Ingresa lo que deseas buscar","OK");
                return;
            }

            DisplayAlert("OK","Encontrados: ","OK");

        }

    }
}
