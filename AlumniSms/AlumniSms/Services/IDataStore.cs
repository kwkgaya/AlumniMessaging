using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlumniSms.Services
{
    public interface IDataStore<T>
    {
        Task<IEnumerable<T>> GetContactsAsync(bool forceRefresh = false);
    }
}
