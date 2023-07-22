namespace Entities;

public class FinancialAccount
{
    public Guid? AccountId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? AccountName { get; set; }
    public decimal? Balance { get; set; }
}