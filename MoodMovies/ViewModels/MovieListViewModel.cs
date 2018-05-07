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
    internal class MovieListViewModel : Screen, IHandle<MovieListMessage>, IHandle<MovieCardViewModel>
    {
        public MovieListViewModel(IEventAggregator events)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);
        }
        #region Events
        private readonly IEventAggregator eventAgg;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies = new ObservableCollection<MovieCardViewModel>();
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }
        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods       

        #endregion

        #region IHandle methods
        public async void Handle(MovieListMessage message)
        {
            var on = new OnlineServiceProvider(eventAgg);   
            //move this to search viewmodel
            //add support for all the api objects(movie full details etc)
            // remove all the rest of the clucky http stuff in searchviewmodel
            var dt = await on.CallTmdbAsync();

            Movies.Clear();
            foreach (var movie in message.Movielist)
            {
                Movies.Add(new MovieCardViewModel(movie.Id.ToString(), movie.Title, new Uri(movie.Poster_path), movie.Overview,
                    movie.Release_date, movie.Vote_count.ToString(), movie.Popularity, movie.Original_language, eventAgg));
            }

            eventAgg.PublishOnUIThread(new ResultsReadyMessage());
        }

        public void Handle(MovieCardViewModel message)
        {
            SelectedItem = message;
        }
        #endregion

    }
}
