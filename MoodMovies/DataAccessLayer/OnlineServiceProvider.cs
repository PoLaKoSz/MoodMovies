using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using MoodMovies.Resources;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.ApiInterfaces;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.DataAccessLayer
{
    internal class OnlineServiceProvider : IOnlineServiceProvider, IHandle<LoggedInMessage>
    {
        public SearchQuery SearchQuery { get; private set; }


        private EasyClient Client { get; set; }
        private IMovieApi MovieClient;
        private readonly IEventAggregator EventAgg;



        public OnlineServiceProvider(IEventAggregator eventAgg)
        {
            EventAgg = eventAgg;
            EventAgg.Subscribe(this);
        }



        public bool IsValidApiKey(string apiKey)
        {
            try
            {
                ChangeClient(apiKey);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Make a new search
        /// </summary>
        /// <returns>Collection of <see cref="Movie"/> from TMDb</returns>
        public async Task<List<Movie>> Search(SearchQuery query)
        {
            SearchQuery = query;

            return await Search(SearchQuery.SearchText,
                SearchQuery.ActorName,
                SearchQuery.Batch.Tag.ToString(),
                SearchQuery.Mood);
        }


        /// <summary>
        /// Change the API Key in the requests
        /// </summary>
        private void ChangeClient(string apiKey)
        {
            Client = new EasyClient(apiKey);
            MovieClient = Client.GetApi<IMovieApi>().Value;
        }

        private async Task<List<Movie>> Search(string SearchText, string ActorText, string SelectedBatch, string SelectedMood)
        {
            List<Movie> finalResult = new List<Movie>();

            MovieList moviesByText = null;
            MovieList moviesByActor = null;
            MovieList moviesByBatch = null;
            MovieList moviesByMood = null;

            if (!string.IsNullOrEmpty(SearchText))
            {
                moviesByText = await SearchByTitleAsync();
            }

            if (!string.IsNullOrEmpty(ActorText))
            {
                moviesByActor = await SearchByActorAsync();
            }

            if (!string.IsNullOrEmpty(SelectedBatch) && SelectedBatch.ToLower() != "everything")
            {
                switch (SelectedBatch)
                {
                    case "TopRated":
                        moviesByBatch = await SearchTopRatedAsync();
                        break;
                    case "Popular":
                        moviesByBatch = await SearchPopularAsync();
                        break;
                    case "Upcoming":
                        moviesByBatch = await SearchUpcomingAsync();
                        break;
                    case "NowPlaying":
                        moviesByBatch = await GetNowPlayingAsync();
                        break;
                }
            }

            finalResult = await Filter(moviesByText, moviesByActor, moviesByBatch, moviesByMood);

            return finalResult;
        }

        private async Task<List<Movie>> Filter(MovieList text, MovieList actors, MovieList batch, MovieList mood)
        {
            await Task.Delay(10);
            List<Movie> movies = new List<Movie>();

            //add them all to the same list
            if (text != null && text.Results != null && text.Results.Count > 0) movies.AddRange(text.Results);
            if (actors != null && actors.Results != null && actors.Results.Count > 0) movies.AddRange(actors.Results);
            if (batch != null && batch.Results != null && batch.Results.Count > 0) movies.AddRange(batch.Results);
            if (mood != null && mood.Results != null && mood.Results.Count > 0) movies.AddRange(mood.Results);

            //get the intersect
            return movies.GroupBy(x => x.Id)
                .Select(x => x.First())?
                .ToList();
        }

        private async Task<MovieList> SearchByTitleAsync()
        {
            return await MovieClient.SearchMoviesAsync(SearchQuery.SearchText, page:SearchQuery.PageNumber);
        }

        private async Task<MovieList> SearchByActorAsync()
        {
            throw new System.NotImplementedException("TMdbEasy IMovieApi not implemented SearchByActorAsync() method yet!");
        }

        private async Task<MovieList> SearchTopRatedAsync(string language = "en")
        {
            return await MovieClient.GetTopRatedAsync(language, page: SearchQuery.PageNumber);
        }

        private async Task<MovieList> GetNowPlayingAsync(string language = "en")
        {
            var datedMovieList = MovieClient.GetNowPlayingAsync(language, page: SearchQuery.PageNumber).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        private async Task<MovieList> SearchUpcomingAsync(string language = "en")
        {
            var datedMovieList = MovieClient.GetUpcomingAsync(language, page: SearchQuery.PageNumber).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        private async Task<MovieList> SearchPopularAsync(string language = "en")
        {
            return await MovieClient.GetPopularAsync(language, page: SearchQuery.PageNumber);
        }

        private MovieList MapDatedMovieList(DatedMovieList dMovieList)
        {
            var movieList = new MovieList();
            PropertyCopier<DatedMovieList, MovieList>.Copy(dMovieList, movieList);
            return movieList;
        }


        public void Handle(LoggedInMessage message)
        {
            ChangeClient(message.CurrentUser.ApiKey);
        }
    }
}
