using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : Screen
    {
        public SearchViewModel(IEventAggregator _event)
        {
            eventAgg = _event;
            OnlineServiceProvider.SetupKey("6d4b546936310f017557b2fb498b370b");
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Properties
        private string _simpleSearchBox;
        public string SimpleSearchBox { get => _simpleSearchBox; set { _simpleSearchBox = value; NotifyOfPropertyChange(); } }
        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; NotifyOfPropertyChange(); } }
        private MovieList MovieList = new MovieList();
        #endregion

        #region Public methods
        public void BeginSearch()
        {
            //thisis where we need to check what options have been selected and what is the best search option for the user

            if (string.IsNullOrEmpty(SearchText))
            {
                //publish message
            }
            else
            {
                GetMovies(SearchText);
            }
        }

        public async void GetMovies(string text)
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));

            //add support for all the api objects(movie full details etc)
            // remove all the rest of the clucky http stuff in searchviewmodel
            MovieList = await OnlineServiceProvider.SearchByTitleAsync(text);

            if (MovieList.Results != null || MovieList.Results.Count != 0)
            {
                eventAgg.BeginPublishOnUIThread(new MovieListMessage(MovieList, true, SearchText));
            }
            else
            {
                // return no search results via a message window
            }

            IsLoading = false;
        }
        #endregion

        #region Private Methods     

        #endregion
    }
}
