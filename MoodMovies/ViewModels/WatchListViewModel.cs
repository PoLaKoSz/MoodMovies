using DataModel.DataModel.Entities;
using MoodMovies.Logic;
using MoodMovies.Models;
using MoodMovies.Resources;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel
    {
        public WatchListViewModel(CommonParameters commonParameters, ImageCacher imageCacher)
            : base(commonParameters)
        {
            DisplayName = "Favourites";
            ImageCacher = imageCacher;
        }

        private readonly ImageCacher ImageCacher;
        
        public async Task LoadWatchListItems()
        {
            try
            {
                var movies = await OfflineDB.GetAllWatchListItems(CurrentUser);
                //build up the movie card view models
                Movies.Clear();
                await Task.Run(() =>
                {
                    foreach (Movies movie in movies)
                    {
                        if (!string.IsNullOrEmpty(movie.Poster_path))
                        {
                            ImageCacher.ScanPoster(movie);

                            var card = new MovieCardViewModel(movie, EventAgg)
                            {
                                IsWatchListed = true,
                                Parent = this
                            };
                            //force updating the list from a different thread using custom cross thread extension method
                            Movies.AddOnUIThread(card);
                        }
                    }
                });
            }
            catch when (CurrentUser == null)
            {
                StatusMessage.Enqueue("Please select a user from the 'User' page.");
            }
            catch
            {
                StatusMessage.Enqueue("Internal Error");
            }
        }
    }
}
