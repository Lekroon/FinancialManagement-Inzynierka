namespace ServiceContracts.DTO.TransactionHistory;

public class TransactionHistoryResponse
{
    public Guid HistoryId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? TransactionId { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(TransactionHistoryResponse))
            return false;

        var transactionHistoryToCompare = (TransactionHistoryResponse)obj;

        return HistoryId == transactionHistoryToCompare.HistoryId &&
               AccountId == transactionHistoryToCompare.AccountId &&
               TransactionId == transactionHistoryToCompare.TransactionId;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"HistoryId:{HistoryId}, AccountId:{AccountId}, TransactionId:{TransactionId}";
    }
}

public static class FinancialAccountExtensions
{
    public static TransactionHistoryResponse ToTransactionHistoryResponse(this Entities.TransactionHistory history)
    {
        return new TransactionHistoryResponse
        {
            HistoryId = history.HistoryId,
            AccountId = history.AccountId,
            TransactionId = history.TransactionId
        };
    }
}