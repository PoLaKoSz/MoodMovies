using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;

namespace MoodMovies.ViewModels
{
    public class MovieListViewModel : Screen, IHandle<ChangeData>
    {
        #region Fields
        private string test;
        private IEventAggregator _events;
        private ObservableCollection<MovieCardViewModel> movies;

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

        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(() => Movies); } }
        #endregion

        #region Methods
        public MovieListViewModel(IEventAggregator events)
        {
            _events = events;
            //subscribe this object to the eventaggregator
            _events.Subscribe(this);

            Movies = new ObservableCollection<MovieCardViewModel>();
            //for testing purposes populate the list with set items
            for(int i = 0; i < 12; i++)
            {
                Movies.Add(new MovieCardViewModel());
            }
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
