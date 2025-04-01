using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly IExpenseService expenseService;

        public ExpenseController(IExpenseService expenseService)
        {
            this.expenseService=expenseService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseDto expenseDto)
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User Id Not Found...");
            }

            int userId = int.Parse(userIdClaim);
            
            var expense = await expenseService.AddExpense(userId, expenseDto);
            return Ok($"Expense {expenseDto.ExpenseName} Added Sucessfully...");
        }

        [Authorize]
        [HttpPut("{expenseId}")]
        public async Task<IActionResult> UpdateExpense(int expenseId, [FromBody] ExpenseDto expenseDto)
        {
            await expenseService.UpdateExpense(expenseId, expenseDto);
            return Ok($"Expense {expenseDto.ExpenseName} Updated Sucessfully...");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllExpenses()
        {
            var userIdClaim = User.FindFirst("UserId")?.Value;
            if (userIdClaim == null)
            {
                return Unauthorized("User Id Not Found...");
            }
            int userId = int.Parse(userIdClaim);
            var expenses = await expenseService.GetAllExpenses(userId);
            return Ok(expenses);
        }

        [Authorize]
        [HttpDelete("{expenseId}")]
        public async Task<IActionResult> DeleteExpense(int expenseId)
        {
            await expenseService.DeleteExpenseById(expenseId);
            return Ok($"Expense {expenseId} Deleted Sucessfully...");
        }

        [Authorize]
        [HttpGet("{expenseId}")]
        public async Task<IActionResult> GetExpenseById(int expenseId)
        {
            var expense = await expenseService.GetExpenseById(expenseId);
            return Ok(expense);
        }
    }
}
