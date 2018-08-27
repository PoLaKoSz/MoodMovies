using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Logic;

namespace MoodMovies.Models
{
    public class CommonParameters : IViewModelParams, IListViewModelParams
    {
        public IEventAggregator EventAggregator { get; }
        public IOfflineServiceProvider OfflineService { get; }
        public IOnlineServiceProvider OnlineService { get; }
        public SnackbarMessageQueue StatusMessage { get; }
        public ImageCacher ImageCacher { get; }



        public CommonParameters(IEventAggregator eventAgg,
            IOfflineServiceProvider offline,
            IOnlineServiceProvider online,
            SnackbarMessageQueue statusMessage,
            ImageCacher imageCacher)
        {
            EventAggregator = eventAgg;
            OfflineService  = offline;
            OnlineService   = online;
            StatusMessage   = statusMessage;
            ImageCacher     = imageCacher;
        }
    }
}
