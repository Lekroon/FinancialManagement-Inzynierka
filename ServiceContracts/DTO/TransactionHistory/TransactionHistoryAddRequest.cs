using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.TransactionHistory;

public class TransactionHistoryAddRequest
{
    [Required]
    public Guid? AccountId { get; set; }
    
    [Required]
    public Guid? TransactionId { get; set; }

    public Entities.TransactionHistory ToTransactionHistory()
    {
        return new Entities.TransactionHistory
        {
            AccountId = AccountId,
            TransactionId = TransactionId
        };
    }
}