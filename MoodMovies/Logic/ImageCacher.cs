using DataModel.DataModel.Entities;
using MoodMovies.DataAccessLayer;
using System;
using System.IO;

namespace MoodMovies.Logic
{
    public class ImageCacher
    {
        private readonly string CacheFolder;
        private readonly IWebClient WebClient;
        private readonly string ImageWebRootPath;



        public ImageCacher(string cacheFolder, IWebClient webClient, string imageWebRootPath)
        {
            CacheFolder = cacheFolder;
            WebClient = webClient;
            ImageWebRootPath = imageWebRootPath;
        }
        


        /// <summary>
        /// Download Movie poster if not exists yet and set the poster cache path
        /// </summary>
        /// <param name="movie"></param>
        /// <exception cref="NullReferenceException">When no poster included to the movie</exception>
        public void ScanPoster(Movies movie)
        {
            if (movie.Backdrop_path == null &&
                movie.Poster_path == null)
                throw new NullReferenceException("Could not find any poster to movie with ID : " + movie.Movie_Id);
            
            string imageRelativePath = movie.Poster_path ?? movie.Backdrop_path;
            string posterDestination = Path.Combine(CacheFolder, imageRelativePath.Replace("/", ""));

            if (ShouldRefreshCacheImage(posterDestination))
            {
                Uri posterURL = new Uri(ImageWebRootPath + imageRelativePath);

                DownloadImage(posterURL, posterDestination);
            }

            movie.Poster_Cache = posterDestination;
        }

        private bool ShouldRefreshCacheImage(string posterPath)
        {
            return !File.Exists(posterPath);
        }

        private void DownloadImage(Uri posterURL, string posterDestination)
        {
            WebClient.DownloadFile(posterURL, posterDestination);
        }

        ~ImageCacher()
        {
            WebClient.Dispose();
        }
    }
}
