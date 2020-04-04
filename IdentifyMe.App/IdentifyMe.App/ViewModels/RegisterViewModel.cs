using System;
using System.Windows.Input;
using Acr.UserDialogs;
using Hyperledger.Aries.Configuration;
using Hyperledger.Aries.Storage;
using IdentifyMe.App.Services.Interfaces;
using Xamarin.Forms;

using IdentifyMe.App.Views;


namespace IdentifyMe.App.ViewModels
{
    class RegisterViewModel : ABaseViewModel
    {
        private readonly ICustomAgentContextProvider _agentContextProvider;

        public RegisterViewModel(IUserDialogs userDialogs,
                    INavigationService navigationService,
                    ICustomAgentContextProvider agentContextProvider) : base (
                    nameof(RegisterViewModel),
                    userDialogs,
                    navigationService)
        {
            _agentContextProvider = agentContextProvider;
        }

        #region Binding command

        public ICommand CreateWalletCommand => new Command( async() =>
        {

            var dialog = UserDialogs.Instance.Loading("Creating wallet");

            var options = new AgentOptions
            {
                GenesisFilename = "pool_genesis.txn",
                PoolName = "EdgeAgentPoolConnection",
                EndpointUri = "https://limegreenmobilecleaninstall--thinhnnd.repl.co/relay-service",
                ProtocolVersion = 2,
                WalletConfiguration = new WalletConfiguration { Id = Guid.NewGuid().ToString() },
                WalletCredentials = new WalletCredentials { Key = "LocalWalletKey" }
            };

            if(await _agentContextProvider.CreateAgentAsync(options))
            {
                await NavigationService.NavigateToAsync<MainViewModel>();
                dialog?.Hide();
                dialog?.Dispose();
            }
            else
            {
                dialog?.Hide();
                dialog?.Dispose();
                UserDialogs.Instance.Alert("Failed to create wallet!");
            }
        });
        #endregion
    }
}
