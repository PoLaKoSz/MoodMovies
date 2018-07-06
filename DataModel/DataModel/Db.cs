using System;
using System.IO;
using System.Reflection;

namespace DataModel.DataModel
{
    public class Db : IDb
    {
        public DatabaseContext context { get; private set; }

        private bool dbIsSetup;

        public void DumpDatabase()
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
                                        
                    //create main and image cache directory in AppData
                    Directory.CreateDirectory(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies/ImageCache"));
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
