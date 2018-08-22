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
            OfflineDB = commonParameters.OfflineService;
            OnlineDB = commonParameters.OnlineService;

            EventAgg.Subscribe(this);
        }


        protected readonly IEventAggregator EventAgg;
        protected readonly SnackbarMessageQueue StatusMessage;

        protected IOfflineServiceProvider OfflineDB { get; private set; }
        protected IOnlineServiceProvider OnlineDB { get; private set; }

        public User CurrentUser { get; private set; }


        public void Handle(LoggedInMessage message)
        {
            CurrentUser = message.CurrentUser;
        }
    }
}
