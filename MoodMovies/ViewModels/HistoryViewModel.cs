using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public class HistoryViewModel : Screen
    {
        // constructor
        public HistoryViewModel()
        {
            Name = "History";
            TestMethod();
        }

        #region General Properties
        private string _name;
        public string Name { get => _name; set { _name = value; NotifyOfPropertyChange(() => Name); } }       
        private ObservableCollection<SearchItem> _searchItems;
        public ObservableCollection<SearchItem> SearchItems { get => _searchItems; set { _searchItems = value; NotifyOfPropertyChange(); } }

        #endregion

        #region Methods
        public void TestMethod()
        {
            SearchItems = new ObservableCollection<SearchItem>();

            for( int i = 0; i< 10; i++)
            {
                SearchItems.Add(new SearchItem($"0{i}/02/2018", "The rock movies"));
            }
        }
        #endregion
    }
}
