using DataModel.Data;
using DataModel.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class UserControl
    {
        private readonly UserLogic userLogic;

        public UserControl()
        {
            userLogic = new UserLogic();
        }

        //public async Task<User> GetUserDetails(int id, string username)
        //{
        //    return await userLogic.GetUserDetails(id,username);
        //}
    }
}
