﻿using System.IO;

namespace MoodMovies.Models
{
    public class AppFolders
    {
        public readonly string AppRootFolder;
        public readonly string ImageCacheFolder;



        /// <summary>
        /// Declare all important folder paths and create directories
        /// </summary>
        /// <param name="rootDirecory">This folder will contain all files and folders
        /// which is neccessary to run the application</param>
        public AppFolders(string rootDirecory)
        {
            AppRootFolder = rootDirecory;

            ImageCacheFolder = Path.Combine(AppRootFolder, "ImageCache");
            
            Directory.CreateDirectory(ImageCacheFolder);
        }
    }
}
