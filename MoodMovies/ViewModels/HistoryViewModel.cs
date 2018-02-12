using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MoodMovies.ViewModels
{
    public class HistoryViewModel : Screen
    {
        // constructor
        public HistoryViewModel()
        {
            Name = "History";
        }

        #region General Properties
        private string _name;
        public string Name { get => _name; set { _name = value; NotifyOfPropertyChange(() => Name); } }
        private ObservableCollection<SearchItem> _searchs;

        #endregion

        #region Methods
        public void TestMethod()
        {

        }
        #endregion
    }
}
