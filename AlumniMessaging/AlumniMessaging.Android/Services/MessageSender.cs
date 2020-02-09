using System;
using System.Threading.Tasks;
using AlumniMessaging.Services;
using Android.App;
using Android.Content;
using Android.Telephony;
using Android.Util;
using Java.Util.Concurrent.Atomic;
using Exception = Java.Lang.Exception;

namespace AlumniMessaging.Droid.Services
{
    public class MessageSender : IMessageSender
    {
        private readonly IPermissionRequest _permissionRequest;
        private readonly Context _context;

        public const string INTENT_MESSAGE_SENT = "message.sent";
        public const string INTENT_MESSAGE_DELIVERED = "message.delivered";
        private const string TAG = "SendSMS";

        public MessageSender(IPermissionRequest permissionRequest, Context context)
        {
            _permissionRequest = permissionRequest;
            _context = context;
        }

        public Task<bool> Send(string message, string recipient)
        {
            var granted = _permissionRequest.CheckAndRequestPermissions(Permission.SendSms);
            if (!granted) return Task.FromResult(false);

            SmsManager.Default.SendTextMessage(recipient, null, message, null, null);

            return Task.FromResult(true);
        }

        public Task<string> Send(string message, string[] recipients)
        {
            var granted = _permissionRequest.CheckAndRequestPermissions(Permission.SendSms);
            if (!granted) return Task.FromResult("Permission denied");

            var sm = SmsManager.Default;

            var parts = sm.DivideMessage(message);
            var sentIntent = new Intent(INTENT_MESSAGE_SENT);

            int sentId = IdGenerator.Next();
            var sentPi = PendingIntent.GetBroadcast(_context, sentId, sentIntent, PendingIntentFlags.CancelCurrent);

            var deliveryIntent = new Intent(INTENT_MESSAGE_DELIVERED);
            int deliveredId = IdGenerator.Next();
            var deliveredPi = PendingIntent.GetBroadcast(_context, deliveredId, deliveryIntent, PendingIntentFlags.CancelCurrent);

            Log.Info(TAG, "sending SMS: parts: " + parts.Count + " message: "+ message);
            if (parts.Count > 1)
            {
                throw new InvalidOperationException("Message is too large for one sms");
                #region send parts Commented
                //var sentIntents = new List<PendingIntent>();
                //var deliveredIntents = new List<PendingIntent>();

                //for (int i = 0; i < parts.Count; i++)
                //{
                //    sentIntents.Add(sentPi);
                //    deliveredIntents.Add(deliveredPi);
                //}

                //foreach (var receiver in receivers)
                //{
                //    try
                //    {
                //        sm.SendMultipartTextMessage(receiver, null, parts,
                //                sentIntents, deliveredIntents);
                //    }
                //    catch (Exception e)
                //    {
                //        Log.Error(TAG, e, "Failed to send to receiver: " + receiver);
                //    }
                //}
#endregion send parts
            }
            else
            {
                foreach (var receiver in recipients)
                {
                    try
                    {
                        sm.SendTextMessage(receiver, null, parts[0], sentPi,deliveredPi);
                    }
                    catch (Exception e)
                    {
                        Log.Error(TAG, e, "Failed to send to receiver: " + receiver);
                    }
                }
            }

            return Task.FromResult($"Successfully sent to {recipients.Length} receivers");
        }

        private static class IdGenerator
        {
            private static readonly AtomicInteger Counter = new AtomicInteger();

            public static int Next() => Counter.GetAndIncrement();
        }
    }
}