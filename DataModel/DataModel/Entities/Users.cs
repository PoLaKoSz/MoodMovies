using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Users")]
    public class Users
    {
        [Column(Name = "User_Id")]
        [Key]
        public int User_Id { get; set; }

        [Column(Name = "User_Name")]
        public string User_Name { get; set; }

        [Column(Name = "User_Surname")]
        public string User_Surname { get; set; }

        [Column(Name = "User_ApiKey")]
        public string User_ApiKey { get; set; }

        [Column(Name = "User_Email")]
        public string User_Email { get; set; }

        [Column(Name = "User_Password")]
        public string User_Password { get; set; }

        [Column(Name = "User_Active")]
        public bool User_Active { get; set; }

        [Column(Name = "Current_User")]
        public bool Current_User { get; set; }
    }
}
