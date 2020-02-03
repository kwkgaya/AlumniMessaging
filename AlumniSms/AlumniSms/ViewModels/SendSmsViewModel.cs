using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniSms.ViewModels
{
    public class SendSmsViewModel : BaseViewModel
    {
        private readonly IEnumerable<Contact> _contacts;
        private string _text;

        public ICommand SendToAllCommand { get; }

        public string SmsText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public SendSmsViewModel()
        {
        }

        public SendSmsViewModel(IEnumerable<Contact> contacts)
        {
            _contacts = contacts;
            Title = "Send Sms";
            SendToAllCommand = new Command(async () => await SendSmsToAll());
        }

        private async Task SendSmsToAll()
        {
            foreach (var contact in _contacts)
            {
                var text = SmsText.Replace("@Name", contact.Name);
                var message = new SmsMessage(text, contact.Mobile);
                await Sms.ComposeAsync(message);
            }
        }
    }
}