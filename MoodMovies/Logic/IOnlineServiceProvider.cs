using System.Threading.Tasks;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Logic
{
    public interface IOnlineServiceProvider
    {
        Task<MovieList> SearchByTitleAsync(string title);

        Task<MovieList> SearchByActorAsync(string title);

        Task<MovieList> SearchTopRatedAsync(string language = "en");

        Task<MovieList> GetNowPlayingAsync(string language = "en");

        Task<MovieList> SearchUpcomingAsync(string language = "en");

        Task<MovieList> SearchPopularAsync(string language = "en");
    }
}
