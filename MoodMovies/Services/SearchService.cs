using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Services
{
    public class SearchService : ISearchService
    {
        public SearchService()
        {

        }

        public async Task<MovieList> Search(string apiKey)
        {
            await Task.Delay(10);
            return new MovieList();
        }
    }
}
