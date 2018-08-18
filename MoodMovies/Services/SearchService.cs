using DataModel.DataModel.Entities;
using MoodMovies.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMDB = TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Services
{
    public class SearchService : ISearchService
    {
        public SearchService(IOnlineServiceProvider serviceProvider)
        {
            onlineDb = serviceProvider;
        }

        private IOnlineServiceProvider onlineDb;

        public async Task<List<TMDB.Movie>> Search(string apiKey, string SearchText, string ActorText, string SelectedBatch, string SelectedMood)
        {
            List<TMDB.Movie> finalResult = new List<TMDB.Movie>();

            TMDB.MovieList moviesByText = null;
            TMDB.MovieList moviesByActor = null;
            TMDB.MovieList moviesByBatch = null;
            TMDB.MovieList moviesByMood = null;
            //we need to do traverse the pages as well as there might be more than 1
            //not implemented yet
            if (!string.IsNullOrEmpty(SearchText))
            {
                moviesByText = await onlineDb.SearchByTitleAsync(SearchText);
            }

            if (!string.IsNullOrEmpty(ActorText))
            {
                moviesByActor = await onlineDb.SearchByActorAsync(ActorText);
            }

            if (!string.IsNullOrEmpty(SelectedBatch) && SelectedBatch.ToLower() != "everything")
            {
                switch (SelectedBatch)
                {
                    case "TopRated":
                        moviesByBatch = await onlineDb.SearchTopRatedAsync();
                        break;
                    case "Popular":
                        moviesByBatch = await onlineDb.SearchPopularAsync();
                        break;
                    case "Upcoming":
                        moviesByBatch = await onlineDb.SearchUpcomingAsync();
                        break;
                    case "NowPlaying":
                        moviesByBatch = await onlineDb.GetNowPlayingAsync();
                        break;
                }
            }

            //not implemented yet
            if (!string.IsNullOrEmpty(SelectedMood))
            {

            }

            finalResult = await Filter(moviesByText, moviesByActor, moviesByBatch, moviesByMood);
            return finalResult;
        }

        private async Task<List<TMDB.Movie>> Filter(TMDB.MovieList text, TMDB.MovieList actors, TMDB.MovieList batch, TMDB.MovieList mood)
        {
            await Task.Delay(10);
            List<TMDB.Movie> movies = new List<TMDB.Movie>();
            //add them all to the same list
            if (text != null && text.Results != null && text.Results.Count > 0) movies.AddRange(text.Results);
            if (actors != null && actors.Results != null && actors.Results.Count > 0) movies.AddRange(actors.Results);
            if (batch != null && batch.Results != null && batch.Results.Count > 0) movies.AddRange(batch.Results);
            if (mood != null && mood.Results != null && mood.Results.Count > 0) movies.AddRange(mood.Results);

            //get the intersect
            return movies.GroupBy(x => x.Id)
                .Select(x => x.First())?
                .ToList();
        }
    }
}
