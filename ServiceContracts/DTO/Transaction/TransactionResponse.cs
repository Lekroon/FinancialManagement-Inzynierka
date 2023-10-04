using ServiceContracts.Enums;

namespace ServiceContracts.DTO.Transaction;

public class TransactionResponse
{
    public Guid TransactionId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? CategoryId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public string? TransactionType { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(TransactionResponse))
            return false;

        var transactionToCompare = (TransactionResponse)obj;

        return TransactionId == transactionToCompare.TransactionId &&
               AccountId == transactionToCompare.AccountId &&
               CategoryId == transactionToCompare.CategoryId &&
               Amount == transactionToCompare.Amount &&
               TransactionDate == transactionToCompare.TransactionDate &&
               TransactionType == transactionToCompare.TransactionType;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"TransactionId:{TransactionId}, AccountId:{AccountId}, CategoryId:{CategoryId}, Amount:{Amount}," +
               $"TransactionDate:{TransactionDate}, TransactionType:{TransactionType}";
    }
}

public static class TransactionExtensions
{
    public static TransactionResponse ToTransactionResponse(this Entities.Transaction transaction)
    {
        return new TransactionResponse
        {
            TransactionId = transaction.TransactionId,
            AccountId = transaction.AccountId,
            CategoryId = transaction.CategoryId,
            Amount = transaction.Amount,
            TransactionDate = transaction.TransactionDate,
            TransactionType = transaction.TransactionType
        };
    }
}