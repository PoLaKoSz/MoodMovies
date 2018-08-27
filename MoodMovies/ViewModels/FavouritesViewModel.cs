using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class FavouritesViewModel: ListBaseViewModel,
        IHandle<AddToFavouritesMessage>,
        IHandle<RemoveFromFavouritesMessage>
    {
        public FavouritesViewModel(IListViewModelParams commonParameters)
            : base(commonParameters)
        {
        }

        
        /// <summary>
        /// Loads up movie cards for the favourite items that are found
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            if (Movies.Count != 0)
                return;

            try
            {
                var movies = await OfflineDB.GetAllFavouriteItems(CurrentUser);

                await base.PushToUI(movies);
            }
            catch
            {
                StatusMessage.Enqueue("Internal Error");
            }
        }

        /// <summary>
        /// Add the parameter Movie to the Movies collection and add the Movie
        /// to the favourites inside the DB
        /// </summary>
        public async void Handle(AddToFavouritesMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) == null)
            {
                Movies.Add(message.MovieCard);

                await OfflineDB.AddMovie(CurrentUser, movieCard.Movie);

                await OfflineDB.AddToFavourites(CurrentUser, message.MovieCard.Movie);
            }
        }

        /// <summary>
        /// Remove the parameter Movie from the Movies collection and remove the Movie
        /// from the favourites inside the DB
        /// </summary>
        public async void Handle(RemoveFromFavouritesMessage message)
        {
            var movieCard = message.MovieCard;

            if (GetMovieCard(movieCard) != null)
            {
                Movies.Remove(movieCard);

                await OfflineDB.RemoveFromFavourites(CurrentUser, movieCard.Movie);
            }
        }
    }
}
