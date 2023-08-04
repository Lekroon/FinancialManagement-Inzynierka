using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.FinancialAccount;

public class FinancialAccountUpdateRequest
{
    [Required]
    public Guid? AccountId { get; set; }

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
            AccountId = AccountId,
            AccountName = AccountName,
            Balance = Balance
        };
    }
}