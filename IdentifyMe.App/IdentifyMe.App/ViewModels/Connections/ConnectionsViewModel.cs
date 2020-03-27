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
        private readonly INavigationService _navigationService;

        //test
        private readonly IMessageService _messageService;
        private readonly IProvisioningService _provisioningService;

        private IUserDialogs _userDialogs;

        public ConnectionsViewModel(IUserDialogs userDialogs,
                                    INavigationService navigationService,
                                    IConnectionService connectionService,
                                    ICustomAgentContextProvider agentContextProvider,
                                    IEventAggregator eventAggregator,
                                    ILifetimeScope scope,
                                    IMessageService message,
                                    IProvisioningService provisioning
            ) :
                                    base("Connections", userDialogs, navigationService)
        {
            _connectionService = connectionService;
            _agentContextProvider = agentContextProvider;
            _eventAggregator = eventAggregator;
            _scope = scope;
            _navigationService = navigationService;
            _userDialogs = userDialogs;
            _provisioningService = provisioning;
            _messageService = message;
        }

        public async Task CreateInvitation()
        {
           // ConnectionInvitationMessage a;
           //_connectionService.CreateInvitationAsync();
        }

        public async Task AcceptConnectionButton()
        {
            ConnectionInvitationMessage invitation;
            try
            {
                invitation =  MessageUtils.DecodeMessageFromUrlFormat<ConnectionInvitationMessage>(InvitationMessageUrl);
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
                    AcceptInvitationViewModel a = new AcceptInvitationViewModel(_userDialogs, _navigationService, _provisioningService, _connectionService,_messageService,_agentContextProvider,_eventAggregator);
                    await NavigationService.NavigateToPopupAsync<AcceptInvitationViewModel>(invitation, true, a);

                });
            };
            await NavigationService.NavigateToAsync((Page)scanPage, NavigationType.Modal);

        }

        public ICommand OpenScannerPageCommand => new Command(async () => await OpenScannerPage());


    }
}
