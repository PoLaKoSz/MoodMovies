using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Logic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class UserControlViewModel : Screen
    {
        public UserControlViewModel()
        {
            AllUsers = new ObservableCollection<Users>();
            GetUsers();
        }

        #region Properties
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

        #region Public methods
        /// <summary>
        /// Sets the selected item to the current user
        /// </summary>
        public async Task SetCurrentUser()
        {
            try
            {
                var offdb = new OfflineServiceProvider();
                if (CurrentUser != null)
                {                    
                    //change the fields in db first
                    await offdb.ChangeCurrentUserField(CurrentUser.User_ApiKey, false);                    
                }
                await offdb.ChangeCurrentUserField(SelectedUser.User_ApiKey, true);
                //then perform the change for the ui and logic
                CurrentUser = SelectedUser;
                UserControl.CurrentUser = CurrentUser;
            }
            catch
            {
                //failed to change currentuser
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
                    var offDb = new OfflineServiceProvider();

                    var user = new Users()
                    {
                        User_Name = FirstName,
                        User_Surname = SurName,
                        User_ApiKey = ApiKey,
                        User_Active = true,
                        Current_User = false
                    };
                    //check if user exists
                    var userfound = await offDb.GetUserByApiKey(ApiKey);
                    if (userfound == null)
                    {
                        await offDb.CreateUser(user);
                        await GetUsers();
                    }
                }
                catch
                {
                    //failed to create user
                }
            }
            else
            {
                //details not complete
            }
        }
        #endregion

        #region Private Methods
        private async Task GetUsers()
        {
            try
            {
                var offDb = new OfflineServiceProvider();

                var users = await offDb.GetAllUsers();

                //clear the list
                AllUsers.Clear();
                //renew the list
                AllUsers = new ObservableCollection<Users>(users);
            }
            catch
            {
                //failed to load users
            }           
        }
        #endregion
    }
}
