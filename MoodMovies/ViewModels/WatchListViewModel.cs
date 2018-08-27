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
                StatusMessage.Enqueue("Internal Error");
            }
        }

        /// <summary>
        /// Add the parameter Movie to the Movies collection and add the Movie
        /// to the WatchList inside the DB
        /// </summary>
        public async void Handle(AddToWatchListMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) == null)
            {
                Movies.Add(message.MovieCard);
            }

            await OfflineDB.AddMovie(CurrentUser, movieCard.Movie);

            await OfflineDB.AddToWatchList(CurrentUser, message.MovieCard.Movie);
        }

        /// <summary>
        /// Remove the parameter Movie from the Movies collection and remove the Movie
        /// from the WatchList inside the DB
        /// </summary>
        public async void Handle(RemoveFromWatchListMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) != null)
            {
                Movies.Remove(movieCard);
            }

            await OfflineDB.RemoveFromWatchList(CurrentUser, message.MovieCard.Movie);
        }
    }
}
