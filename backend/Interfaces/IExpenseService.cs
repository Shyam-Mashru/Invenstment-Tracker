using backend.DTOs;
using backend.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace backend.Interfaces
{
    public interface IExpenseService
    {
        Task<IEnumerable<ExpenseDto>> GetAllExpenses (int userId);
        Task<ExpenseDto> GetExpenseById (int expenseId);
        Task DeleteExpenseById (int expenseId);
        Task<Expense> AddExpense (int userId, ExpenseDto expense);
        Task UpdateExpense(int expenseId, ExpenseDto expense);    }
}
