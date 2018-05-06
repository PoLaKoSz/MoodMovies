using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    internal class OnlineServiceProvider
    {
        public OnlineServiceProvider(IEventAggregator _event)
        {
            eventAgg = _event;
        }

        #region Events
        public IEventAggregator eventAgg;
        #endregion
    }
}
