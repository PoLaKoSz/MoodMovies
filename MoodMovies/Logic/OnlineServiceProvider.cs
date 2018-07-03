using MoodMovies.Resources;
using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.ApiInterfaces;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Logic
{
    internal class OnlineServiceProvider : IOnlineServiceProvider
    {
        public OnlineServiceProvider(string key)
        {
            OnlineClient = new EasyClient(key);
        }

        #region Properties
        private EasyClient OnlineClient;
        private IMovieApi MovieClient;
        #endregion

        public async Task<MovieList> SearchByTitleAsync(string title)
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public async Task<MovieList> SearchByActorAsync(string title)
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public async Task<MovieList> SearchTopRatedAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.GetTopRatedAsync(language);
        }

        public async Task<MovieList> GetNowPlayingAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetNowPlayingAsync(language).Result;

            return await Task.Run(()=> MapDatedMovieList(datedMovieList));
        }

        public async Task<MovieList> SearchUpcomingAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetUpcomingAsync(language).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        public async Task<MovieList> SearchPopularAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            return await MovieClient.GetPopularAsync(language);
        }

        private MovieList MapDatedMovieList(DatedMovieList dMovieList)
        {
            var movieList = new MovieList();
            PropertyCopier<DatedMovieList,MovieList>.Copy(dMovieList, movieList);
            return movieList;
        }
    }
}
