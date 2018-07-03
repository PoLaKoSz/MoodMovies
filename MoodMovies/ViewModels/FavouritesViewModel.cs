using System;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoodMovies.Logic;
using MoodMovies.Resources;

namespace MoodMovies.ViewModels
{
    public class FavouritesViewModel: ListBaseViewModel
    {
        public FavouritesViewModel(IEventAggregator _event, IOfflineServiceProvider serviceProvider) : base(_event)
        {
            DisplayName = "Favourites";
            offDb = serviceProvider;
        }

        #region Fields

        #endregion

        #region Properties
        readonly IOfflineServiceProvider offDb;
        #endregion

        #region Properties

        #endregion

        #region Methods
        /// <summary>
        /// Loads up movie cards for the favourite items that are found
        /// </summary>
        /// <returns></returns>
        public async Task LoadFavouriteItems()
        {
            try
            {
                var user = await offDb.GetUser(UserControl.CurrentUser.User_Id);
                var movies = await offDb.GetAllFavouriteItems(user);
                //build up the movie card view models
                Movies.Clear();
                await Task.Run(() =>
                {
                    foreach (var movie in movies)
                    {
                        if (!string.IsNullOrEmpty(movie.Poster_path))
                        {                           
                            var card = new MovieCardViewModel(movie.Movie_Id, movie.Title, new Uri(movie.Poster_path), movie.Overview,
                                movie.Release_date, movie.Vote_count, movie.Vote_average, movie.Video, movie.Adult, movie.Popularity, movie.Original_language, eventAgg)
                            {
                                IsFavourited = true,
                                Parent = this
                            };
                            //force updating the list from a different thread using custom cross thread extension method
                            Movies.AddOnUIThread(card);
                        }
                    }
                });
            }
            catch
            {
                throw;
                //implement some error handling*************
            }

        }
        #endregion

    }
}
