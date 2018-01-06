using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : PropertyChangedBase, IHandle<ChangeData>
    {
        #region Fields
        private string test;
        private IEventAggregator _events;

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

        #region Methods
        public MovieListViewModel(IEventAggregator events)
        {
            _events = events;
            //subscribe this object to the eventaggregator
            _events.Subscribe(this);
            Test = "This is a test";
        }

        #region IHandle methods
        public void Handle(ChangeData message)
        {
            Test = message.Data;
        }
        #endregion


        #endregion
    }
}
