using ServiceContracts.DTO;
using ServiceContracts.Enums;

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
    
    /// <summary>
    /// Returns sorted list of financial accounts
    /// </summary>
    /// <param name="allAccounts">List of accounts to sort</param>
    /// <param name="sortBy">Name of the property, based on which financial accounts will be sorted</param>
    /// <param name="sortOrder">Ascend, or Descend (Asc, Desc)</param>
    /// <returns>Sorted list of financial accounts</returns>
    public List<FinancialAccountResponse> GetSortedFinancialAccounts(List<FinancialAccountResponse> allAccounts,
        string sortBy,
        SortOrderOptions sortOrder);
}