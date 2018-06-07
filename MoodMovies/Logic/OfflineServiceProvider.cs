using DataModel.DataModel;
using DataModel.DataModel.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider : IServiceProvider
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
        /// Add a movie to the movie table
        /// </summary>
        /// <param name="user"></param>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task AddMovie(Movies movie)
        {
            try
            {
                await Task.Run(() =>
                {
                    Db.context.Set<Movies>().Add(movie);
                    Db.context.SaveChanges();
                });
            }
            catch
            {
                //movie exists already
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

        public async Task<List<Movies>> GetAllWatchListItems(Users user)
        {
            var links = await Task.Run(() => Db.context.UserMovies.Where(x => x.User_Id == user.User_Id));
            List<Movies> movies = null;

            //needs implementation
            return movies;
        }
        #endregion
        
    }
}
