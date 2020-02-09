using AlumniMessaging.Services;
using Android.App;
using Android.Support.V4.App;
using Android.Support.V4.Content;

namespace AlumniMessaging.Droid.Services
{
    public class PermissionRequester : IPermissionRequest
    {
        private const int RequestIdMultiplePermissions = 1;

        public bool CheckAndRequestPermissions(string permission)
        {
            var activity = (Activity)App.ServiceContainer.GetInstance(typeof(Activity));

            var result = ContextCompat.CheckSelfPermission(activity, permission);

            if (result != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new[] { permission },
                    RequestIdMultiplePermissions);
                return false;
            }
            return true;
        }
    }
}