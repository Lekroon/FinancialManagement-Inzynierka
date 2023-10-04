namespace ServiceContracts.DTO.Transfer;

public class TransferResponse
{
    public Guid TransferId { get; set; }
    public Guid? SenderAccountId { get; set; }
    public Guid? RecipientAccountId { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (obj.GetType() != typeof(TransferResponse))
            return false;

        var transferToCompare = (TransferResponse)obj;

        return TransferId == transferToCompare.TransferId &&
               SenderAccountId == transferToCompare.SenderAccountId &&
               RecipientAccountId == transferToCompare.RecipientAccountId &&
               Amount == transferToCompare.Amount &&
               TransactionDate == transferToCompare.TransactionDate;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"TransferId:{TransferId}, SenderId:{SenderAccountId}, RecipientId:{RecipientAccountId}, " +
               $"Amount:{Amount}, TransactionDate:{TransactionDate}";
    }
}

public static class TransferExtensions
{
    public static TransferResponse ToTransferResponse(this Entities.Transfer transfer)
    {
        return new TransferResponse
        {
            TransferId = transfer.TransferId,
            SenderAccountId = transfer.SenderAccountId,
            RecipientAccountId = transfer.RecipientAccountId,
            Amount = transfer.Amount,
            TransactionDate = transfer.TransactionDate
        };
    }
}