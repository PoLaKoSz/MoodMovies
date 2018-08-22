using Caliburn.Micro;
using MoodMovies.Messages;
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
        private EasyClient Client { get; set; }
        private IMovieApi MovieClient;
        private readonly IEventAggregator EventAgg;



        public OnlineServiceProvider(IEventAggregator eventAgg)
        {
            EventAgg = eventAgg;
            EventAgg.Subscribe(this);
        }



        /// <summary>
        /// Change the API Key in the requests
        /// </summary>
        private void ChangeClient(string apiKey)
        {
            Client = new EasyClient(apiKey);
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

        public async Task<List<Movie>> Search(string apiKey, string SearchText, string ActorText, string SelectedBatch, string SelectedMood)
        {
            List<Movie> finalResult = new List<Movie>();

            MovieList moviesByText = null;
            MovieList moviesByActor = null;
            MovieList moviesByBatch = null;
            MovieList moviesByMood = null;
            //we need to do traverse the pages as well as there might be more than 1
            //not implemented yet
            if (!string.IsNullOrEmpty(SearchText))
            {
                moviesByText = await SearchByTitleAsync(SearchText);
            }

            if (!string.IsNullOrEmpty(ActorText))
            {
                moviesByActor = await SearchByActorAsync(ActorText);
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

        public async Task<MovieList> SearchByTitleAsync(string title)
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public async Task<MovieList> SearchByActorAsync(string title)
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public async Task<MovieList> SearchTopRatedAsync(string language = "en")
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;
            return await MovieClient.GetTopRatedAsync(language);
        }

        public async Task<MovieList> GetNowPlayingAsync(string language = "en")
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetNowPlayingAsync(language).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        public async Task<MovieList> SearchUpcomingAsync(string language = "en")
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetUpcomingAsync(language).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        public async Task<MovieList> SearchPopularAsync(string language = "en")
        {
            MovieClient = Client.GetApi<IMovieApi>().Value;

            return await MovieClient.GetPopularAsync(language);
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
