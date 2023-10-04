namespace Entities;

public class TransactionHistory
{
    public Guid HistoryId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? TrasactionId { get; set; }
}