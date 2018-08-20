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
        public ListBaseViewModel(IListViewModelParams commonParameters)
        {
            EventAgg = commonParameters.EventAggregator;
            EventAgg.Subscribe(this);

            OfflineDB = commonParameters.OfflineService;

            Movies = new ObservableCollection<MovieCardViewModel>();

            StatusMessage = commonParameters.StatusMessage;
        }


        protected readonly IEventAggregator EventAgg;
        protected readonly IOfflineServiceProvider OfflineDB;


        #region Properties
        private ObservableCollection<MovieCardViewModel> movies;
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }

        private MovieCardViewModel _selectedItem;
        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }

        protected SnackbarMessageQueue StatusMessage { get; }

        protected User CurrentUser { get; private set; }
        #endregion


        public void Handle(LoggedInMessage message)
        {
            CurrentUser = message.CurrentUser;
        }
    }
}
