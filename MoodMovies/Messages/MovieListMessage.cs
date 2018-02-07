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
        public List<MovieSearchResult> Movielist { get; private set; }

        public MovieListMessage(List<MovieSearchResult> results)
        {
            Movielist = results;
        }
    }
}
