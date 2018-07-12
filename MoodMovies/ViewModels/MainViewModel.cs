using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using DataModel.DataModel;
using WPFLocalizeExtension.Engine;
using System.Globalization;
using MoodMovies.Logic;
using MaterialDesignThemes.Wpf;
using MoodMovies.Models;
using System;
using System.IO;
using MoodMovies.DataAccessLayer;

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive,
        IHandle<ResultsReadyMessage>,
        IHandle<StartLoadingMessage>,
        IHandle<NavigateToUsersMenu>,
        IHandle<ClientChangeMessage>
    {
        public MainViewModel()
        {
            CanNavigate = false;

            eventAgg.Subscribe(this);
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            string appRootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies");
            _appFolders = new AppFolders(appRootFolder);

            IDb database = new Db();
            database.DumpDatabase();

            _offlineDb = new OfflineServiceProvider(database);
            _onlineDB = new OnlineServiceProvider();
            UserVM = new UserControlViewModel(eventAgg, _offlineDb, StatusMessage);
            UserVM.GetUsers();
        }

        #region Events
        public IEventAggregator eventAgg = new EventAggregator();
        #endregion

        #region Fields
        readonly IOfflineServiceProvider _offlineDb;
        private readonly AppFolders _appFolders;
        private IOnlineServiceProvider _onlineDB;
        private string _loadingMessage;
        private bool _isLoading;
        private bool _canNavigate;
        #endregion

        #region General Properties
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }

        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }

        public bool CanNavigate { get => _canNavigate; set { _canNavigate = value; NotifyOfPropertyChange(); } }

        public SnackbarMessageQueue StatusMessage { get; set; } = new SnackbarMessageQueue();
        #endregion

        #region Child View Models
        private SearchViewModel _searchVM;
        public SearchViewModel SearchVM { get => _searchVM; set { _searchVM = value; NotifyOfPropertyChange(); } }
        private MovieListViewModel movieListVM;
        public MovieListViewModel MovieListVM { get => movieListVM; set { movieListVM = value; NotifyOfPropertyChange(); } }
        private FavouritesViewModel _favouriteVM;
        public FavouritesViewModel FavouriteVM { get => _favouriteVM; set { _favouriteVM = value; NotifyOfPropertyChange(); } }
        private WatchListViewModel _watchListVM;
        public WatchListViewModel WatchListVM { get => _watchListVM; set { _watchListVM = value; NotifyOfPropertyChange(); } }
        private UserControlViewModel _userVM;

        public UserControlViewModel UserVM { get => _userVM; set { _userVM = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Public Methods
        public void CloseApp()
        {
            TryClose();
        }
        #endregion

        #region Private Methods
        private void InitialiseVMs()
        {
            ImageCacher imageCacher = new ImageCacher(_appFolders.ImageCacheFolder, new WebClient(), "https://image.tmdb.org/t/p/w500");

            Items.Add(SearchVM = new SearchViewModel(eventAgg, _offlineDb, _onlineDB, StatusMessage));
            Items.Add(MovieListVM = new MovieListViewModel(eventAgg, _offlineDb, StatusMessage, imageCacher, UserVM.CurrentUser));
            Items.Add(FavouriteVM = new FavouritesViewModel(eventAgg, _offlineDb, StatusMessage, imageCacher, UserVM.CurrentUser));
            Items.Add(WatchListVM = new WatchListViewModel(eventAgg, _offlineDb, StatusMessage, imageCacher, UserVM.CurrentUser));

            eventAgg.Subscribe(SearchVM);
            eventAgg.Subscribe(MovieListVM);
            eventAgg.Subscribe(FavouriteVM);
            eventAgg.Subscribe(WatchListVM);

            ActivateItem(SearchVM);
        }
        #endregion

        #region Item Activation Methods
        public void DisplayUserVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(UserVM);
        }

        public void DisplaySearchVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(SearchVM);
        }

        public void DisplayMovieListVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(MovieListVM);
        }

        public async Task DisplayFavouriteVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(FavouriteVM);
            await FavouriteVM.LoadFavouriteItems();
        }

        public async Task DisplayWatchListVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(WatchListVM);
            await WatchListVM.LoadWatchListItems();
        }

        public void Handle(ResultsReadyMessage message)
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(MovieListVM);
            IsLoading = false;
        }

        public void Handle(StartLoadingMessage message)
        {
            IsLoading = true;
            LoadingMessage = message.Text;
        }

        public void Handle(NavigateToUsersMenu message)
        {
            DisplayUserVM();
            IsLoading = false;
        }

        public void Handle(ClientChangeMessage message)
        {
            CanNavigate = true;
            IsLoading = false;
        }
        #endregion

        #region Caliburn Override
        protected override void OnViewLoaded(object view)
        {
            InitialiseVMs();
            base.OnViewLoaded(view);
        }
        #endregion
    }
}