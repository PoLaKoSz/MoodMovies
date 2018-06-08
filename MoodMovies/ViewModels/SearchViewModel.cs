using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : Screen, IHandle<IMovieCardMessage>
    {
        public SearchViewModel(IEventAggregator _event)
        {
            eventAgg = _event;
            eventAgg.Subscribe(this);
            //remember to unsubscribe?? or not

            //need to write implementation to get user to input this one time and save to db or
            //create a guest session
            onlineDb.SetupKey("6d4b546936310f017557b2fb498b370b");
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region providers
        OfflineServiceProvider offDb = new OfflineServiceProvider();
        OnlineServiceProvider onlineDb = new OnlineServiceProvider();
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
                
        private string _selectedBatch;
        public string SelectedBatch { get => _selectedBatch; set { _selectedBatch = value; NotifyOfPropertyChange(); } }

        private string _selectedMood;
        public string SelectedMood { get => _selectedMood; set { _selectedMood = value; NotifyOfPropertyChange(); } }

        private object _selectedSource;
        public object SelectedSource { get => _selectedSource; set { _selectedSource = value; NotifyOfPropertyChange(); } }

        private MovieList MovieList = new MovieList();
        #endregion

        #region Public methods
        public async void BeginSearch()
        {
            CheckInputs();
            if (SelectedSource is ComboBoxItem obj)
            {
                var value = Convert.ToString(obj.Content);
                //this needs to change(add a viewmodel and associate an enum possibly) translation wont work here
                //or never translate this
                if (value == "Online" || String.IsNullOrEmpty(value))
                {
                    if (string.IsNullOrEmpty(SearchText))
                    {
                        //publish message
                    }
                    else
                    {
                        await GetMoviesByTitle(SearchText);
                    }
                }
                else if (value == "Favourites" || value == "Watchlist")
                {         
                    //search the watchlist/favourites
                }
            }
        }

        public async Task GetMoviesByTitle(string text)
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));

            //add support for all the api objects(movie full details etc)            
            MovieList = await OnlineServiceProvider.SearchByTitleAsync(text);

            if (MovieList.Results != null || MovieList.Results.Count != 0)
            {
                eventAgg.PublishOnUIThread(new MovieListMessage(MovieList, true, SearchText));
            }
            else
            {
                // return no search results via a message window
            }

            IsLoading = false;
        }
        #endregion

        #region Private Methods     
        /// <summary>
        /// Checks what inputs are provided in order to perform the relevant searches
        /// </summary>
        private void CheckInputs()
        {

        }
        #endregion

        #region
        public async void Handle(IMovieCardMessage message)
        {
            switch (message)
            {
                case AddToWatchListMessage addWatch:
                    await AddMovieToWatchList(message.MovieCard);
                    break;
                case RemoveFromWatchListMessage removeWatch:
                    await RemoveMovieFromWatchList(message.MovieCard);
                    break;
                case AddToFavouritesMessage addFavourites:
                    await AddMovieToFavourites(message.MovieCard);
                    break;
                case RemoveFromFavouritesMessage removeFavourites:
                    await RemoveMovieFromFavourites(message.MovieCard);
                    break;
            }
        }

        public async Task AddMovieToWatchList(MovieCardViewModel mvCard)
        {
            var movie = new Movies();
            //copy the properties that can be copied
            PropertyCopier<MovieCardViewModel, Movies>.Copy(mvCard, movie);

            //get the user from a static class that will contain all the various users
            // for now this is ok ****************
            //first add the movie to the database
            try
            {
                await offDb.AddMovie(movie);
                //then create the link between the user and the movie and the watchlist
                var user = await offDb.GetFirstUser();
                await offDb.AddToWatchList(user, movie);
            }
            catch
            {
                //ping an message to the user if necessary
            }
          
            
        }

        public async Task RemoveMovieFromWatchList(MovieCardViewModel mvCard)
        {
            //await
        }

        public async Task AddMovieToFavourites(MovieCardViewModel mvCard)
        {
            var movie = new Movies();
            //copy the properties that can be copied
            PropertyCopier<MovieCardViewModel, Movies>.Copy(mvCard, movie);

            //get the user from a static class that will contain all the various users
            // for now this is ok ****************
            //first add the movie to the database
            try
            {
                await offDb.AddMovie(movie);
                //then create the link between the user and the movie and the watchlist
                var user = await offDb.GetFirstUser();
                await offDb.AddToFavourites(user, movie);
            }
            catch
            {
                //ping a message to the user if necessary
            }
        }

        public async Task RemoveMovieFromFavourites(MovieCardViewModel mvCard)
        {
            //await
        }
        #endregion
    }
}
