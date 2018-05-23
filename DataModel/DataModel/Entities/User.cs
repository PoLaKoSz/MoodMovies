﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Linq.Mapping;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Users")]
    public class User
    {        
        public User()
        {

        }

        [Column(Name = "User_Id", IsDbGenerated = true, IsPrimaryKey = true, DbType = "INTEGER")]
        [Key]
        public string User_Name { get; set; }

        [Column(Name = "User_Name", DbType = "STRING")]
        public string EmpName { get; set; }

        [Column(Name = "User_Surname", DbType = "STRING")]
        public double Salary { get; set; }

        [Column(Name = "User_ApiKey", DbType = "STRING")]
        public string Designation { get; set; }
    }
}
