using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Services
{
    public interface ISearchService
    {
        Task<MovieList> Search(string apiKey);
    }
}
