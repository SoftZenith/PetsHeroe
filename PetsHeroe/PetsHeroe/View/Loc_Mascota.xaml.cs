using System;
using PetsHeroe.Services;
using PetsHeroe.View;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Loc_Mascota : ContentPage
    {
        private int opcion = 0;
        IWebService wsDependency;
        IQRScanning scanningDepen;
        private MediaFile _mediaFile;
        private string URL { get; set; }

        public Loc_Mascota()
        {
            InitializeComponent();
            if (!CrossConnectivity.Current.IsConnected)
            {
                DisplayAlert("Error", "No estás conectado a internet", "Ok");
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += MyHandler;

            wsDependency = DependencyService.Get<IWebService>();
            scanningDepen = DependencyService.Get<IQRScanning>();
            txtCodigo.Text = "";
            rdOpcion.SelectedItemChanged += onOpcionSeleccionada;
        }

        void onOpcionSeleccionada(object sender, EventArgs args) {
            opcion = rdOpcion.SelectedIndex;
            if (true)
            {
                btnTakePhoto.IsVisible = false;
            }
            else {
                btnTakePhoto.IsVisible = true;
            }
        }

        public async void onScan(object sender, EventArgs args) {

            var status = await CrossPermissions.Current.RequestPermissionsAsync(Permission.Camera);

            var cameraStatus = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
            if (cameraStatus != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
            {
                await DisplayAlert("Error","La app no tiene permisos para utilizar la camara","OK");
                return;
            }

            try
            {
                var result = await scanningDepen.ScanAsync();
                if (result != null)
                {
                    result = result.Replace("=","");
                    txtCodigo.Text = result;
                }
            }
            catch (Exception ex)
            {
                txtCodigo.Text = "";
            }
        }

        async void onSiguiente(object sender, EventArgs args) {

            string codigo;
            try { 
                codigo = txtCodigo.Text;
            }catch (Exception){
                codigo = "";
            }

            bool valido = false;
            try
            {
                wsDependency.getCodigo_Valida(codigo); //call function to verify code
                valido = wsDependency.Codigo_Valida; //get result from code verified 
            }
            catch (Exception) {
                await DisplayAlert("Error", "Hubo un error al verificar el código", "OK");
            }

            if (!valido) {
                await DisplayAlert("Código no valido", "Código no valido", "OK");
                return;
            }

            switch (opcion) {
                case 0:
                    try
                    {
                        _ = Navigation.PushAsync(new Llevar_centro(codigo)); //Display "Llevar a CAM" screen
                    }
                    catch (Exception ex) {
                        await DisplayAlert("Error","Opción no disponible actualmente "+ex,"OK");
                    }
                    break;
                case 1:
                    try
                    {
                        _ = Navigation.PushAsync(new Mensaje_Dueno(codigo));
                    }
                    catch (Exception) {
                        await DisplayAlert("Error", "Opción no disponible actualmente", "OK"); //Display ""
                    }
                    break;
                case 2:
                    try
                    {
                        _ = Navigation.PushAsync(new Tomar_Nota(codigo));
                    }
                    catch (Exception) {
                        
                    }
                    break;
                default:
                    await DisplayAlert("Seleccionado", "Ninguno seleccionado", "OK");
                    break;
            }
        }

        async void onTakePhoto(object sender, EventArgs args) {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await DisplayAlert("Error", "La aplicación no tiene permisos para acceder a la cámara", "OK");
                return;
            }
            else
            {
                _mediaFile = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "myImage.jpg"
                });

                if (_mediaFile == null) return;
                //imageView.Source = ImageSource.FromStream(() => _mediaFile.GetStream());
                var mediaOption = new PickMediaOptions()
                {
                    PhotoSize = PhotoSize.Medium
                };
                //UploadedUrl.Text = "Image URL:";
            }
        }

        public void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            DisplayAlert("Error", "No tienes conexión a internet", "Ok");
        }

    }
}
