using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class LoginMessage : IAccountMessage
    {
        public LoginMessage(string email, string password, bool keeploggedin)
        {
            Email = email;
            Password = password;
            KeepLoggedIn = keeploggedin;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public bool KeepLoggedIn { get; set; }
    }

    public class RegisterMessage :IAccountMessage
    {
        public  RegisterMessage(string firstname, string surname, string apikey, string email, string password)
        {
            FirstName = firstname;
            Surname = surname;
            ApiKey = apikey;
            Email = email;
            Password = password;
        }

        public string FirstName { get; set; }
        public string Surname { get; set; }
        public string ApiKey { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }      
    }

    public interface IAccountMessage
    {

    }

    public class LoggedInMessage
    {
        public LoggedInMessage(Users user)
        {
            CurrentUser = user;
        }

        public Users CurrentUser { get; set; }
    }
}
