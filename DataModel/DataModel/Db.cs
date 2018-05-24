using DataModel.DataModel.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.DataModel
{
    public static class Db
    {
        public static DatabaseContext context;

        private static bool dbIsSetup;

        public static void DumpDatabase()
        {
            var fullDbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies\\MoodMoviesDB.db");
            //check the database has not already been setup
            if (!dbIsSetup)
            {
                if (!File.Exists(fullDbPath))
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
                }

                //set the connection string
                context = new DatabaseContext(fullDbPath);
                dbIsSetup = true;
            }            
        }
    }
}
