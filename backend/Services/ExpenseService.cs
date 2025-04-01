using backend.Data;
using backend.DTOs;
using backend.Interfaces;
using backend.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly BackendDbContext context;

        public ExpenseService(BackendDbContext context)
        {
            this.context=context;
        }

        public async Task<Expense> AddExpense(int userId, ExpenseDto expense)
        {
            var newExpense = new Expense
            {
                UserId = userId,
                ExpenseName = expense.ExpenseName,
                Amount = expense.Amount,
                PaymentMethod = expense.PaymentMethod,
                ExpenseDescription = expense.ExpenseDescription,
                ExpenseDate = expense.ExpenseDate,
                ExpenseTime = expense.ExpenseTime,
            };

            await context.Expenses.AddAsync(newExpense);
            await context.SaveChangesAsync(); 
            
            return newExpense;
        }

        public async Task DeleteExpenseById(int expenseId)
        {
            var expense = await context.Expenses.FindAsync(expenseId);
            if (expense == null)
            {
                throw new KeyNotFoundException(nameof(expense));
            }

            context.Expenses.Remove(expense);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ExpenseDto>> GetAllExpenses(int userId)
        {
            var expenses = await context.Expenses.Where(context => context.UserId == userId).ToListAsync();

            return expenses.Select(e => new ExpenseDto
            {
                ExpenseName = e.ExpenseName,
                Amount = e.Amount,
                PaymentMethod = e.PaymentMethod,
                ExpenseDescription = e.ExpenseDescription,
                ExpenseDate = e.ExpenseDate,
                ExpenseTime= e.ExpenseTime,
            }).ToList();
        }

        public async Task<ExpenseDto> GetExpenseById(int expenseId)
        {
            var expense = await context.Expenses.FindAsync(expenseId);
            if(expense == null)
            {
                return null;
            }

            return new ExpenseDto
            {
                ExpenseName = expense.ExpenseName,
                Amount = expense.Amount,
                PaymentMethod = expense.PaymentMethod,
                ExpenseDescription = expense.ExpenseDescription,
                ExpenseDate = expense.ExpenseDate,
                ExpenseTime= expense.ExpenseTime,
            };
        }

        public async Task UpdateExpense(int expenseId, ExpenseDto expense)
        {
            var updateExpense = await context.Expenses.FindAsync(expenseId);
            if(updateExpense == null)
            {
                throw new KeyNotFoundException(nameof(expense));
            }

            updateExpense.ExpenseName = expense.ExpenseName;
            updateExpense.Amount = expense.Amount;
            updateExpense.PaymentMethod = expense.PaymentMethod;
            updateExpense.ExpenseDescription = expense.ExpenseDescription;
            updateExpense.ExpenseDate = expense.ExpenseDate;
            updateExpense.ExpenseTime = expense.ExpenseTime;
           
            context.Expenses.Update(updateExpense);
            await context.SaveChangesAsync();
        }
    }
}
