using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniSms.Services;
using Android.Content;
using Android.Net;

namespace AlumniSms.Droid.Services
{
    public class ReadSmsService : IReadSmsService
    {
        private readonly Context _context;

        public ReadSmsService(Context context)
        {
            _context = context;
        }

        const string Inbox = "content://sms/inbox";

        public Task<IEnumerable<ReceivedSms>> ReadSms(string startTag)
        {
            return Task.Run(() =>
            {
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

                        smsList.Add(new ReceivedSms {Sender = "", Text = msgData});
                    } while (cursor.MoveToNext());
                }

                return smsList.AsEnumerable();
            });
        }
    }
}