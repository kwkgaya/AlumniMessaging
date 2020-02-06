using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AlumniSms.Services
{
    public interface IReadSmsService
    {
        Task<IEnumerable<ReceivedSms>> ReadSms(string startTag);
    }

    public class ReceivedSms
    {
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}
