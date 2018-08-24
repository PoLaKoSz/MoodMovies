﻿using Caliburn.Micro;
using DataModel.DataModel;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;
using MoodMovies.Messages;
using MoodMovies.Models;
using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using WPFLocalizeExtension.Engine;

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : BaseViewModel,
        IHandle<ResultsReadyMessage>,
        IHandle<StartLoadingMessage>,
        IHandle<StopLoadingMessage>,
        IHandle<LoggedInMessage>
    {
        public MainViewModel()
            : base(InitializeCommonParameter(GetAppFolders()))
        {
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            _database.AutoMigrate();
            _database.Connect();

            ImageCacher imageCacher = new ImageCacher(
                GetAppFolders().ImageCacheFolder, new WebClient(), "https://image.tmdb.org/t/p/w500");

            _loginViewModel = new LoginViewModel(_commonParameters);
            _startViewModel = new StartPageViewModel(_commonParameters, _loginViewModel);
            _userViewModel = new UserControlViewModel(_commonParameters, _loginViewModel);
            _searchViewModel = new SearchViewModel(_commonParameters);
            _movieListViewModel = new SearchResultsViewModel(_commonParameters, imageCacher);
            _favouriteViewMdel = new FavouritesViewModel(_commonParameters, imageCacher);
            _watchListViewModel = new WatchListViewModel(_commonParameters, imageCacher);
        }

        private static CommonParameters InitializeCommonParameter(AppFolders appFolders)
        {
            _database = new Db(appFolders.AppRootFolder);

            var eventAgg = new EventAggregator();

            return _commonParameters = new CommonParameters(
                eventAgg,
                new OfflineServiceProvider(_database),
                new OnlineServiceProvider(eventAgg),
                new SnackbarMessageQueue());
        }

        private static AppFolders GetAppFolders()
        {
            string appRootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "MoodMovies");

            return new AppFolders(appRootFolder);
        }

        #region Fields
        private string _loadingMessage;
        private bool _isLoading;
        private bool _canNavigate;

        private static IDb _database;

        private static CommonParameters _commonParameters;
        
        private readonly LoginViewModel _loginViewModel;
        private readonly StartPageViewModel _startViewModel;
        private readonly SearchViewModel _searchViewModel;
        private readonly SearchResultsViewModel _movieListViewModel;
        private readonly FavouritesViewModel _favouriteViewMdel;
        private readonly WatchListViewModel _watchListViewModel;
        private readonly UserControlViewModel _userViewModel;
        #endregion

        #region General Properties
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        public bool IsLoading { get => _isLoading; set { _isLoading = value; NotifyOfPropertyChange(); } }
        public bool CanNavigate { get => _canNavigate; set { _canNavigate = value; NotifyOfPropertyChange(); } }
        #endregion


        public void CloseApp()
        {
            TryClose();
        }


        private async Task DisplayStartPage()
        {
            IsLoading = true;
            LoadingMessage = "Loading..";

            NavigateToStartPage();
            await _startViewModel.DisplayInitialPage();

            IsLoading = false;
        }

        #region Item Activation Methods
        public void NavigateToStartPage()
        {
            DisplayMenu(_startViewModel);
            CanNavigate = false;
        }

        public async void NavigateToUsersMenu()
        {
            DisplayMenu(_userViewModel);
            await _userViewModel.GetUsers();
        }

        public void NavigateToSearchMenu()
        {
            DisplayMenu(_searchViewModel);
        }

        public void NavigateToSearchResults()
        {
            DisplayMenu(_movieListViewModel);
        }

        public async Task NavigateToFavouritesMenu()
        {
            DisplayMenu(_favouriteViewMdel);
            await _favouriteViewMdel.LoadFavouriteItems();
        }

        public async Task NavigateToWatchlistMenu()
        {
            DisplayMenu(_watchListViewModel);
            await _watchListViewModel.LoadWatchListItems();
        }

        private void DisplayMenu(Screen screen)
        {
            CanNavigate = true;
            DeactivateItem(ActiveItem, true);
            ActivateItem(screen);
        }

        public void Handle(ResultsReadyMessage message)
        {
            NavigateToSearchResults();
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

        public override void Handle(LoggedInMessage message)
        {
            base.Handle(message);

            NavigateToSearchMenu();
        }

        public void LogOut()
        {
            NavigateToStartPage();
        }
        #endregion

        #region Caliburn Override
        protected async override void OnViewLoaded(object view)
        {
            Items.Add(_startViewModel);
            Items.Add(_userViewModel);
            Items.Add(_searchViewModel);
            Items.Add(_movieListViewModel);
            Items.Add(_favouriteViewMdel);
            Items.Add(_watchListViewModel);

            await DisplayStartPage();
            base.OnViewLoaded(view);
        }
        #endregion
    }
}