using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel
    {
        public WatchListViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider) : base(_event)
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
                var user = await offlineDb.GetFirstUser();          //replace by getting from static class**********
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
                            movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, eventAgg)
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
            catch
            {
                throw;
                //implement some error handling*************
            }

        }
        #endregion

    }
}
