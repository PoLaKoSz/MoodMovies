using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MainViewModel : Conductor<Screen>
    {
        #region Field
        private MovieListViewModel movieListVM;
        private AboutViewModel aboutVM;
        private string mainViewMessage;
        IEventAggregator events;
        #endregion

        #region Properties
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

            MovieListVM = new MovieListViewModel(events);
            AboutVM = new AboutViewModel();
            ActivateItem(MovieListVM);
            MainViewMessage = "Initial";
        }

        public void ChangeMainMessage()
        {
            MainViewMessage = "New";
            //publish message to be received by other viewmodel subscribers
            events.PublishOnUIThread(new ChangeData(MainViewMessage));
        }

        public void DisplayMovieListVM()
        {
            ActivateItem(MovieListVM);
        }

        public void DisplayAboutVM()
        {
            System.Diagnostics.Debug.WriteLine("in aboutvm function");
            ActivateItem(AboutVM);
            
        }
        #endregion
    }
}
