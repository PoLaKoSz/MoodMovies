using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider : IServiceProvider, IOfflineServiceProvider
    {
        #region User Methods
        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUser(Users user)
        {
            //check to see if a user already exists
            await Task.Run(() => Db.context.Users.Add(user));
            Db.context.SaveChanges();
        }
        /// <summary>
        /// Gets first user in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Users> GetFirstUser()
        {
            return await Task.Run(() => Db.context.Users.FirstOrDefault());
        }
        /// <summary>
        /// Gets user in db using id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Users> GetUser(int id)
        {
            return await Task.Run(() => Db.context.Users.Where(x => x.User_Id == id).SingleOrDefault());
        }
        /// <summary>
        /// Gets user in db using apikey
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Users> GetUserByApiKey(string apikey)
        {
            return await Task.Run(() => Db.context.Users.Where(x => x.User_ApiKey == apikey).SingleOrDefault());
        }
        /// <summary>
        /// Gets all users in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<List<Users>> GetAllUsers()
        {
            return await Task.Run(() => Db.context.Users.ToList());
        }
        /// <summary>
        /// Set/Unset current user field
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task ChangeCurrentUserField(string apikey, bool value)
        {
            var user = await Task.Run(() => Db.context.Users.Where(x => x.User_ApiKey == apikey).SingleOrDefault());

            if(user != null)
            {
                user.Current_User = (value) ? true : false;
            }
            SaveChanges();
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
            await Task.Run(() => Db.context.Movies.Add(movie));
            Db.context.SaveChanges();
        }
        /// <summary>
        /// gets the usermovie link between a user and a movie
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<User_Movies> GetUserMovieLink(Users user, Movies movie)
        {
            return await Task.Run(() => Db.context.UserMovies.Where(x => x.UId == movie.Movie_Id && x.User_Id == user.User_Id).SingleOrDefault());
        }
        /// <summary>
        /// Gets the movie using the id
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Movies> GetMovie(int movieId)
        {
            return await Task.Run(() => Db.context.Movies.Where(x => x.Movie_Id == movieId).SingleOrDefault());
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
                if (Db.context.Movies.Any(x => x.Movie_Id == movie.Movie_Id))
                {
                    //movie exists already
                    return false;
                }
                else
                {
                    await Task.Run(() =>
                    {
                        Db.context.Set<Movies>().Add(movie);
                        Db.context.SaveChanges();
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
        /// <summary>
        /// Gets first movie in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Movies> GetFirstMovie()
        {
            return await Task.Run(() => Db.context.Movies.FirstOrDefault());
        }
        #endregion

        #region WatchList methods
        /// <summary>
        /// Adds a movie to the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddToWatchList(Users user, Movies movie)
        {            
            await Task.Run(()=> {
                Db.context.Set<User_Movies>().Add(new User_Movies()
                {
                    UId = movie.Movie_Id,
                    User_Id = user.User_Id,
                    Watchlist = true
                });
                Db.context.SaveChanges();
            });
        }
        /// <summary>
        /// Removes a movie from the watchlist
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task RemoveFromWatchList(Users user, Movies movie)
        {
            await Task.Run(() => {
                var usermovie = Db.context.Set<User_Movies>().Where(x => x.User_Id == user.User_Id
                && x.UId == movie.Movie_Id).SingleOrDefault();
                if(usermovie != null)
                {
                    usermovie.Watchlist = false;
                    Db.context.SaveChanges();
                }                
            });
        }
        /// <summary>
        /// Returns all movies linked to a specific user as a watchlist item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Movies>> GetAllWatchListItems(Users user)
        {
            var links = await Task.Run(() => Db.context.UserMovies.Where(x => x.User_Id == user.User_Id && x.Watchlist == true));
            return await Task.Run(() => Db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        #region Favourite methods
        /// <summary>
        /// Adds a movie to the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddToFavourites(Users user, Movies movie)
        {
            await Task.Run(() => {
                Db.context.Set<User_Movies>().Add(new User_Movies()
                {
                    UId = movie.Movie_Id,
                    User_Id = user.User_Id,
                    Favourite = true
                });
                Db.context.SaveChanges();
            });
        }
        /// <summary>
        /// Removes a movie from the favourites list
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task RemoveFromFavourites(Users user, Movies movie)
        {
            await Task.Run(() => {
                var usermovie = Db.context.Set<User_Movies>().Where(x => x.User_Id == user.User_Id
                && x.UId == movie.Movie_Id).SingleOrDefault();

                if (usermovie != null)
                {
                    usermovie.Favourite = false;
                    Db.context.SaveChanges();
                }                
            });
        }
        /// <summary>
        /// Returns all movies linked to a specific user as a favourites item
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<List<Movies>> GetAllFavouriteItems(Users user)
        {
            var links = await Task.Run(() => Db.context.UserMovies.Where(x => x.User_Id == user.User_Id && x.Favourite == true));
            return await Task.Run(() => Db.context.Movies.Join(links, x => x.Movie_Id, y => y.UId, (x, y) => x).ToList());
        }
        #endregion

        //Commit changes to database
        public void SaveChanges()
        {
            Db.context.SaveChanges();
        }
    }
}
