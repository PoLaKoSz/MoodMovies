using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Models
{
    public class Genre
    {
        public Genre() { }
        public Genre(string name, bool ischecked)
        {
            GenreName = name;
            GenreChecked = ischecked;
        }
        public string GenreName { get; set; }
        public bool GenreChecked { get; set; }
    }
}
