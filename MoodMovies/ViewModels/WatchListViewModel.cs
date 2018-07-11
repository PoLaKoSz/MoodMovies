﻿using System.Threading.Tasks;
using Caliburn.Micro;
using DataModel.DataModel.Entities;
using MaterialDesignThemes.Wpf;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class WatchListViewModel : ListBaseViewModel
    {
        public WatchListViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider, SnackbarMessageQueue statusMessage, ImageCacher imageCacher, Users currentUser)
            : base(_event, statusMessage, currentUser)
        {
            DisplayName = "Favourites";
            offlineDb = serviceProvider;
            ImageCacher = imageCacher;
        }

        #region Fields
        private IOfflineServiceProvider offlineDb;
        private readonly ImageCacher ImageCacher;
        #endregion

        #region Methods
        public async Task LoadWatchListItems()
        {
            try
            {
                var movies = await offlineDb.GetAllWatchListItems(CurrentUser);
                //build up the movie card view models
                Movies.Clear();
                await Task.Run(() =>
                {
                    foreach (Movies movie in movies)
                    {
                        if (!string.IsNullOrEmpty(movie.Poster_path))
                        {
                            ImageCacher.ScanPoster(movie);

                            var card = new MovieCardViewModel(movie, eventAgg)
                            {
                                IsWatchListed = true,
                                Parent = this
                            };
                            //force updating the list from a different thread using custom cross thread extension method
                            Movies.AddOnUIThread(card);
                        }
                    }
                });
            }
            catch when (UserControl.CurrentUser == null)
            {
                StatusMessage.Enqueue("Please select a user from the 'User' page.");
            }
            catch
            {
                StatusMessage.Enqueue("Internal Error");
            }
        }
        #endregion
    }
}
