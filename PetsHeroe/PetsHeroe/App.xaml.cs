using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;

namespace PetsHeroe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new MainPage();
            MainPage = new NavigationPage(new MainPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            AppCenter.Start("android=9e332103-ef2c-4245-833c-cb6c73858cda;" +
                  "ios=3b7d4718-790b-4c17-b398-ca875b3fe71d",
                  typeof(Analytics), typeof(Crashes));
            CrossConnectivity.Current.ConnectivityChanged += HandleConnectivityChanged;
        }

        private void HandleConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (!CrossConnectivity.Current.IsConnected)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Sin conexión a internet", "OK");
            }
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
