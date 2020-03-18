using System;
using System.Collections.Generic;
using System.Text;
using Acr.UserDialogs;
using IdentifyMe.App.Services.Interfaces;

namespace IdentifyMe.App.ViewModels.Setting
{
    public class SettingViewModel : ABaseViewModel
    {
        SettingViewModel(IUserDialogs userDialogs,
INavigationService navigationService
) : base(
nameof(SettingViewModel),
userDialogs,
navigationService
)
        {

        }
    }
}
