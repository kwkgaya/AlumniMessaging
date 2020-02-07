using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using AlumniMessaging.Models;
using AlumniMessaging.Services;

namespace AlumniMessaging.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private readonly IMessageReader _messageReaderService;
        public ObservableCollection<Contact> Contacts { get; set; }
        public Command LoadContactsCommand { get; }
        public Command ReadMessagesCommand { get; }
        
        public ContactsViewModel(IContactsStore contactsStore, IMessageReader messageReaderService) : base(contactsStore)
        {
            _messageReaderService = messageReaderService;
            Title = "Contacts";
            Contacts = new ObservableCollection<Contact>();

            LoadContactsCommand = new Command(async () => await ExecuteLoadContactsCommand());
            ReadMessagesCommand = new Command(async () => await ExecuteReadMessagesCommand());
        }

        async Task ExecuteLoadContactsCommand()
        {
            ReadMessagesCommand.Execute(null);

            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Contacts.Clear();
                var items = await ContactsStore.GetContacts();
                foreach (var item in items)
                {
                    Contacts.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async Task ExecuteReadMessagesCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var messages = await _messageReaderService.ReadMessage("AGM", new DateTime(2020, 2, 7));
                var newContacts = ParseContactsFromMessage(messages);
                var oldContacts = await ContactsStore.GetContacts();
                var mergedContacts = newContacts.Union(oldContacts);
                await ContactsStore.OverwriteContacts(mergedContacts);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private List<Contact> ParseContactsFromMessage(IEnumerable<ReceivedTextMessage> messages)
        {
            var newContacts = new List<Contact>();

            foreach (var message in messages)
            {
                var contact = ParseContact(message);
                newContacts.Add(contact);
            }

            return newContacts;
        }

        public Contact ParseContact(ReceivedTextMessage message)
        {
            var text = message.Text.Remove(0, 3);
            var name = string.Empty;
            if (int.TryParse(text, out int batch))
            {
                name = text.Substring(0, text.IndexOf(batch.ToString(), StringComparison.OrdinalIgnoreCase));
            }
            var contact = new Contact
            {
                Id = Guid.NewGuid(),
                Batch = batch,
                Mobile = message.Sender,
                Name = name
            };
            return contact;
        }
    }
}