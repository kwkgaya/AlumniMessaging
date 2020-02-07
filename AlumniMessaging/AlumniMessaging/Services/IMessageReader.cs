using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlumniMessaging.Services
{
    public interface IMessageReader
    {
        Task<IEnumerable<ReceivedTextMessage>> ReadMessage(string startTag, DateTime fromDate);
    }
    public class ReceivedTextMessage
    {
        public string Sender { get; set; }
        public string Text { get; set; }
    }
}