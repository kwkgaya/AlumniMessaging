using System;
using System.Collections.Generic;
using AlumniMessaging.Services;

namespace AlumniMessaging.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        public ContactsViewModel ContactsViewModel { get; }
        public SendMessageViewModel SendMessageViewModel { get; }

        public MainPageViewModel(
            SendMessageViewModel sendMessageViewModel,
            ContactsViewModel contactsViewModel)
        {
            SendMessageViewModel = sendMessageViewModel;
            ContactsViewModel = contactsViewModel;
        }
    }
}
