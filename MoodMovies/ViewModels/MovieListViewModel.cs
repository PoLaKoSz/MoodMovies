using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : Screen, IHandle<MovieListMessage>, IHandle<MovieCardViewModel>
    {
        public MovieListViewModel(IEventAggregator events)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);
        }
        #region Events
        protected readonly IEventAggregator eventAgg;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies = new ObservableCollection<MovieCardViewModel>();
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }
        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }
        #endregion

        protected const string posterAddress = "https://image.tmdb.org/t/p/w500/";

        #region Public Methods       

        #endregion

        #region IHandle methods
        public async void Handle(MovieListMessage message)
        {
            Movies.Clear();
            await Task.Run(() =>
            {                
                foreach (var movie in message.Movielist.Results)
                {
                    if (!string.IsNullOrEmpty(movie.Poster_path))
                    {
                        //force updating the list from a different thread using custom cross thread extension method
                        Movies.AddOnUIThread(new MovieCardViewModel(movie.Id, movie.Title, new Uri(posterAddress + movie.Poster_path), movie.Overview,
                        movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, eventAgg));
                    }
                }
            });

            eventAgg.PublishOnUIThread(new ResultsReadyMessage());
        }

        public void Handle(MovieCardViewModel message)
        {
            SelectedItem = message;
        }
        #endregion

    }
}
