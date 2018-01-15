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
    public class MainViewModel : Conductor<Screen>
    {
        #region Field
        private MovieListViewModel movieListVM;
        private AboutViewModel aboutVM;
        private FavouritesViewModel _favouriteVM;
        private FavActorViewModel _favActorVM;
        private string mainViewMessage;
        IEventAggregator events;        
        #endregion
                
        #region Child ViewModel Properties
        public MovieListViewModel MovieListVM
        {
            get => movieListVM;
            set
            {
                movieListVM = value;
                NotifyOfPropertyChange(() => MovieListVM);
            }
        }
        public AboutViewModel AboutVM
        {
            get => aboutVM;
            set
            {
                aboutVM = value;
                NotifyOfPropertyChange(() => AboutVM);
            }
        }
        public FavouritesViewModel FavouriteVM
        {
            get => _favouriteVM;
            set
            {
                _favouriteVM = value;
                NotifyOfPropertyChange(() => FavouriteVM);
            }
        }
        public FavActorViewModel FavActorVM
        {
            get => _favActorVM;
            set
            {
                _favActorVM = value;
                NotifyOfPropertyChange(() => FavActorVM);
            }
        }

        #endregion

        #region Properties
        public string MainViewMessage
        {
            get
            {
                return mainViewMessage;
            }
            set
            {
                mainViewMessage = value;
                NotifyOfPropertyChange(() => MainViewMessage);
            }
        }

        

        #endregion

        #region Methods
        public MainViewModel()
        {
            events = new EventAggregator();
            InitialiseVMs();
            
            //on startup we want our main Usercontrol displayed
            ActivateItem(MovieListVM);
            MainViewMessage = "Initial";
        }

        private void InitialiseVMs()
        {
            MovieListVM = new MovieListViewModel(events);
            FavActorVM = new FavActorViewModel();
            AboutVM = new AboutViewModel();
            FavouriteVM = new FavouritesViewModel();            
        }

        public void ChangeMainMessage()
        {
            MainViewMessage = "New";
            //publish message to be received by other viewmodel subscribers
            events.PublishOnUIThread(new ChangeData(MainViewMessage));
        }
        #endregion

        #region UserControl Activation Methods
        public void DisplayMovieListVM()
        {
            ActivateItem(MovieListVM);
        }

        public void DisplayAboutVM()
        {
            ActivateItem(AboutVM);            
        }

        public void DisplayFavouriteVM()
        {
            ActivateItem(FavouriteVM);
        }
        
        public void DisplayFavActorVM()
        {
            ActivateItem(FavActorVM);
        }
        #endregion
    }
}
