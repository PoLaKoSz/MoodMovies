using System;
using System.Collections.Generic;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Messages
{
    public class MovieListMessage
    {
        public List<Movie> Movielist { get; }
        public bool IsAdult { get; set; }
        public string SearchString { get; set; }
        public DateTime DateOfSearch { get; }

        public MovieListMessage(List<Movie> results, bool isadult, string text)
        {
            Movielist = results;
            IsAdult = isadult;
            SearchString = text;
            DateOfSearch = DateTime.Now;
        }
    }
}
