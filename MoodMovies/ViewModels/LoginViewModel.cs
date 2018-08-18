using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public LoginViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage) : base(
            _event, offlineService, onlineService, statusMessage)
        {

        }

        #region  public Properties
        private string _userEmail;
        public string UserEmail { get => _userEmail; set { _userEmail = value; NotifyOfPropertyChange(); } }

        private string _userPassword;
        public string UserPassword { get => _userPassword; set { _userPassword = value; NotifyOfPropertyChange(); } }

        private bool _keepLoggedIn;
        public bool KeepLoggedIn { get => _keepLoggedIn; set { _keepLoggedIn = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public void Login()
        {
            if (!string.IsNullOrEmpty(UserEmail)
                && !string.IsNullOrEmpty(UserPassword))
            {
                eventAgg.PublishOnUIThread(new LoginMessage(UserEmail, UserPassword, KeepLoggedIn));
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
