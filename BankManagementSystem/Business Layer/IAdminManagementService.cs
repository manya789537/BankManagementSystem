using BankManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Business_Layer
{
    public interface IAdminManagementService
    {
        Task<IActionResult> UpdateCustomer(int id1, int id2, User user);
    }
}
