using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.ComponentModel.DataAnnotations;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Users")]
    public class Users
    {        
        public Users()
        {

        }

        [Column(Name = "User_Id", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int User_Id { get; set; }

        [Column(Name = "User_Name", DbType = "STRING", CanBeNull = false)]
        public string User_Name { get; set; }

        [Column(Name = "User_Surname", DbType = "STRING", CanBeNull = false)]
        public string User_Surname { get; set; }

        [Column(Name = "User_ApiKey", DbType = "STRING", CanBeNull = false)]
        public string User_ApiKey { get; set; }

        public virtual ICollection<User_Movies> Movies { get; set; }
    }
}
