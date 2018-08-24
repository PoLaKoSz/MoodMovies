using DataModel.DataModel.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoodMovies.DataAccessLayer
{
    public interface IOfflineServiceProvider
    {
        #region User Methods
        /// <summary>
        /// Adds a <see cref="User"/> to the DB
        /// </summary>
        Task CreateUser(User user);

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an API Key
        /// </summary>
        Task<User> GetUserByApiKey(string apikey);

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an Email address and Password
        /// </summary>
        Task<User> GetUserByEmailPassword(string emailAddress, string password);

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an Email address
        /// </summary>
        Task<User> GetUserByEmail(string emailAddress);

        /// <summary>
        /// Gets the current <see cref="User"/> from DB
        /// </summary>
        Task<User> GetCurrentUser();

        /// <summary>
        /// Gets all users from DB
        /// </summary>
        Task<List<User>> GetAllUsers();

        /// <summary>
        /// Enable auto login for the given <see cref="User"/>. Disable for everybody else
        /// </summary>
        Task SetAsCurrentUser(User user);

        /// <summary>
        /// Disable auto login for the given <see cref="User"/>
        /// </summary>
        void DisableAutoLogin(User user);

        /// <summary>
        /// CHeck if the API Key exists in the DB
        /// </summary>
        Task<bool> ApiKeyExists(string apikey);
        #endregion

        #region Movie Methods
        /// <summary>
        /// Add the <see cref="Movies"/> to the Movies table
        /// </summary>
        /// <returns></returns>
        Task<bool> AddMovie(User user, Movies movie);
        #endregion

        #region WatchList methods
        /// <summary>
        /// Adds the <see cref="Movies"/> to the WatchList
        /// </summary>
        Task AddToWatchList(User user, Movies movie);

        /// <summary>
        /// Removes the <see cref="Movies"/> from the WatchList
        /// </summary>
        Task RemoveFromWatchList(User user, Movies movie);

        /// <summary>
        /// Returns all watchlisted <see cref="Movies"/> for the given <see cref="User"/>
        /// </summary>
        Task<List<Movies>> GetAllWatchListItems(User user);
        #endregion

        #region Favourite methods
        /// <summary>
        /// Adds the <see cref="Movies"/> to the Favourites
        /// </summary>
        Task AddToFavourites(User user, Movies movie);

        /// <summary>
        /// Removes the <see cref="Movies"/> from the Favourites
        /// </summary>
        Task RemoveFromFavourites(User user, Movies movie);

        /// <summary>
        /// Returns all favourited <see cref="Movies"/> for the given <see cref="User"/>
        /// </summary>
        Task<List<Movies>> GetAllFavouriteItems(User user);
        #endregion
    }
}
