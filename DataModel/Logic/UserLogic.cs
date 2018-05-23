using DataModel.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.Logic
{
    public class UserLogic
    {
        private MoodMoviesEntities db;

        #region Get Procedures
        public Task<User> GetUserDetails(int id, string username)
        {
            try
            {
                db = new MoodMoviesEntities();
                User u = null;
                u = db.Users.Where(x => x.User_ID == id && x.User_Name == username).FirstOrDefault();
                if(u == null)
                {
                    throw new UserException("User does not Exist");
                }
                return Task.FromResult(u);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Set Procedures

        #endregion

        #region Exceptions
        public class UserException: ApplicationException
        {
            public UserException(string message):base(message)
            {

            }
            public UserException(string message, Exception innerException, User user):base(message,innerException)
            {
                User = user;
            }
            public User User { get; }
        }
        #endregion
    }
}
