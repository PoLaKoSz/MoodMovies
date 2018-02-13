using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public class HistoryViewModel : Screen, IHandle<MovieListMessage>
    {
        // constructor
        public HistoryViewModel(EventAggregator _event)
        {
            Name = "History";
            myEvent = _event;

            AddDummyData();
            Init();            
        }

        
        #region Events
        EventAggregator myEvent;
        #endregion

        #region General Properties
        private string _name;
        public string Name { get => _name; set { _name = value; NotifyOfPropertyChange(() => Name); } }
        private DateTime _thisMonth = DateTime.Now;
        public DateTime ThisMonth { get => _thisMonth; }

        private ObservableCollection<SearchMonth> _monthList;
        public ObservableCollection<SearchMonth> MonthList { get => _monthList; set { _monthList = value; NotifyOfPropertyChange(); } }
        private SearchMonth _selectedMonth;
        public SearchMonth SelectedMonth { get => _selectedMonth; set { _selectedMonth = value; NotifyOfPropertyChange(); }  }
       
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialise any lists and properties necessary
        /// </summary>
        private void Init()
        {
            if( MonthList.Count == 0 )
            {
                MonthList.Add(new SearchMonth(ThisMonth.ToString("MMMM")));
            }
            //ensure there is always one month selected
            SelectedMonth = MonthList[0];
        }
        /// <summary>
        /// to be replaced with database transaction that pulls any data from local db
        /// </summary>
        private void AddDummyData()
        {
            MonthList = new ObservableCollection<SearchMonth>();
            MonthList.Add(new SearchMonth("January"));
            MonthList.Add(new SearchMonth("May"));
            MonthList.Add(new SearchMonth("March"));
            MonthList.Add(new SearchMonth("April"));
        }
        #endregion

        #region Ihandle Interface
        /// <summary>
        /// Checks to see if the current month exists in the list of months. If not it will create a month item.
        /// It will then add the searchresult to that items movielist property(which is a list of MovieSearchResultViewmodels)
        /// </summary>
        /// <param name="message"></param>
        public void Handle(MovieListMessage message)
        {
            if ( MonthList.Any(months => months.MonthName == ThisMonth.ToString("MMMM")) == false )
            {
                //add new month
                MonthList.Add(new SearchMonth(ThisMonth.ToString("MMMM")));
                //sort the month alphabetically
                MonthList = new ObservableCollection<SearchMonth>(MonthList.OrderBy(month => month.MonthName));
                //reset the current selection t the first item
                SelectedMonth = MonthList[0];
            }

            MonthList.First(month => month.MonthName == ThisMonth.ToString("MMMM"))
                .AddSearchResult(new MovieSearchResultViewModel(message.Movielist, message.IsAdult.ToString(), message.SearchString, message.DateOfSearch));                   
        }
        #endregion
    }
}
