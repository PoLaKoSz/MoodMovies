using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Controls.Primitives;
using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Views;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel: Screen
    {
        public MovieCardViewModel(string id, string title, Uri imagepath, string overview, string releasedate, string votecount, double popularity, string language, IEventAggregator _event)
        {
            ID = id;
            Title = title;
            ImagePath = imagepath;
            Overview = overview;
            ReleaseDate = releasedate;
            VoteCount = votecount;
            Popularity = popularity;
            Language = language;
            myEvent = _event;
        }

        #region Events
        public IEventAggregator myEvent;
        #endregion

        #region Movie Properties
        public int Vote_count { get; set; }
        private string _id;
        public string ID { get => _id; set { _id = value; NotifyOfPropertyChange(); } }
        public bool Video { get; set; }
        public double Vote_average { get; set; }
        private string _title;
        public string Title { get => _title; set { _title = value; NotifyOfPropertyChange(); } }
        private Uri imagepath;
        public Uri ImagePath { get => imagepath; set { imagepath = value; NotifyOfPropertyChange(); } }
        private string _overview;
        public string Overview { get => _overview; set { _overview = value; NotifyOfPropertyChange(); } }
        private string _releaseDate;
        public string ReleaseDate { get => _releaseDate; set { _releaseDate = value; NotifyOfPropertyChange(); } }
        private string _voteCount;
        public string VoteCount { get => _voteCount; set { _voteCount = value; NotifyOfPropertyChange(); } }
        private double _popularity;
        public double Popularity { get => _popularity; set { _popularity = value; NotifyOfPropertyChange(); } }
        private string _language;
        public string Language { get => _language; set { _language = value; NotifyOfPropertyChange(); } }
        private string _trailerUrl;
        public string TrailerUrl { get => _trailerUrl; set { _trailerUrl = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public void SetSelectedItem()
        {
            myEvent.PublishOnUIThread(this);
        }

        public void AddToWatchList(object sender)
        {
            var button = sender as ToggleButton;         
            if (button.IsChecked == true)
            {
                myEvent.PublishOnUIThread(new RemoveFromWatchListMessage(this));
            }
            else
            {
                myEvent.PublishOnUIThread(new AddToWatchListMessage(this));
            }
        }

        public void AddToFavourites(object sender)
        {
            var button = sender as ToggleButton;
            if (button.IsChecked == true)
            {
                myEvent.PublishOnUIThread(new RemoveFromFavouritesMessage(this));
            }
            else
            {
                myEvent.PublishOnUIThread(new AddToFavouritesMessage(this));
            }
        }
        #endregion

        #region Ihandle Interface
        public void RequestTrailer()
        {
            myEvent.PublishOnUIThread(ID);
        }
        #endregion

    }
}
