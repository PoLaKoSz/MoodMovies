using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using DataModel.DataModel;
using WPFLocalizeExtension.Engine;
using System.Globalization;
using MoodMovies.Logic;
using MaterialDesignThemes.Wpf;

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<ResultsReadyMessage>, IHandle<StartLoadingMessage>
    {
        // constructor
        public MainViewModel()
        {
            eventAgg.Subscribe(this);
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");
            //initial setup of the database
            IDb database = new Db();
            database.DumpDatabase();

            offlineDb = new OfflineServiceProvider(database);
            UserVM = new UserControlViewModel(offlineDb);
            StatusMessage = new SnackbarMessageQueue();
            StatusMessage.Enqueue("Setup succcess");
        }

        #region Events
        public IEventAggregator eventAgg = new EventAggregator();
        #endregion

        #region Fields
        readonly IOfflineServiceProvider offlineDb;
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        public SnackbarMessageQueue StatusMessage { get; set; }
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
            //pages that will change

            //need to write implementation to get user to input this one time and save to db or
            //create a guest session
            Items.Add(SearchVM = new SearchViewModel(eventAgg, offlineDb, new OnlineServiceProvider()));
            Items.Add(MovieListVM = new MovieListViewModel(eventAgg, offlineDb));
            Items.Add(FavouriteVM = new FavouritesViewModel(eventAgg, offlineDb));
            Items.Add(WatchListVM = new WatchListViewModel(eventAgg, offlineDb));

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

        #region IHandle Interface

        #endregion

        #region Caliburn Override
        protected override void OnViewLoaded(object view)
        {
            if (UserControl.CurrentUser == null)
            {
                //prompt user to set the user accoutn etc
            }

            InitialiseVMs();
            base.OnViewLoaded(view);
        }
        #endregion
    }
}