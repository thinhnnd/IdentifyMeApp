using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using Autofac;
using Hyperledger.Aries.Contracts;
using Hyperledger.Aries.Features.DidExchange;
using Hyperledger.Aries.Utils;
using IdentifyMe.App.Events;
using IdentifyMe.App.Extensions;
using IdentifyMe.App.Services;
using IdentifyMe.App.Services.Interfaces;
//using IdentifyMe.App.ViewModels.CreateInvitation;
using ReactiveUI;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;
using ZXing.Net.Mobile;
using Rg.Plugins.Popup.Services;
using IdentifyMe.App.Views.Connections;
using Hyperledger.Aries.Agents;
using Hyperledger.Aries.Configuration;

namespace IdentifyMe.App.ViewModels.Connections
{
    public class ConnectionsViewModel : ABaseViewModel
    {
        private readonly IConnectionService _connectionService;
        private readonly ICustomAgentContextProvider _agentContextProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILifetimeScope _scope;
        private readonly IMessageService _messageService;
        private readonly IProvisioningService _provisioningService;


        public ConnectionsViewModel(IUserDialogs userDialogs,
                                    INavigationService navigationService,
                                    IConnectionService connectionService,
                                    ICustomAgentContextProvider agentContextProvider,
                                    IMessageService messageService,
                                    IProvisioningService provisioningService,
                                    IEventAggregator eventAggregator,
                                    ILifetimeScope scope) :
                                    base("Connections", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _agentContextProvider = agentContextProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;
            _messageService = messageService;
            _provisioningService = provisioningService;
        }

        public override async Task InitializeAsync(object navigationData)
        {
            await RefreshConnections();

            //When an Event of ConnectionsUpdated is occur refresh ConnectionsViewModel to update.
            _eventAggregator.GetEventByType<ApplicationEvent>()
                .Where(_ => _.Type == ApplicationEventType.ConnectionsUpdated)
                .Subscribe(async _ => await RefreshConnections());
        }

        public async Task RefreshConnections()
        {
            RefreshingConnections = true;

            var context = await _agentContextProvider.GetContextAsync();
            var records = await _connectionService.ListAsync(context);

            IList<ConnectionViewModel> connectionVms = new List<ConnectionViewModel>();
            foreach (var record in records)
            {
                var connection = _scope.Resolve<ConnectionViewModel>(new NamedParameter("record", record));
                connectionVms.Add(connection);
            }

            //TODO need to compare with the currently displayed connections rather than disposing all of them
            Connections.Clear();
            Connections.InsertRange(connectionVms);
            HasConnections = connectionVms.Any();

            RefreshingConnections = false;
        }

        public async Task AcceptConnectionButton()
        {
            ConnectionInvitationMessage invitation;
            try
            {
                invitation =   MessageUtils.DecodeMessageFromUrlFormat<ConnectionInvitationMessage>(InvitationMessageUrl);
            }
            catch (Exception)
            {
                DialogService.Alert("Invalid invitation!");
                Device.BeginInvokeOnMainThread(async () => await NavigationService.PopModalAsync());
                return;
            }
        }

