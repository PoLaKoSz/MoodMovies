using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.DataModel.Entities
{
    [Table("User_Movies")]
    public class User_Movies
    {
        [Key]
        [Column("User_Id")]
        public int User_Id { get; set; }

        [Key]
        [Column("UId")]
        public int UId { get; set; }

        [Column("Favourite")]
        public bool Favourite { get; set; }

        [Column("Watchlist")]
        public bool Watchlist { get; set; }
    }
}
