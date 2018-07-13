﻿using System.Collections.Generic;
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

        [Column(Name = "User_Name", DbType = "STRING", IsPrimaryKey = false, CanBeNull = false)]
        public string User_Name { get; set; }

        [Column(Name = "User_Surname", DbType = "STRING", IsPrimaryKey = false, CanBeNull = false)]
        public string User_Surname { get; set; }

        [Column(Name = "User_ApiKey", DbType = "STRING", IsPrimaryKey = false, CanBeNull = false)]
        public string User_ApiKey { get; set; }

        [Column(Name = "User_Email", DbType ="STRING", IsPrimaryKey = false, CanBeNull = false)]
        public string User_Email { get; set; }

        [Column(Name = "User_Password", DbType = "STRING", IsPrimaryKey = false, CanBeNull = false)]
        public string User_Password { get; set; }

        [Column(Name = "User_Active", DbType = "BOOLEAN", IsPrimaryKey = false, CanBeNull = false)]
        public bool User_Active { get; set; }

        [Column(Name = "Current_User", DbType = "BOOLEAN", IsPrimaryKey = false, CanBeNull = false)]
        public bool Current_User { get; set; }

       // [Column(Name = "User_Password", DbType = "STRING", IsPrimaryKey = false, CanBeNull = false)]
       // public string User_Password { get; set; }

        public virtual ICollection<User_Movies> Movies { get; set; }
    }
}
