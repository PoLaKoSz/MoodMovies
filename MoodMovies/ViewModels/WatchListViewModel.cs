using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel,
        IHandle<AddToWatchListMessage>,
        IHandle<RemoveFromWatchListMessage>
    {
        public WatchListViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
        }

        
        public async Task Load()
        {
            if (Movies.Count != 0)
                return;

            try
            {
                var movies = await OfflineDB.GetAllWatchListItems(CurrentUser);

                await base.PushToUI(movies);
            }
            catch
            {
                StatusMessage.Enqueue("Error while loading WatchList!");
            }
        }

        /// <summary>
        /// Add the parameter Movie to the Movies collection and add the Movie
        /// to the WatchList inside the DB
        /// </summary>
        public async void Handle(AddToWatchListMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) != null)
                return;

            try
            {
                Movies.Add(message.MovieCard);

                await OfflineDB.AddMovie(CurrentUser, movieCard.Movie);

                await OfflineDB.AddToWatchList(CurrentUser, message.MovieCard.Movie);
            }
            catch
            {
                StatusMessage.Enqueue("Error while adding the movie to the WatchList!");
                Movies.Remove(message.MovieCard);
            }
        }

        /// <summary>
        /// Remove the parameter Movie from the Movies collection and remove the Movie
        /// from the WatchList inside the DB
        /// </summary>
        public async void Handle(RemoveFromWatchListMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) == null)
                return;

            try
            {
                Movies.Remove(movieCard);

                await OfflineDB.RemoveFromWatchList(CurrentUser, movieCard.Movie);
            }
            catch
            {
                StatusMessage.Enqueue("Error while removing the movie from the WatchList!");
                Movies.Remove(movieCard);
            }
        }
    }
}
