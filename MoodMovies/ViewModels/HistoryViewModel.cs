using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public class HistoryViewModel : Screen, IHandle<MovieListMessage>
    {
        // constructor
        public HistoryViewModel(EventAggregator _event)
        {
            Name = "History";
            myEvent = _event;            
        }
        #region Events
        EventAggregator myEvent;
        #endregion

        #region General Properties
        private string _name;
        public string Name { get => _name; set { _name = value; NotifyOfPropertyChange(() => Name); } }      
        private ObservableCollection<SearchMonths>;

        private ObservableCollection<MovieSearchResultViewModel> _SelectedMovieSearches = new ObservableCollection<MovieSearchResultViewModel>();
        public ObservableCollection<MovieSearchResultViewModel> SelectedMovieSearches { get => _SelectedMovieSearches; set { _SelectedMovieSearches = value; NotifyOfPropertyChange(); } }

        #endregion

        #region Methods
       
        #endregion

        #region Ihandle Interface
        public void Handle(MovieListMessage message)
        {
            SelectedMovieSearches.Add(new MovieSearchResultViewModel(message.Movielist, message.IsAdult.ToString(), message.SearchString));
        }
        #endregion
    }
}
