using MoodMovies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class AddToWatchListMessage
    {
        public AddToWatchListMessage(MovieCardViewModel mvCard)
        {
            MovieVM = mvCard;
        }

        public MovieCardViewModel MovieVM { get; private set; }
    }

    public class RemoveFromWatchListMessage
    {
        public RemoveFromWatchListMessage(MovieCardViewModel mvCard)
        {
            MovieVM = mvCard;
        }

        public MovieCardViewModel MovieVM { get; private set; }
    }

    public class AddToFavouritesMessage
    {
        public AddToFavouritesMessage(MovieCardViewModel mvCard)
        {
            MovieVM = mvCard;
        }

        public MovieCardViewModel MovieVM { get; private set; }
    }

    public class RemoveFromFavouritesMessage
    {
        public RemoveFromFavouritesMessage(MovieCardViewModel mvCard)
        {
            MovieVM = mvCard;
        }

        public MovieCardViewModel MovieVM { get; private set; }
    }
}
