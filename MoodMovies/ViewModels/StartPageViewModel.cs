using Caliburn.Micro;
using DataModel.DataModel.Entities;
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
    public class StartPageViewModel : Conductor<Screen>.Collection.OneActive
    {
        public StartPageViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage)
        {
            eventAgg = _event;
            eventAgg.Subscribe(this);

            offlineDb = offlineService;
            onlineDb = onlineService;
            StatusMessage = statusMessage;

            Items.Add(LoginVM = new LoginViewModel(eventAgg, offlineDb, onlineDb, statusMessage));
            Items.Add(RegisterVM = new RegisterViewModel(eventAgg, offlineDb, onlineDb, statusMessage));
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Providers
        readonly IOfflineServiceProvider offlineDb;
        private IOnlineServiceProvider onlineDb;
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        public SnackbarMessageQueue StatusMessage { get; set; }
        #endregion

        #region Child View Models
        private LoginViewModel _loginVM;
        public LoginViewModel LoginVM { get => _loginVM; set { _loginVM = value; NotifyOfPropertyChange(); } }
        private RegisterViewModel _registerVM;
        private bool _canShowLoginPage;

        public RegisterViewModel RegisterVM { get => _registerVM; set { _registerVM = value; NotifyOfPropertyChange(); } }
        public bool CanShowLoginPage { get => _canShowLoginPage; set { _canShowLoginPage = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public async Task DisplayInitialPage()
        {
            ShowLoginPage();

            var lastActiveUser = await offlineDb.GetCurrentUSer();

            if (lastActiveUser != null)
            {
                eventAgg.PublishOnUIThread(new LoggedInMessage(lastActiveUser));
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
            ActivateItem(LoginVM);
        }

        public void ShowRegistrationPage()
        {
            DeactivateItem(ActiveItem, false);
            ActivateItem(RegisterVM);
        }
        #endregion

        #region Private Methods
        private async Task<bool> HasExistingUser()
        {
            var users = await offlineDb.GetAllUsers();
            return 0 < users.Count;
        }

        private void CleanUp()
        {
            eventAgg.Unsubscribe(this);
        }
        #endregion
    }
}
