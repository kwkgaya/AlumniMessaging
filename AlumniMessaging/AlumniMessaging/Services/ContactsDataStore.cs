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
        private readonly IPermissionRequest _permissionRequest;
        readonly string _path = Path.Combine(FileSystem.AppDataDirectory, "contacts.csv");
        readonly string _initPath = Path.Combine(FileSystem.AppDataDirectory, "init.csv");

        public ContactsDataStore(IPermissionRequest permissionRequest)
        {
            _permissionRequest = permissionRequest;
        }

        public async Task OverwriteContacts(IEnumerable<Contact> mergedContacts)
        {
            try
            {
                _permissionRequest.CheckAndRequestPermissions(Permission.WriteExternalStorage);
                _permissionRequest.CheckAndRequestPermissions(Permission.ReadExternalStorage);

                using var writer = new StreamWriter(_path);
                await using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
                await csv.WriteRecordsAsync(mergedContacts);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public Task<bool> Initialize()
        {
            _permissionRequest.CheckAndRequestPermissions(Permission.ReadExternalStorage);

            if (File.Exists(_initPath)) return Task.FromResult(false);

            File.Create(_initPath);
            return Task.FromResult(true);
        }

        public async Task<List<Contact>> GetContacts()
        {
            _permissionRequest.CheckAndRequestPermissions(Permission.ReadExternalStorage);

            if (!File.Exists(_path))
                return new List<Contact>();

            using var writer = new StreamReader(_path);
            using var csv = new CsvReader(writer, CultureInfo.InvariantCulture);
            var result = await csv.GetRecordsAsync<Contact>().ToListAsync();
            return result;
        }
    }
}