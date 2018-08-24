﻿using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class StartPageViewModel : BaseViewModel, IHandle<RegisteredMessage>
    {
        public StartPageViewModel(CommonParameters commonParameters, LoginViewModel loginViewModel)
            : base(commonParameters)
        {
            _loginViewModel = loginViewModel;
            Items.Add(_registerViewModel = new RegisterViewModel(commonParameters));
        }


        private readonly LoginViewModel _loginViewModel;
        private readonly RegisterViewModel _registerViewModel;
        private bool _canShowLoginPage;


        public bool CanShowLoginPage { get => _canShowLoginPage; set { _canShowLoginPage = value; NotifyOfPropertyChange(); } }


        #region Public Methods
        public async Task DisplayInitialPage()
        {
            ShowLoginPage();

            var lastActiveUser = await OfflineDB.GetCurrentUSer();

            if (lastActiveUser != null)
            {
                EventAgg.PublishOnUIThread(new LoggedInMessage(lastActiveUser));
                CanShowLoginPage = true;
                return;
            }

            var canLogin = await HasExistingUser();

            if (canLogin)
            {
                CanShowLoginPage = true;
            }
            else
                ShowRegistrationPage();
        }

        public void ShowLoginPage()
        {
            DeactivateItem(ActiveItem, false);
            ActivateItem(_loginViewModel);
        }

        public void ShowRegistrationPage()
        {
            DeactivateItem(ActiveItem, false);
            ActivateItem(_registerViewModel);
        }

        public void Handle(RegisteredMessage message)
        {
            CanShowLoginPage = true;
        }
        #endregion


        private async Task<bool> HasExistingUser()
        {
            var users = await OfflineDB.GetAllUsers();
            return 0 < users.Count;
        }
    }
}
