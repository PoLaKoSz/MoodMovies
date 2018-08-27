using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.ViewModels
{
    internal class SearchViewModel : BaseViewModel, IHandle<BrowseSearchResultsMessage>
    {
        public SearchViewModel(CommonParameters commonParameters)
            : base(commonParameters)
        {
            SearchQuery = new SearchQuery();
            MovieList = new List<Movie>();
        }

        #region Properties
        public SearchQuery SearchQuery { get; set; }

        private List<Movie> MovieList;
        #endregion

        public async Task BeginSearch()
        {
            try
            {
                if (!string.IsNullOrEmpty(SearchQuery.SearchText)
                    || !string.IsNullOrEmpty(SearchQuery.ActorName)
                    || SearchQuery.Batch != null
                    || !string.IsNullOrEmpty(SearchQuery.Mood))
                {
                    EventAgg.PublishOnUIThread(new StartLoadingMessage("Searching for movies..."));

                    MovieList = await OnlineDB.Search(SearchQuery);

                    if (MovieList != null || MovieList.Count != 0)
                    {
                        EventAgg.PublishOnUIThread(new MovieListMessage(MovieList, true, SearchQuery.SearchText));
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

        public async void Handle(BrowseSearchResultsMessage message)
        {
            SearchQuery.PageNumber = message.PageNumber;

            await BeginSearch();
        }
    }
}
