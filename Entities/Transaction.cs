namespace Entities;

public class Transaction
{
    public Guid TransactionId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
}