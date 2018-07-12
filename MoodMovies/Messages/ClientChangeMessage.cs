using DataModel.DataModel.Entities;
using MoodMovies.Logic;

namespace MoodMovies.Messages
{
    public class ClientChangeMessage
    {
        public IOnlineServiceProvider OnlineDB { get; private set; }
        public Users NewUser { get; private set; }



        public ClientChangeMessage(Users newUser)
        {
            NewUser = newUser;

            OnlineDB = new OnlineServiceProvider();
            OnlineDB.ChangeClient(NewUser.User_ApiKey);
        }
    }
}
