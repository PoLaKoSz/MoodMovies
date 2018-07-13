using System;

namespace MoodMovies.DataAccessLayer
{
    public interface IWebClient : IDisposable
    {
        /// <summary>
        /// Downloads the resource with the specified URI to a local file.
        /// </summary>
        /// <param name="address">The URI specified as a System.String, from which to download data.</param>
        /// <param name="fileName">The name of the local file that is to receive the data.</param>
        /// <exception cref="ArgumentNullException">The address parameter is null. -or-The fileName parameter is null.</exception>
        /// <exception cref="WebException">The URI formed by combining System.Net.WebClient.BaseAddress and address is invalid.-or-
        /// filename is null or System.String.Empty.-or- The file does not exist. -or- An
        /// error occurred while downloading data.</exception>
        /// <exception cref="NotSupportedException">The method has been called simultaneously on multiple threads.</exception>
        void DownloadFile(Uri address, string fileName);
    }
}
