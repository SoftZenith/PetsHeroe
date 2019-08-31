using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using PetsHeroe.Model;
using Xamarin.Forms;

namespace PetsHeroe
{
    public partial class Tomar_Nota : ContentPage
    {
        private DataTable estados;
        private DataTable ciudades;
        private int idEstado = -1;
        private int idCiudad = -1;
        private string codigo_pre = "";
        private string nombre = "";
        private string correo = "";
        private string telefono = "";
        private string localizacion = "";
        private string notas = "";
        private double latitud = -1;
        private double longitud = -1;
        //Dictionarios para guardar nombre - id
        Dictionary<string, int> estadoDic = new Dictionary<string, int>();
        Dictionary<string, int> ciudadDic = new Dictionary<string, int>();

        public Tomar_Nota(string codigo)
        {
            InitializeComponent();
            codigo_pre = codigo;
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
                idCiudad = ciudadDic[pkrMunicipio.SelectedItem.ToString()];
            };

        }


        async void onAviso(object sender, EventArgs args) {

            bool estatus = false;

            try
            {
                localizacion = txtLocalizacion.Text;
                notas = txtNotas.Text;
                nombre = txtNombre.Text;
                correo = txtCorreo.Text;
                telefono = txtTelefono.Text;

                int[] enteros = { idEstado, idCiudad };
                string[] textos = { codigo_pre, nombre, correo, telefono, localizacion, notas };

                if (enteros.Any(item => item < 0)) {
                    await DisplayAlert("Error","Faltan campos por llenar","OK");
                    return;
                }

                if (textos.Any(item => item.Trim().Length == 0)) {
                    await DisplayAlert("Error", "Faltan campos por llenar", "OK");
                    return;
                }

                if (Device.RuntimePlatform == Device.Android)
                {
                    estatus = DependencyService.Get<IAndroid>().setEntrega_Localizacion(new MensajeDueno()
                    {
                        codigo = codigo_pre,
                        nombre = nombre,
                        correo = correo,
                        telefono = telefono,
                        localizacion = localizacion,
                        notas = notas,
                        latitud = latitud,
                        longitud = longitud,
                        idCiudad = idCiudad
                    });
                } else if (Device.RuntimePlatform == Device.iOS) {
                    estatus = DependencyService.Get<IIOS>().setEntrega_Localizacion(new MensajeDueno()
                    {
                        codigo = codigo_pre,
                        nombre = nombre,
                        correo = correo,
                        telefono = telefono,
                        localizacion = localizacion,
                        notas = notas,
                        latitud = latitud,
                        longitud = longitud,
                        idCiudad = idCiudad
                    });
                }

                if (estatus)
                {
                    await DisplayAlert("OK", "Se envio tu localización al dueño", "OK");
                }
                else {
                    await DisplayAlert("ERROR", "Algo salio mal enviando tu localización", "OK");
                }

            }catch (Exception ex){
                await DisplayAlert("Error","Faltan campos por llenar","OK");
                Console.WriteLine("Error: " + ex.ToString());
                return;
            }
        }
    }
}
