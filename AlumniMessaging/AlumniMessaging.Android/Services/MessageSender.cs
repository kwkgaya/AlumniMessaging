using System.Threading.Tasks;
using AlumniMessaging.Services;
using Android.Telephony;

namespace AlumniMessaging.Droid.Services
{
    public class MessageSender : IMessageSender
    {
        public Task<bool> Send(string recipient, string text)
        {
            SmsManager.Default.SendTextMessage(recipient, null, text, null, null);

            return Task.FromResult(true);
        }
    }
}