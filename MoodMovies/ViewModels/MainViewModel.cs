using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        #region Field
        private MovieListViewModel movieImageBar;
        private string mainViewMessage;
        IEventAggregator events;
        #endregion

        #region Properties
        public MovieListViewModel MovieImageBar
        {
            get => movieImageBar;
            set
            {
                movieImageBar = value;
                NotifyOfPropertyChange(() => MovieImageBar);
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

            MovieImageBar = new MovieListViewModel(events);
            MainViewMessage = "Initial";
        }

        public void ChangeMainMessage()
        {
            MainViewMessage = "New";
            //publish message to be received by other viewmodel subscribers
            events.PublishOnUIThread(new ChangeData(MainViewMessage));
        }
        #endregion
    }
}
