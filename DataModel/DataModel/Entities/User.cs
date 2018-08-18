using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataModel.DataModel.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("User_Id")]
        public int ID { get; set; }

        [Column("User_Name")]
        public string Name { get; set; }

        [Column("User_Surname")]
        public string Surname { get; set; }

        [Column("User_ApiKey")]
        public string ApiKey { get; set; }

        [Column("User_Email")]
        public string Email { get; set; }

        [Column("User_Password")]
        public string Password { get; set; }

        [Column("User_Active")]
        public bool IsActive { get; set; }

        [Column("Current_User")]
        public bool IsCurrentUser { get; set; }
    }
}
