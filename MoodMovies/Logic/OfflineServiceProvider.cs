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
        /// Gets first user in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Users> GetFirstUser()
        {            
            return await Task.Run(()=> Db.context.Users.FirstOrDefault());
            //var b = Db.context.Movies.Where(x => x.Movie_Id == 1).SingleOrDefault();
        }
        /// <summary>
        /// Gets first movie in db
        /// </summary>
        /// <param name="movie"></param>
        /// <returns></returns>
        public async Task<Movies> GetFirstMovie()
        {
            return await Task.Run(() => Db.context.Movies.FirstOrDefault());
            //var b = Db.context.Movies.Where(x => x.Movie_Id == 1).SingleOrDefault();
        }

        public async Task AddToWatchList(Users user, Movies movie)
        {
            await Task.Run(()=> {
                Db.context.Set<User_Movies>().Add(new User_Movies()
                {
                    Movie_Id = movie.Movie_Id,
                    User_Id = user.User_Id
                });
                Db.context.SaveChanges();
            });
        }
    }
}
