using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AlumniMessaging.Models;
using CsvHelper;
using Xamarin.Essentials;

namespace AlumniMessaging.Services
{
    public class ContactsDataStore : IContactsStore
    {
        readonly string _path = Path.Combine(FileSystem.AppDataDirectory, "contacts.csv");

        public async Task OverwriteContacts(IEnumerable<Contact> mergedContacts)
        {
            try
            {
                using var writer = new StreamWriter(_path);
                await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(mergedContacts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task<List<Contact>> GetContacts()
        {
            if(!File.Exists(_path))
                return new List<Contact>();

            using var writer = new StreamReader(_path);
            using var csv = new CsvReader(writer, CultureInfo.InvariantCulture);
            var result = await csv.GetRecordsAsync<Contact>().ToListAsync();
            return result;
        }
    }
}