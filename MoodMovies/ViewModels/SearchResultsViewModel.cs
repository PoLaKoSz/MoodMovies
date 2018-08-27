using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.DataAccessLayer;
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
        IHandle<AddToWatchListMessage>,
        IHandle<RemoveFromWatchListMessage>,
        IHandle<AddToFavouritesMessage>,
        IHandle<RemoveFromFavouritesMessage>
    {
        public SearchResultsViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
            _onlineDB = commonParameters.OnlineService;
        }

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

                        base.ImageCacher.ScanPoster(movieEntity);

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

        public void Handle(AddToWatchListMessage message)
        {
            UpdateMovie(message.MovieCard);
        }

        public void Handle(RemoveFromWatchListMessage message)
        {
            UpdateMovie(message.MovieCard);
        }

        public void Handle(AddToFavouritesMessage message)
        {
            UpdateMovie(message.MovieCard);
        }

        public void Handle(RemoveFromFavouritesMessage message)
        {
            UpdateMovie(message.MovieCard);
        }

        private void UpdateMovie(MovieCardViewModel movieCard)
        {
            int movieIndex = Movies.IndexOf(movieCard);

            if (0 <= movieIndex)
            {
                Movies[movieIndex] = movieCard;
            }
        }
        #endregion
    }
}
