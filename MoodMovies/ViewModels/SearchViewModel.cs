using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Services;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(CommonParameters commonParameters, ISearchService searchService)
            : base(commonParameters)
        {
            SearchService = searchService;
        }

        private readonly ISearchService SearchService;

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

        private string _actorText;
        public string ActorText { get => _actorText; set { _actorText = value; NotifyOfPropertyChange(); } }

        private ComboBoxItem _selectedBatch;
        public ComboBoxItem SelectedBatch { get => _selectedBatch; set { _selectedBatch = value; NotifyOfPropertyChange(); } }

        private string _selectedMood;
        public string SelectedMood { get => _selectedMood; set { _selectedMood = value; NotifyOfPropertyChange(); } }

        private List<Movie> MovieList = new List<Movie>();

        public User CurrentUser { get; set; }
        #endregion

        public async void BeginSearch()
        {
            //if no cient has been set
            if (OnlineDb.Client == null)
            {
                OnlineDb.ChangeClient(CurrentUser.ApiKey);
            }

            if (OnlineDb.Client == null)
            {
                StatusMessage.Enqueue("Please select a user account from the 'User' page.");
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(SearchText) 
                        || !string.IsNullOrEmpty(ActorText) 
                        || SelectedBatch != null 
                        || !string.IsNullOrEmpty(SelectedMood))
                    {
                        EventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));
                        MovieList = await SearchService.Search(CurrentUser.ApiKey, SearchText, ActorText, (SelectedBatch != null) ? SelectedBatch.Tag.ToString() : null, SelectedMood);
                        if (MovieList != null || MovieList.Count != 0)
                        {
                            EventAgg.PublishOnUIThread(new MovieListMessage(MovieList, true, SearchText));
                        }
                        else
                        {
                            // return no search results via a message window
                        }
                    }
                }
                catch(Exception ex)
                {
                    StatusMessage.Enqueue("Failed to connect with the current User's Api Key");
                }
                finally
                {
                    IsLoading = false;
                    EventAgg.PublishOnUIThread(new StopLoadingMessage());
                }
            }
        }
    }
}
