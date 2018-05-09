using Caliburn.Micro;
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


        public static async Task<MovieList> SearchByTitleAsync(string title)
        {
            MovieClient = DbClient.GetApi<IMovieApi>().Value;
            return await MovieClient.SearchMoviesAsync(title);
        }
    }
}
