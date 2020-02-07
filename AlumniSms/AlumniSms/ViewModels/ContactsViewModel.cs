using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using AlumniSms.Models;
using AlumniSms.Services;

namespace AlumniSms.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        private readonly IReadSmsService _readSmsService;
        public ObservableCollection<Contact> Contacts { get; set; }
        public Command LoadContactsCommand { get; }
        public Command ReadSmsCommand { get; }
        
        public ContactsViewModel(IContactsStore contactsStore, IReadSmsService readSmsService) : base(contactsStore)
        {
            _readSmsService = readSmsService;
            Title = "Contacts";
            Contacts = new ObservableCollection<Contact>();

            LoadContactsCommand = new Command(async () => await ExecuteLoadContactsCommand());
            ReadSmsCommand = new Command(async () => await ExecuteReadSmsCommand());
        }

        async Task ExecuteLoadContactsCommand()
        {
            ReadSmsCommand.Execute(null);

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
        async Task ExecuteReadSmsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var allSms = await _readSmsService.ReadSms("AGM", new DateTime(2020, 2, 7));
                var newContacts = ParseContactsFromSms(allSms);
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

        private List<Contact> ParseContactsFromSms(IEnumerable<ReceivedSms> allSms)
        {
            var newContacts = new List<Contact>();

            foreach (var receivedSms in allSms)
            {
                var text = receivedSms.Text.Remove(0, 3);
                var name = string.Empty;
                if (int.TryParse(text, out int batch))
                {
                    name = text.Substring(0, text.IndexOf(batch.ToString(), StringComparison.OrdinalIgnoreCase));
                }
                var contact = new Contact
                {
                    Id = Guid.NewGuid(),
                    Batch = batch,
                    Mobile = receivedSms.Sender,
                    Name = name
                };
                newContacts.Add(contact);
            }

            return newContacts;
        }
    }
}