using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public class MainViewModel : Conductor<Screen>.Collection.OneActive
    {
        // constructor
        public MainViewModel()
        {
            events = new EventAggregator();
            InitialiseVMs();
            ActivateItem(MovieListVM);
        }

        #region General Properties
        private string _simpleSearchBox;
        public string SimpleSearchBox { get => _simpleSearchBox; set { _simpleSearchBox = value; NotifyOfPropertyChange(); } }
        #endregion

        #region Child View Models
        private MovieListViewModel movieListVM;
        public MovieListViewModel MovieListVM { get => movieListVM; set { movieListVM = value; NotifyOfPropertyChange(); } }
        private AboutViewModel aboutVM;
        public AboutViewModel AboutVM { get => aboutVM; set { aboutVM = value; NotifyOfPropertyChange(); } }
        private FavouritesViewModel _favouriteVM;
        public FavouritesViewModel FavouriteVM { get => _favouriteVM; set { _favouriteVM = value; NotifyOfPropertyChange(); } }
        private FavActorViewModel _favActorVM;
        public FavActorViewModel FavActorVM { get => _favActorVM; set { _favActorVM = value; NotifyOfPropertyChange(); } }
        private ASearchViewModel _asearchVM;
        public ASearchViewModel ASearchVM { get => _asearchVM; set { _asearchVM = value; NotifyOfPropertyChange(); } }
        #endregion
        #region Events
        IEventAggregator events;
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
            Items.Add( MovieListVM = new MovieListViewModel(events) );
            Items.Add( FavActorVM = new FavActorViewModel() );
            Items.Add( AboutVM = new AboutViewModel() );
            Items.Add( FavouriteVM = new FavouritesViewModel() );

            //static, will not change
            ASearchVM = new ASearchViewModel();
        }       
        #endregion

        #region Item Activation Methods
        public void DisplayMovieListVM()
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(MovieListVM);
        }

        public void DisplayAboutVM()            
        {
            DeactivateItem(ActiveItem, true);
            ActivateItem(AboutVM);            
        }

        public void DisplayFavouriteVM()
        {
        DeactivateItem(ActiveItem, true);
        ActivateItem(FavouriteVM);
        }
        
        public void DisplayFavActorVM()
        {
        DeactivateItem(ActiveItem, true);
        ActivateItem(FavActorVM);
        }
        #endregion
    }
}
