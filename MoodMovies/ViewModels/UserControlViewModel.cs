using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TMdbEasy;

namespace MoodMovies.ViewModels
{
    public class UserControlViewModel : Screen
    {
        public UserControlViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage)
        {
            eventAgg = _event;
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Logging in ..."));

            AllUsers = new ObservableCollection<Users>();
            offlineDb = serviceProvider;
            StatusMessage = statusMessage;
            NewUser = new Users();
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion
        
        #region Properties
        public SnackbarMessageQueue StatusMessage { get; set; }

        private Users _selectedUser;
        public Users SelectedUser { get => _selectedUser; set { _selectedUser = value; NotifyOfPropertyChange(); } }

        private Users _currentUser;
        public Users CurrentUser { get => _currentUser; set { _currentUser = value; NotifyOfPropertyChange(); } }

        private ObservableCollection<Users> _allUsers;
        public ObservableCollection<Users> AllUsers { get => _allUsers; set { _allUsers = value; NotifyOfPropertyChange(); } }

        public Users NewUser { get => _newUser; set { _newUser = value; NotifyOfPropertyChange(); } }
        private Users _newUser;
        #endregion

        readonly IOfflineServiceProvider offlineDb;

        #region Public methods        
        /// <summary>
        /// Sets the selected item to the current user
        /// </summary>
        public async Task SetCurrentUser()
        {
            try
            {
                eventAgg.PublishOnUIThread(new ClientChangeMessage(SelectedUser));

                if (CurrentUser != null)
                {                    
                    //change the fields in db first
                    await offlineDb.ChangeCurrentUserField(CurrentUser.User_ApiKey, false);
                }
                await offlineDb.ChangeCurrentUserField(SelectedUser.User_ApiKey, true);

                CurrentUser = SelectedUser;                
            }
            catch
            {
                StatusMessage.Enqueue("Failed to change the current user.");
            }
        }

        /// <summary>
        /// Creates and adds a new user to the database
        /// </summary>
        public async void CreateNewUser()
        {
            if (string.IsNullOrEmpty(NewUser.User_ApiKey)
                && string.IsNullOrEmpty(NewUser.User_Name)
                && string.IsNullOrEmpty(NewUser.User_Surname))
            {
                StatusMessage.Enqueue("Please fill in all the fields and try again.");
                return;
            }

            try
            {
                NewUser.User_Active = true;
                NewUser.Current_User = false;

                var userfound = await offlineDb.GetUserByApiKey(NewUser.User_ApiKey);
                if (userfound == null)
                {
                    if (!IsValidApiKey(NewUser.User_ApiKey))
                    {
                        StatusMessage.Enqueue("The API Key not valid.");
                        return;
                    }

                    await offlineDb.CreateUser(NewUser);

                    AllUsers.Add(NewUser);

                    NewUser = new Users();
                }
                else
                {
                    StatusMessage.Enqueue("A user with the same Api Key already exists.");
                }
            }
            catch
            {
                StatusMessage.Enqueue("Failed to create the user.");
            }
        }

        /// <summary>
        /// Validate the API Key with the TMDB Easy wrapper
        /// </summary>
        /// <param name="apiKey">TMDB API Key</param>
        /// <returns></returns>
        private bool IsValidApiKey(string apiKey)
        {
            try
            {
                new EasyClient(apiKey);
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Deletes current user
        /// </summary>
        public async void DeleteCurrentUser()
        {
            if (SelectedUser != null)
            {
                if (SelectedUser == CurrentUser)
                {
                    CurrentUser = null;
                    eventAgg.PublishOnUIThread(new ClientChangeMessage(new Users()));
                }
                await offlineDb.DeleteUser(SelectedUser);
            }
            await GetUsers();
        }

        public async Task GetUsers()
        {
            try
            {
                var users = await offlineDb.GetAllUsers();

                CurrentUser = users.Where(x => x.Current_User).SingleOrDefault();

                if (CurrentUser == null)
                    eventAgg.PublishOnUIThread(new NavigateToUsersMenu());
                else
                    eventAgg.PublishOnUIThread(new ClientChangeMessage(CurrentUser));

                AllUsers.Clear();

                AllUsers = new ObservableCollection<Users>(users);
            }
            catch
            {
                eventAgg.PublishOnUIThread(new NavigateToUsersMenu());
                StatusMessage.Enqueue("Failed to load users.");
            }
        }
        #endregion
    }
}
