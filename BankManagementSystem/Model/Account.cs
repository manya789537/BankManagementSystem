using System.ComponentModel.DataAnnotations;

namespace BankManagementSystem.Model
{
    public class Account
    {
        [Key]
        public int AccntNo { get; set; }
        public string AccntType { get; set; }
        public decimal Balance { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public int BranchId { get; set; }
        public virtual Branch Branch { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
    }
}
