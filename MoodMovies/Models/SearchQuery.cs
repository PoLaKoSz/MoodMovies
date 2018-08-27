using System.Windows.Controls;

namespace MoodMovies.Models
{
    public class SearchQuery
    {
        public string ApiKey { get; set; }
        public string SearchText { get; set; }
        public string ActorName { get; set; }
        public ComboBoxItem Batch { get; set; }
        public string Mood { get; set; }
        public int PageNumber { get; set; }



        public SearchQuery()
        {
            ApiKey = "";
            SearchText = "";
            ActorName = "";
            Batch = new ComboBoxItem() { Tag = "" };
            Mood = "";
            PageNumber = 1;
        }
    }
}
