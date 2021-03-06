﻿using AlumniMessaging.Droid.Services;
using AlumniMessaging.Services;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using LightInject;
using Permission = Android.Content.PM.Permission;

namespace AlumniMessaging.Droid
{
    [Activity(Label = "AlumniMessaging", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            App.ServiceContainer.Register<IMessageReader, MessageReaderService>(new PerContainerLifetime());
            App.ServiceContainer.Register<IMessageSender, MessageSender>(new PerContainerLifetime());
            App.ServiceContainer.Register<IPermissionRequest, PermissionRequester>(new PerContainerLifetime());
            App.ServiceContainer.Register<Context>(f => this);
            App.ServiceContainer.Register<Activity>(f => this);

            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            
            LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}