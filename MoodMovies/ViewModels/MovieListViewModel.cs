using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : ListBaseViewModel, IHandle<MovieListMessage>, IHandle<MovieCardViewModel>, IHandle<IMovieCardMessage>
    {
        public MovieListViewModel(IEventAggregator events):base(events)
        {

        }

        #region Properties
        OfflineServiceProvider offlineDb = new OfflineServiceProvider();
        #endregion

        #region Public Methods       

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
                        //force updating the list from a different thread using custom cross thread extension method
                        Movies.AddOnUIThread(new MovieCardViewModel(movie.Id, movie.Title, new Uri(posterAddress + movie.Poster_path), movie.Overview,
                        movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, eventAgg));
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
                    var user = await offlineDb.GetFirstUser();  //this will betaken from static class******************
                    await offlineDb.AddToWatchList(user, movie);
                }
                else
                {
                    var user = await offlineDb.GetFirstUser(); //this must be taken from static class*************************
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
                var user = await offlineDb.GetFirstUser();  //this will betaken from static class******************
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
                    var user = await offlineDb.GetFirstUser(); //this must be taken from static class*************************
                    await offlineDb.AddToFavourites(user, movie);
                }
                else
                {
                    var user = await offlineDb.GetFirstUser(); //this must be taken from static class*************************
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
                var user = await offlineDb.GetFirstUser();  //this will betaken from static class******************
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
