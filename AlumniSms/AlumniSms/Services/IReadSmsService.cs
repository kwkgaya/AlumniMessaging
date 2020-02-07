using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlumniSms.Services
{
    public interface IReadSmsService
    {
        Task<IEnumerable<ReceivedSms>> ReadSms(string startTag, DateTime fromDate);
    }

    public class ReceivedSms
    {
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}
