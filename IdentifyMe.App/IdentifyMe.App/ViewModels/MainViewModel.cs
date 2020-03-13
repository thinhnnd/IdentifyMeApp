using System.Threading.Tasks;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;
//using IdentifyMe.App.ViewModels.Account;
//using IdentifyMe.App.ViewModels.Connections;
//using IdentifyMe.App.ViewModels.CreateInvitation;
//using IdentifyMe.App.ViewModels.Credentials;
using ReactiveUI;

namespace IdentifyMe.App.ViewModels
{
    public class MainViewModel : ABaseViewModel
    {
        public MainViewModel(
            IUserDialogs userDialogs,
            INavigationService navigationService
            //ConnectionsViewModel connectionsViewModel,
            //CredentialsViewModel credentialsViewModel,
            //AccountViewModel accountViewModel,
            //CreateInvitationViewModel createInvitationViewModel
        ) : base(
                nameof(MainViewModel),
                userDialogs,
                navigationService
        )
        {
            //Connections = connectionsViewModel;
            //Credentials = credentialsViewModel;
            //Account = accountViewModel;
            //CreateInvitation = createInvitationViewModel;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            //await Connections.InitializeAsync(null);
            //await Credentials.InitializeAsync(null);
            //await Account.InitializeAsync(null);
            //await CreateInvitation.InitializeAsync(null);
            await base.InitializeAsync(navigationData);
        }

       // #region Bindable Properties
        //private ConnectionsViewModel _connections;
        //public ConnectionsViewModel Connections
        //{
        //    get => _connections;
        //    set => this.RaiseAndSetIfChanged(ref _connections, value);
        //}

        //private CredentialsViewModel _credentials;
        //public CredentialsViewModel Credentials
        //{
        //    get => _credentials;
        //    set => this.RaiseAndSetIfChanged(ref _credentials, value);
        //}

        //private AccountViewModel _account;
        //public AccountViewModel Account
        //{
        //    get => _account;
        //    set => this.RaiseAndSetIfChanged(ref _account, value);
        //}

        //private CreateInvitationViewModel _createInvitation;
        //public CreateInvitationViewModel CreateInvitation
        //{
        //    get => _createInvitation;
        //    set => this.RaiseAndSetIfChanged(ref _createInvitation, value);
        //}
        //#endregion
    }
}
