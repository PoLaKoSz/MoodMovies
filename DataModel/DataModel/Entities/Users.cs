using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Users")]
    public class Users
    {
        [Column(Name = "User_Id", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public int User_Id { get; set; }

        [Column(Name = "User_Name", DbType = "STRING", CanBeNull = false)]
        public string User_Name { get; set; }

        [Column(Name = "User_Surname", DbType = "STRING", CanBeNull = false)]
        public string User_Surname { get; set; }

        [Column(Name = "User_ApiKey", DbType = "STRING", CanBeNull = false)]
        public string User_ApiKey { get; set; }

        [Column(Name = "User_Active", DbType = "BOOLEAN", CanBeNull = false)]
        public bool User_Active { get; set; }

        [Column(Name = "User_ApiKey", DbType = "BOOLEAN", CanBeNull = false)]
        public bool Current_User { get; set; }

        [Column(Name = "User_Email", DbType = "STRING", CanBeNull = false)]
        public bool User_Email { get; set; }

        public virtual ICollection<User_Movies> Movies { get; set; }
    }
}
