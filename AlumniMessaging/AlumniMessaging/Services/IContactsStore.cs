using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniMessaging.Models;

namespace AlumniMessaging.Services
{
    public interface IContactsStore
    {
        Task<List<Contact>> GetContacts();
        Task OverwriteContacts(IEnumerable<Contact> mergedContacts);
    }
}
