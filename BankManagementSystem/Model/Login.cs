using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Model
{
    public class Login
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int LoginId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
