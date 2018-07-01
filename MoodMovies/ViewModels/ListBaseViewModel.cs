using Caliburn.Micro;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public class ListBaseViewModel : Screen
    {
        public ListBaseViewModel(IEventAggregator events)
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
    }
}
