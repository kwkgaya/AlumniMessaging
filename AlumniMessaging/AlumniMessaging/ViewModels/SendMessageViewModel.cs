using System.Threading.Tasks;
using System.Windows.Input;
using AlumniMessaging.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniMessaging.ViewModels
{
    public class SendMessageViewModel : BaseViewModel
    {
        private string _text;

        public ICommand SendToAllCommand { get; }

        public string MessageText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public SendMessageViewModel(IContactsStore contactsStore) : base(contactsStore)
        {
            Title = "Send Messages";
            SendToAllCommand = new Command(async () => await SendMessageToAll());
        }

        private async Task SendMessageToAll()
        {
            foreach (var contact in await ContactsStore.GetContacts())
            {
                var salutation = string.IsNullOrWhiteSpace(contact.Name) ? "Sir/Madam" : contact.Name;
                var text = MessageText.Replace("@Name", salutation);
                var message = new SmsMessage(text, contact.Mobile);
                await Sms.ComposeAsync(message);
            }
        }
    }
}