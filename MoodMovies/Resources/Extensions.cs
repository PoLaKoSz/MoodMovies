using System;
using System.Collections.Generic;
using System.Windows;

namespace MoodMovies.Resources
{
    public static class Extensions
    {
        /// <summary>
        /// Adds an item created on a background thread to a collection on the UI thread.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="obj"></param>
        public static void AddOnUIThread<T>(this ICollection<T> collection, T obj)
        {
            Action<T> AddFunction = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(AddFunction, obj);
        }
    }
}
