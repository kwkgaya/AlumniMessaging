using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniSms.Models;

namespace AlumniSms.Services
{
    public class ContactsDataStore : IContactsStore
    {
        readonly IContactsStore _tempStore = new MockContactsStore();

        public Task<bool> AddContactAsync(Contact contact)
        {
            return _tempStore.AddContactAsync(contact);
        }

        public Task<IEnumerable<Contact>> GetContactsAsync()
        {
            return _tempStore.GetContactsAsync();
        }
    }
}