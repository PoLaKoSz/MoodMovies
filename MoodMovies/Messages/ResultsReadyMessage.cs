using System;

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
