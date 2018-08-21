using Caliburn.Micro;
using MoodMovies.DataAccessLayer;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class StartPageViewModel : Conductor<Screen>.Collection.OneActive, IHandle<RegisteredMessage>
    {
        public StartPageViewModel(CommonParameters commonParameters)
        {
            _eventAgg = commonParameters.EventAggregator;
            _eventAgg.Subscribe(this);

            _offlineDb = commonParameters.OfflineService;
            var onlineDb = commonParameters.OnlineService;
            var statusMessage = commonParameters.StatusMessage;

            Items.Add(LoginVM = new LoginViewModel(commonParameters));
            Items.Add(RegisterVM = new RegisterViewModel(commonParameters));
        }

        private readonly IEventAggregator _eventAgg;
        private readonly IOfflineServiceProvider _offlineDb;

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
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

            var lastActiveUser = await _offlineDb.GetCurrentUSer();

            if (lastActiveUser != null)
            {
                _eventAgg.PublishOnUIThread(new LoggedInMessage(lastActiveUser));
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

        public void Handle(RegisteredMessage message)
        {
            CanShowLoginPage = true;
        }
        #endregion

        #region Private Methods
        private async Task<bool> HasExistingUser()
        {
            var users = await _offlineDb.GetAllUsers();
            return 0 < users.Count;
        }

        private void CleanUp()
        {
            _eventAgg.Unsubscribe(this);
        }
        #endregion
    }
}
