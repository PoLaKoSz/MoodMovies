using System;

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
