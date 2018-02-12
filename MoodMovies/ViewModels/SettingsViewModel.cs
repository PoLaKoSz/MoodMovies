﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace MoodMovies.ViewModels
{
    public class SettingsViewModel: Screen
    {
        #region Fields
        private string _name;
        #endregion

        #region Properties
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }
        #endregion

        #region Methods
        public SettingsViewModel()
        {
            Name = "Settings";
        }
        #endregion
    }
}
