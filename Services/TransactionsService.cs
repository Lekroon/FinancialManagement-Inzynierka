using ServiceContracts;
using ServiceContracts.DTO.Transaction;
using ServiceContracts.Enums;

namespace Services;

public class TransactionsService : ITransactionsService
{
    public TransactionResponse AddTransaction(TransactionAddRequest? transactionAddRequest)
    {
        throw new NotImplementedException();
    }

    public List<TransactionResponse> GetAllTransactions()
    {
        throw new NotImplementedException();
    }

    public TransactionResponse? GetTransactionById(Guid? id)
    {
        throw new NotImplementedException();
    }

    public bool DeleteTransaction(Guid? id)
    {
        throw new NotImplementedException();
    }

    public List<TransactionResponse> GetFilteredTransactions(string? searchBy, string? searchString)
    {
        throw new NotImplementedException();
    }

    public List<TransactionResponse> GetSortedTransactions(List<TransactionResponse> allTransactions, string sortBy, SortOrderOptions sortOrder)
    {
        throw new NotImplementedException();
    }

    public List<TransactionResponse> GetTransactionsByCategory(Guid? categoryId)
    {
        throw new NotImplementedException();
    }

    public List<TransactionResponse> GetTransactionsByType(TransactionTypes? type)
    {
        throw new NotImplementedException();
    }

    public TransactionResponse UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest)
    {
        throw new NotImplementedException();
    }
}