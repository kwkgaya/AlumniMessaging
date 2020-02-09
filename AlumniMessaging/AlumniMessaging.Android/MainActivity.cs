using AlumniMessaging.Droid.Services;
using AlumniMessaging.Services;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LightInject;

namespace AlumniMessaging.Droid
{
    [Activity(Label = "AlumniMessaging", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            App.ServiceContainer.Register<IMessageReader, MessageReaderService>(new PerContainerLifetime());
            App.ServiceContainer.Register<Context>(f => this);
            App.ServiceContainer.Register<Activity>(f => this);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            PermissionRequester.CheckAndRequestPermissions(Android.Manifest.Permission.WriteExternalStorage);
            PermissionRequester.CheckAndRequestPermissions(Android.Manifest.Permission.ReadSms);
            PermissionRequester.CheckAndRequestPermissions(Android.Manifest.Permission.ReadExternalStorage);
            PermissionRequester.CheckAndRequestPermissions(Android.Manifest.Permission.SendSms);

            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}