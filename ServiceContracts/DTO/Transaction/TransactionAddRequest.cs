using System.ComponentModel.DataAnnotations;
using ServiceContracts.Enums;

namespace ServiceContracts.DTO.Transaction;

public class TransactionAddRequest
{
    [Required]
    public Guid? AccountId { get; set; }
    
    [Required]
    public Guid? CategoryId { get; set; }
    
    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Transaction amount cannot be negative")]
    public decimal? Amount { get; set; }
    
    [Required]
    public DateTime? TransactionDate { get; set; }
    
    [Required]
    public TransactionTypes? TransactionType { get; set; }

    public Entities.Transaction ToTransaction()
    {
        return new Entities.Transaction
        {
            AccountId = AccountId,
            CategoryId = CategoryId,
            Amount = Amount,
            TransactionDate = TransactionDate,
            TransactionType = TransactionType.ToString()
        };
    }
}