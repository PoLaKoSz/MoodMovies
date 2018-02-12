using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Models
{
    public class SearchItem
    {
        // constructor
        public SearchItem() { }

        public SearchItem(string date, string name)
        {
            Name = name;
            Date = date;
        }

        #region General Properties
        private string _name;
        public string Name { get => _name; set => _name = value; }
        private string _date;        
        public string Date { get => _date; set => _date = value; }
        #endregion
    }
}
