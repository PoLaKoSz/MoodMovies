using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Collections.Generic;
using System.Windows.Controls;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : BaseViewModel
    {
        public SearchViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
        }

        #region Properties
        private string _simpleSearchBox;
        public string SimpleSearchBox { get => _simpleSearchBox; set { _simpleSearchBox = value; NotifyOfPropertyChange(); } }

        private string _searchText;
        public string SearchText { get => _searchText; set { _searchText = value; NotifyOfPropertyChange(); } }

        private string _actorText;
        public string ActorText { get => _actorText; set { _actorText = value; NotifyOfPropertyChange(); } }

        private ComboBoxItem _selectedBatch;
        public ComboBoxItem SelectedBatch { get => _selectedBatch; set { _selectedBatch = value; NotifyOfPropertyChange(); } }

        private string _selectedMood;
        public string SelectedMood { get => _selectedMood; set { _selectedMood = value; NotifyOfPropertyChange(); } }

        private List<Movie> MovieList = new List<Movie>();
        #endregion

        public async void BeginSearch()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchText) 
                    || !string.IsNullOrEmpty(ActorText) 
                    || SelectedBatch != null 
                    || !string.IsNullOrEmpty(SelectedMood))
                {
                    EventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));

                    MovieList = await OnlineDb.Search(CurrentUser.ApiKey, SearchText, ActorText, (SelectedBatch != null) ? SelectedBatch.Tag.ToString() : null, SelectedMood);

                    if (MovieList != null || MovieList.Count != 0)
                    {
                        EventAgg.PublishOnUIThread(new MovieListMessage(MovieList, true, SearchText));
                    }
                }
            }
            catch
            {
                StatusMessage.Enqueue("Unknow error occured while searchig for movies!");
            }
            finally
            {
                EventAgg.PublishOnUIThread(new StopLoadingMessage());
            }
        }
    }
}
