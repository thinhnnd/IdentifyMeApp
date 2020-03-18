using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;

namespace IdentifyMe.App.ViewModels.Profile
{
    public class ProfileViewModel : ABaseViewModel
    {

        ProfileViewModel(IUserDialogs userDialogs,
    INavigationService navigationService
) : base(
    nameof(ProfileViewModel),
    userDialogs,
    navigationService
)
        {

        }
    }
}
