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

namespace MoodMovies.ViewModels
{
    internal class MainViewModel : Conductor<Screen>.Collection.OneActive, IHandle<ResultsReadyMessage> 
    {
        // constructor
        public MainViewModel()
        {
            eventAgg.Subscribe(this);
            InitialiseVMs();
        }

        #region Events
        public IEventAggregator eventAgg = new EventAggregator();
        #endregion

        #region General Properties

        #endregion

        #region Child View Models
        private SearchViewModel _searchVM;
        public SearchViewModel SearchVM { get => _searchVM; set { _searchVM = value; NotifyOfPropertyChange(); } }
        private MovieListViewModel movieListVM;
        public MovieListViewModel MovieListVM { get => movieListVM; set { movieListVM = value; NotifyOfPropertyChange(); } }
        private FavouritesViewModel _favouriteVM;
        public FavouritesViewModel FavouriteVM { get => _favouriteVM; set { _favouriteVM = value; NotifyOfPropertyChange(); } }
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
            Items.Add(SearchVM = new SearchViewModel(eventAgg));
            Items.Add(MovieListVM = new MovieListViewModel(eventAgg));
            Items.Add(FavouriteVM = new FavouritesViewModel(eventAgg));

            eventAgg.Subscribe(SearchVM);
            eventAgg.Subscribe(MovieListVM);
            eventAgg.Subscribe(FavouriteVM);

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

        public void DisplayFavouriteVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(FavouriteVM);
        }

        public void Handle(ResultsReadyMessage message)
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(MovieListVM);
        }
        #endregion

        #region IHandle Interface

        #endregion

    }
}