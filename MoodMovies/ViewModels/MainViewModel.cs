using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using System.Collections.ObjectModel;
using System.Windows.Input;
using MoodMovies.Resources;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using MoodMovies.Models;
using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System.Collections;
using WPFLocalizeExtension.Engine;
using System.Globalization;

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<ResultsReadyMessage>, IHandle<StartLoadingMessage>
    {
        // constructor
        public MainViewModel()
        {
            eventAgg.Subscribe(this);
            InitialiseVMs();
            
            LocalizeDictionary.Instance.Culture = new CultureInfo("en");

            //initial setup of the database
            Db.DumpDatabase();            
        }

        #region Events
        public IEventAggregator eventAgg = new EventAggregator();
        #endregion

        #region General Properties
        private string _loadingMessage;
        public string LoadingMessage { get => _loadingMessage; set { _loadingMessage = value; NotifyOfPropertyChange(); } }
        private bool _isLoading;
        public bool IsLoading { get => _isLoading; set { _isLoading= value; NotifyOfPropertyChange(); } }
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
        #endregion

        #region Public Methods
        public void CloseApp()
        {
            TryClose();
        }

        public void RunShit()
        {
            IsLoading = true;
        }
        #endregion

        #region Private Methods
        private void InitialiseVMs()
        {
            //pages that will change
            Items.Add(SearchVM = new SearchViewModel(eventAgg));
            Items.Add(MovieListVM = new MovieListViewModel(eventAgg));
            Items.Add(FavouriteVM = new FavouritesViewModel(eventAgg));
            Items.Add(WatchListVM = new WatchListViewModel(eventAgg));

            eventAgg.Subscribe(SearchVM);
            eventAgg.Subscribe(MovieListVM);
            eventAgg.Subscribe(FavouriteVM);
            eventAgg.Subscribe(WatchListVM);

            ActivateItem(SearchVM);
        }
        #endregion

        #region Item Activation Methods
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

    }
}