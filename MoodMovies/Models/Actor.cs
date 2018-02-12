using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
