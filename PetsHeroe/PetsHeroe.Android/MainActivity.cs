using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using Plugin.CurrentActivity;
using ZXing.Mobile;
using Android.Content;
using Android.Content.Res;
using System.Threading.Tasks;
using Android.Widget;

namespace PetsHeroe.Droid
{
    [Activity(Label = "PetsHeroe", Icon = "@drawable/icono_app", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        private static bool _hasEverInited;

        protected override void OnCreate(Bundle savedInstanceState)
        {

            base.OnCreate(savedInstanceState);

            if (!_hasEverInited)
            {
                TabLayoutResource = Resource.Layout.Tabbar;
                ToolbarResource = Resource.Layout.Toolbar;

                AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;
                TaskScheduler.UnobservedTaskException += TaskSchedulerOnUnobservedTaskException;

                Plugin.InputKit.Platforms.Droid.Config.Init(this, savedInstanceState);
                Xamarin.Essentials.Platform.Init(this, savedInstanceState);
                //Xamarin.Forms.Forms.Init(this, savedInstanceState);
                global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
                global::ZXing.Net.Mobile.Forms.Android.Platform.Init();
                MobileBarcodeScanner.Initialize(Application);
                Xamarin.FormsMaps.Init(this, savedInstanceState);
                CrossCurrentActivity.Current.Init(this, savedInstanceState);
                LeoJHarris.FormsPlugin.Droid.EnhancedEntryRenderer.Init(this);

                _hasEverInited = true;

            }
            LoadApplication(new App());
        }

        private void TaskSchedulerOnUnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs unobservedTaskExceptionEventArgs)
        {
            var newExc = new Exception("TaskSchedulerOnUnobservedTaskException", unobservedTaskExceptionEventArgs.Exception);
            LogUnhandledException(newExc);
        }

        private void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs unhandledExceptionEventArgs)
        {
            var newExc = new Exception("CurrentDomainOnUnhandledException", unhandledExceptionEventArgs.ExceptionObject as Exception);
            LogUnhandledException(newExc);
        }

        private void LogUnhandledException(Exception exception)
        {
            try
            {
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("Confirm delete");
                alertDiag.SetMessage("Once deleted the move cannot be undone");
                alertDiag.SetPositiveButton("Delete", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
            }catch
            {
                AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);
                alertDiag.SetTitle("Confirm delete");
                alertDiag.SetMessage("Once deleted the move cannot be undone");
                alertDiag.SetPositiveButton("Delete", (senderAlert, args) =>
                {
                    Toast.MakeText(this, "Deleted", ToastLength.Short).Show();
                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            try
            {
                global::ZXing.Net.Mobile.Android.PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                Plugin.Permissions.PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
                base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            }
            catch (Exception ex) {

            }
        }
        
        protected override void AttachBaseContext(Context @base)
        {
            var configuration = new Configuration(@base.Resources.Configuration);

            configuration.FontScale = 1f;
            var config = Application.Context.CreateConfigurationContext(configuration);

            base.AttachBaseContext(config);
        }

    }
}