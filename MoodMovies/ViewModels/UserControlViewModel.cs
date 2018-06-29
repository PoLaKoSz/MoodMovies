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

        private Users _selectedUser;
        public Users SelectedUser { get => _selectedUser; set { _selectedUser = value; NotifyOfPropertyChange(); } }

        private ObservableCollection<Users> _allUsers = new ObservableCollection<Users>();
        public ObservableCollection<Users> AllUsers { get => _allUsers; set { _allUsers = value; NotifyOfPropertyChange(); } }

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
    }
}
