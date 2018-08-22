using System.Collections.Generic;
using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.DataAccessLayer
{
    public interface IOnlineServiceProvider
    {
        void ChangeClient(string Key);

        Task<List<Movie>> Search(string apiKey, string SearchText, string ActorText, string SelectedBatch, string SelectedMood);

        Task<MovieList> SearchByTitleAsync(string title);

        Task<MovieList> SearchByActorAsync(string title);

        Task<MovieList> SearchTopRatedAsync(string language = "en");

        Task<MovieList> GetNowPlayingAsync(string language = "en");

        Task<MovieList> SearchUpcomingAsync(string language = "en");

        Task<MovieList> SearchPopularAsync(string language = "en");
    }
}
