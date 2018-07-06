using System;
using Caliburn.Micro;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel: Screen
    {
        public MovieCardViewModel(int id, string title, Uri imagepath, string overview, string releasedate, int votecount, 
            double voteAverage, bool video, bool adult, double popularity, string language, string posterCache, IEventAggregator _event)
        {
            Movie_Id = id;
            Title = title;
            ImagePath = imagepath;
            Poster_path = imagepath.ToString();
            Overview = overview;
            ReleaseDate = releasedate;
            Vote_count = votecount;
            Vote_average = voteAverage;
            Video = video;
            Adult = adult;
            Popularity = popularity;
            Language = language;
            myEvent = _event;
            Poster_Cache = posterCache;
        }

        #region Events
        public IEventAggregator myEvent;
        #endregion

        #region Binding Properties
        private bool _isFavourited;
        public bool IsFavourited { get => _isFavourited ; set { _isFavourited = value; NotifyOfPropertyChange(); } }
        private bool _isWatchListed;
        public bool IsWatchListed { get => _isWatchListed; set { _isWatchListed = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Movie Properties
        public int Vote_count { get; set; }
        private int _id;
        public int Movie_Id { get => _id; set { _id = value; NotifyOfPropertyChange(); } }
        public bool Video { get; set; }
        public bool Adult { get; set; }
        public double Vote_average { get; set; }
        private string _title;
        public string Title { get => _title; set { _title = value; NotifyOfPropertyChange(); } }
        private string posterpath;
        public string Poster_path { get => posterpath; set { posterpath = value; NotifyOfPropertyChange(); } }
        private Uri imagepath;
        public Uri ImagePath { get => imagepath; set { imagepath = value; NotifyOfPropertyChange(); } }
        private string _overview;
        public string Overview { get => _overview; set { _overview = value; NotifyOfPropertyChange(); } }
        private string _releaseDate;
        public string ReleaseDate { get => _releaseDate; set { _releaseDate = value; NotifyOfPropertyChange(); } }       
        private double _popularity;
        public double Popularity { get => _popularity; set { _popularity = value; NotifyOfPropertyChange(); } }
        private string _language;
        public string Language { get => _language; set { _language = value; NotifyOfPropertyChange(); } }
        private string _trailerUrl;
        public string TrailerUrl { get => _trailerUrl; set { _trailerUrl = value; NotifyOfPropertyChange(); } }
        private string _poster_Cache;
        public string Poster_Cache { get => _poster_Cache; set { _poster_Cache = value; NotifyOfPropertyChange(); } }
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


        #endregion

        #region Ihandle Interface
        public void RequestTrailer()
        {
            myEvent.PublishOnUIThread(Movie_Id);
        }
        #endregion

    }
}
