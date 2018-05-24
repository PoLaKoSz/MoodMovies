using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Logic
{
    public static class UserLogic
    {        
        
        #region Get Procedures
        //public Task<User> GetUserDetails(int id, string username)
        //{
            //try
            //{
            //    db = new MoodMoviesEntities();
            //    User u = null;
            //    u = db.Users.Where(x => x.User_ID == id && x.User_Name == username).FirstOrDefault();
            //    if(u == null)
            //    {
            //        throw new UserException("User does not Exist");
            //    }
            //    return Task.FromResult(u);
            //}
            //catch (Exception ex)
            //{
            //    throw;
            //}
        //}
        #endregion

        #region Set Procedures

        #endregion

        #region Exceptions
        public class UserException: ApplicationException
        {
            public UserException(string message):base(message)
            {

            }
            public UserException(string message, Exception innerException, Users user):base(message,innerException)
            {
                User = user;
            }
            public Users User { get; }
        }
        #endregion
    }
}
