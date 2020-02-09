using System.Threading.Tasks;

namespace AlumniMessaging.Services
{
    public interface IMessageSender
    {
        Task<bool> Send(string recipient, string text);
    }
}
