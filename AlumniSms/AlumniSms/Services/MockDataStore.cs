using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlumniSms.Models;

namespace AlumniSms.Services
{
    public class MockDataStore : IDataStore<Contact>
    {
        private readonly List<Contact> _contacts;

        public MockDataStore()
        {
            _contacts = new List<Contact>()
            {
                new Contact { Id = Guid.NewGuid(), Name = "First  Contact", Batch = 1, Mobile = "0716475048"},
                new Contact { Id = Guid.NewGuid(), Name = "Second Contact", Batch = 2, Mobile = "077306716"},
                new Contact { Id = Guid.NewGuid(), Name = "Third  Contact", Batch = 3, Mobile = "012345789"},
                new Contact { Id = Guid.NewGuid(), Name = "Fourth Contact", Batch = 4, Mobile = "012345689"},
                new Contact { Id = Guid.NewGuid(), Name = "Fifth  Contact", Batch = 5, Mobile = "012345679"},
                new Contact { Id = Guid.NewGuid(), Name = "Sixth  Contact", Batch = 6, Mobile = "012345678"}
            };
        }

        public async Task<bool> AddContactAsync(Contact Contact)
        {
            _contacts.Add(Contact);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateContactAsync(Contact Contact)
        {
            var oldContact = _contacts.Where((Contact arg) => arg.Id == Contact.Id).FirstOrDefault();
            _contacts.Remove(oldContact);
            _contacts.Add(Contact);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteContactAsync(Guid id)
        {
            var oldContact = _contacts.FirstOrDefault(arg => arg.Id == id);
            _contacts.Remove(oldContact);

            return await Task.FromResult(true);
        }

        public async Task<Contact> GetContactAsync(Guid id)
        {
            return await Task.FromResult(_contacts.FirstOrDefault(s => s.Id == id));
        }

        public async Task<IEnumerable<Contact>> GetContactsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(_contacts);
        }
    }
}