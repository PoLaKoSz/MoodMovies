using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using System.IO;
using System.Reflection;

namespace MoodMovies.ViewModels
{
    public sealed class AboutViewModel: Screen
    {
        #region Fields
        private string license;        
        #endregion

        #region Properties     
        public string License { get => license; set { license = value; NotifyOfPropertyChange(() => License); } }

        #endregion

        #region Methods
        public AboutViewModel()
        {
            DisplayName = "About";
            LoadLicense();
        }

        public void LoadLicense()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MoodMovies.Resources.LGPL License.txt";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    license = reader.ReadToEnd();
                }
            }
            
        }
        #endregion

    }
}
