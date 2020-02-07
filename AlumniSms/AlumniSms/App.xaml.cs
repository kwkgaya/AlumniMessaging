using Xamarin.Forms;
using AlumniSms.Services;
using AlumniSms.ViewModels;
using AlumniSms.Views;
using LightInject;

namespace AlumniSms
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

            DependencyService.Register<MockContactsStore>();

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
            ServiceContainer.Register<SendSmsViewModel>();
            ServiceContainer.Register<MainPageViewModel>();
        }
    }
}
