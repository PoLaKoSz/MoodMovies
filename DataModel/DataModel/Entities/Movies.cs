using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Movies")]
    public class Movies        
    {
        [Column(Name = "UId")]
        [Key]
        public int UId { get; set; }

        [Column(Name = "Movie_Id")]
        public int Movie_Id { get; set; }

        [Column(Name = "Vote_Count")]
        public int Vote_count { get; set; }  
        
        [Column(Name = "Video")]
        public bool Video { get; set; }

        [Column(Name = "Vote_Average")]
        public double Vote_average { get; set; }

        [Column(Name = "Title")]
        public string Title { get; set; }

        [Column(Name = "Popularity")]
        public double Popularity { get; set; }

        [Column(Name = "Poster_Path")]
        public string Poster_path { get; set; }

        [Column(Name = "Original_Language")]
        public string Original_language { get; set; }

        [Column(Name = "Original_Title")]
        public string Original_title { get; set; }

        [Column(Name = "Backdrop_Path")]
        public string Backdrop_path { get; set; }

        [Column(Name = "Adult")]
        public bool Adult { get; set; }

        [Column(Name = "Overview")]
        public string Overview { get; set; }

        [Column(Name = "Release_Date")]
        public string Release_date { get; set; }

        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        public string Poster_Cache { get; set; }
    }    
}
