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
            GetUsers();            
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

        private string _firstName;
        public string FirstName { get => _firstName; set { _firstName = value; NotifyOfPropertyChange(); } }

        private string _surName;
        public string SurName { get => _surName; set { _surName = value; NotifyOfPropertyChange(); } }

        private string _apiKey;
        public string ApiKey { get => _apiKey; set { _apiKey = value; NotifyOfPropertyChange(); } }
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
                //then perform the change for the ui and logic
                CurrentUser = SelectedUser;
                UserControl.CurrentUser = CurrentUser;
                eventAgg.PublishOnUIThread(new ClientChangeMessage());
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
            if (!string.IsNullOrEmpty(ApiKey) 
                && !string.IsNullOrEmpty(FirstName)
                && !string.IsNullOrEmpty(SurName))
            {
                try
                {
                    var user = new Users()
                    {
                        User_Name = FirstName,
                        User_Surname = SurName,
                        User_ApiKey = ApiKey,
                        User_Active = true,
                        Current_User = false
                    };
                    //check if user exists
                    var userfound = await offlineDb.GetUserByApiKey(ApiKey);
                    if (userfound == null)
                    {
                        await offlineDb.CreateUser(user);
                        await GetUsers();
                        FirstName = "";
                        SurName = "";
                        ApiKey = "";
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
            if(SelectedUser != null)
            {
                if(SelectedUser == UserControl.CurrentUser)
                {
                    UserControl.CurrentUser = null;
                    CurrentUser = null;
                    eventAgg.PublishOnUIThread(new ClientChangeMessage());
                }
                await offlineDb.DeleteUser(SelectedUser);                
            }
            await GetUsers();
        }
        #endregion

        #region Private Methods
        private async Task GetUsers()
        {
            try
            {
                var users = await offlineDb.GetAllUsers();

                // Get the current user if one has been set
                UserControl.CurrentUser = users.Where(x => x.Current_User).SingleOrDefault();
                if(UserControl.CurrentUser  != null)
                    CurrentUser = UserControl.CurrentUser;

                //clear the list
                AllUsers.Clear();
                //renew the list
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
