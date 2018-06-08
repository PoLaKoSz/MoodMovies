using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;
using Schema = System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "User_Movies")]
    public class User_Movies
    {
        public User_Movies()
        {

        }

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

        //public Movies Movie { get; set; }
        //public Users User { get; set; }
    }
}
