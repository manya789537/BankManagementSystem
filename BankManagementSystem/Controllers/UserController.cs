using BankManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace BankManagementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly MyContext _context;

        public UserController(MyContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] Login model)
        {
            var user = _context.Logins
                .Include(u => u.User)
                .FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var userDetails = new
            {
                userId = user.User.UserId,
                firstName = user.User.FirstName,
                lastName = user.User.LastName,
                dob = user.User.DateOfBirth,
                gender = user.User.Gender,
                email = user.User.Email,
                address = user.User.Address,
                createdBy = user.User.CreatedBy,
                createdOn = user.User.CreatedOn,
                updatedBy = user.User.UpdatedBy,
                updatedOn = user.User.UpdatedOn,
                mobileNo = user.User.MobileNo,
                userType = user.User.UserType,
                isActive = user.User.IsActive
            };

            return Ok(userDetails);
        }

        [HttpPut("change-password/{updatedPassword}")]
        public IActionResult ChangePassword([FromBody] Login model, string updatedPassword) //ask abt hashing method for security
        {
            var user = _context.Logins.
                FirstOrDefault(u => u.Username == model.Username && u.Password == model.Password);

            if (user == null)
            {
                return Unauthorized("User not found.");
            }

            if (string.IsNullOrEmpty(updatedPassword))
            {
                return BadRequest("Invalid password.");
            }

            if (user.Password != model.Password)
            {
                return BadRequest("Current password does not match.");
            }

            if (model.Password == updatedPassword)
            {
                return BadRequest("New password is same as the current password.");
            }

            try
            {
                user.Password = updatedPassword;
                _context.SaveChanges();

                return Ok("Password updated successfully.");
            }

            catch (Exception e)
            {
                return BadRequest("Failed to update the password.");
            }
        }
        //forgot password(method)
    }
}
