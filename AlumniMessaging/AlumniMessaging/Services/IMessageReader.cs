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
        private string _sender;

        public string Sender
        {
            get => _sender;
            set => _sender = value?.Trim().Replace("+94", "0");
        }

        public string Text { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is ReceivedTextMessage message)) return false;
            return string.Equals(Sender, message.Sender);
        }

        public override int GetHashCode()
        {
            return Sender.GetHashCode();
        }
    }
}