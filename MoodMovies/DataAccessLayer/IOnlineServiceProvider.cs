using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.DataAccessLayer
{
    public interface IOnlineServiceProvider
    {
        void ChangeClient(string Key);

        Task<MovieList> SearchByTitleAsync(string title);

        Task<MovieList> SearchByActorAsync(string title);

        Task<MovieList> SearchTopRatedAsync(string language = "en");

        Task<MovieList> GetNowPlayingAsync(string language = "en");

        Task<MovieList> SearchUpcomingAsync(string language = "en");

        Task<MovieList> SearchPopularAsync(string language = "en");
    }
}
