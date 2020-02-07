using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using AlumniMessaging.Models;
using AlumniMessaging.Views;
using AlumniMessaging.ViewModels;

namespace AlumniMessaging.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class ContactsPage : ContentPage
    {
        public ContactsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = (ContactsViewModel)BindingContext;

            if (viewModel.Contacts.Count == 0)
                viewModel.LoadContactsCommand.Execute(null);
        }
    }
}