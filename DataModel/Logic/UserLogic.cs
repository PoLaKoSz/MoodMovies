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
        private static DatabaseContext context;

        public static void DumpDatabase()
        {
            var fullDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies\\MoodMoviesDB.db");
            if(!File.Exists(fullDbPath))
            {
                //grab the resource
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("DataModel.Resources.MoodMoviesDB.db");

                //create directory in AppData
                Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies"));

                var fileStream = File.Create(fullDbPath);
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
                //File.WriteAllBytes(fullDbPath, Properties.Resources.MoodMoviesDb.db);                
            }

            //set the connection string
            context = new DatabaseContext(fullDbPath);

            context.Users.Add(new Users()
            {                
                User_Name = "Ela",
                User_Surname = "Maciejewska",
                User_ApiKey="liygfi6wegkvh"
            });

            context.SaveChanges();
        }
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
