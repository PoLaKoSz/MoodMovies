using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    public class SearchResultsViewModel : ListBaseViewModel,
        IHandle<MovieListMessage>,
        IHandle<MovieCardViewModel>,
        IHandle<IMovieCardMessage>
    {
        public SearchResultsViewModel(CommonParameters commonParameters, ImageCacher imageCacher)
            : base(commonParameters)
        {
            ImageCacher = imageCacher;
            _onlineDB = commonParameters.OnlineService;
        }

        private readonly ImageCacher ImageCacher;
        private readonly IOnlineServiceProvider _onlineDB;

        public bool CanNavigateToPreviousPage => _onlineDB.SearchQuery.PageNumber != 1;


        public void NavigateToPreviousPage()
        {
            EventAgg.PublishOnUIThread(new BrowseSearchResultsMessage(--_onlineDB.SearchQuery.PageNumber));
        }

        public void NavigateToNextPage()
        {
            EventAgg.PublishOnUIThread(new BrowseSearchResultsMessage(++_onlineDB.SearchQuery.PageNumber));
        }



        /// <summary>
        /// Create a <see cref="Movies"/> object from a <see cref="Movie"/> one
        /// </summary>
        /// <param name="movie">TMDB Movie object</param>
        /// <param name="cachedPosterPath">Movie poster absolute path on the HDD</param>
        /// <returns></returns>
        private Movies ParseFromTmdb(Movie movie)
        {
            return new Movies()
            {
                Movie_Id = movie.Id,
                Vote_count = movie.Vote_count,
                Video = movie.Video,
                Vote_average = movie.Vote_average,
                Title = movie.Title,
                Popularity = movie.Popularity,
                Poster_path = movie.Poster_path,
                Original_language = movie.Original_language,
                Original_title = movie.Original_title,
                Backdrop_path = movie.Backdrop_path,
                Adult = movie.Adult,
                Overview = movie.Overview,
                Release_date = movie.Release_date,
            };
        }
        
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
                foreach (Movie movie in message.Movielist)
                {
                    if (!string.IsNullOrEmpty(movie.Poster_path))
                    {
                        Movies movieEntity = ParseFromTmdb(movie);
                        ImageCacher.ScanPoster(movieEntity);

                        Movies.AddOnUIThread(new MovieCardViewModel(movieEntity, EventAgg));
                    }
                }
            });

            EventAgg.PublishOnUIThread(new ResultsReadyMessage());
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
            try
            {
                if (await OfflineDB.AddMovie(mvCard.Movie))
                {
                    //then create the link between the user and the movie and the watchlist
                    await OfflineDB.AddToWatchList(CurrentUser, mvCard.Movie);
                }
                else
                {
                    var usermovie = await OfflineDB.GetUserMovieLink(CurrentUser, mvCard.Movie);

                    usermovie.Watchlist = true;
                    OfflineDB.SaveChanges();
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
                var movie = await OfflineDB.GetMovie(mvCard.Movie.Movie_Id);
                await OfflineDB.RemoveFromWatchList(CurrentUser, movie);
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
            try
            {
                if (await OfflineDB.AddMovie(mvCard.Movie))
                {
                    //then create the link between the user and the movie and the watchlist
                    await OfflineDB.AddToFavourites(CurrentUser, mvCard.Movie);
                }
                else
                {
                    var usermovie = await OfflineDB.GetUserMovieLink(CurrentUser, mvCard.Movie);

                    usermovie.Favourite = true;
                    OfflineDB.SaveChanges();
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
                var movie = await OfflineDB.GetMovie(mvCard.Movie.Movie_Id);
                await OfflineDB.RemoveFromFavourites(CurrentUser, movie);
            }
            catch
            {

            }
        }
        #endregion
    }
}
