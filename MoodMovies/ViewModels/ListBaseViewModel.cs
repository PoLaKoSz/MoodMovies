﻿using Caliburn.Micro;
using MaterialDesignThemes.Wpf;
using System.Collections.ObjectModel;

namespace MoodMovies.ViewModels
{
    public class ListBaseViewModel : Screen
    {
        public ListBaseViewModel(IEventAggregator events, SnackbarMessageQueue statusMessage)
        {
            eventAgg = events;
            eventAgg.Subscribe(this);
            StatusMessage = statusMessage;
        }

        public ListBaseViewModel(IEventAggregator @event)
        {
            _event = @event;
        }

        #region Events
        protected readonly IEventAggregator eventAgg;
        #endregion

        #region Properties
        private ObservableCollection<MovieCardViewModel> movies = new ObservableCollection<MovieCardViewModel>();
        public ObservableCollection<MovieCardViewModel> Movies { get => movies; set { movies = value; NotifyOfPropertyChange(); } }
        private MovieCardViewModel _selectedItem;
        private IEventAggregator _event;

        public MovieCardViewModel SelectedItem { get => _selectedItem; set { _selectedItem = value; NotifyOfPropertyChange(); } }
        public SnackbarMessageQueue StatusMessage { get; set; }
        #endregion

        protected const string posterAddress = "https://image.tmdb.org/t/p/w500";
    }
}
