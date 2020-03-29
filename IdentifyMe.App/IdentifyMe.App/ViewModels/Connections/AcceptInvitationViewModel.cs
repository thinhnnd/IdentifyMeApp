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
       

        private ConnectionInvitationMessage _invitation;
        private ICustomAgentContextProvider _agentContextProvider;
        private INavigationService _navigationService;
        private IConnectionService _connectionService;
        private IUserDialogs _userDialogs;
        private IMessageService _messageService;
        private readonly IProvisioningService _provisioningService;


        public AcceptInvitationViewModel(IUserDialogs userDialogs,
            INavigationService navigationService,
            IConnectionService connectionService, 
            ICustomAgentContextProvider agentContextProvider,
            IProvisioningService provisioningService,
            IMessageService messageService) 
            : base("Accept Invitiation", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _navigationService = navigationService;
            _agentContextProvider = agentContextProvider;
            _userDialogs = userDialogs;
            _messageService = messageService;
            _provisioningService = provisioningService;
        }

        private async Task AcceptInvitation()
        {
            var loadingDialog = _userDialogs.Loading("Proccessing");
            if (_invitation != null)
            {
                try
                {
                    var agentContext = await _agentContextProvider.GetContextAsync();
                    if (agentContext == null)
                    {
                        loadingDialog.Hide();
                        DialogService.Alert("Failed to decode invitation!");
                        return;
                    }
                    var (requestMessage, connectionRecord) = await _connectionService.CreateRequestAsync(agentContext, _invitation);
                    var provisioningRecord = await _provisioningService.GetProvisioningAsync(agentContext.Wallet);
                    var isEndpointUriAbsent = provisioningRecord.Endpoint.Uri == null;

                    var respone = await _messageService.SendReceiveAsync<ConnectionResponseMessage>(agentContext.Wallet, requestMessage, connectionRecord);
                    if (isEndpointUriAbsent)
                    {
                        string processRes = await _connectionService.ProcessResponseAsync(agentContext, respone, connectionRecord);
                    }
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(e.ToString());
                }
                loadingDialog.Hide();
                await _navigationService.CloseAllPopupsAsync();
                var toastConfig = new ToastConfig("Connection Saved!");
                toastConfig.BackgroundColor = Color.Green;
                toastConfig.Position = ToastPosition.Top;
                toastConfig.SetDuration(3000);
                _userDialogs.Toast(toastConfig);
            }

        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is ConnectionInvitationMessage invitation)
            {
                InvitationTitle = $"Connect to {invitation.Label}?";
                InvitationImageUrl = invitation.ImageUrl;
                InvitationContents = $"{invitation.Label} has invited you to connect?";
                InvitationLabel = invitation.Label;
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

        #region Bindable Properties
        private string _invitationTitle;
        public string InvitationTitle
        {
            get => _invitationTitle;
            set => this.RaiseAndSetIfChanged(ref _invitationTitle, value);
        }

        private string _invitationContents = "Someone wants to connect?";
        public string InvitationContents
        {
            get => _invitationContents;
            set => this.RaiseAndSetIfChanged(ref _invitationContents, value);
        }

        private string _invitationImageUrl;
        public string InvitationImageUrl
        {
            get => _invitationImageUrl;
            set => this.RaiseAndSetIfChanged(ref _invitationImageUrl, value);
        }

        private string _invitationLabel;

        public string InvitationLabel
        {
            get => _invitationLabel;
            set => this.RaiseAndSetIfChanged(ref _invitationLabel, value);
        }
        #endregion

        #region Bindable Commands
        public ICommand AcceptInvitationCommand => new Command(async () => await AcceptInvitation());

        public ICommand AcceptInviteCommand => new Command(async () =>
        {
            var loadingDialog = DialogService.Loading("Processing");

            var context = await _agentContextProvider.GetContextAsync();

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
                errorMessage = "Error";
            }
            //_eventAggregator.Publish(new ApplicationEvent() { Type = ApplicationEventType.ConnectionsUpdated });
            if (loadingDialog.IsShowing)
                loadingDialog.Hide();

            if (!String.IsNullOrEmpty(errorMessage))
                DialogService.Alert(errorMessage);

            await NavigationService.CloseAllPopupsAsync();
        });
        #endregion
    }
}
