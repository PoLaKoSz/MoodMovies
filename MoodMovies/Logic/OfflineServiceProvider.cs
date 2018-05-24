using DataModel.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoodMovies.Logic
{
    public class OfflineServiceProvider
    {
        public OfflineServiceProvider()
        {

        }

        public void SetupDatabase()
        {
            UserLogic.DumpDatabase();
        }
    }
}
