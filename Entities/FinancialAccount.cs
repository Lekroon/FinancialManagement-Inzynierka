using System.ComponentModel.DataAnnotations;

namespace Entities;

public class FinancialAccount
{
    [Key]
    public Guid? AccountId { get; set; }
    
    public Guid? UserId { get; set; }
    
    public Guid? CurrencyId { get; set; }
    
    [StringLength(60)]
    public string? AccountName { get; set; }
    
    public decimal? Balance { get; set; }
}