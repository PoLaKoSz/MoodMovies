using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Logic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using MaterialDesignThemes.Wpf;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class UserControlViewModel : Screen
    {
        public UserControlViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage)
        {
            eventAgg = _event;
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

        private ObservableCollection<Users> _allUsers = new ObservableCollection<Users>();
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
                if (CurrentUser != null)
                {
                    //change the fields in db first
                    await offlineDb.ChangeCurrentUserField(CurrentUser.User_ApiKey, false);
                }
                await offlineDb.ChangeCurrentUserField(SelectedUser.User_ApiKey, true);

                CurrentUser = SelectedUser;

                eventAgg.PublishOnUIThread(new ClientChangeMessage(CurrentUser));
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
            if (!string.IsNullOrEmpty(NewUser.User_ApiKey)
                && !string.IsNullOrEmpty(NewUser.User_Name)
                && !string.IsNullOrEmpty(NewUser.User_Surname))
            {
                try
                {
                    NewUser.User_Active = true;
                    NewUser.Current_User = false;

                    var userfound = await offlineDb.GetUserByApiKey(NewUser.User_ApiKey);
                    if (userfound == null)
                    {
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
            else
            {
                StatusMessage.Enqueue("Please fill in all the fields and try again.");
            }
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

                eventAgg.PublishOnUIThread(new ClientChangeMessage(CurrentUser));

                AllUsers.Clear();

                AllUsers = new ObservableCollection<Users>(users);
            }
            catch
            {
                StatusMessage.Enqueue("Failed to load users.");
            }
        }
        #endregion
    }
}
