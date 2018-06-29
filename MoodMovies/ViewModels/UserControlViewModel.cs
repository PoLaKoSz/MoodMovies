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

        #endregion

        #region Public methods
        public void SetCurrentUser()
        {
            CurrentUser = SelectedUser;
            UserControl.CurrentUser = CurrentUser;
        }
        #endregion

        #region Private Methods
        private async void GetUsers()
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
