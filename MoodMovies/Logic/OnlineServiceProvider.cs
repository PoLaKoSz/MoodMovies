using Caliburn.Micro;
using MoodMovies.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.ApiInterfaces;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Logic
{
    internal static class OnlineServiceProvider
    {
        public static void SetupKey(string key)
        {
            DbClient = new EasyClient(key);
        }

        #region Properties
        public static EasyClient DbClient;
        private static IMovieApi MovieClient;
        #endregion

        public static Task<MovieList> SearchByTitleAsync(string title)
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;
            return MovieClient.SearchMoviesAsync(title);
        }

        public static Task<MovieList> SearchByActorAsync(string title)
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;
            return MovieClient.SearchMoviesAsync(title);
        }

        public static Task<MovieList> SearchTopRatedAsync(string language = "en")
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;
            return MovieClient.GetTopRatedAsync(language);
        }

        public static Task<MovieList> GetNowPlayingAsync(string language = "en")
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetNowPlayingAsync(language).Result;

            return Task.Run(()=> MapDatedMovieList(datedMovieList));
        }

        public static Task<MovieList> SearchUpcomingAsync(string language = "en")
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;

            var datedMovieList = MovieClient.GetUpcomingAsync(language).Result;

            return Task.Run(() => MapDatedMovieList(datedMovieList));
        }

        public static Task<MovieList> SearchPopularAsync(string language = "en")
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;

            return MovieClient.GetPopularAsync(language);
        }

        private static MovieList MapDatedMovieList(DatedMovieList dMovieList)
        {
            var movieList = new MovieList();
            PropertyCopier<DatedMovieList,MovieList>.Copy(dMovieList, movieList);
            return movieList;
        }
    }
}
