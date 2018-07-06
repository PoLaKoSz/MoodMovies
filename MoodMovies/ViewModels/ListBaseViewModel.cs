using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;

namespace MoodMovies.ViewModels
{
    public class ListBaseViewModel : Screen
    {
        public ListBaseViewModel(IEventAggregator events, SnackbarMessageQueue statusMessage)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);
            StatusMessage = statusMessage;
        }

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
        public SnackbarMessageQueue StatusMessage { get; set; }
        #endregion

        #region Private Methods
        /// <summary>
        /// Downloads an image from the specified Uri and return the path to that image if it exists.
        /// </summary>
        /// <param name="poster_path">Poster web URL</param>
        /// <param name="fileName">File relative path with extension</param>
        /// <returns></returns>
        protected string DownloadImage(Uri poster_path, string fileName)
        {
            string cacheDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies\\ImageCache\\");
            string file = Path.Combine(cacheDirectory, fileName.Replace("/", ""));

            if (!File.Exists(file))
            {
                if (!Directory.Exists(cacheDirectory))
                    Directory.CreateDirectory(cacheDirectory);

                using (WebClient webClient = new WebClient())
                {
                    webClient.DownloadFile(poster_path, file);
                }
            }
            return file;
        }
        #endregion

        protected const string posterAddress = "https://image.tmdb.org/t/p/w500";
    }
}
