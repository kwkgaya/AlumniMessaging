using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AlumniSms.Models;
using AlumniSms.Views;
using AlumniSms.ViewModels;

namespace AlumniSms.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ContactsPage : ContentPage
    {
        ContactsViewModel viewModel;

        public ContactsPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new ContactsViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Contacts.Count == 0)
                viewModel.LoadContactsCommand.Execute(null);
        }
    }
}