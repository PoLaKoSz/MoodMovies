using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class TrailerMessage
    {
        public TrailerMessage(string _trailerUrl)
        {
            TrailerUrl = _trailerUrl;
        }
        public string TrailerUrl { get; set; }
    }
}
