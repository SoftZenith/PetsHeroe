using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using PetsHeroe.Droid.mx.com.petshero;
using System.Data;

namespace PetsHeroe.Droid
{
    [Activity(Label = "PetsHeroe", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());

            wsPetsApp wsPets = new wsPetsApp();
            AuthHeader auth = new AuthHeader() {
                Usuario = "appcelmypets2019",
                Password = "RRW7G0ZiF4D1bUasqazmTg",
                IDUsuario = 0,
                IPAddress = "0"
            };
            wsPets.AuthHeaderValue = auth;
            wsPets.TipoAsociado_BuscaAsync();
            wsPets.TipoAsociado_BuscaCompleted += tipoAsociadoBuscaComplete;
        }

        public void tipoAsociadoBuscaComplete(object sender, TipoAsociado_BuscaCompletedEventArgs e) {
            Console.WriteLine("Columnas: "+ e.Result.Columns.Count);
            Console.WriteLine("Filas: "+ e.Result.Rows.Count);
            DataTable datos = new DataTable();
            datos = e.Result.Copy();
            foreach (DataRow dr in datos.Rows) {
                foreach (DataColumn dc in datos.Columns) {
                    Console.WriteLine(dc.ColumnName +": "+ dr[dc.Ordinal]);
                }
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}