using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Hyperledger.Aries.Contracts;
using IdentifyMe.App.Events;
using IdentifyMe.App.Services.Interfaces;
using ReactiveUI;
using Xamarin.Forms;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Agents;

namespace IdentifyMe.App.ViewModels.Connections
{
    public class AcceptInvitationViewModel : ABaseViewModel
    {
        private readonly IProvisioningService _provisioningService;
        private readonly IConnectionService _connectionService;
        private readonly IMessageService _messageService;
        private readonly ICustomAgentContextProvider _contextProvider;
        private readonly IEventAggregator _eventAggregator;

        private ConnectionInvitationMessage _invitation;

        public AcceptInvitationViewModel(IUserDialogs userDialogs,
                                     INavigationService navigationService,
                                     IProvisioningService provisioningService,
                                     IConnectionService connectionService,
                                     IMessageService messageService,
                                     ICustomAgentContextProvider contextProvider,
                                     IEventAggregator eventAggregator)
                                     : base("Accept Invitiation", userDialogs, navigationService)
        {
            _provisioningService = provisioningService;
            _connectionService = connectionService;
            _contextProvider = contextProvider;
            _messageService = messageService;
            _contextProvider = contextProvider;
            _eventAggregator = eventAggregator;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invitation)
            {
                InvitationTitle = $"Connect to {invitation.Label}?";
                InvitationrUrl = invitation.ImageUrl;
                InvitationContents = $"{invitation.Label} has invited you to connect?";
                _invitation = invitation;
            }
            return base.InitializeAsync(navigationData);
        }
        private async Task CreateConnection(IAgentContext context, ConnectionInvitationMessage invite)
        {
            var provisioningRecord = await _provisioningService.GetProvisioningAsync(context.Wallet);
            var isEndpointUriAbsent = provisioningRecord.Endpoint.Uri == null;
            var (msg, rec) = await _connectionService.CreateRequestAsync(context, _invitation);
            var rsp = await _messageService.SendReceiveAsync<ConnectionResponseMessage>(context.Wallet, msg, rec);
            if (isEndpointUriAbsent)
            {
                await _connectionService.ProcessResponseAsync(context, rsp, rec);
            }
        }
        private async Task AcceptInvitation()
        {
            var loadingDialog = DialogService.Loading("Processing");

            var context = await _contextProvider.GetContextAsync();

            if (context == null || _invitation == null)
            {
                loadingDialog.Hide();
                DialogService.Alert("Failed to decode invite!");
                return;
            }

            String errorMessage = String.Empty;
            try
            {
                await CreateConnection(context, _invitation);
            }
            catch (Hyperledger.Aries.AriesFrameworkException ariesFrameworkException)
            {
                errorMessage = ariesFrameworkException.Message;
            }
            catch (Exception) //TODO more granular error protection
            {
                errorMessage = "Failed to accept";
            }

            _eventAggregator.Publish(new ApplicationEvent() { Type = ApplicationEventType.ConnectionsUpdated });

            if (loadingDialog.IsShowing)
                loadingDialog.Hide();

            if (!String.IsNullOrEmpty(errorMessage))
                DialogService.Alert(errorMessage);

            // await NavigationService.PopModalAsync();
            Console.WriteLine("Connect ok");

        }

        #region Bindable Commands
        public ICommand LoginCommand => new Command(async () => await AcceptInvitation());
        #endregion

        #region Bindable Properties
        private string _invitationTitle;
        public string InvitationTitle
        {
            get => _invitationTitle;
            set => this.RaiseAndSetIfChanged(ref _invitationTitle, value);
        }

        private string _invitationContents = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua";
        public string InvitationContents
        {
            get => _invitationContents;
            set => this.RaiseAndSetIfChanged(ref _invitationContents, value);
        }

        private string _invitationrUrl;
        public string InvitationrUrl
        {
            get => _invitationrUrl;
            set => this.RaiseAndSetIfChanged(ref _invitationrUrl, value);
        }
        #endregion
    }
}
