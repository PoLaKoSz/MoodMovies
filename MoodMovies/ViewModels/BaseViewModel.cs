using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.DataAccessLayer;
using MoodMovies.Messages;
using MoodMovies.Models;

namespace MoodMovies.ViewModels
{
    public abstract class BaseViewModel : Screen, IHandle<LoggedInMessage>
    {
        public BaseViewModel(CommonParameters commonParameters)
        {
            EventAgg = commonParameters.EventAggregator;
            StatusMessage = commonParameters.StatusMessage;
            OfflineDb = commonParameters.OfflineService;
            OnlineDb = commonParameters.OnlineService;

            EventAgg.Subscribe(this);
        }


        protected readonly IEventAggregator EventAgg;
        protected readonly SnackbarMessageQueue StatusMessage;

        protected IOfflineServiceProvider OfflineDb { get; private set; }
        protected IOnlineServiceProvider OnlineDb { get; private set; }

        protected User CurrentUser { get; private set; }


        public void Handle(LoggedInMessage message)
        {
            CurrentUser = message.CurrentUser;
        }
    }
}
