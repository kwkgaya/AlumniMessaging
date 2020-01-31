using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AlumniSms.ViewModels
{
    public class AboutViewModel : BaseViewModel
    {
        private readonly IEnumerable<Contact> _contacts;
        private string _text;

        public ICommand SendToAllCommand { get; }

        public string SmsText
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public AboutViewModel()
        {
        }

        public AboutViewModel(IEnumerable<Contact> contacts)
        {
            _contacts = contacts;
            Title = "Send Sms";
            SendToAllCommand = new Command(async () => await SendSms());
        }

        private async Task SendSms()
        {
            foreach (var contact in _contacts)
            {
                var text = SmsText.Replace("@Name", contact.Name);
                var message = new SmsMessage(SmsText, contact.Mobile);
            }
        }
    }

    public class Contact
    {
        public string Name { get; set; }
        public int Batch { get; set; }
        public string Mobile { get; set; }
    }
}