        #region Bindable Props
        //Bindable Properties
        private string _invitationMessageUrl = "http://10.0.0.12:8000?c_i=eyJsYWJlbCI6IlBlZGFudGljIEZleW5tYW4iLCJpbWFnZVVybCI6bnVsbCwic2VydmljZUVuZHBvaW50IjoiaHR0cDovLzEwLjAuMC4xMjo4MDAwIiwicm91dGluZ0tleXMiOlsiQ0ZjZWNLMnBVelZxdG0zR2phWFRkY1BaQThmZUtaZU5qRDlwc2NHanZIS1MiXSwicmVjaXBpZW50S2V5cyI6WyJDRWNVcGhKS3RUemUxTVVEMnN0elgyV1RYaWtUZXhpZVhoZ2o1ZnpIc29UTSJdLCJAaWQiOiI2ZWNhMTA2NC0zNzg2LTQwMzAtYmVkNS0xMWM5Y2M5MzFmOTYiLCJAdHlwZSI6ImRpZDpzb3Y6QnpDYnNOWWhNcmpIaXFaRFRVQVNIZztzcGVjL2Nvbm5lY3Rpb25zLzEuMC9pbnZpdGF0aW9uIn0=";
        // "http://10.0.0.12:8000?c_i=eyJsYWJlbCI6IlRydXN0aW5nIEhvZGdraW4iLCJpbWFnZVVybCI6Imh0dHBzOi8vY29ycC52Y2RuLnZuL3VwbG9hZC92bmcvc291cmNlL0FydGljbGUvbG9nb18zNjBsaXZlLnBuZyIsc2VydmljZUVuZHBvaW50IjoiaHR0cDovLzEwLjAuMC4xMTo3MDAwIiwicm91dGluZ0tleXMiOlsiRWN6RWQ0Zkg5NzE2YjZHWEhqZnVVOFNOdjdzd2tMUXoyWWlIeTdRTlNZYjUiXSwicmVjaXBpZW50S2V5cyI6WyJEdkVtbVU5U0dydUxUNmpFTkw3SHA0MXZ0QXdzeDVnVUEzeUpiVnZCTlhFciJdLCJAaWQiOiI1NGFjZmNkZS04ZGZhLTRmZGMtYjBhMi03M2YwNWM2Zjk1OGEiLCJAdHlwZSI6ImRpZDpzb3Y6QnpDYnNOWWhNcmpIaXFaRFRVQVNIZztzcGVjL2Nvbm5lY3Rpb25zLzEuMC9pbnZpdGF0aW9uIn0=";

        public string InvitationMessageUrl
        {
            get => _invitationMessageUrl;
            set => this.RaiseAndSetIfChanged(ref _invitationMessageUrl, value);
        }

        private string _qrScanedResult = "null";
        public string QRScanedResult
        {
            get => _qrScanedResult;
            set => this.RaiseAndSetIfChanged(ref _qrScanedResult, value);
        }

        private RangeEnabledObservableCollection<ConnectionViewModel> _connections = new RangeEnabledObservableCollection<ConnectionViewModel>();

        public RangeEnabledObservableCollection<ConnectionViewModel> Connections
        {
            get => _connections;
            set => this.RaiseAndSetIfChanged(ref _connections, value);
        }

        private bool _refreshingConnections;

        public bool RefreshingConnections
        {
            get => _refreshingConnections;
            set => this.RaiseAndSetIfChanged(ref _refreshingConnections, value);
        }

        private bool _hasConnections;
        public bool HasConnections
        {
            get => _hasConnections;
            set => this.RaiseAndSetIfChanged(ref _hasConnections, value);
        }

        #endregion
        public ICommand AcceptConnectionCommand => new Command( async () => await AcceptConnectionButton());

        public async Task OpenScannerPage()
        {

            var options = new ZXing.Mobile.MobileBarcodeScanningOptions
            {
             
                PossibleFormats = new List<ZXing.BarcodeFormat>()
                {
                    ZXing.BarcodeFormat.QR_CODE,
                }
            };

            var scanPage = new ZXingScannerPage();
           // await _navigationService.NavigateToAsync(scanPage);

            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;
                ConnectionInvitationMessage invitation;

                QRScanedResult = result.Text;
                try
                {
                    invitation = MessageUtils.DecodeMessageFromUrlFormat<ConnectionInvitationMessage>(QRScanedResult);     
                }
                catch (Exception)
                {
                    DialogService.Alert("Invalid invitation!");
                    Device.BeginInvokeOnMainThread(async () => await NavigationService.PopModalAsync());
                    return;
                }

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await NavigationService.PopModalAsync();
                    AcceptInvitationViewModel acceptInvitationViewModel = new AcceptInvitationViewModel(
                        DialogService, 
                        NavigationService, 
                        _connectionService, 
                        _agentContextProvider, 
                        _provisioningService,
                        _messageService);
                    await NavigationService.NavigateToPopupAsync<AcceptInvitationViewModel>(invitation, true, acceptInvitationViewModel);

                });
            };
            await NavigationService.NavigateToAsync((Page)scanPage, NavigationType.Modal);

        }

        public ICommand OpenScannerPageCommand => new Command(async () => await OpenScannerPage());


    }
}
