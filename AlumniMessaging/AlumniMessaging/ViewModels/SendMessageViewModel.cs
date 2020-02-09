using System;
using System.Linq;
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
            try
            {
                //var salutation = string.IsNullOrWhiteSpace(contact.Name) ? "Sir/Madam" : contact.Name;
                //var text = MessageText.Replace("@Name", salutation);
                var recipients = _contactsVm.Contacts.Select(c => c.Mobile).ToArray();
                await _sender.Send(MessageText, recipients);
            }
            catch (InvalidOperationException e) when(e.Message.Contains("too large"))
            {
                //await DisplayAlert("Message too large", e.Message, "OK");
                Console.WriteLine(e);
            }

            //foreach (var contact in _contactsVm.Contacts)
            //{
            //    var salutation = string.IsNullOrWhiteSpace(contact.Name) ? "Sir/Madam" : contact.Name;
            //    var text = MessageText.Replace("@Name", salutation);
            //    await _sender.Send(contact.Mobile, text);
            //}
        }
    }
}