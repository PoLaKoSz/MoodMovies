using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMdbEasy;
using TMdbEasy.ApiInterfaces;
using TMdbEasy.TmdbObjects.Movies;

namespace MoodMovies.Logic
{
    internal class OnlineServiceProvider
    {
        public OnlineServiceProvider(IEventAggregator _event)
        {
            eventAgg = _event;
            TmdbClient = new EasyClient("6d4b546936310f017557b2fb498b370b");
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion

        private readonly EasyClient TmdbClient;

        public Task<MovieFullDetails> CallTmdbAsync()
        {
            var movieApi = TmdbClient.GetApi<IMovieApi>().Value;
            return movieApi.GetDetailsAsync(500);
        }
    }
}
