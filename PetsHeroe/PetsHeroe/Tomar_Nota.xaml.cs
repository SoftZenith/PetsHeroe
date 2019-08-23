using System;
using System.Collections.Generic;
using System.Data;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Tomar_Nota : ContentPage
    {
        private DataTable estados;
        private DataTable ciudades;
        private int idEstado = 0;
        private int idCiudad = 0;
        //Dictionarios para guardar nombre - id
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Tomar_Nota()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getEstado_Busca();
                estados = DependencyService.Get<IAndroid>().Estado_Busca;
            } else if(Device.RuntimePlatform == Device.iOS){
                DependencyService.Get<IIOS>().getEstado_Busca();
                estados = DependencyService.Get<IIOS>().Estado_Busca;
            }

            pkrEstado.Items.Clear();
            estadoDic.Clear();
            foreach (DataRow dr in estados.Rows)
            {
                pkrEstado.Items.Add(dr[5].ToString());
                estadoDic.Add(dr[5].ToString(), Convert.ToInt32(dr[1]));
            }

            pkrEstado.SelectedIndexChanged += PkrEstado_SelectedIndexChanged;   

        }

        private void PkrEstado_SelectedIndexChanged(object sender, EventArgs e)
        {
            idEstado = estadoDic[pkrEstado.SelectedItem.ToString()];

            if (Device.RuntimePlatform == Device.Android)
            {
                DependencyService.Get<IAndroid>().getCiudad_Busca(idEstado);
                ciudades = DependencyService.Get<IAndroid>().Ciudad_Busca;
            } else if (Device.RuntimePlatform == Device.iOS) {
                DependencyService.Get<IIOS>().getCiudad_Busca(idEstado);
                ciudades = DependencyService.Get<IIOS>().Ciudad_Busca;
            }

            pkrMunicipio.Items.Clear();
            ciudadDic.Clear();
            foreach (DataRow dr in ciudades.Rows) {
                pkrMunicipio.Items.Add(dr[8].ToString());
                ciudadDic.Add(dr[8].ToString(), Convert.ToInt32(dr[2]));
            }

            pkrMunicipio.SelectedIndexChanged += (object send, EventArgs ev) => {

            };

        }
    }
}
