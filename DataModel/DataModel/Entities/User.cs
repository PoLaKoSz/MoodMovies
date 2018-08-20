using System.ComponentModel.DataAnnotations;
using System.Data.Linq.Mapping;

namespace DataModel.DataModel.Entities
{
    [Table(Name = "Users")]
    public class User
    {
        [Key]
        [Column(Name = "User_Id")]
        public int ID { get; set; }

        [Column(Name = "User_Name")]
        public string Name { get; set; }

        [Column(Name = "User_Surname")]
        public string Surname { get; set; }

        [Column(Name = "User_ApiKey")]
        public string ApiKey { get; set; }

        [Column(Name = "User_Email")]
        public string Email { get; set; }

        [Column(Name = "User_Password")]
        public string Password { get; set; }

        [Column(Name = "User_Active")]
        public bool IsActive { get; set; }

        [Column(Name = "Current_User")]
        public bool IsCurrentUser { get; set; }
    }
}
