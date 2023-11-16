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
    public bool? IsRecurring { get; set; }
    public DateTime? RecurringDate { get; set; }
    public string? Description { get; set; }
    public bool? IsReminderSet { get; set; }
    public string? SendingMethod { get; set; }
    
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
               TransactionType == transactionToCompare.TransactionType &&
               IsRecurring == transactionToCompare.IsRecurring &&
               RecurringDate == transactionToCompare.RecurringDate &&
               Description == transactionToCompare.Description &&
               IsReminderSet == transactionToCompare.IsReminderSet &&
               SendingMethod == transactionToCompare.SendingMethod;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"TransactionId:{TransactionId}, AccountId:{AccountId}, CategoryId:{CategoryId}, Amount:{Amount}, " +
               $"TransactionDate:{TransactionDate}, TransactionType:{TransactionType}, IsRecurring:{IsRecurring}, " +
               $"RecurringDate:{RecurringDate}, Description:{Description}, IsReminderSet:{IsReminderSet}, " +
               $"SendingMethod:{SendingMethod}";
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
            TransactionType = transaction.TransactionType,
            IsRecurring = transaction.IsRecurring,
            RecurringDate = transaction.RecurringDate,
            Description = transaction.Description,
            IsReminderSet = transaction.IsReminderSet,
            SendingMethod = transaction.SendingMethod
        };
    }
}