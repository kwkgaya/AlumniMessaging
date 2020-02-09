using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;

namespace AlumniMessaging.Droid.Services
{
    public static class PermissionRequester
    {
        private const int RequestIdMultiplePermissions = 1;

        public static bool CheckAndRequestPermissions(string permission)
        {
            var activity = (Activity)App.ServiceContainer.GetInstance(typeof(Activity));

            var result = ContextCompat.CheckSelfPermission(activity, permission);

            if (result != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new[] { permission },
                    RequestIdMultiplePermissions);
                return false;
            }
            return true;
        }
    }
}