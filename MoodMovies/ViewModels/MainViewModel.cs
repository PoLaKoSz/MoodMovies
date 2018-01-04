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
        private MovieImageViewModel movieImageBar;

        public MovieImageViewModel MovieImageBar
        {
            get => movieImageBar;
            set
            {
                movieImageBar = value;
                NotifyOfPropertyChange(() => MovieImageBar);
            }
        }

        public string Message
        {
            get
            {
                return message;
            }
            set
            {
                message = value;
                NotifyOfPropertyChange(() => Message);
            }
        }

        private string message;



        public MainViewModel()
        {
            MovieImageBar = new MovieImageViewModel();
            Message = "test message";
        }
    }
}
