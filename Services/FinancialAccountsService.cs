using System.Text.RegularExpressions;
using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class FinancialAccountsService : IFinancialAccountsService
{
    private readonly List<FinancialAccount> _listOfAccounts;

    public FinancialAccountsService()
    {
        _listOfAccounts = new List<FinancialAccount>();
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

        return financialAccount.ToUserFinancialAccountResponse();
    }

    public List<FinancialAccountResponse> GetAllFinancialAccounts()
    {
        return _listOfAccounts.Select(financialAccount => financialAccount.ToUserFinancialAccountResponse()).ToList();
    }

    public FinancialAccountResponse? GetFinancialAccountById(Guid? financialAccountId, Guid? userId)
    {
        return _listOfAccounts.FirstOrDefault(financialAccount => financialAccount.UserId == userId &&
                                                                  financialAccount.AccountId == financialAccountId)?
            .ToUserFinancialAccountResponse();
    }

    public FinancialAccountResponse? GetFinancialAccountByName(string? financialAccountName, Guid? userId)
    {
        return _listOfAccounts.FirstOrDefault(financialAccount => financialAccount.UserId == userId &&
                                                                  financialAccount.AccountName == financialAccountName)?
            .ToUserFinancialAccountResponse();
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
        string sortOrder)
    {
        throw new NotImplementedException();
    }
}