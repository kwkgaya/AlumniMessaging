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
        private readonly char[] _digits = new[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
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
                var mergedContacts = MergeContacts(newContacts, oldContacts);
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
            var text = message.Text.Trim();
            if (string.IsNullOrEmpty(text))
                return new Contact {Mobile = message.Sender};

            var name = string.Empty;
            int batch = 0;
            try
            {
                text = text.Remove(0, 3).Trim();
                var indexOfBatch = text.IndexOfAny(_digits);
                if (indexOfBatch > 0)
                {
                    name = text.Substring(0, indexOfBatch).Trim();
                    int.TryParse(text.Substring(indexOfBatch), out batch);
                }
                else
                {
                    name = text;
                }
            }
            catch
            {
                // ignore Parse errors. Just use the sender
            }

            var contact = new Contact
            {
                Batch = batch,
                Mobile = message.Sender,
                Name = name
            };
            return contact;
        }

        public IEnumerable<Contact> MergeContacts(IEnumerable<Contact> first, IEnumerable<Contact> second)
        {
            return first.Union(second);
        }
    }
}