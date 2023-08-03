using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.FinancialAccount;

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

    public Entities.FinancialAccount ToFinancialAccount()
    {
        return new Entities.FinancialAccount
        {
            UserId = UserId,
            CurrencyId = CurrencyId,
            AccountName = AccountName,
            Balance = Balance
        };
    }
}