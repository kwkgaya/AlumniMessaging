using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

using Xamarin.Forms;

using AlumniSms.Models;

namespace AlumniSms.ViewModels
{
    public class ContactsViewModel : BaseViewModel
    {
        public ObservableCollection<Contact> Contacts { get; set; }
        public Command LoadContactsCommand { get; set; }

        public ContactsViewModel()
        {
            Title = "Contacts";
            Contacts = new ObservableCollection<Contact>();
            LoadContactsCommand = new Command(async () => await ExecuteLoadContactsCommand());
        }

        async Task ExecuteLoadContactsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Contacts.Clear();
                var items = await DataStore.GetContactsAsync(true);
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
    }
}