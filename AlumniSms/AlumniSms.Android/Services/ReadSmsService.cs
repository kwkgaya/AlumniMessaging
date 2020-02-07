using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniSms.Services;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Java.Security;
using Permission = Android.Content.PM.Permission;
using Uri = Android.Net.Uri;

namespace AlumniSms.Droid.Services
{
    [GrantUriPermission(Path = Inbox)]
    public class ReadSmsService : IReadSmsService
    {
        private readonly Context _context;
        private const int RequestIdMultiplePermissions = 1;
        const string Inbox = "content://sms/inbox";

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
            var cursor = _context.ContentResolver.Query(Uri.Parse(Inbox), null, null, null, null);

            if (cursor.MoveToFirst())
            {
                // must check the result to prevent exception
                do
                {
                    string msgData = "";
                    for (int idx = 0; idx < cursor.ColumnCount; idx++)
                    {
                        msgData += " " + cursor.GetColumnName(idx) + ":" + cursor.GetString(idx);
                    }

                    smsList.Add(new ReceivedSms { Sender = "", Text = msgData });
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