using System.Windows;
using MoodMovies.ViewModels;
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
