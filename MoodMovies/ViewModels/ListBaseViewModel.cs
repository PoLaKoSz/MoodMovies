using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Messages;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public abstract class ListBaseViewModel : Screen, IHandle<ClientChangeMessage>
    {
        public ListBaseViewModel(IEventAggregator events, SnackbarMessageQueue statusMessage, User currentUser)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);

            Movies = new ObservableCollection<MovieCardViewModel>();

            StatusMessage = statusMessage;

            CurrentUser = currentUser;
        }

        protected readonly IEventAggregator eventAgg;

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies;
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }

        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }

        public SnackbarMessageQueue StatusMessage { get; set; }

        public User CurrentUser { get; set; }
        #endregion

        /// <summary>
        /// Handle when a new User selected
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ClientChangeMessage message)
        {
            CurrentUser = message.NewUser;
        }
    }
}
