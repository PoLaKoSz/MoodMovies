﻿using Caliburn.Micro;
using DataModel.DataModel;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Services;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive,
        IHandle<ResultsReadyMessage>,
        IHandle<StartLoadingMessage>,
        IHandle<StopLoadingMessage>,
        IHandle<LoggedInMessage>
    {
        public MainViewModel()
        {
            _eventAggregator = new EventAggregator();
            _eventAggregator.Subscribe(this);

            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            string appRootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies");
            var appFolders = new AppFolders(appRootFolder);

            IDb database = new Db(appRootFolder);
            database.AutoMigrate();
            database.Connect();

            var offlineDb = new OfflineServiceProvider(database);
            var onlineDB = new OnlineServiceProvider();

            ImageCacher imageCacher = new ImageCacher(
                appFolders.ImageCacheFolder, new WebClient(), "https://image.tmdb.org/t/p/w500");

            StatusMessage = new SnackbarMessageQueue();

            CommonParameters commonParameters = new CommonParameters(
                _eventAggregator, offlineDb, onlineDB, StatusMessage);

            StartVM = new StartPageViewModel(commonParameters);
            UserVM = new UserControlViewModel(commonParameters);
            SearchVM = new SearchViewModel(commonParameters, new SearchService(onlineDB));
            MovieListVM = new MovieListViewModel(commonParameters, imageCacher);
            FavouriteVM = new FavouritesViewModel(commonParameters, imageCacher);
            WatchListVM = new WatchListViewModel(commonParameters, imageCacher);
        }

        #region Fields
        private string _loadingMessage;
        private bool _isLoading;
        private bool _canNavigate;
        private readonly IEventAggregator _eventAggregator;
        #endregion

        #region General Properties
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        public bool CanNavigate { get => _canNavigate; set { _canNavigate = value; NotifyOfPropertyChange(); } }

        public SnackbarMessageQueue StatusMessage { get; }
        #endregion

        #region Child View Models
        private StartPageViewModel _startVM;
        public StartPageViewModel StartVM { get => _startVM; set { _startVM = value; NotifyOfPropertyChange(); } }

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

        private async Task DisplayStartPage()
        {
            IsLoading = true;
            LoadingMessage = "Loading..";

            DisplayStartVM();
            await _startVM.DisplayInitialPage();

            IsLoading = false;
        }

        #region Item Activation Methods
        public void DisplayStartVM()
        {
            CanNavigate = false;
            DeactivateItem(ActiveItem, true);
            ActivateItem(StartVM);
        }

        public async void DisplayUserVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(UserVM);
            await UserVM.GetUsers();
        }

        public void DisplaySearchVM()
        {
            CanNavigate = true;
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

        public void Handle(StopLoadingMessage message)
        {
            IsLoading = false;
        }

        public void Handle(LoggedInMessage message)
        {
            DisplaySearchVM();
        }

        public void LogOut()
        {
            DisplayStartVM();
        }
        #endregion

        #region Caliburn Override
        protected async override void OnViewLoaded(object view)
        {
            Items.Add(StartVM);
            Items.Add(UserVM);
            Items.Add(SearchVM);
            Items.Add(MovieListVM);
            Items.Add(FavouriteVM);
            Items.Add(WatchListVM);

            await DisplayStartPage();
            base.OnViewLoaded(view);
        }
        #endregion
    }
}