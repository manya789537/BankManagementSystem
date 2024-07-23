using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankManagementSystem.Model
{
    public class Transaction
    {
        [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int TransID { get; set; }
        public string TransType { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public decimal IssuedAmount { get; set; }
        public int AccntNo { get; set; }
        public virtual Account Account { get; set; }
    }
}
