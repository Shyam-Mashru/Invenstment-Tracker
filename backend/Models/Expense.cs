using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Expense
    {
        [Key]
        public int ExpenseId { get; set; }
        public int UserId { get; set; }
        public string ExpenseName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string ExpenseDescription { get; set; } = string.Empty;
        public string ExpenseDate { get; set; } = string.Empty;
        public string ExpenseTime { get; set; } = string.Empty; 

        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
