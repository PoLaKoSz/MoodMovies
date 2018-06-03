using MoodMovies.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Messages
{
    public class AddToWatchListMessage : IMovieCardMessage
    {
        public AddToWatchListMessage(MovieCardViewModel mvCard)
        {
            MovieCard = mvCard;
        }

        public MovieCardViewModel MovieCard { get; private set; }
    }

    public class RemoveFromWatchListMessage : IMovieCardMessage
    {
        public RemoveFromWatchListMessage(MovieCardViewModel mvCard)
        {
            MovieCard = mvCard;
        }

        public MovieCardViewModel MovieCard { get; private set; }
    }

    public class AddToFavouritesMessage : IMovieCardMessage
    {
        public AddToFavouritesMessage(MovieCardViewModel mvCard)
        {
            MovieCard = mvCard;
        }

        public MovieCardViewModel MovieCard { get; private set; }
    }

    public class RemoveFromFavouritesMessage : IMovieCardMessage
    {
        public RemoveFromFavouritesMessage(MovieCardViewModel mvCard)
        {
            MovieCard = mvCard;
        }

        public MovieCardViewModel MovieCard { get; private set; }
    }

    public interface IMovieCardMessage
    {
        MovieCardViewModel MovieCard { get; }
    }
}
