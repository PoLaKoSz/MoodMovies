using System;

namespace MoodMovies.Models
{
    public class Genre
    {
        public Genre(string name, bool isChecked)
        {
            GenreName = name;
            GenreChecked = isChecked;
        }

        public string GenreName { get; set; }
        public bool GenreChecked { get; set; }
    }
}
