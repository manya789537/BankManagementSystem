using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Security.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using BankManagementSystem.Model;
using Microsoft.AspNetCore.SignalR;

namespace BankManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var context = new MyContext())
            {
                var user = new User
                {
                    FirstName = "Alain",
                    LastName = "Bomer",
                    DateOfBirth = new DateTime(1989, 1, 18),
                    Gender = "Male",
                    Email = "alain.bomer18gmail.com",
                    Address = "Gurugram",
                    CreatedOn = DateTime.Now,
                    MobileNo = 9278610339,
                    IsActive = true
                };

                var login = new Login
                {
                    Username = "AlainBomer",
                    Password = "Alainbomar0118",
                    UserId = user.UserId
                };

                context.Users.Add(user);
                context.Logins.Add(login);
                context.SaveChanges();

                var users = context.Users.ToList();

                foreach (var usr in users)
                {
                    Console.WriteLine(usr);
                }

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }
    }
}