using System;

namespace MoodMovies.Messages
{
    public class BrowseSearchResultsMessage
    {
        public int PageNumber { get; }


        
        /// <summary>
        /// Send this message to the SearchViewModel to display the next or previous
        /// page from the search results
        /// </summary>
        /// <param name="pageNumber">Requested page number</param>
        public BrowseSearchResultsMessage(int pageNumber)
        {
            PageNumber = pageNumber;
        }
    }
}
