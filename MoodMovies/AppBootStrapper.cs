using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MoodMovies.ViewModels;
using MoodMovies.Views;
using Caliburn.Micro;

namespace MoodMovies
{
    class AppBootStrapper: BootstrapperBase
    {
        public AppBootStrapper()
        {
            Initialize();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainViewModel>();
        }

    }
}
