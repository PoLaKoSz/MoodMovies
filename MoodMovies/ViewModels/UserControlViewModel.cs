using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class UserControlViewModel : BaseViewModel
    {
        public UserControlViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
            AllUsers = new ObservableCollection<User>();
        }

        #region Properties
        private User _selectedUser;
        public User SelectedUser { get => _selectedUser; set { _selectedUser = value; NotifyOfPropertyChange(); Email = SelectedUser?.Email ?? ""; } }

        private User _currentUser;
        public User CurrentUser { get => _currentUser; set { _currentUser = value; NotifyOfPropertyChange(); } }

        private ObservableCollection<User> _allUsers;
        public ObservableCollection<User> AllUsers { get => _allUsers; set { _allUsers = value; NotifyOfPropertyChange(); } }

        private string _email;
        public string Email { get => _email; set { _email = value; NotifyOfPropertyChange(); } }

        private string _password;
        public string Password { get => _password; set { _password = value; NotifyOfPropertyChange(); } }

        private bool _keepLoggedIn;
        public bool KeepLoggedIn { get => _keepLoggedIn; set { _keepLoggedIn = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public methods                 
        /// <summary>
        /// Get all users from db
        /// </summary>
        /// <returns></returns>
        public async Task GetUsers()
        {
            try
            {
                var users = await offlineDb.GetAllUsers();

                AllUsers.Clear();

                AllUsers = new ObservableCollection<User>(users);
            }
            catch
            {
                StatusMessage.Enqueue("Failed to load users.");
            }
        }

        public async Task Login()
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Logging in"));
            try
            {
                var user = await offlineDb.GetUserByEmailPassword(Email, Password);

                if (user != null)
                {
                    //set to current user if keep me logged in checkbock selected
                    if (KeepLoggedIn)
                    {
                        //change status in db
                        await offlineDb.SetCurrentUserFieldToTrue(user);
                    }
                    else if (user.IsCurrentUser && !KeepLoggedIn)
                    {
                        offlineDb.SetCurrentUserFieldToFalse(user);
                    }
                    CurrentUser = SelectedUser;
                    eventAgg.PublishOnUIThread(new SwitchedUserMessage(user));
                    Email = "";
                    Password = "";
                }
                else
                {
                    StatusMessage.Enqueue("Login credentials do not match. Email or the password is incorrect.");
                }
            }
            catch
            {

            }

            eventAgg.PublishOnUIThread(new StopLoadingMessage());
        }
        #endregion
    }
}
