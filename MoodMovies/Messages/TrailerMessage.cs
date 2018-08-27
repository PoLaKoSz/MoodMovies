using System;

namespace MoodMovies.Messages
{
    public class TrailerMessage
    {
        public TrailerMessage(string trailerUrl)
        {
            TrailerUrl = trailerUrl;
        }

        public string TrailerUrl { get; set; }
    }
}
