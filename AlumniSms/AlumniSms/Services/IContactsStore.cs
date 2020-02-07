using System.Collections.Generic;
using System.Threading.Tasks;
using AlumniSms.Models;

namespace AlumniSms.Services
{
    public interface IContactsStore
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
        Task<bool> AddContactAsync(Contact contact);
    }
}
