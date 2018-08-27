using System;

namespace MoodMovies.Models
{
    public class Actor
    {
        public Actor(string name, bool isChecked)
        {
            ActorName = name;
            ActorChecked = isChecked;
        }

        public string ActorName { get; set; }
        public bool ActorChecked { get; set; }
    }
}
