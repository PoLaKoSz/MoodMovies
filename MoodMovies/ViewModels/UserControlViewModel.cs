using DataModel.DataModel.Entities;
using MoodMovies.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class UserControlViewModel : BaseViewModel
    {
        public UserControlViewModel(CommonParameters commonParameters, LoginViewModel loginViewModel)
            : base(commonParameters)
        {
            AllUsers = new ObservableCollection<User>();
            _loginVM = loginViewModel;
            ActivateItem(_loginVM);
        }

        private readonly LoginViewModel _loginVM;

        #region Properties
        private User _selectedUser;
        public User SelectedUser { get => _selectedUser; set { _selectedUser = value; NotifyOfPropertyChange(); _loginVM.UserEmail = SelectedUser.Email; } }

        private ObservableCollection<User> _allUsers;
        public ObservableCollection<User> AllUsers { get => _allUsers; set { _allUsers = value; NotifyOfPropertyChange(); } }
        #endregion

        
        /// <summary>
        /// Get all users from db
        /// </summary>
        /// <returns></returns>
        public async Task GetUsers()
        {
            try
            {
                var users = await OfflineDB.GetAllUsers();

                AllUsers = new ObservableCollection<User>(users);
            }
            catch
            {
                StatusMessage.Enqueue("Failed to load users.");
            }
        }
    }
}
