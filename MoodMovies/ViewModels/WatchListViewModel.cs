using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel
    {
        public WatchListViewModel(IEventAggregator _event) : base(_event)
        {
            DisplayName = "Favourites";
        }

        #region Fields

        #endregion

        #region Properties
        OfflineServiceProvider offDb = new OfflineServiceProvider();
        #endregion

        #region Methods
        public async Task LoadWatchListItems()
        {
            var user = await offDb.GetFirstUser();
            var movies = await offDb.GetAllWatchListItems(user);
            //build up the movie card view models
            Movies.Clear();
            await Task.Run(() =>
            {
                foreach (var movie in movies)
                {
                    if (!string.IsNullOrEmpty(movie.Poster_path))
                    {
                        //force updating the list from a different thread using custom cross thread extension method
                        Movies.AddOnUIThread(new MovieCardViewModel(movie.Movie_Id, movie.Title, new Uri(movie.Poster_path), movie.Overview,
                        movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, eventAgg));
                    }
                }
            });

        }
        #endregion

    }
}
