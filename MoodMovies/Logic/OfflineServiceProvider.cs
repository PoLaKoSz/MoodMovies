using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider : IServiceProvider
    {      
        public async Task CreateUser(Users user)
        {
            //check to see if a user already exists
            await Task.Run(() => Db.context.Users.Add(user));
            Db.context.SaveChanges();
        }        
    }
}
