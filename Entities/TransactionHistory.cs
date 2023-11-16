using System.ComponentModel.DataAnnotations;

namespace Entities;

public class TransactionHistory
{
    [Key]
    public Guid HistoryId { get; set; }
    
    public Guid? AccountId { get; set; }
    
    public Guid? TransactionId { get; set; }
}