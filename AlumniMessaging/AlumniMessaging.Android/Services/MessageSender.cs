using System.Threading.Tasks;
using AlumniMessaging.Services;
using Android.Telephony;

namespace AlumniMessaging.Droid.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly IPermissionRequest _permissionRequest;

        public MessageSender(IPermissionRequest permissionRequest)
        {
            _permissionRequest = permissionRequest;
        }

        public Task<bool> Send(string recipient, string text)
        {
            _permissionRequest.CheckAndRequestPermissions(Permission.SendSms);

            SmsManager.Default.SendTextMessage(recipient, null, text, null, null);

            return Task.FromResult(true);
        }
    }
}