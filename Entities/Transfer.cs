using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Transfer
{
    [Key]
    public Guid TransferId { get; set; }
    public Guid? SenderAccountId { get; set; }
    public Guid? RecipientAccountId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
}