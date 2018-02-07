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
    public class MovieListViewModel : Screen, IHandle<MovieListMessage>
    {
        // constructor
        public MovieListViewModel(IEventAggregator events)
        {
            _events = events;           
            _events.Subscribe(this);
        }
        #region Events
        private IEventAggregator _events;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies;
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods

        #endregion
        #region IHandle methods
        public void Handle(MovieListMessage message)
        {
            Movies = new ObservableCollection<MovieCardViewModel>();
            foreach ( var movie in message.Movielist)
            {
                Movies.Add(new MovieCardViewModel(movie.Title, new Uri(movie.Poster_path), movie.Overview, 
                    movie.Release_date, movie.Vote_count.ToString(), movie.Popularity, movie.Original_language));
            }
        }
        #endregion



    }
}
