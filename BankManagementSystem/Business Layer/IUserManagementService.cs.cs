using BankManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Business_Layer
{
    public interface IUserManagementService
    {
        Task<IActionResult> Login(Login model);
        Task<IActionResult> ChangePassword(string username, string updatedPassword);
    }


    // IAdminManagementService.cs


}
