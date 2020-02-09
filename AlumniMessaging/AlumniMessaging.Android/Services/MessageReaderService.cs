using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniMessaging.Services;
using Android.Content;
using Java.Util;
using Uri = Android.Net.Uri;

namespace AlumniMessaging.Droid.Services
{
    [GrantUriPermission(Path = Inbox)]
    public class MessageReaderService : IMessageReader
    {
        private readonly Context _context;
        private readonly IPermissionRequest _permissionRequest;
        private const string Inbox = "content://sms/inbox";
        private const long TicksAtEpoch = 621355968000000000L;
        private const long TicksPerMillisecond = 10000;

        public MessageReaderService(Context context, IPermissionRequest permissionRequest)
        {
            _context = context;
            _permissionRequest = permissionRequest;
        }

        public Task<IEnumerable<ReceivedTextMessage>> ReadMessage(string startTag, DateTime fromDate)
        {
            return Task.Run(() =>
            {
                try
                {
                    var granted = _permissionRequest.CheckAndRequestPermissions(Permission.ReadSms);
                    if (!granted) return Enumerable.Empty<ReceivedTextMessage>();

                    return ReadMessagePrivate(startTag, fromDate);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return Enumerable.Empty<ReceivedTextMessage>();
                }
            });
        }
        
        private IEnumerable<ReceivedTextMessage> ReadMessagePrivate(string startTag, DateTime fromDateTime)
        {
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
    }
}