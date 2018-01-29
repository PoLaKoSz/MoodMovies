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
        // constructor
        public AboutViewModel()
        {
            DisplayName = "About";
            LoadLicense();
        }

        #region Properties Bound to visibility
        private bool _creditsExpanderIsOpen;
        public bool CreditsExpanderIsOpen { get => _creditsExpanderIsOpen; set { _creditsExpanderIsOpen = value; NotifyOfPropertyChange(); } }
        private bool _creatorExpanderIsOpen;
        public bool CreatorExpanderIsOpen { get => _creatorExpanderIsOpen; set { _creatorExpanderIsOpen = value; NotifyOfPropertyChange(); } }
        private bool _licenseExpanderIsOpen;
        public bool LicenseExpanderIsOpen { get => _licenseExpanderIsOpen; set { _licenseExpanderIsOpen = value; NotifyOfPropertyChange(); } }
        #endregion

        #region General Properties
        private string license;
        public string License { get => license; set { license = value; NotifyOfPropertyChange(); } }

        #endregion
                
        #region Public Methods
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

        public void CloseExpanders()
        {
            if( CreditsExpanderIsOpen )
            {
                if( CreatorExpanderIsOpen || LicenseExpanderIsOpen )
                {
                    CreatorExpanderIsOpen = false;
                    LicenseExpanderIsOpen = false;
                }
                
            }
            else if ( CreatorExpanderIsOpen )
            {
                if (CreditsExpanderIsOpen || LicenseExpanderIsOpen)
                {
                    CreditsExpanderIsOpen = false;
                    LicenseExpanderIsOpen = false;
                }
            }
            else if ( LicenseExpanderIsOpen )
            {                
                    CreatorExpanderIsOpen = false;
                    CreditsExpanderIsOpen = false;                
            }
        }
        #endregion

    }
}
