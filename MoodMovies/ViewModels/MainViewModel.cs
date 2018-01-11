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
        private string mainViewMessage;
        IEventAggregator events;
        #endregion

        #region Properties
        #region Child ViewModels
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
                NotifyOfPropertyChange(() => _favouriteVM);
            }
        }
        #endregion
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
            //initialize Child ViewModels
            MovieListVM = new MovieListViewModel(events);
            AboutVM = new AboutViewModel();
            FavouriteVM = new FavouritesViewModel();
            //on startup we want our main Usercontrol displayed
            ActivateItem(MovieListVM);
            MainViewMessage = "Initial";
        }

        public void ChangeMainMessage()
        {
            MainViewMessage = "New";
            //publish message to be received by other viewmodel subscribers
            events.PublishOnUIThread(new ChangeData(MainViewMessage));
        }
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
        #endregion
        #endregion
    }
}
