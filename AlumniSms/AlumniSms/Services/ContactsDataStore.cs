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

        public Task OverwriteContacts(IEnumerable<Contact> mergedContacts)
        {
            return _tempStore.OverwriteContacts(mergedContacts);
        }

        public Task<IEnumerable<Contact>> GetContacts()
        {
            return _tempStore.GetContacts();
        }
    }
}