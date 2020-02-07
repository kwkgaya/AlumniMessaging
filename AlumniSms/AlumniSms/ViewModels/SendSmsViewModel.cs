using System.Threading.Tasks;
using System.Windows.Input;
using AlumniSms.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniSms.ViewModels
{
    public class SendSmsViewModel : BaseViewModel
    {
        private string _text;

        public ICommand SendToAllCommand { get; }

        public string SmsText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public SendSmsViewModel(IContactsStore contactsStore) : base(contactsStore)
        {
            Title = "Send Sms";
            SendToAllCommand = new Command(async () => await SendSmsToAll());
        }

        private async Task SendSmsToAll()
        {
            foreach (var contact in await ContactsStore.GetContactsAsync())
            {
                var text = SmsText.Replace("@Name", contact.Name);
                var message = new SmsMessage(text, contact.Mobile);
                await Sms.ComposeAsync(message);
            }
        }
    }
}