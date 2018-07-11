using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Messages;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMdbEasy.TmdbObjects.Movies;
using MaterialDesignThemes.Wpf;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : Screen, IHandle<ClientChangeMessage>
    {
        public SearchViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage)
        {
            eventAgg = _event;
            eventAgg.Subscribe(this);

            offlineDb = offlineService;
            onlineDb = onlineService;
            StatusMessage = statusMessage;
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Providers
        readonly IOfflineServiceProvider offlineDb;
        readonly IOnlineServiceProvider onlineDb;
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        public SnackbarMessageQueue StatusMessage { get; set; }
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
            //if no cient has been set
            if(onlineDb.Client == null && Logic.UserControl.CurrentUser == null)
            {
                StatusMessage.Enqueue("Please select a user account form the 'User' page.");
            }            
            else 
            {
                try
                {
                    if (onlineDb.Client == null)
                        onlineDb.ChangeClient(Logic.UserControl.CurrentUser.User_ApiKey);

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
                catch
                {
                    StatusMessage.Enqueue("Failed to connect with the current User's Api Key");
                }
            }         
        }

        public async Task GetMoviesByTitle(string text)
        {
            eventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));

            //add support for all the api objects(movie full details etc)            
            MovieList = await onlineDb.SearchByTitleAsync(text);

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

        public void Handle(ClientChangeMessage message)
        {
            try
            {
                if (Logic.UserControl.CurrentUser != null)
                {
                    onlineDb.ChangeClient(Logic.UserControl.CurrentUser.User_ApiKey);                    
                }
                else
                {
                    onlineDb.Client = null;
                }
            }
            catch
            {
                StatusMessage.Enqueue("Failed to load the Current user's Api Key");
            }
        }
    }
}
