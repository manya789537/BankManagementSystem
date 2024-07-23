using BankManagementSystem.Model;
using System.Data.Entity;
using System.Reflection.Emit;

namespace BankManagementSystem
{
    public class MyContext : DbContext
    {
        public virtual DbSet<Login> Logins { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
    }
}