using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class LoggedInMessage
    {
        public LoggedInMessage(User user)
        {
            CurrentUser = user;
        }

        public User CurrentUser { get; }
    }

    public class SwitchedUserMessage : LoggedInMessage
    {
        public SwitchedUserMessage(User user)
            : base(user)
        {
        }
    }
}
