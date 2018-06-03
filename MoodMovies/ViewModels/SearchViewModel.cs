﻿using Caliburn.Micro;
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
    internal class SearchViewModel : Screen, IHandle<>
    {
        public SearchViewModel(IEventAggregator _event)
        {
            eventAgg = _event;

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
                    OfflineServiceProvider offDb = new OfflineServiceProvider();

                    //await offDb.CreateUser(new Users() { User_Name = "test name1", User_Surname = "surname 2", User_ApiKey = "liuhliuh" });
                    //await offDb.CreateMovie(new Movies() { Movie_Id = 12345, Vote_count = 12, Video =true, Vote_average = 12, Title = "TestMovie", Popularity = 560, Adult = true });
                    var user = await offDb.GetFirstUser();
                    var movie = await offDb.GetFirstMovie();
                    await offDb.AddToWatchList(user, movie);
                    
                    //await
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
    }
}
