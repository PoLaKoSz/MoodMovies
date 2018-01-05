using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MoodMovies.ViewModels
{
    public class MovieImageViewModel : PropertyChangedBase
    {
        #region Fields
        private string test;

        #endregion

        #region Properties
        public string Test
        {
            get => test;
            set
            {
                test = value;
                NotifyOfPropertyChange(() => Test);
            }
        }
        #endregion

        #region methods
        public MovieImageViewModel()
        {
            Test = "This is a test";
        }

        #endregion
    }
}
