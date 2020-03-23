using System.Threading.Tasks;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;
//using IdentifyMe.App.ViewModels.Account;
using IdentifyMe.App.ViewModels.Connections;
//using IdentifyMe.App.ViewModels.CreateInvitation;
using IdentifyMe.App.ViewModels.Credentials;
using IdentifyMe.App.ViewModels.Profile;
using IdentifyMe.App.ViewModels.Notification;
using IdentifyMe.App.ViewModels.Setting;
using ReactiveUI;

namespace IdentifyMe.App.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {

        public MainViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService,
            ConnectionsViewModel connectionsViewModel
        ) : base(
                nameof(MainViewModel),
                userDialogs,
                navigationService
        )
        {
            Connections = connectionsViewModel;
            //Credentials = credentialsViewModel;
            //Profile = profileViewModel;
            //Setting = settingViewModel;
            //Notification = notificationViewModel;
            //CreateInvitation = createInvitationViewModel;
            //CredentialsViewModel credentialsViewModel,
            //ConnectionsViewModel connectionsViewModel,
            //ProfileViewModel profileViewModel,
            //NotificationViewModel notificationViewModel,
            //SettingViewModel settingViewModel
        }

        public override async Task InitializeAsync(object navigationData) 
        { 

           await Connections.InitializeAsync(null);
        //    await Credentials.InitializeAsync(null);
        //    await Profile.InitializeAsync(null);
        //    await Setting.InitializeAsync(null);
        //    await Notification.InitializeAsync(null);
        //    await base.InitializeAsync(navigationData);
        }

        #region Bindable Properties
        private ConnectionsViewModel _connections;
        public ConnectionsViewModel Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        private CredentialsViewModel _credentials;
        public CredentialsViewModel Credentials
        {
            get => _credentials;
            set => this.RaiseAndSetIfChanged(ref _credentials, value);
        }

        private ProfileViewModel _profile;
        public ProfileViewModel Profile
        {
            get => _profile;
            set => this.RaiseAndSetIfChanged(ref _profile, value);
        }

        private NotificationViewModel _notification;
        public NotificationViewModel Notification
        {
            get => _notification;
            set => this.RaiseAndSetIfChanged(ref _notification, value);
        }

        private SettingViewModel _setting;
        public SettingViewModel Setting
        {
            get => _setting;
            set => this.RaiseAndSetIfChanged(ref _setting, value);
        }
        #endregion
    }
}
