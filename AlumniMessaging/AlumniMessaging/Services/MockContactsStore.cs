using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniMessaging.Models;

namespace AlumniMessaging.Services
{
    public class MockContactsStore : IContactsStore
    {
        private readonly List<Contact> _contacts;

        public MockContactsStore()
        {
            _contacts = new List<Contact>()
            {
                new Contact { Name = "First  Contact", Batch = 1, Mobile = "0716475048"},
                new Contact { Name = "Second Contact", Batch = 2, Mobile = "077306716"},
                new Contact { Name = "Third  Contact", Batch = 3, Mobile = "012345789"},
                new Contact { Name = "Fourth Contact", Batch = 4, Mobile = "012345689"},
                new Contact { Name = "Fifth  Contact", Batch = 5, Mobile = "012345679"},
                new Contact { Name = "Sixth  Contact", Batch = 6, Mobile = "012345678"}
            };
        }

        public Task<bool> AddContactAsync(Contact contact)
        {
            _contacts.Add(contact);

            return Task.FromResult(true);
        }

        public Task OverwriteContacts(IEnumerable<Contact> mergedContacts)
        {
            _contacts.Clear();
            _contacts.AddRange(mergedContacts);

            return Task.CompletedTask;
        }

        public Task<List<Contact>> GetContacts()
        {
            return Task.FromResult(_contacts.ToList());
        }
    }
}