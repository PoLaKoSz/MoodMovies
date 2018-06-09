using System;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Messages
{
    public class MovieListMessage
    {
        public MovieList Movielist { get; }
        public bool IsAdult { get; set; }
        public string SearchString { get; set; }
        public DateTime DateOfSearch { get; }

        public MovieListMessage(MovieList results, bool isadult, string text)
        {
            Movielist = results;
            IsAdult = isadult;
            SearchString = text;
            DateOfSearch = DateTime.Now;
        }
    }
}
