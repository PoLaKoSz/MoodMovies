using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "User_Movies")]
    public class User_Movies
    {
        [Column(Name = "User_Id", IsDbGenerated = false, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int User_Id { get; set; }

        [Column(Name = "UId", IsDbGenerated = false, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int UId { get; set; }

        [Column(Name = "Favourite", IsDbGenerated = false, IsPrimaryKey = false, DbType = "BOOLEAN")]
        public bool Favourite { get; set; }

        [Column(Name = "Watchlist", IsDbGenerated = false, IsPrimaryKey = false, DbType = "BOOLEAN")]
        public bool Watchlist {get; set;}
    }
}
