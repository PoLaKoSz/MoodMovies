using MoodMovies.Resources;
using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.ApiInterfaces;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Logic
{
    internal class OnlineServiceProvider
    {
        public void SetupKey(string key)
        {
            OnlineClient = new EasyClient(key);
        }

        #region Properties
        public static EasyClient OnlineClient;
        private static IMovieApi MovieClient;
        #endregion

        public static async Task<MovieList> SearchByTitleAsync(string title)
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public static async Task<MovieList> SearchByActorAsync(string title)
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }

        public static async Task<MovieList> SearchTopRatedAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;
            return await MovieClient.GetTopRatedAsync(language);
        }

        public static async Task<MovieList> GetNowPlayingAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetNowPlayingAsync(language).Result;

            return await Task.Run(()=> MapDatedMovieList(datedMovieList));
        }

        public static async Task<MovieList> SearchUpcomingAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetUpcomingAsync(language).Result;

            return await Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        public static async Task<MovieList> SearchPopularAsync(string language = "en")
        {
            MovieClient = OnlineClient.GetApi<IMovieApi>().Value;

            return await MovieClient.GetPopularAsync(language);
        }

        private static MovieList MapDatedMovieList(DatedMovieList dMovieList)
        {
            var movieList = new MovieList();
            PropertyCopier<DatedMovieList,MovieList>.Copy(dMovieList, movieList);
            return movieList;
        }
    }
}
