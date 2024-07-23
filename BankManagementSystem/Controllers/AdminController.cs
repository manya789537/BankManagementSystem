using BankManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly MyContext _context;

        public AdminController(MyContext context)
        {
            _context = context;
        }

        [HttpGet("view-userinfo")]
        public IActionResult ViewCustomerInfo(int userId)
        {
            var user = _context.Users.FirstOrDefault(u => u.UserId ==userId);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost("create-user/{loggedInUserId}")]
        public IActionResult CreateUser(int loggedInUserId, [FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = _context.Users.Find(loggedInUserId);

            if (!loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
            {
                return Forbid("User is not authorized to create.");
            }

            try
            {
                var user = new User
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Email = model.Email,
                    Address = model.Address,
                    CreatedBy = loggedInUser.UserId,
                    CreatedOn = DateTime.Now,
                    UpdatedBy = null,
                    UpdatedOn = null,
                    MobileNo = model.MobileNo,
                    UserType = model.UserType,
                    IsActive = true
                };

                var login = new Login
                {
                    Username = model.Email,
                    Password = "DefaultPassword",
                    UserId = user.UserId
                };

                _context.Users.Add(user);
                _context.Logins.Add(login);
                _context.SaveChanges();

                return Ok("User created successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to create the user.");
            }
        }

        [HttpPut("update-user/{loggedInUserId}/{currentUserId}")]
        public IActionResult UpdateUser(int loggedInUserId, int currentUserId, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = _context.Users.Find(loggedInUserId);

            if (loggedInUser == null || !loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
            {
                return Forbid("User is not authorized to update details");
            }

            try
            {
                var currentUser = _context.Users.Find(currentUserId);
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;

                _context.Entry(currentUser).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok("User details updated successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to update user details.");
            }
        }

        [HttpPost("create-branch/{loggedInUserId}")]
        public IActionResult CreateBranch(int loggedInUserId, [FromBody] Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = _context.Users.Find(loggedInUserId);

            if (!loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
            {
                return Forbid("User is not authorized to create branch details.");
            }

            try
            {
                _context.Branches.Add(branch);
                _context.SaveChanges();
                return Ok("Branch details created successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to create the branch details.");
            }
        }

        [HttpPut("update-branch/{loggedInUserId}/{branchId}")]
        public IActionResult UpdateBranch(int loggedInUserId, int branchId, [FromBody] Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = _context.Users.Find(loggedInUserId);

            if (loggedInUser == null || !loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
            {
                return Forbid("User is not authorized to update branch details");
            }

            try
            {
                var branch1 = _context.Users.Find(branchId);
                branch1.FirstName = branch.BranchName;
                branch1.LastName = branch.BranchAddress;

                _context.Entry(branch1).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok("Branch details updated successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to update branch details.");
            }
        }
    }
}