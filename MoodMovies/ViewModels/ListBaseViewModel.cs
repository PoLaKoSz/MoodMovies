using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public class ListBaseViewModel : Screen
    {
        public ListBaseViewModel(IEventAggregator events, SnackbarMessageQueue statusMessage)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);

            Movies = new ObservableCollection<MovieCardViewModel>();

            StatusMessage = statusMessage;
        }

        public ListBaseViewModel(IEventAggregator events)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);
        }

        #region Events
        protected readonly IEventAggregator eventAgg;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies;
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }

        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }

        public SnackbarMessageQueue StatusMessage { get; set; }
        #endregion
    }
}
