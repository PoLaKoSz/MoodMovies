using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage) : base(
            _event, offlineService, onlineService, statusMessage)
        {

        }

        #region  public Properties
        private string _userFirstName;
        public string UserFirstName { get => _userFirstName; set { _userFirstName = value; NotifyOfPropertyChange(); } }

        private string _userSurname;
        public string UserSurname { get => _userSurname; set { _userSurname = value; NotifyOfPropertyChange(); } }

        private string _userApiKey;
        public string UserApiKey { get => _userApiKey; set { _userApiKey = value; NotifyOfPropertyChange(); } }

        private string _userEmail;
        public string UserEmail { get => _userEmail; set { _userEmail = value; NotifyOfPropertyChange(); } }

        private string _userPassword;
        public string UserPassword { get => _userPassword; set { _userPassword = value; NotifyOfPropertyChange(); } }   
        #endregion

        #region Public Methods
        public void Register()
        {
            if (!string.IsNullOrEmpty(UserEmail)
                && !string.IsNullOrEmpty(UserPassword))
            {
                eventAgg.PublishOnUIThread(new RegisterMessage(UserFirstName, UserSurname, UserApiKey, UserEmail, UserPassword));
            }
            else
            {
                StatusMessage.Enqueue("Please complete all fields.");
            }
        }
        #endregion

        #region Private Methods

        #endregion
    }
}
