using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel
    {
        public WatchListViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage) : base(_event, statusMessage)
        {
            DisplayName = "Favourites";
            offlineDb = serviceProvider;
        }

        #region Properties
        IOfflineServiceProvider offlineDb;
        #endregion

        #region Methods
        public async Task LoadWatchListItems()
        {
            try
            {
                var user = await offlineDb.GetUser(UserControl.CurrentUser.User_Id);
                var movies = await offlineDb.GetAllWatchListItems(user);
                //build up the movie card view models
                Movies.Clear();
                await Task.Run(() =>
                {
                    foreach (var movie in movies)
                    {
                        if (!string.IsNullOrEmpty(movie.Poster_path))
                        {
                            var card =  new MovieCardViewModel(movie.Movie_Id, movie.Title, new Uri(movie.Poster_path), movie.Overview,
                            movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, movie.Poster_Cache, eventAgg)
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
