using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.Transfer;

public class TransferAddRequest
{
    [Required]
    public Guid? SenderAccountId { get; set; }
    
    [Required]
    public Guid? RecipientAccountId { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Transfer amount cannot be negative")]
    public decimal? Amount { get; set; }
    
    [Required]
    public DateTime? TransactionDate { get; set; }

    public Entities.Transfer ToTransfer()
    {
        return new Entities.Transfer
        {
            SenderAccountId = SenderAccountId,
            RecipientAccountId = RecipientAccountId,
            Amount = Amount,
            TransactionDate = TransactionDate
        };
    }
}