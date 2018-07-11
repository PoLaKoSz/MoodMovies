using System.Threading.Tasks;
using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class FavouritesViewModel: ListBaseViewModel
    {
        public FavouritesViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage, ImageCacher imageCacher, Users currentUser)
            : base(_event, statusMessage, currentUser)
        {
            DisplayName = "Favourites";
            offDb = serviceProvider;
            ImageCacher = imageCacher;
        }

        #region Fields
        readonly IOfflineServiceProvider offDb;
        private readonly ImageCacher ImageCacher;
        #endregion

        #region Methods
        /// <summary>
        /// Loads up movie cards for the favourite items that are found
        /// </summary>
        /// <returns></returns>
        public async Task LoadFavouriteItems()
        {
            try
            {
                var movies = await offDb.GetAllFavouriteItems(CurrentUser);
                //build up the movie card view models
                Movies.Clear();
                await Task.Run(() =>
                {
                    foreach (Movies movie in movies)
                    {
                        if (!string.IsNullOrEmpty(movie.Poster_path))
                        {
                            ImageCacher.ScanPoster(movie);

                            var card = new MovieCardViewModel(movie, eventAgg)
                            {
                                IsFavourited = true,
                                Parent = this
                            };
                            //force updating the list from a different thread using custom cross thread extension method
                            Movies.AddOnUIThread(card);
                        }
                    }
                });
            }
            catch when (UserControl.CurrentUser == null)
            {
                StatusMessage.Enqueue("Please select a user from the 'User' page.");
            }
            catch
            {
                StatusMessage.Enqueue("Internal Error");
            }

        }
        #endregion
    }
}
