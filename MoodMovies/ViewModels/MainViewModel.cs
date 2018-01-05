using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MoodMovies.ViewModels
{
    public class MainViewModel : PropertyChangedBase
    {
        #region Field
        private MovieImageViewModel movieImageBar;
        private string mainViewMessage;
        #endregion

        #region Properties
        public MovieImageViewModel MovieImageBar
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
            MovieImageBar = new MovieImageViewModel();
            MainViewMessage = "Initial message";
        }

        public void ChangeMainMessage()
        {
            MainViewMessage = "This is a new message you just created";
        }
        #endregion
    }
}
