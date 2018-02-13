using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : Screen, IHandle<MovieListMessage>, IHandle<MovieCardViewModel>, IHandle<TrailerMessage>
    {
        // constructor
        public MovieListViewModel(EventAggregator events)
        {
            _events = events;           
            _events.Subscribe(this);               
        }
        #region Events
        private EventAggregator _events;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies;
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }
        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }
        private string _selectedTrailer;
        public string SelectedTrailer { get => _selectedTrailer; set { _selectedTrailer = value; NotifyOfPropertyChange(); } }
        private bool _isTrailerPlaying = true;
        public bool IsTrailerPlaying { get => _isTrailerPlaying; set { _isTrailerPlaying = value; NotifyOfPropertyChange(); } } 
        #endregion

        #region Public Methods       
        public void RemoveSelectedTrailer()
        {
            //hacky fix. needs to be sorted out at a later stage
            //without assigning this dummy value, cef does not stop playing the video when the dialoghost is closed
            SelectedTrailer = "http://gitub.com/tonykaralis/MoodMovies/README.md";
            IsTrailerPlaying = false;
            IsTrailerPlaying = true;
        }
        #endregion

        #region IHandle methods
        public void Handle(MovieListMessage message)
        {
            Movies = new ObservableCollection<MovieCardViewModel>();
            foreach ( var movie in message.Movielist)
            {
                Movies.Add(new MovieCardViewModel(movie.Id.ToString(), movie.Title, new Uri(movie.Poster_path), movie.Overview, 
                    movie.Release_date, movie.Vote_count.ToString(), movie.Popularity, movie.Original_language, _events));
            }
        }

        public void Handle(MovieCardViewModel message)
        {
            SelectedItem = message;
        }

        public void Handle(TrailerMessage message)
        {
            SelectedTrailer = message.TrailerUrl;
            NotifyOfPropertyChange(() => SelectedTrailer);        
        }
        #endregion
        
    }
}
