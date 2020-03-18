using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;

namespace IdentifyMe.App.ViewModels.Connections
{
    public class ConnectionsViewModel : ABaseViewModel
    {
        ConnectionsViewModel(IUserDialogs userDialogs,
            INavigationService navigationService
        ) : base (
            nameof(ConnectionsViewModel), 
            userDialogs, 
            navigationService
        )
        {

        }


    }
}
