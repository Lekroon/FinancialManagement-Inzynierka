using ServiceContracts.DTO.Transaction;
using ServiceContracts.Enums;

namespace ServiceContracts;

public interface ITransactionsService
{
    public TransactionResponse AddTransaction(TransactionAddRequest? transactionAddRequest);

    public List<TransactionResponse> GetAllTransactions();

    public TransactionResponse? GetTransactionById(Guid? id);

    public bool DeleteTransaction(Guid? id);

    public List<TransactionResponse> GetFilteredTransactions(string? searchBy, string? searchString);

    public List<TransactionResponse> GetSortedTransactions(List<TransactionResponse> allTransactions,
        string sortBy, SortOrderOptions sortOrder);

    public List<TransactionResponse> GetTransactionsByCategory(string? category);
    
    public List<TransactionResponse> GetTransactionsByType(TransactionTypes? type);

    public List<TransactionResponse> GetCyclicalTransactions(); // ?????????????

    public TransactionResponse UpdateTransaction(TransactionUpdateRequest transactionUpdateRequest);
}