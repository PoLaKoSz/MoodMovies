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
    public  class SearchMonth : PropertyChangedBase
    {
        // constructor
        public SearchMonth(string monthname)
        {
            SearchList = new ObservableCollection<MovieSearchResultViewModel>();
            MonthName = monthname;
        }

        #region General properties
        private string _monthName;
        public string MonthName { get => _monthName; private set => _monthName = value; }
        private ObservableCollection<MovieSearchResultViewModel> _searchList;
        public ObservableCollection<MovieSearchResultViewModel> SearchList { get => _searchList; private set { _searchList = value; NotifyOfPropertyChange(); } }
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
                SearchList.Add(item);
            }
        }

        public override string ToString()
        {
            return MonthName;
        }
        #endregion
    }
}