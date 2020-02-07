using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
                var items = await ContactsStore.GetContactsAsync();
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
                var sms = await _readSmsService.ReadSms("AGM", new DateTime(2020, 2, 7));
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
    }
}