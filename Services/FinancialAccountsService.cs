using System.Text.RegularExpressions;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.FinancialAccount;
using ServiceContracts.Enums;
using Services.Helpers;

namespace Services;

public class FinancialAccountsService : IFinancialAccountsService
{
    private readonly List<FinancialAccount> _listOfAccounts;
    private readonly CurrenciesService _currenciesService;
    private readonly UsersService _usersService;

    public FinancialAccountsService()
    {
        _listOfAccounts = new List<FinancialAccount>();
        _currenciesService = new CurrenciesService();
        _usersService = new UsersService();
    }

    private FinancialAccountResponse ConvertFinancialAccountToFinancialAccountResponse(FinancialAccount financialAccount)
    {
        FinancialAccountResponse financialAccountResponse = financialAccount.ToFinancialAccountResponse();
        
        financialAccountResponse.CurrencyName = _currenciesService.GetCurrencyByCurrencyId(financialAccount.CurrencyId)?.CurrencyName;
        financialAccountResponse.UserLogin = _usersService.GetUserByUserId(financialAccount.UserId)?.Login;
        
        return financialAccountResponse;
    }

    public FinancialAccountResponse AddFinancialAccount(FinancialAccountAddRequest? financialAccountAddRequest)
    {
        // Argument cannot be null
        if (financialAccountAddRequest == null)
        {
            throw new ArgumentNullException(nameof(financialAccountAddRequest));
        }
        
        // Model validation
        ValidationHelper.ModelValidation(financialAccountAddRequest);
        
        // Converting types
        var financialAccount = financialAccountAddRequest.ToFinancialAccount();

        // Account name cannot be duplicated 
        if (_listOfAccounts.Any(financialAccountInList => financialAccountInList.UserId == financialAccount.UserId &&
                                                          financialAccountInList.AccountName == financialAccount.AccountName))
        {
            throw new ArgumentException("Account name is already in use");
        }
        
        // If balance have more than 2 decimal places it have to be rounded
        var balance = financialAccount.Balance;

        if (balance == null)
        {
            throw new NullReferenceException("Balance cannot be null");
        }
        
        if (!Regex.IsMatch(balance.ToString()!, @"^\d+(\.\d{2})?$"))
        {
            var roundedNumber = Math.Round((decimal)balance, 2).ToString("0.00");
            financialAccount.Balance = Convert.ToDecimal(roundedNumber);
        }
        
        // Generating new id
        financialAccount.AccountId = Guid.NewGuid();

        _listOfAccounts.Add(financialAccount);

        return ConvertFinancialAccountToFinancialAccountResponse(financialAccount);
    }

    public List<FinancialAccountResponse> GetAllFinancialAccounts()
    {
        return _listOfAccounts.Select(financialAccount => financialAccount.ToFinancialAccountResponse()).ToList();
    }

    public FinancialAccountResponse? GetFinancialAccountById(Guid? financialAccountId)
    {
        return _listOfAccounts.FirstOrDefault(financialAccount => financialAccount.AccountId == financialAccountId)?
            .ToFinancialAccountResponse();
    }

    public FinancialAccountResponse? GetFinancialAccountByName(string? financialAccountName, Guid? userId)
    {
        return _listOfAccounts.FirstOrDefault(financialAccount => financialAccount.UserId == userId &&
                                                                  financialAccount.AccountName == financialAccountName)?
            .ToFinancialAccountResponse();
    }

    public List<FinancialAccountResponse> GetFilteredFinancialAccounts(string? searchString)
    {
        /*
         * 1. Check searchString is empty (returns all accounts if null or empty)
         * 2. Get matching accounts based on AccountName property and searchString argument
         * 3. Convert matching accounts from FinancialAccount type to FinancialAccountResponse
         * 4. Return all matching financial accounts (type: FinancialAccountResponse)
         */

        var allFinancialAccountsList = GetAllFinancialAccounts();

        if (string.IsNullOrEmpty(searchString))
        {
            return allFinancialAccountsList;
        }

        return allFinancialAccountsList
            .Where(account => account.AccountName != null && account.AccountName.Contains(searchString, 
                StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public List<FinancialAccountResponse> GetSortedFinancialAccounts(List<FinancialAccountResponse> allAccounts, 
        string sortBy, 
        SortOrderOptions sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortBy))
        {
            return allAccounts;
        }
        
        var sortedFinancialAccounts = (sortBy, sortOrder) switch
        {
            (nameof(FinancialAccountResponse.AccountName), SortOrderOptions.Asc) =>
                allAccounts.OrderBy(account => account.AccountName, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(FinancialAccountResponse.AccountName), SortOrderOptions.Desc) =>
                allAccounts.OrderByDescending(account => account.AccountName, StringComparer.OrdinalIgnoreCase)
                    .ToList(),
        
            (nameof(FinancialAccountResponse.Balance), SortOrderOptions.Asc) =>
                allAccounts.OrderBy(account => account.Balance).ToList(),
            (nameof(FinancialAccountResponse.Balance), SortOrderOptions.Desc) =>
                allAccounts.OrderByDescending(account => account.Balance).ToList(),

            (nameof(FinancialAccountResponse.CurrencyName), SortOrderOptions.Asc) =>
                allAccounts.OrderBy(account => account.CurrencyName, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(FinancialAccountResponse.CurrencyName), SortOrderOptions.Desc) =>
                allAccounts.OrderByDescending(account => account.CurrencyName, StringComparer.OrdinalIgnoreCase).ToList(),
            
            (nameof(FinancialAccountResponse.UserLogin), SortOrderOptions.Asc) =>
                allAccounts.OrderBy(account => account.UserLogin, StringComparer.OrdinalIgnoreCase).ToList(),
            (nameof(FinancialAccountResponse.UserLogin), SortOrderOptions.Desc) =>
                allAccounts.OrderByDescending(account => account.UserLogin, StringComparer.OrdinalIgnoreCase).ToList(),
            
            _ => allAccounts
        };

        return sortedFinancialAccounts;
    }

    public FinancialAccountResponse UpdateFinancialAccount(FinancialAccountUpdateRequest? accountUpdateRequest)
    {
        if (accountUpdateRequest == null)
        {
            throw new ArgumentNullException(nameof(accountUpdateRequest));
        }
        
        ValidationHelper.ModelValidation(accountUpdateRequest);
        
        // financial account object to update
        var matchingAccount = _listOfAccounts.FirstOrDefault(account => account.AccountId == accountUpdateRequest.AccountId);

        if (matchingAccount == null)
        {
            throw new ArgumentException("Given financial account ID doesn't exist");
        }
        
        // update financial account
        matchingAccount.AccountName = accountUpdateRequest.AccountName;
        matchingAccount.Balance = accountUpdateRequest.Balance;

        return matchingAccount.ToFinancialAccountResponse();
    }

    public bool DeleteFinancialAccount(Guid? financialAccountId)
    {
        if (financialAccountId == null)
        {
            throw new ArgumentNullException(nameof(financialAccountId));
        }

        var foundAccount = _listOfAccounts.FirstOrDefault(account => account.AccountId == financialAccountId);

        if (foundAccount == null)
            return false;

        _listOfAccounts.RemoveAll(account => account.AccountId == financialAccountId);

        return true;
    }
}