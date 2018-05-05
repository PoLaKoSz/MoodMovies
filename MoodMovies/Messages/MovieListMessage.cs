using MoodMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class MovieListMessage
    {
        public List<MovieSearchResult> Movielist { get; }
        public bool IsAdult { get; set; }
        public string SearchString { get; set; }
        public DateTime DateOfSearch { get; }

        public MovieListMessage(List<MovieSearchResult> results, bool isadult, string text)
        {
            Movielist = results;
            IsAdult = isadult;
            SearchString = text;
            DateOfSearch = DateTime.Now;
        }
    }
}
