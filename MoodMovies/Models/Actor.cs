using System;

namespace MoodMovies.Models
{
    public class Actor
    {
        public Actor() { }
        public Actor( string name, bool ischecked)
        {
            ActorName = name;
            ActorChecked = ischecked;
        }
        public string ActorName { get; set; }
        public bool ActorChecked { get; set; }
    }
}
