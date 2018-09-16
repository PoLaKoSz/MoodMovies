using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodMovies.DataAccessLayer
{
    public class OfflineServiceProvider : IServiceProvider, IOfflineServiceProvider
    {
        public OfflineServiceProvider(IDb database)
        {
            db = database;
        }


        readonly IDb db;


        #region User Methods
        /// <summary>
        /// Adds a <see cref="User"/> to the DB
        /// </summary>
        public async Task CreateUser(User user)
        {
            await Task.Run(() => db.context.Users.Add(user));

            SaveChanges();
        }

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an API Key
        /// </summary>
        public async Task<User> GetUserByApiKey(string apikey)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.ApiKey == apikey).SingleOrDefault());
        }

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an Email address and Password
        /// </summary>
        public async Task<User> GetUserByEmailPassword(string emailAddress, string password)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.Email == emailAddress && x.Password == password).SingleOrDefault());           
        }

        /// <summary>
        /// Gets a <see cref="User"/> from the DB using an Email address
        /// </summary>
        public async Task<User> GetUserByEmail(string emailAddress)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.Email == emailAddress).SingleOrDefault());
        }

        /// <summary>
        /// Gets the current <see cref="User"/> from DB
        /// </summary>
        public async Task<User> GetCurrentUser()
        {
            return await Task.Run(() => db.context.Users.Where(x => x.IsCurrentUser == true).SingleOrDefault());
        }

        /// <summary>
        /// Gets all users from DB
        /// </summary>
        public async Task<List<User>> GetAllUsers()
        {
            return await Task.Run(() => db.context.Users.ToList());
        }

        /// <summary>
        /// Enable auto login for the given <see cref="User"/>. Disable for everybody else
        /// </summary>
        public async Task SetAsCurrentUser(User user)
        {
            var previousCurrentUser = await Task.Run(() => db.context.Users.Where(x => x.IsCurrentUser == true).SingleOrDefault());

            if(previousCurrentUser != null)
            {
                DisableAutoLogin(previousCurrentUser);
            }

            user.IsCurrentUser = true;

            SaveChanges();
        }

        /// <summary>
        /// Disable auto login for the given <see cref="User"/>
        /// </summary>
        public void DisableAutoLogin(User user)
        {
            user.IsCurrentUser = false;

            SaveChanges();
        }

        /// <summary>
        /// CHeck if the API Key exists in the DB
        /// </summary>
        public async Task<bool> ApiKeyExists(string apikey)
        {
            var count = await Task.Run(() => db.context.Users.Where(x => x.ApiKey == apikey).Count());

            return (count == 0) ? false : true;
        }
        #endregion

        #region Movie Methods
        /// <summary>
        /// Gets the <see cref="User_Movies"/> corresponding for the given
        /// <see cref="User"/> and <see cref="Movies"/>
        /// </summary>
        private async Task<User_Movies> GetUserMovie(User user, Movies movie)
        {
            return await Task.Run(() => db.context.UserMovies.Where(x => x.UId == movie.Movie_Id && x.User_Id == user.ID).SingleOrDefault());
        }

        /// <summary>
        /// Add the <see cref="Movies"/> to the Movies table
        /// </summary>
        /// <returns></returns>
        public async Task<bool> AddMovie(User user, Movies movie)
        {
            try
            {
                if (db.context.Movies.Any(x => x.Movie_Id == movie.Movie_Id))
                {
                    return false;
                }
                else
                {
                    await Task.Run(() =>
                    {
                        db.context.Set<Movies>().Add(movie);
                        AddUserMovie(user, movie);

                        SaveChanges();
                    });
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Adds a new <see cref="User_Movies"/> for the specific <see cref="User"/>
        /// and <see cref="Movies"/>
        /// </summary>
        private void AddUserMovie(User user, Movies movie)
        {
            db.context.UserMovies.Add(new User_Movies()
            {
                UId = movie.Movie_Id,
                User_Id = user.ID,
            });
        }
        #endregion

        #region WatchList methods
        /// <summary>
        /// Adds the <see cref="Movies"/> to the WatchList
        /// </summary>
        public async Task AddToWatchList(User user, Movies movie)
        {
            var usermovie = await GetUserMovie(user, movie);

            await Task.Run(() =>
            {
                if (usermovie != null)
                {
                    usermovie.Watchlist = true;

                    SaveChanges();
                }
            });
        }

        /// <summary>
        /// Removes the <see cref="Movies"/> from the WatchList
        /// </summary>
        public async Task RemoveFromWatchList(User user, Movies movie)
        {
            var usermovie = await GetUserMovie(user, movie);

            await Task.Run(() =>
            {
                if (usermovie != null)
                {
                    usermovie.Watchlist = false;

                    SaveChanges();
                }
            });
        }

        /// <summary>
        /// Returns all watchlisted <see cref="Movies"/> for the given <see cref="User"/>
        /// </summary>
        public async Task<List<Movies>> GetAllWatchListItems(User user)
        {
            var links = await Task.Run(() => db.context.UserMovies.Where(x => x.User_Id == user.ID && x.Watchlist == true));
            return await Task.Run(() => db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        #region Favourite methods
        /// <summary>
        /// Adds the <see cref="Movies"/> to the Favourites
        /// </summary>
        public async Task AddToFavourites(User user, Movies movie)
        {
            var usermovie = await GetUserMovie(user, movie);

            await Task.Run(() =>
            {
                if (usermovie != null)
                {
                    usermovie.Favourite = true;

                    SaveChanges();
                }
            });
        }

        /// <summary>
        /// Removes the <see cref="Movies"/> from the Favourites
        /// </summary>
        public async Task RemoveFromFavourites(User user, Movies movie)
        {
            var usermovie = await GetUserMovie(user, movie);

            await Task.Run(() =>
            {
                if (usermovie != null)
                {
                    usermovie.Favourite = false;

                    SaveChanges();
                }
            });
        }

        /// <summary>
        /// Returns all favourited <see cref="Movies"/> for the given <see cref="User"/>
        /// </summary>
        public async Task<List<Movies>> GetAllFavouriteItems(User user)
        {
            var links = await Task.Run(() => db.context.UserMovies.Where(x => x.User_Id == user.ID && x.Favourite == true));
            return await Task.Run(() => db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        /// <summary>
        /// Commit changes to the DB
        /// </summary>
        public void SaveChanges()
        {
            db.context.SaveChanges();
        }
    }
}
