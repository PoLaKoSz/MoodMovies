using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MoodMovies.Resources.Validation;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage) : base(
            _event, offlineService, onlineService, statusMessage)
        {
            NewUser = new User();
        }

        private User _newUser;
        public User NewUser { get => _newUser; set { _newUser = value; NotifyOfPropertyChange(); } }

        public async void Register()
        {
            var isValidFields = await IsFieldsValid();

            if (!isValidFields)
                return;

            var isUserWithEmailExists = await offlineDb.GetUserByEmail(NewUser.Email);

            if (isUserWithEmailExists != null)
            {
                StatusMessage.Enqueue("A User already registered with the same Email adddress!");
                NewUser.Email = "";
                return;
            }

            var isValidApiKey = await Task.Run(() => CredentialValidation.IsValidApiKey(NewUser.ApiKey, onlineDb));

            if (!isValidApiKey)
            {
                StatusMessage.Enqueue("TMDB API Key is not valid!");
                NewUser.ApiKey = "";
                return;
            }

            await offlineDb.CreateUser(NewUser);

            StatusMessage.Enqueue("Your account has been successfully created!");
            NewUser = new User();
        }

        private async Task<bool> IsFieldsValid()
        {
            if (!CredentialValidation.IsValidFirstName(NewUser.Name))
            {
                StatusMessage.Enqueue("First name is not valid!");
                NewUser.Name = "";
                return false;
            }

            if (!CredentialValidation.IsValidSurName(NewUser.Surname))
            {
                StatusMessage.Enqueue("Surname is not valid!");
                NewUser.Surname = "";
                return false;
            }

            var isUserWithApiKeyEmailExists = await offlineDb.GetUserByApiKey(NewUser.ApiKey);

            if (isUserWithApiKeyEmailExists != null)
            {
                StatusMessage.Enqueue("A User already registered with the same API Key!");
                NewUser.ApiKey = "";
                return false;
            }

            if (!CredentialValidation.IsValidEmail(NewUser.Email))
            {
                StatusMessage.Enqueue("Email address is not valid!");
                NewUser.Email = "";
                return false;
            }

            if (!CredentialValidation.IsValidPassword(NewUser.Password))
            {
                StatusMessage.Enqueue("Password is not valid!");
                NewUser.Password = "";
                return false;
            }

            return true;
        }
    }
}
