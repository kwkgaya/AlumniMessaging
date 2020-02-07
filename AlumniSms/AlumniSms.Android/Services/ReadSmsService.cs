using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniSms.Services;
using Android;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Java.Util;
using Permission = Android.Content.PM.Permission;
using Uri = Android.Net.Uri;

namespace AlumniSms.Droid.Services
{
    [GrantUriPermission(Path = Inbox)]
    public class ReadSmsService : IReadSmsService
    {
        private readonly Context _context;
        private const int RequestIdMultiplePermissions = 1;
        private const string Inbox = "content://sms/inbox";
        private const long TicksAtEpoch = 621355968000000000L;
        private const long TicksPerMillisecond = 10000;

        public ReadSmsService(Context context)
        {
            _context = context;
        }

        public Task<IEnumerable<ReceivedSms>> ReadSms(string startTag, DateTime fromDate)
        {
            return Task.Run(() =>
            {
                try
                {
                    return ReadSmsPrivate(startTag, fromDate);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return new List<ReceivedSms>();
                }
            });
        }
        
        private IEnumerable<ReceivedSms> ReadSmsPrivate(string startTag, DateTime fromDateTime)
        {
            CheckAndRequestPermissions();

            var smsList = new List<ReceivedSms>();
            var reqCols = new[] { "_id", "address", "date", "body" };
            var epochMillis = (fromDateTime.Ticks - TicksAtEpoch) / TicksPerMillisecond;
            var cursor = _context.ContentResolver.Query(
                Uri.Parse(Inbox), 
                reqCols, 
                "date > ?", new []{epochMillis.ToString()},
                null);
            
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

                    smsList.Add(new ReceivedSms {Sender = sender, Text = body.Trim() });
                } while (cursor.MoveToNext());
            }

            return smsList;
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