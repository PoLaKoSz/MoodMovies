using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public abstract class ListBaseViewModel : Screen, IHandle<LoggedInMessage>
    {
        public ListBaseViewModel(IListViewModelParams commonParameters, User currentUser)
        {
            eventAgg = commonParameters.EventAggregator;
            eventAgg.Subscribe(this);

            OfflineDB = commonParameters.OfflineService;

            Movies = new ObservableCollection<MovieCardViewModel>();

            StatusMessage = commonParameters.StatusMessage;

            CurrentUser = currentUser;
        }

        protected readonly IEventAggregator eventAgg;
        protected readonly IOfflineServiceProvider OfflineDB;

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
        public void Handle(LoggedInMessage message)
        {
            CurrentUser = message.CurrentUser;
        }
    }
}
