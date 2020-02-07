using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniMessaging.Services;
using Android;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Java.Util;
using Permission = Android.Content.PM.Permission;
using Uri = Android.Net.Uri;

namespace AlumniMessaging.Droid.Services
{
    [GrantUriPermission(Path = Inbox)]
    public class MessageReaderService : IMessageReader
    {
        private readonly Context _context;
        private const int RequestIdMultiplePermissions = 1;
        private const string Inbox = "content://sms/inbox";
        private const long TicksAtEpoch = 621355968000000000L;
        private const long TicksPerMillisecond = 10000;

        public MessageReaderService(Context context)
        {
            _context = context;
        }

        public Task<IEnumerable<ReceivedTextMessage>> ReadMessage(string startTag, DateTime fromDate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ReadMessagePrivate(startTag, fromDate);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<ReceivedTextMessage>();
                }
            });
        }
        
        private IEnumerable<ReceivedTextMessage> ReadMessagePrivate(string startTag, DateTime fromDateTime)
        {
            CheckAndRequestPermissions();

            var messageSet = new HashSet<ReceivedTextMessage>();
            var reqCols = new[] { "_id", "address", "date", "body" };
            var epochMillis = (fromDateTime.Ticks - TicksAtEpoch) / TicksPerMillisecond;
            var cursor = _context.ContentResolver.Query(
                Uri.Parse(Inbox), 
                reqCols, 
                "date > ?", new []{epochMillis.ToString()},
                "date DESC");
            
            if (cursor.MoveToFirst())
            {
                // must check the result to prevent exception
                do
                {
                    var body = cursor.GetString(3);
                    if(!body.Trim().StartsWith(startTag, StringComparison.OrdinalIgnoreCase)) continue;

                    var sender = cursor.GetString(1);
                    var dateMillis = cursor.GetLong(2);
                    var receivedDate = new Date(dateMillis);

                    messageSet.Add(new ReceivedTextMessage {Sender = sender, Text = body.Trim() });
                } while (cursor.MoveToNext());
            }

            return messageSet;
        }
        private bool CheckAndRequestPermissions()
        {
            var permission = ContextCompat.CheckSelfPermission(_context, Manifest.Permission.ReadSms);

            if (permission != Permission.Granted)
            {
                ActivityCompat.RequestPermissions((Activity)_context, new String[] { Manifest.Permission.ReadSms },
                    RequestIdMultiplePermissions);
                return false;
            }
            return true;
        }
    }
}