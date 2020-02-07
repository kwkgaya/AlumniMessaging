using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniSms.Models;

namespace AlumniSms.Services
{
    public interface IContactsStore
    {
        Task<IEnumerable<Contact>> GetContacts();
        Task<bool> AddContactAsync(Contact contact);
        Task OverwriteContacts(IEnumerable<Contact> mergedContacts);
    }
}
