using Caliburn.Micro;
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

        public bool KeepLoggedIn;
        #endregion

        public async void Login()
        {
            if (!IsFieldsValid())
                return;

            var loggingUser = await OfflineDb.GetUserByEmailPassword(UserEmail, UserPassword);

            if (loggingUser == null)
            {
                StatusMessage.Enqueue("Couldn't find a User with this Email and Password combination!");
                return;
            }

            ModifyKeepLoggedIn(loggingUser);

            UserEmail = "";
            UserPassword = "";

            EventAgg.PublishOnUIThread(new LoggedInMessage(loggingUser));
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
            if (KeepLoggedIn)
            {
                OfflineDb.SetCurrentUserFieldToTrue(loggingUser);
            }
            else if (loggingUser.IsCurrentUser && KeepLoggedIn)
            {
                OfflineDb.SetCurrentUserFieldToFalse(loggingUser);
            }
        }
    }
}
