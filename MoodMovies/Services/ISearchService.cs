using System.Collections.Generic;
using System.Threading.Tasks;
using TMDB = TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Services
{
    public interface ISearchService
    {
        Task<List<TMDB.Movie>> Search(string apiKey, string SearchText, string ActorText, string SelectedBatch, string SelectedMood);
    }
}
