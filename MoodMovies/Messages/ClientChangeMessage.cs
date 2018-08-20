using DataModel.DataModel.Entities;
using MoodMovies.Logic;

namespace MoodMovies.Messages
{
    public class ClientChangeMessage
    {
        public IOnlineServiceProvider OnlineDB { get; private set; }
        public User NewUser { get; private set; }



        public ClientChangeMessage(User newUser)
        {
            NewUser = newUser;

            OnlineDB = new OnlineServiceProvider();
            OnlineDB.ChangeClient(NewUser.ApiKey);
        }
    }
}
