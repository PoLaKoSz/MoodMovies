﻿using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources.Validation;

namespace MoodMovies.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
            UserEmail = string.Empty;
            UserPassword = string.Empty;
        }

        private string _userEmail;
        private string _userPassword;

        #region  public Properties
        public string UserEmail { get => _userEmail; set { _userEmail = value; NotifyOfPropertyChange(); } }

        public string UserPassword { get => _userPassword; set { _userPassword = value; NotifyOfPropertyChange(); } }

        public bool KeepLoggedIn { get; set; }
        #endregion

        public async void Login()
        {
            if (!IsFieldsValid())
                return;

            EventAgg.PublishOnUIThread(new StartLoadingMessage("Logging in"));

            User loggingUser = null;

            try
            {
                loggingUser = await OfflineDB.GetUserByEmailPassword(UserEmail, UserPassword);
            }
            catch
            {
                StatusMessage.Enqueue("Error while getting User informations. Please try again!");
                return;
            }

            if (loggingUser == null)
            {
                EventAgg.PublishOnUIThread(new StopLoadingMessage());

                StatusMessage.Enqueue("Couldn't find a User with this Email and Password combination!");
                return;
            }

            ModifyKeepLoggedIn(loggingUser);

            UserEmail = "";
            UserPassword = "";

            EventAgg.PublishOnUIThread(new LoggedInMessage(loggingUser));
            EventAgg.BeginPublishOnUIThread(new StopLoadingMessage());
        }

        private bool IsFieldsValid()
        {
            if (!CredentialValidation.IsValidEmail(UserEmail))
            {
                StatusMessage.Enqueue("Email field is not valid!");
                UserEmail = "";
                return false;
            }

            if (!CredentialValidation.IsValidPassword(UserPassword))
            {
                StatusMessage.Enqueue("Password field is not valid!");
                UserPassword = "";
                return false;
            }

            return true;
        }

        private void ModifyKeepLoggedIn(User loggingUser)
        {
            try
            {
                if (KeepLoggedIn)
                {
                    OfflineDB.SetAsCurrentUser(loggingUser);
                }
                else if (loggingUser.IsCurrentUser && !KeepLoggedIn)
                {
                    OfflineDB.DisableAutoLogin(loggingUser);
                }
            }
            catch
            {
                StatusMessage.Enqueue("Error while saving \"Keep logged In\" status. Try to relog to your account!");
            }
        }
    }
}
