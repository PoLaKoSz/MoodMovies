using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
            ImageCacher = commonParameters.ImageCacher;
        }


        protected readonly IEventAggregator EventAgg;
        protected readonly IOfflineServiceProvider OfflineDB;
        protected readonly ImageCacher ImageCacher;


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


        protected MovieCardViewModel GetMovieCard(MovieCardViewModel movieCard)
        {
            return Movies.FirstOrDefault(m => m.Movie.Movie_Id == movieCard.Movie.Movie_Id);
        }

        protected async Task PushToUI(List<Movies> movies)
        {
            await Task.Run(() =>
            {
                foreach (Movies movie in movies)
                {
                    if (string.IsNullOrEmpty(movie.Poster_path))
                        continue;

                    ImageCacher.ScanPoster(movie);

                    var card = new MovieCardViewModel(movie, EventAgg)
                    {
                        IsFavourited = true,
                    };

                    Movies.AddOnUIThread(card);
                }
            });
        }
    }
}
