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

        public AcceptInvitationViewModel(IUserDialogs userDialogs,
                                     INavigationService navigationService)
                                     : base("Accept Invitiation", userDialogs, navigationService)
        {
           
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

        #region Bindable Commands

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
