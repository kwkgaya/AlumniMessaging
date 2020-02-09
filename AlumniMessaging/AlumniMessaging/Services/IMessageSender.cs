using System.Threading.Tasks;

namespace AlumniMessaging.Services
{
    public interface IMessageSender
    {
        Task<bool> Send(string message, string receiver);
        Task<string> Send(string message, string[] receivers);
    }
}
