using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class ChangeData
    {
        public String Data
        {
            get;
            private set;
        }

        public ChangeData(string text)
        {
            Data = text;
        }

    }
}
