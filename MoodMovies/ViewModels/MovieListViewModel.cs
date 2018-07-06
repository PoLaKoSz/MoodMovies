using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : ListBaseViewModel, IHandle<MovieListMessage>, IHandle<MovieCardViewModel>, IHandle<IMovieCardMessage>
    {
        public MovieListViewModel(IEventAggregator events, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage) : base(events, statusMessage)
        {
            offlineDb = serviceProvider;
        }

        #region Properties
        IOfflineServiceProvider offlineDb;
        #endregion

        #region Public Methods       

        #endregion

        #region Private Methods
        /// <summary>
        /// Downloads an image from the specified Uri and return the path to that image if it exists.
        /// </summary>
        /// <param name="poster_path">Poster web URL</param>
        /// <param name="fileName">File relative path with extension</param>
        /// <returns></returns>
        private string DownloadImage(Uri poster_path, string fileName)
        {
            string cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), $"MoodMovies\\ImageCache\\");
            string file = Path.Combine(cacheDirectory, fileName.Replace("/", ""));

            if (!File.Exists(file))
            {
                if (!Directory.Exists(cacheDirectory))
                    Directory.CreateDirectory(cacheDirectory);

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(poster_path, file);
                }
            }
            return file;
        }
        #endregion

        #region IHandle methods
        /// <summary>
        /// Adds movies to the list that have been downloaded either from offline or online db
        /// </summary>
        /// <param name="message"></param>
        public async void Handle(MovieListMessage message)
        {
            Movies.Clear();
            await Task.Run(() =>
            {
                foreach (var movie in message.Movielist.Results)
                {
                    if (!string.IsNullOrEmpty(movie.Poster_path))
                    {
                        var fullUri = new Uri(posterAddress + movie.Poster_path);
                        var cachedImage = DownloadImage(fullUri, movie.Poster_path);
                        //force updating the list from a different thread using custom cross thread extension method
                        Movies.AddOnUIThread(new MovieCardViewModel(movie.Id, movie.Title, fullUri, movie.Overview,
                        movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, cachedImage, eventAgg));
                    }
                }
            });

            eventAgg.PublishOnUIThread(new ResultsReadyMessage());
        }

        public void Handle(MovieCardViewModel message)
        {
            SelectedItem = message;
        }

        public async void Handle(IMovieCardMessage message)
        {
            switch (message)
            {
                case AddToWatchListMessage addWatch:
                    await AddMovieToWatchList(message.MovieCard);
                    break;
                case RemoveFromWatchListMessage removeWatch:
                    await RemoveMovieFromWatchList(message.MovieCard);
                    break;
                case AddToFavouritesMessage addFavourites:
                    await AddMovieToFavourites(message.MovieCard);
                    break;
                case RemoveFromFavouritesMessage removeFavourites:
                    await RemoveMovieFromFavourites(message.MovieCard);
                    break;
            }
        }
        #endregion

        #region MovieCardViewModel Handling
        /// <summary>
        /// Add movie to the watchlist
        /// </summary>
        /// <param name="mvCard"></param>
        public async Task AddMovieToWatchList(MovieCardViewModel mvCard)
        {
            var movie = new Movies();
            //copy the properties that can be copied
            PropertyCopier<MovieCardViewModel, Movies>.Copy(mvCard, movie);

            try
            {
                if (await offlineDb.AddMovie(movie))
                {
                    //then create the link between the user and the movie and the watchlist
                    var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                    await offlineDb.AddToWatchList(user, movie);
                }
                else
                {
                    var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                    var usermovie = await offlineDb.GetUserMovieLink(user, movie);

                    usermovie.Watchlist = true;
                    offlineDb.SaveChanges();
                }
            }
            catch
            {
                //ping a message to the user if necessary
            }
        }
        /// <summary>
        /// Remove movie from the watchlist
        /// </summary>
        /// <param name="mvCard"></param>
        public async Task RemoveMovieFromWatchList(MovieCardViewModel mvCard)
        {
            try
            {
                var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                var movie = await offlineDb.GetMovie(mvCard.Movie_Id);
                await offlineDb.RemoveFromWatchList(user, movie);
            }
            catch
            {

            }
        }
        /// <summary>
        /// Adds movie to the favourites list
        /// </summary>
        /// <param name="mvCard"></param>
        public async Task AddMovieToFavourites(MovieCardViewModel mvCard)
        {
            var movie = new Movies();
            //copy the properties that can be copied
            PropertyCopier<MovieCardViewModel, Movies>.Copy(mvCard, movie);

            try
            {
                if (await offlineDb.AddMovie(movie))
                {
                    //then create the link between the user and the movie and the watchlist
                    var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                    await offlineDb.AddToFavourites(user, movie);
                }
                else
                {
                    var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                    var usermovie = await offlineDb.GetUserMovieLink(user, movie);

                    usermovie.Favourite = true;
                    offlineDb.SaveChanges();
                }
            }
            catch
            {
                //ping a message to the user if necessary
            }
        }
        /// <summary>
        /// Removes a movie from the favourites list
        /// </summary>
        /// <param name="mvCard"></param>
        public async Task RemoveMovieFromFavourites(MovieCardViewModel mvCard)
        {
            try
            {
                var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                var movie = await offlineDb.GetMovie(mvCard.Movie_Id);
                await offlineDb.RemoveFromFavourites(user, movie);
            }
            catch
            {

            }
        }
        #endregion        
    }
}
