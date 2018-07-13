using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class LoginMessage : IAccountMessage
    {
        public LoginMessage()
        {

        }
    }

    public class Registermessage :IAccountMessage
    {
        public  Registermessage()
        {

        }
    }

    public interface IAccountMessage
    {

    }
}
