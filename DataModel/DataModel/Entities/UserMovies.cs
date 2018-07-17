using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "User_Movies")]
    public class User_Movies
    {
        [Column(Name = "User_Id")]
        [Key]
        public int User_Id { get; set; }

        [Column(Name = "UId")]
        [Key]
        public int UId { get; set; }

        [Column(Name = "Favourite")]
        public bool Favourite { get; set; }

        [Column(Name = "Watchlist")]
        public bool Watchlist {get; set;}
    }
}
