using BankManagementSystem.Model;
using Microsoft.AspNetCore.Mvc;

namespace BankManagementSystem.Business_Layer
{
    public interface ICustomerManagementService
    {
        Task<IActionResult> CreateCustomer(int id, User user);
        // Add other customer-related methods here.
    }
}
