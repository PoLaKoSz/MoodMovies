using Caliburn.Micro;
using MoodMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.ViewModels
{
    /// <summary>
    /// This is used to store a search that has been performed. The list of search results ias passed as a bool stating whether this search contains adult content or not.
    /// Finally there is a name parameter which the user can set should they choose to save 
    /// </summary>
    public class MovieSearchResultViewModel: Screen
    {
        // constructor
        public MovieSearchResultViewModel(List<MovieSearchResult> movies, string isAdult, string text)
        {
            Movies = movies;
            SearchText = text;
            IsAdult = isAdult;
            Date = DateTime.Now.ToString();
        }

        #region General Properties
        private List<MovieSearchResult> _movies;
        public List<MovieSearchResult> Movies { get => _movies; set { _movies = value; NotifyOfPropertyChange(); } }
        private string _searchText;
        public string SearchText { get => _searchText; set => _searchText = value; }
        private string _date;
        public string Date { get => _date; set => _date = value; }
        private string _isAdult;
        public string IsAdult { get => _isAdult; set => _isAdult = value; }        
        #endregion

    }
}
