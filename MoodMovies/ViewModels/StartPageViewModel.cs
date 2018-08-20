using Caliburn.Micro;
using DataModel.DataModel.Entities;
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
    public class StartPageViewModel : Conductor<Screen>.Collection.OneActive, IHandle<IAccountMessage>
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
        /// <summary>
        /// Verifies the given user credentials are correct and proceeds to log them in if so.
        /// Checks: Email and Password match
        /// </summary>
        /// <param name="message"></param>
        private async Task VerifyLogin(IAccountMessage obj)
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Logging in"));
            try
            {
                if (obj is LoginMessage message)
                {
                    var user = await offlineDb.GetUserByEmailPassword(message.Email, message.Password);

                    if (user != null)
                    {
                        //set the apikey
                        onlineDb.ChangeClient(user.ApiKey);
                        //set to current user if keep me logged in checkboc selected
                        if (message.KeepLoggedIn)
                        {
                            //change status in db
                            await offlineDb.SetCurrentUserFieldToTrue(user);
                        }
                        else if (user.IsCurrentUser && !message.KeepLoggedIn)
                        {
                            //change status in db for 
                            offlineDb.SetCurrentUserFieldToFalse(user);
                        }

                        eventAgg.PublishOnUIThread(new LoggedInMessage(user));
                        LoginVM.UserEmail = "";
                        LoginVM.UserPassword = "";
                    }
                    else
                    {
                        StatusMessage.Enqueue("Login credentials do not match. Email or the password is incorrect.");
                    }
                }
            }
            catch
            {
                StatusMessage.Enqueue("Api Key is not valid");
            }

            eventAgg.PublishOnUIThread(new StopLoadingMessage());
        }
        /// <summary>
        /// Creates a new user account and verifies the given credentials are correct.
        /// Checks: Email does not exist already, ApiKey is Valid.
        /// </summary>
        /// <param name="message"></param>
        private async Task RegisterNewUser(IAccountMessage obj)
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Verifying your details"));

            if (obj is RegisterMessage message)
            {
                var user = await offlineDb.GetUserByEmail(message.Email);

                if (user == null)
                {
                    try
                    {
                        onlineDb.ChangeClient(message.ApiKey);
                        var apikeyExists = await offlineDb.ApiKeyExists(message.ApiKey);
                        if (!apikeyExists)
                        {
                            var newUser = new User()
                            {
                                Name = message.FirstName,
                                Surname = message.Surname,
                                ApiKey = message.ApiKey,
                                Email = message.Email,
                                Password = message.Password
                            };
                            await offlineDb.CreateUser(newUser);

                            //send them straight to login page
                            ShowLoginPage();
                            LoginVM.UserEmail = message.Email;
                            LoginVM.UserPassword = message.Password;
                            LoginVM.KeepLoggedIn = true;

                            //cleare the data from the register page
                            RegisterVM.UserApiKey = "";
                            RegisterVM.UserFirstName = "";
                            RegisterVM.UserSurname = "";
                            RegisterVM.UserEmail = "";
                            RegisterVM.UserPassword = "";
                        }
                        else
                        {
                            StatusMessage.Enqueue("The Api Key You provided is already allocated to a user.");
                        }
                    }
                    catch
                    {
                        StatusMessage.Enqueue("The Api Key You provided is invalid.");
                    }
                }
                else
                {
                    StatusMessage.Enqueue("A User with that email already exists.");
                }
            }

            eventAgg.PublishOnUIThread(new StopLoadingMessage());
        }
        #endregion
        /// <summary>
        /// Handle The various account messages
        /// </summary>
        /// <param name="message"></param>
        public async void Handle(IAccountMessage message)
        {
            switch (message)
            {
                case LoginMessage lm:
                    await VerifyLogin(message);
                    break;
                case RegisterMessage rm:
                    await RegisterNewUser(message);
                    break;
            }
        }
    }
}
