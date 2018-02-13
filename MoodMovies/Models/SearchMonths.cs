using Caliburn.Micro;
using MoodMovies.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Models
{
    public  class SearchMonths : PropertyChangedBase
    {
        // constructor
        public SearchMonths()
        {

        }

        #region General properties
        private ObservableCollection<MovieSearchResultViewModel> _movieList;
        public ObservableCollection<MovieSearchResultViewModel> MovieList { get => _movieList; set { _movieList = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        /// <summary>
        /// Add a MovieSearchViewmodel to the list of searches for this month
        /// </summary>
        /// <param name="item"></param>
        public void AddSearchResult(MovieSearchResultViewModel item)
        {
            if( item != null )
            {
                MovieList.Add(item);
            }
        }
        #endregion
    }
}