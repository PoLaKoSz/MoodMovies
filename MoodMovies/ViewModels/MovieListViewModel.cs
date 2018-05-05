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
        public void Handle(MovieListMessage message)
        {

        }

        public void Handle(MovieCardViewModel message)
        {
            SelectedItem = message;
        }
        #endregion

    }
}
