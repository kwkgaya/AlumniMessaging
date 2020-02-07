using System;
using System.Collections.Generic;
using AlumniSms.Services;

namespace AlumniSms.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ContactsViewModel ContactsViewModel { get; }
        public SendSmsViewModel SendSmsViewModel { get; }

        public MainPageViewModel(
            IContactsStore contactsStore,
            SendSmsViewModel sendSmsViewModel,
            ContactsViewModel contactsViewModel) : base(contactsStore)
        {
            SendSmsViewModel = sendSmsViewModel;
            ContactsViewModel = contactsViewModel;
        }
    }
}
