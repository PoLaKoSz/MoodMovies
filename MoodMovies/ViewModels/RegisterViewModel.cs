using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources.Validation;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        public RegisterViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
            NewUser = new User();
        }

        private User _newUser;
        public User NewUser { get => _newUser; set { _newUser = value; NotifyOfPropertyChange(); } }

        public async void Register()
        {
            if (!await IsFieldsValid())
                return;

            if (await IsEmailExists())
                return;

            if (!await IsValidApiKey())
                return;

            try
            {
                await OfflineDB.CreateUser(NewUser);
            }
            catch
            {
                StatusMessage.Enqueue("Error while saving new User. Please try again!");
            }

            EventAgg.PublishOnUIThread(new RegisteredMessage());
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

            try
            {
                var isUserWithApiKeyEmailExists = await OfflineDB.GetUserByApiKey(NewUser.ApiKey);

                if (isUserWithApiKeyEmailExists != null)
                {
                    StatusMessage.Enqueue("A User already registered with the same API Key!");
                    NewUser.ApiKey = "";
                    return false;
                }
            }
            catch
            {
                StatusMessage.Enqueue("Error while getting User informations! Please try again!");
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

        private async Task<bool> IsEmailExists()
        {
            try
            {
                var isUserWithEmailExists = await OfflineDB.GetUserByEmail(NewUser.Email);

                if (isUserWithEmailExists != null)
                {
                    StatusMessage.Enqueue("A User already registered with the same Email adddress!");

                    NewUser.Email = "";

                    return true;
                }
            }
            catch
            {
                StatusMessage.Enqueue("Error while getting User informations. Please try again!");
            }

            return false;
        }

        private async Task<bool> IsValidApiKey()
        {
            var isValidApiKey = await Task.Run(() => CredentialValidation.IsValidApiKey(NewUser.ApiKey, OnlineDB));

            if (!isValidApiKey)
            {
                StatusMessage.Enqueue("TMDB API Key is not valid!");
                NewUser.ApiKey = "";
            }

            return isValidApiKey;
        }
    }
}
