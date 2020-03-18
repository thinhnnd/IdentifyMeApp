using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using IdentifyMe.App.Services;
using IdentifyMe.App.Views;

using System.Threading.Tasks;
using Autofac;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using IdentifyMe.App.Services.Interfaces;
using IdentifyMe.App.Utilities;
using IdentifyMe.App.ViewModels;
using IdentifyMe.App.ViewModels.Profile;
using IdentifyMe.App.ViewModels.Connections;
using IdentifyMe.App.ViewModels.Credentials;
using IdentifyMe.App.ViewModels.Notification;
using IdentifyMe.App.ViewModels.Setting;

using IdentifyMe.App.Views.Connections;
using IdentifyMe.App.Views.Credentials;
using IdentifyMe.App.Views.Notification;
using IdentifyMe.App.Views.Profile;
using IdentifyMe.App.Views.Setting;
//using IdentifyMe.App.Views.CreateInvitation;

using Xamarin.Forms.Internals;

using MainPage = IdentifyMe.App.Views.MainPage;

namespace IdentifyMe.App
{
    public partial class App : Application
    {
        public new static App Current => Application.Current as App;
        public Palette Colors;

        private readonly INavigationService _navigationService;
        private readonly ICustomAgentContextProvider _contextProvider;
        Task InitializeTask;

        public App(IContainer container)
        {
            InitializeComponent();

            Colors.Init();

            //osma
            _navigationService = container.Resolve<INavigationService>();
            _contextProvider = container.Resolve<ICustomAgentContextProvider>();
            InitializeTask = Initialize();

            DependencyService.Register<MockDataStore>();

        }

        //osma code
        private async Task Initialize()
        {
            //Bind ViewModel with Page

            _navigationService.AddPageViewModelBinding<MainViewModel, MainPage>();
            _navigationService.AddPageViewModelBinding<ProfileViewModel, ProfilePage>();
            _navigationService.AddPageViewModelBinding<CredentialsViewModel, CredentialsPage>();
            _navigationService.AddPageViewModelBinding<ConnectionsViewModel, ConnectionsPage>();
            _navigationService.AddPageViewModelBinding<NotificationViewModel, NotificationPage>();
            _navigationService.AddPageViewModelBinding<SettingViewModel, SettingPage>();
           // _navigationService.AddPageViewModelBinding<BetterMainViewModel, MainPage2>();
            _navigationService.AddPageViewModelBinding<RegisterViewModel, RegisterPage>();

            if (_contextProvider.AgentExists())
            {
                await _navigationService.NavigateToAsync<MainViewModel>();
            }
            else
            {
                await _navigationService.NavigateToAsync<RegisterViewModel>();
            }
        }

        protected override void OnStart()
        {
#if !DEBUG
                AppCenter.Start("ios=" + AppConstant.IosAnalyticsKey + ";" +
                                "android=" + AppConstant.AndroidAnalyticsKey + ";",
                        typeof(Analytics), typeof(Crashes));
#endif
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }


    //public partial class App : Application
    //{

    //    public App()
    //    {
    //        InitializeComponent();

    //        DependencyService.Register<MockDataStore>();
    //        MainPage = new MainPage();
    //    }



    //    protected override void OnStart()
    //    {
    //    }

    //    protected override void OnSleep()
    //    {
    //    }

    //    protected override void OnResume()
    //    {
    //    }
    //}
}
