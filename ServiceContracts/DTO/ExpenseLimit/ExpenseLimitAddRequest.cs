using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.ExpenseLimit;

public class ExpenseLimitAddRequest
{
    [Required]
    public Guid? AccountId { get; set; }
    
    [Required]
    public Guid? CategoryId { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Expense limit cannot be negative")]
    public decimal? LimitAmount { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Time limit cannot be negative")]
    public int? LimitPeriod { get; set; }

    public Entities.ExpenseLimit ToExpenseLimit()
    {
        return new Entities.ExpenseLimit
        {
            AccountId = AccountId,
            CategoryId = CategoryId,
            LimitAmount = LimitAmount,
            LimitPeriod = LimitPeriod
        };
    }
}