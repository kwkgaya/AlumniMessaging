using System.Threading.Tasks;
using System.Windows.Input;
using AlumniMessaging.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniMessaging.ViewModels
{
    public class SendMessageViewModel : BaseViewModel
    {
        private readonly ContactsViewModel _contactsVm;
        private readonly IMessageSender _sender;
        private string _text;

        public ICommand SendToAllCommand { get; }

        public string MessageText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public SendMessageViewModel(ContactsViewModel contactsVm, IMessageSender sender)
        {
            _contactsVm = contactsVm;
            _sender = sender;
            Title = "Send Messages";
            SendToAllCommand = new Command(
                async () => await SendMessageToAll(), 
                () => !string.IsNullOrWhiteSpace(MessageText));
        }

        private async Task SendMessageToAll()
        {
            foreach (var contact in _contactsVm.Contacts)
            {
                var salutation = string.IsNullOrWhiteSpace(contact.Name) ? "Sir/Madam" : contact.Name;
                var text = MessageText.Replace("@Name", salutation);
                await _sender.Send(contact.Mobile, text);

                //var message = new SmsMessage(text, contact.Mobile);
                //await Sms.ComposeAsync(message);
            }
        }
    }
}