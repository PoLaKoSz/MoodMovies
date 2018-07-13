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
        public  RegisterMessage()
        {

        }
    }

    public interface IAccountMessage
    {

    }
}
