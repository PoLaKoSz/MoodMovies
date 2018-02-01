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
    public class ASearchViewModel : Screen
    {
        // constructor
        public ASearchViewModel()
        {
            TestMethod();
        }

        #region Actor Properties
        private ObservableCollection<Actor> _actorsCollection;
        public ObservableCollection<Actor> ActorsCollection { get => _actorsCollection; set { _actorsCollection = value; NotifyOfPropertyChange(); } }
        #endregion

        private void TestMethod()
        {
            _actorsCollection = new ObservableCollection<Actor>();
            ActorsCollection.Add(new Actor("Dwayne Johnson",false));
            ActorsCollection.Add(new Actor("Kate Moss", false));
            ActorsCollection.Add(new Actor("Sylvester Stalone", false));
            ActorsCollection.Add(new Actor("Arnold Schwatzenegger", false));
            ActorsCollection.Add(new Actor("John Kuzak", false));
            ActorsCollection.Add(new Actor("Denzel Washington", false));
        }
    }

    
}
