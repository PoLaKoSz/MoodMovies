using Caliburn.Micro;
using MoodMovies.Messages;
using MoodMovies.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.DataAccessLayer
{
    public interface IOnlineServiceProvider : IHandle<LoggedInMessage>
    {
        SearchQuery SearchQuery { get; }


        bool IsValidApiKey(string apiKey);
        
        /// <summary>
        /// Make a new search
        /// </summary>
        /// <returns>Collection of <see cref="Movie"/> from TMDb</returns>
        Task<List<Movie>> Search(SearchQuery query);
    }
}
