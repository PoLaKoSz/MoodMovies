using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public class BaseViewModel : Screen
    {
        public BaseViewModel(CommonParameters commonParameters)
        {
            eventAgg = commonParameters.EventAggregator;
            StatusMessage = commonParameters.StatusMessage;
            offlineDb = commonParameters.OfflineService;
            onlineDb = commonParameters.OnlineService;
        }

        public IEventAggregator eventAgg;

        #region Providers
        protected IOfflineServiceProvider offlineDb;
        protected IOnlineServiceProvider onlineDb;
        #endregion

        public SnackbarMessageQueue StatusMessage { get; set; }
    }
}
