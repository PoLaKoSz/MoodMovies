using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Caliburn.Micro;
using MoodMovies.Views;

namespace MoodMovies.ViewModels
{
    public class MovieCardViewModel: Screen
    {
        public MovieCardViewModel(string title, Uri imagepath, string overview, string releasedate, string votecount, double popularity, string language)
        {
            Title = title;
            ImagePath = imagepath;
            Overview = overview;
            ReleaseDate = releasedate;
            VoteCount = votecount;
            Popularity = popularity;
            Language = language;           
        }

        private string title;
        public string Title { get => title; set { title = value; NotifyOfPropertyChange(); } }
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
       
    }    
}
