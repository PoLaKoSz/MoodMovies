using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Logic;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : MovieListViewModel
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
        }
        #endregion

    }
}
