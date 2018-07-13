using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
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
            ActivateItem(RegisterVM);
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
        public RegisterViewModel RegisterVM { get => _registerVM; set { _registerVM = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
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
        private void CleanUp()
        {
            eventAgg.Unsubscribe(this);
        }
        #endregion
    }
}
