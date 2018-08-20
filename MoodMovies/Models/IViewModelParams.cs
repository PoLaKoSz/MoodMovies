using MoodMovies.DataAccessLayer;

namespace MoodMovies.Models
{
    public interface IViewModelParams
    {
        IOnlineServiceProvider OnlineService { get; }
    }
}
