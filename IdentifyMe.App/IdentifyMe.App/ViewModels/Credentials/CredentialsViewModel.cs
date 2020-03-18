using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Hyperledger.Aries.Features.IssueCredential;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;

namespace IdentifyMe.App.ViewModels.Credentials
{
    public class CredentialsViewModel : ABaseViewModel
    {
        private readonly ICredentialService _credentialService;
        private readonly ICustomAgentContextProvider _agentContextProvider;

        CredentialsViewModel(
            IUserDialogs userDialogs, 
            INavigationService navigationService, 
            ICredentialService credentialService, 
            ICustomAgentContextProvider agentContextProvider ) : 
            base(
                nameof(CredentialsViewModel), 
                userDialogs, 
                navigationService)
        {
            _credentialService = credentialService;
            _agentContextProvider = agentContextProvider;
        }
    }
}
