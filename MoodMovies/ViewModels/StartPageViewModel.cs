using Caliburn.Micro;
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
            _loginVM = loginViewModel;
            Items.Add(RegisterVM = new RegisterViewModel(commonParameters));
        }


        #region Child View Models
        private readonly LoginViewModel _loginVM;
        private RegisterViewModel _registerVM;
        private bool _canShowLoginPage;

        public RegisterViewModel RegisterVM { get => _registerVM; set { _registerVM = value; NotifyOfPropertyChange(); } }
        public bool CanShowLoginPage { get => _canShowLoginPage; set { _canShowLoginPage = value; NotifyOfPropertyChange(); } }
        #endregion

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
            ActivateItem(_loginVM);
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
            var users = await OfflineDB.GetAllUsers();
            return 0 < users.Count;
        }
        #endregion
    }
}
