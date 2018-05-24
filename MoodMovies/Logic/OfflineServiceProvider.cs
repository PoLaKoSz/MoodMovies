using DataModel.DataModel;
using DataModel.DataModel.Entities;
using DataModel.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider : IServiceProvider
    {      
        public void CreateUser(Users user)
        {
            //check to see if a user already exists
            Db.context.Users.Add(user);
            Db.context.SaveChanges();
        }        
    }
}
