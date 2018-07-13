using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        #region Providers
        protected IOfflineServiceProvider offlineDb;
        protected IOnlineServiceProvider onlineDb;
        #endregion

        #region Properties
        public SnackbarMessageQueue StatusMessage { get; set; }
        #endregion
    }
}
