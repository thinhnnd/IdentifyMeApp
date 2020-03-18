using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;

namespace IdentifyMe.App.ViewModels.Notification
{
    public class NotificationViewModel : ABaseViewModel
    {

        NotificationViewModel(IUserDialogs userDialogs,
INavigationService navigationService
) : base(
nameof(NotificationViewModel),
userDialogs,
navigationService
)
        {

        }
    }
}
