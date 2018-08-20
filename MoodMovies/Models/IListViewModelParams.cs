using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;

namespace MoodMovies.Models
{
    public interface IListViewModelParams
    {
        IEventAggregator EventAggregator { get; }
        IOfflineServiceProvider OfflineService { get; }
        SnackbarMessageQueue StatusMessage { get; }
    }
}
