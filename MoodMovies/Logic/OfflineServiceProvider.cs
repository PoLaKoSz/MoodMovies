using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider : IServiceProvider, IOfflineServiceProvider
    {
        public OfflineServiceProvider(IDb database)
        {
            db = database;
        }

        #region Fields
        readonly IDb db;
        #endregion

        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUser(User user)
        {
            //check to see if a user already exists
            await Task.Run(() => db.context.Users.Add(user));
            db.context.SaveChanges();
        }
        /// <summary>
        /// Gets user in db using id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<User> GetUser(int id)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.ID == id).SingleOrDefault());
        }
        /// <summary>
        /// Gets user in db using apikey
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<User> GetUserByApiKey(string apikey)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.ApiKey == apikey).SingleOrDefault());
        }
        /// <summary>
        /// Gets user in db using email and password
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmailPassword(string emailAddress, string password)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.Email == emailAddress && x.Password == password).SingleOrDefault());           
        }
        /// <summary>
        /// Gets user in db using email
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <returns></returns>
        public async Task<User> GetUserByEmail(string emailAddress)
        {
            return await Task.Run(() => db.context.Users.Where(x => x.Email == emailAddress).SingleOrDefault());
        }
        /// <summary>
        /// Gets current user in db. there should only be one
        /// </summary>
        /// <returns></returns>
        public async Task<User> GetCurrentUSer()
        {
            return await Task.Run(() => db.context.Users.Where(x => x.IsCurrentUser == true).SingleOrDefault());
        }
        /// <summary>
        /// Gets all users in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<List<User>> GetAllUsers()
        {
            return await Task.Run(() => db.context.Users.ToList());
        }
        /// <summary>
        /// Set current user field and unset previous user if any
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task SetCurrentUserFieldToTrue(User user)
        {
            //previous user that was current if any
            var previousCurrentUser = await Task.Run(() => db.context.Users.Where(x => x.IsCurrentUser == true).SingleOrDefault());

            if(previousCurrentUser != null)
            {
                previousCurrentUser.IsCurrentUser = false;
            }

            if (user != null)
            {
                user.IsCurrentUser = true;
            }
            SaveChanges();
        }
        /// <summary>
        /// Unset current user field of current user
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public void SetCurrentUserFieldToFalse(User user)
        {
            if (user != null)
            {
                user.IsCurrentUser = false;
            }
            SaveChanges();
        }
        /// <summary>
        /// Set/Unset current user field
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task ChangeCurrentUserField(string apikey, bool value)
        {
            var user = await Task.Run(() => db.context.Users.Where(x => x.ApiKey == apikey).SingleOrDefault());

            if (user != null)
            {
                user.IsCurrentUser = (value) ? true : false;
            }
            SaveChanges();
        }
        /// <summary>
        /// 
        /// </summary>
        public async Task DeleteUser(User currentUser)
        {
            await Task.Run(() => db.context.Users.Remove(currentUser));
            SaveChanges();
        }
        /// <summary>
        /// See if Api Key exists in db
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> ApiKeyExists(string apikey)
        {
            //check to see if a user already exists
            var count = await Task.Run(() => db.context.Users.Where(x => x.ApiKey == apikey).Count());
            return (count == 0) ? false : true;
        }
        #endregion

        #region Movie Methods
        /// <summary>
        /// Adds a movie to the movies table database
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task CreateMovie(Movies movie)
        {
            //check to see if a user already exists
            await Task.Run(() => db.context.Movies.Add(movie));
            db.context.SaveChanges();
        }
        /// <summary>
        /// gets the usermovie link between a user and a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<User_Movies> GetUserMovieLink(User user, Movies movie)
        {
            return await Task.Run(() => db.context.UserMovies.Where(x => x.UId == movie.Movie_Id && x.User_Id == user.ID).SingleOrDefault());
        }
        /// <summary>
        /// Gets the movie using the id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Movies> GetMovie(int movieId)
        {
            return await Task.Run(() => db.context.Movies.Where(x => x.Movie_Id == movieId).SingleOrDefault());
        }
        /// <summary>
        /// Add a movie to the movie table
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<bool> AddMovie(Movies movie)
        {
            try
            {
                if (db.context.Movies.Any(x => x.Movie_Id == movie.Movie_Id))
                {
                    //movie exists already
                    return false;
                }
                else
                {
                    await Task.Run(() =>
                    {
                        db.context.Set<Movies>().Add(movie);
                        db.context.SaveChanges();
                    });
                    return true;
                }
            }
            catch
            {
                //error
                return false;
            }
        }      
        #endregion

        #region WatchList methods
        /// <summary>
        /// Adds a movie to the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddToWatchList(User user, Movies movie)
        {
            await Task.Run(() =>
            {
                db.context.Set<User_Movies>().Add(new User_Movies()
                {
                    UId = movie.Movie_Id,
                    User_Id = user.ID,
                    Watchlist = true
                });
                db.context.SaveChanges();
            });
        }
        /// <summary>
        /// Removes a movie from the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task RemoveFromWatchList(User user, Movies movie)
        {
            await Task.Run(() =>
            {
                var usermovie = db.context.Set<User_Movies>().Where(x => x.User_Id == user.ID
                && x.UId == movie.Movie_Id).SingleOrDefault();
                if (usermovie != null)
                {
                    usermovie.Watchlist = false;
                    db.context.SaveChanges();
                }
            });
        }
        /// <summary>
        /// Returns all movies linked to a specific user as a watchlist item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Movies>> GetAllWatchListItems(User user)
        {
            var links = await Task.Run(() => db.context.UserMovies.Where(x => x.User_Id == user.ID && x.Watchlist == true));
            return await Task.Run(() => db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        #region Favourite methods
        /// <summary>
        /// Adds a movie to the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddToFavourites(User user, Movies movie)
        {
            await Task.Run(() =>
            {
                db.context.Set<User_Movies>().Add(new User_Movies()
                {
                    UId = movie.Movie_Id,
                    User_Id = user.ID,
                    Favourite = true
                });
                db.context.SaveChanges();
            });
        }
        /// <summary>
        /// Removes a movie from the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task RemoveFromFavourites(User user, Movies movie)
        {
            await Task.Run(() =>
            {
                var usermovie = db.context.Set<User_Movies>().Where(x => x.User_Id == user.ID
                && x.UId == movie.Movie_Id).SingleOrDefault();

                if (usermovie != null)
                {
                    usermovie.Favourite = false;
                    db.context.SaveChanges();
                }
            });
        }
        /// <summary>
        /// Returns all movies linked to a specific user as a favourites item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Movies>> GetAllFavouriteItems(User user)
        {
            var links = await Task.Run(() => db.context.UserMovies.Where(x => x.User_Id == user.ID && x.Favourite == true));
            return await Task.Run(() => db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        /// <summary>
        /// Commit changes to the database
        /// </summary>
        public void SaveChanges()
        {
            db.context.SaveChanges();
        }
    }
}
