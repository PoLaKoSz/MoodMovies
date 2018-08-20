using DataModel.DataModel.Entities;

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
