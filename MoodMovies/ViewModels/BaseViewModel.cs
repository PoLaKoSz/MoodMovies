using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;

namespace MoodMovies.ViewModels
{
    public class BaseViewModel : Screen
    {
        public BaseViewModel(IEventAggregator _event, IOfflineServiceProvider offlineService, IOnlineServiceProvider onlineService, SnackbarMessageQueue statusMessage)
        {
            eventAgg = _event;
            StatusMessage = statusMessage;
            offlineDb = offlineService;
            onlineDb = onlineService;
        }

        public IEventAggregator eventAgg;

        #region Providers
        protected IOfflineServiceProvider offlineDb;
        protected IOnlineServiceProvider onlineDb;
        #endregion

        public SnackbarMessageQueue StatusMessage { get; set; }
    }
}
