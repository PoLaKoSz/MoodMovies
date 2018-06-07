using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

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

        #endregion

        #region Methods

        #endregion

    }
}
