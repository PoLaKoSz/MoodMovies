using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;

namespace MoodMovies.Models
{
    public interface IListViewModelParams
    {
        IEventAggregator EventAggregator { get; }
        IOfflineServiceProvider OfflineService { get; }
        SnackbarMessageQueue StatusMessage { get; }
        ImageCacher ImageCacher { get; }
    }
}
