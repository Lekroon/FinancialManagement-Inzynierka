using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO;

public class FinancialAccountAddRequest
{
    [Required]
    public Guid? UserId { get; set; }
    
    [Required]
    public Guid? CurrencyId { get; set; }
    
    [Required]
    [MaxLength(60)]
    public string? AccountName { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
    public decimal? Balance { get; set; }

    public FinancialAccount ToFinancialAccount()
    {
        return new FinancialAccount
        {
            UserId = UserId,
            CurrencyId = CurrencyId,
            AccountName = AccountName,
            Balance = Balance
        };
    }
}