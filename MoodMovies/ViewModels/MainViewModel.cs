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
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<ResultsReadyMessage>, IHandle<StartLoadingMessage>
    {
        public MainViewModel()
        {
            eventAgg.Subscribe(this);
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            string appRootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies");
            AppFolders = new AppFolders(appRootFolder);
            
            IDb database = new Db();
            database.DumpDatabase();

            offlineDb = new OfflineServiceProvider(database);
            UserVM = new UserControlViewModel(eventAgg, offlineDb, StatusMessage);
            UserVM.GetUsers();
        }

        #region Events
        public IEventAggregator eventAgg = new EventAggregator();
        #endregion

        #region Fields
        readonly IOfflineServiceProvider offlineDb;
        private readonly AppFolders AppFolders;
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
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
            ImageCacher imageCacher = new ImageCacher(AppFolders.ImageCacheFolder, new WebClient(), "https://image.tmdb.org/t/p/w500");

            Items.Add(SearchVM = new SearchViewModel(eventAgg, offlineDb, new OnlineServiceProvider(), StatusMessage));
            Items.Add(MovieListVM = new MovieListViewModel(eventAgg, offlineDb, StatusMessage, imageCacher));
            Items.Add(FavouriteVM = new FavouritesViewModel(eventAgg, offlineDb, StatusMessage, imageCacher));
            Items.Add(WatchListVM = new WatchListViewModel(eventAgg, offlineDb, StatusMessage, imageCacher));

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