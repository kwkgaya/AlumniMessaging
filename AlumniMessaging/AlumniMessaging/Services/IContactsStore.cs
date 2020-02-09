using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AlumniMessaging.Models;

namespace AlumniMessaging.Services
{
    public interface IContactsStore
    {
        Task<bool> Initialize();
        Task<List<Contact>> GetContacts();
        Task OverwriteContacts(IEnumerable<Contact> mergedContacts);
    }
}
