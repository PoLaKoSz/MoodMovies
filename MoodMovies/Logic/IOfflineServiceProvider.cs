using DataModel.DataModel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public interface IOfflineServiceProvider
    {
        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateUser(User user);       
        /// <summary>
        /// Gets user in db using id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<User> GetUser(int id);
        /// <summary>
        /// Gets user in db using apikey
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<User> GetUserByApiKey(string apikey);
        /// <summary>
        /// Gets user in db using email
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<User> GetUserByEmailPassword(string emailAdress, string password);
        /// <summary>
        /// Gets user in db using email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        Task<User> GetUserByEmail(string emailAddress);
        /// <summary>
        /// Gets current user in db. there should only be one
        /// </summary>
        /// <returns></returns>
        Task<User> GetCurrentUSer();
        /// <summary>
        /// Gets all users in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<List<User>> GetAllUsers();
        /// <summary>
        /// Set current user field and unset previous user if any
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task SetCurrentUserFieldToTrue(User user);
        /// <summary>
        /// Unset current user field of current user
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        void SetCurrentUserFieldToFalse(User user);
        /// <summary>
        /// Set/Unset current user field
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task ChangeCurrentUserField(string apikey, bool value);
        /// <summary>
        /// Delete user from db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task DeleteUser(User user);
        #endregion

        #region Movie Methods
        /// <summary>
        /// Adds a movie to the movies table database
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task CreateMovie(Movies movie);
        /// <summary>
        /// gets the usermovie link between a user and a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<User_Movies> GetUserMovieLink(User user, Movies movie);
        /// <summary>
        /// Gets the movie using the id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<Movies> GetMovie(int movieId);
        /// <summary>
        /// Add a movie to the movie table
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task<bool> AddMovie(Movies movie);       
        #endregion

        #region WatchList methods
        /// <summary>
        /// Adds a movie to the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task AddToWatchList(User user, Movies movie);
        /// <summary>
        /// Removes a movie from the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task RemoveFromWatchList(User user, Movies movie);
        /// <summary>
        /// Returns all movies linked to a specific user as a watchlist item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<Movies>> GetAllWatchListItems(User user);
        #endregion

        #region Favourite methods
        /// <summary>
        /// Adds a movie to the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task AddToFavourites(User user, Movies movie);
        /// <summary>
        /// Removes a movie from the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        Task RemoveFromFavourites(User user, Movies movie);
        /// <summary>
        /// Returns all movies linked to a specific user as a favourites item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<List<Movies>> GetAllFavouriteItems(User user);
        #endregion

        /// <summary>
        /// See if Api Key exists in db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> ApiKeyExists(string apikey);
        /// <summary>
        /// Commit changes to database
        /// </summary>
        void SaveChanges();
    }
}
