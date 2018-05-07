using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class ResultsReadyMessage
    {
    }

    public class StartLoadingMessage
    {
        public StartLoadingMessage(string message)
        {
            Text = message;
        }

        public string Text { get; set; }
    }
}
