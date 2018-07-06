using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Movies")]
    public class Movies        
    {
        public Movies() { }

        [Column(Name = "UId", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER NOT NULL" )]
        [Key]
        public int UId { get; set; }
        [Column(Name = "Movie_Id", IsDbGenerated = false, DbType = "INTEGER NOT NULL")]
        public int Movie_Id { get; set; }
        [Column(Name = "Vote_Count", DbType = "STRING", CanBeNull = false)]
        public int Vote_count { get; set; }        
        [Column(Name = "Video", DbType = "BOOLEAN", CanBeNull = false)]
        public bool Video { get; set; }
        [Column(Name = "Vote_Average", DbType = "DOUBLE", CanBeNull = false)]
        public double Vote_average { get; set; }
        [Column(Name = "Title", DbType = "STRING", CanBeNull = false)]
        public string Title { get; set; }
        [Column(Name = "Popularity", DbType = "DOUBLE", CanBeNull = false)]
        public double Popularity { get; set; }
        [Column(Name = "Poster_Path", DbType = "STRING", CanBeNull = true)]
        public string Poster_path { get; set; }
        [Column(Name = "Original_Language", DbType = "STRING", CanBeNull = true)]
        public string Original_language { get; set; }
        [Column(Name = "Original_Title", DbType = "STRING", CanBeNull = true)]
        public string Original_title { get; set; }
        [Column(Name = "Backdrop_Path", DbType = "STRING", CanBeNull = true)]
        public string Backdrop_path { get; set; }
        [Column(Name = "Adult", DbType = "BOOLEAN", CanBeNull = false)]
        public bool Adult { get; set; }
        [Column(Name = "Overview", DbType = "STRING", CanBeNull = true)]
        public string Overview { get; set; }
        [Column(Name = "Release_Date", DbType = "STRING", CanBeNull = true)]
        public string Release_date { get; set; }
        [Column(Name = "Poster_Cache", DbType = "STRING", CanBeNull = true)]
        public string Poster_Cache { get; set; }

        public virtual ICollection<User_Movies> Users { get; set; }
    }    
}
