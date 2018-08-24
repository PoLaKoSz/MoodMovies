using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel : Screen
    {
        public MovieCardViewModel(Movies movie, IEventAggregator eventAgg)
        {
            Movie = movie;
            _eventAgg = eventAgg;
        }
        
        private IEventAggregator _eventAgg;

        #region Binding Properties
        private bool _isFavourited;
        public bool IsFavourited { get => _isFavourited; set { _isFavourited = value; NotifyOfPropertyChange(); NotifyFavourites(); } }

        private bool _isWatchListed;
        public bool IsWatchListed { get => _isWatchListed; set { _isWatchListed = value; NotifyOfPropertyChange(); NotifyWatchList(); } }

        private Movies _movie;
        public Movies Movie { get => _movie; set { _movie = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public void SetSelectedItem()
        {
            _eventAgg.PublishOnUIThread(this);
        }

        /// <summary>
        /// Adds or removes this movie to / from the WatchList
        /// </summary>
        private void NotifyWatchList()
        {
            if (IsWatchListed)
            {
                _eventAgg.PublishOnUIThread(new AddToWatchListMessage(this));
            }
            else
            {
                _eventAgg.PublishOnUIThread(new RemoveFromWatchListMessage(this));
            }
        }

        /// <summary>
        /// Adds or removes this movie to / from the Favourites
        /// </summary>
        private void NotifyFavourites()
        {
            if (IsFavourited)
            {
                _eventAgg.PublishOnUIThread(new AddToFavouritesMessage(this));
            }
            else
            {
                _eventAgg.PublishOnUIThread(new RemoveFromFavouritesMessage(this));
            }
        }
        
        public void RequestTrailer()
        {
            _eventAgg.PublishOnUIThread(Movie.Movie_Id);
        }
        #endregion
    }
}
