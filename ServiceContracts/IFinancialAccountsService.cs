using ServiceContracts.DTO;

namespace ServiceContracts;

public interface IFinancialAccountsService
{
    public FinancialAccountResponse AddFinancialAccount(FinancialAccountAddRequest? financialAccountAddRequest);

    public List<FinancialAccountResponse> GetAllFinancialAccounts();

    public FinancialAccountResponse? GetFinancialAccountById(Guid? financialAccountId, Guid? userId);

    public FinancialAccountResponse? GetFinancialAccountByName(string? financialAccountName, Guid? userId);

    /// <summary>
    /// Returns financial account filtered by certain property
    /// </summary>
    /// <param name="searchString">String to search</param>
    /// <returns></returns>
    public List<FinancialAccountResponse> GetFilteredFinancialAccounts(string? searchString);
    
    
    public List<FinancialAccountResponse> GetSortedFinancialAccounts(List<FinancialAccountResponse> allAccounts,
        string sortBy,
        string sortOrder);
}