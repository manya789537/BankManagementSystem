using BankManagementSystem.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Security.Principal;

namespace BankManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly MyContext _context;
        public CustomerController(MyContext context)
        {
            _context = context;
        }

        [HttpPut("update-customerinfo/{loggedInUserId}")]
        public IActionResult UpdateCustomerInfo(int loggedInUserId, [FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loggedInUser = _context.Users.Find(loggedInUserId);

            if (loggedInUser == null || !loggedInUser.IsActive)
            {
                return Forbid("User is not authorized to update details");
            }

            try
            {
                loggedInUser.FirstName = user.FirstName;
                loggedInUser.LastName = user.LastName;

                _context.Entry(loggedInUser).State = EntityState.Modified;
                _context.SaveChanges();

                return Ok("User details updated successfully.");
            }
            catch (Exception e)
            {
                return BadRequest("Failed to update user details.");
            }
        }

        [HttpGet("view-balance")]
        public IActionResult ViewBalance(int accntNo)
        {
            var accnt = _context.Accounts.Find(accntNo);

            if (accnt == null)
            {
                return NotFound();
            }

            var balance = _context.Accounts.SingleOrDefault(a => a.AccntNo == accnt.AccntNo);

            if (balance == null)
            {
                return NotFound();
            }

            return Ok(balance.Balance);
        }

        [HttpGet("view-transactions")]
        public IActionResult ViewTransactions(int accntNo, DateTime fromDate, DateTime toDate)
        {
            var accnt = _context.Accounts.Find(accntNo);

            if (accnt == null)
            {
                return NotFound();
            }

            var trans = _context.Transactions
                .Where(t => t.AccntNo == accnt.AccntNo && t.CreatedOn >= fromDate && t.CreatedOn <= toDate)
                .ToList();

            if (trans.Count == 0)
            {
                return NotFound("No transactions found for this user.");
            }

            return Ok(trans);
        }

        [HttpPost("transfer-money")]
        public IActionResult TransferMoney(int loggedInUserId, int recieverAccntNo, [FromBody] Transaction model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var senderAccount = _context.Accounts.Find(model.AccntNo);
            var recieverAccount = _context.Accounts.Find(recieverAccntNo);

            if (senderAccount == null || recieverAccount == null)
            {
                return NotFound("One or Both accounts not found.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (senderAccount.Balance < model.IssuedAmount)
                    {
                        return BadRequest("Insufficient funds in the sender's account.");
                    }

                    senderAccount.Balance -= model.IssuedAmount;
                    recieverAccount.Balance += model.IssuedAmount;

                    var loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == loggedInUserId);

                    if (loggedInUser == null || !loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
                    {
                        return Forbid("User cannot create transaction record.");
                    }

                    var transactionRecord = new Transaction
                    {
                        TransType = model.TransType,
                        CreatedBy = loggedInUserId,
                        CreatedOn = DateTime.Now,
                        IssuedAmount = model.IssuedAmount,
                        AccntNo = model.AccntNo
                    };

                    var transactionRecord2 = new Transaction
                    {
                        TransType = "Credit",
                        CreatedBy = loggedInUserId,
                        CreatedOn = DateTime.Now,
                        IssuedAmount = model.IssuedAmount,
                        AccntNo = recieverAccntNo
                    };

                    _context.Transactions.Add(transactionRecord);
                    _context.Transactions.Add(transactionRecord2);
                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok($"Transaction ({model.TransType}) completed successfully.");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest($"Failed to perform {model.TransType.ToLower()} operation.");
                }
            }
        }

        [HttpPost("transaction/{loggedInUserId}/{accntNo}")]
        public IActionResult PerformTransaction(int loggedInUserId, int accntNo, [FromBody] Transaction model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var account = _context.Accounts.Find(accntNo);

            if (account == null)
            {
                return NotFound("Account not found.");
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (model.TransType == "Withdraw")
                    {
                        if (account.Balance < model.IssuedAmount)
                        {
                            return BadRequest("Insufficient funds in the account.");
                        }

                        account.Balance -= model.IssuedAmount;
                    }
                    else if (model.TransType == "Deposit")
                    {
                        account.Balance += model.IssuedAmount;
                    }
                    else
                    {
                        return BadRequest("Invalid transaction type.");
                    }

                    var loggedInUser = _context.Users.FirstOrDefault(u => u.UserId == loggedInUserId);

                    if (loggedInUser == null || !loggedInUser.IsActive || loggedInUser.UserType != UserType.Admin)
                    {
                        return Forbid("User cannot create transaction record.");
                    }

                    var transactionRecord = new Transaction
                    {
                        TransType = model.TransType,
                        CreatedBy = loggedInUserId,
                        CreatedOn = DateTime.Now,
                        IssuedAmount = model.IssuedAmount,
                        AccntNo = account.AccntNo
                    };

                    _context.Transactions.Add(transactionRecord);
                    _context.SaveChanges();

                    transaction.Commit();

                    return Ok($"Transaction ({model.TransType}) completed successfully.");
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return BadRequest($"Failed to perform {model.TransType.ToLower()} operation.");
                }
            }
        }
    }
}
