using Xamarin.Forms;
using AlumniMessaging.Services;
using AlumniMessaging.ViewModels;
using AlumniMessaging.Views;
using LightInject;

namespace AlumniMessaging
{
    public partial class App : Application
    {
        public static ServiceContainer ServiceContainer { get; }
            = new ServiceContainer(
                new ContainerOptions
                {
                    EnablePropertyInjection = false
                });

        public App()
        {
            RegisterDependencies();
            InitializeComponent();

            ServiceContainer.Compile();
            var mainPageVm = ServiceContainer.GetInstance<MainPageViewModel>();
            MainPage = new MainPage {BindingContext = mainPageVm};
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static void RegisterDependencies()
        {
            ServiceContainer.Register(f => ServiceContainer, new PerContainerLifetime());
            ServiceContainer.Register<IContactsStore, ContactsDataStore>(new PerContainerLifetime());
            ServiceContainer.Register<ContactsViewModel>();
            ServiceContainer.Register<SendMessageViewModel>();
            ServiceContainer.Register<MainPageViewModel>();
        }
    }
}
