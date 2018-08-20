using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel : Screen
    {
        
        public MovieCardViewModel(Movies movie, IEventAggregator _event)
        {
            Movie = movie;
            myEvent = _event;
        }
        
        public IEventAggregator myEvent;

        #region Binding Properties
        private bool _isFavourited;
        public bool IsFavourited { get => _isFavourited; set { _isFavourited = value; NotifyOfPropertyChange(); } }

        private bool _isWatchListed;
        public bool IsWatchListed { get => _isWatchListed; set { _isWatchListed = value; NotifyOfPropertyChange(); } }

        private Movies _movie;
        public Movies Movie { get => _movie; set { _movie = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public void SetSelectedItem()
        {
            myEvent.PublishOnUIThread(this);
        }

        /// <summary>
        /// Adds or removes a movie from thewatchlist. Fires an event with a message
        /// </summary>
        /// <param name="sender"></param>
        public void AddToWatchList(object sender)
        {
            var isChecked = (bool)sender;
            if (isChecked == true)
            {
                myEvent.PublishOnUIThread(new AddToWatchListMessage(this));
            }
            else
            {
                myEvent.PublishOnUIThread(new RemoveFromWatchListMessage(this));
                if (Parent is WatchListViewModel p)
                {
                    p.Movies.Remove(this);
                }
            }
        }

        /// <summary>
        /// Adds or removes a movie from the favourites. Fires an event with a message
        /// </summary>
        /// <param name="sender"></param>
        public void AddToFavourites(object sender)
        {
            var isChecked = (bool)sender;
            if (isChecked == true)
            {
                myEvent.PublishOnUIThread(new AddToFavouritesMessage(this));
            }
            else
            {
                myEvent.PublishOnUIThread(new RemoveFromFavouritesMessage(this));
                if (Parent is FavouritesViewModel p)
                {
                    p.Movies.Remove(this);
                }
            }
        }
        
        public void RequestTrailer()
        {
            myEvent.PublishOnUIThread(Movie.Movie_Id);
        }
        #endregion
    }
}
