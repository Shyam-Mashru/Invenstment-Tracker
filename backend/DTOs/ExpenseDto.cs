namespace backend.DTOs
{
    public class ExpenseDto
    {
        public string ExpenseName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string ExpenseDescription { get; set; } = string.Empty;
        public string ExpenseDate { get; set; } = string.Empty;
        public string ExpenseTime { get; set; } = string.Empty;

    }
}
