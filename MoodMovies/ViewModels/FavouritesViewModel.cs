using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MoodMovies.ViewModels
{
    public class FavouritesViewModel: Screen
    {
        public FavouritesViewModel(IEventAggregator _event)
        {
            DisplayName = "Favourites";
            eventAgg = _event;
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Methods

        #endregion

    }
}
