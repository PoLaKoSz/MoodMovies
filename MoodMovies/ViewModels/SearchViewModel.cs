using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Services;
using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : Screen, IHandle<ClientChangeMessage>
    {
        public SearchViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage, ISearchService searchService)
        {
            eventAgg = _event;
            eventAgg.Subscribe(this);

            offlineDb = offlineService;
            onlineDb = onlineService;
            StatusMessage = statusMessage;
            SearchService = searchService;
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Providers & Services
        readonly IOfflineServiceProvider offlineDb;
        private IOnlineServiceProvider onlineDb;
        private readonly ISearchService SearchService;
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

        private string _actorText;
        public string ActorText { get => _actorText; set { _actorText = value; NotifyOfPropertyChange(); } }

        private string _selectedBatch;
        public string SelectedBatch { get => _selectedBatch; set { _selectedBatch = value; NotifyOfPropertyChange(); } }

        private string _selectedMood;
        public string SelectedMood { get => _selectedMood; set { _selectedMood = value; NotifyOfPropertyChange(); } }

        private MovieList MovieList = new MovieList();

        public Users CurrentUser { get; set; }
        #endregion

        #region Public methods
        public async void BeginSearch()
        {
            //if no cient has been set
            if (onlineDb.Client == null)
            {
                StatusMessage.Enqueue("Please select a user account form the 'User' page.");
            }
            else
            {
                try
                {
                    if (!string.IsNullOrEmpty(SearchText))
                    {
                        eventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));
                        MovieList = await SearchService.Search(CurrentUser.User_ApiKey);
                    }
                }
                catch
                {
                    StatusMessage.Enqueue("Failed to connect with the current User's Api Key");
                }
                finally
                {
                    IsLoading = false;
                }
            }
        }

        public async Task GetMoviesByTitle(string text)
        {
            

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

            
        }

        public void Handle(ClientChangeMessage message)
        {
            onlineDb.ChangeClient(message.NewUser.User_ApiKey);
        }
        #endregion
    }
}
