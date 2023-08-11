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
    private readonly ICurrenciesService _currenciesService;

    public FinancialAccountsService(bool initialize = true)
    {
        _listOfAccounts = new List<FinancialAccount>();
        _currenciesService = new CurrenciesService();

        if (initialize)
        {
            MockData();
        }
    }

    private void MockData()
    {
        _listOfAccounts.AddRange(new List<FinancialAccount>
        {
            new()
            {
                AccountId = Guid.Parse("6BEED48F-AEF5-4E2E-9225-4C9287174519"),
                AccountName = "MyFirstAcc",
                Balance = 5000,
                CurrencyId = Guid.Parse("57D46BB9-0354-4718-B3E7-5BC28BBB3994"),
                UserId = Guid.Parse("7FC64B0D-089A-4016-BF62-B20B9E4B6531")
            },
            new()
            {
                AccountId = Guid.Parse("050CF386-EFCF-4F39-970A-8165263AD607"),
                AccountName = "Some Dummy Account",
                Balance = 0,
                CurrencyId = Guid.Parse("4749A472-F7FB-4114-9D79-D4CE13A7B9B7"),
                UserId = Guid.Parse("7FC64B0D-089A-4016-BF62-B20B9E4B6531")
            },
            new()
            {
                AccountId = Guid.Parse("627235AC-5D36-4E01-9FD6-A16C53897858"),
                AccountName = "Someone's Account",
                Balance = 10000.49m,
                CurrencyId = Guid.Parse("4749A472-F7FB-4114-9D79-D4CE13A7B9B7"),
                UserId = Guid.Parse("144454D3-19D6-4081-8C21-2FCE41723461")
            },
            new()
            {
                AccountId = Guid.Parse("FCDCD45A-AC70-47DC-B037-F7574057D736"),
                AccountName = "Fake account",
                Balance = 99999.99m,
                CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE"),
                UserId = Guid.Parse("82177757-4F2C-4877-80B3-5CBF4CF49E9A")
            },
            new()
            {
                AccountId = Guid.Parse("00D6086A-0CAA-4A70-80D2-15CC2C91F245"),
                AccountName = "Abcdef123",
                Balance = 1.1m,
                CurrencyId = Guid.Parse("57D46BB9-0354-4718-B3E7-5BC28BBB3994"),
                UserId = Guid.Parse("DFE23AA9-7C09-4B3A-BD10-87D38FDD809B")
            },
            new()
            {
                AccountId = Guid.Parse("9CF303F3-09D2-4DC2-AE42-675932B3D565"),
                AccountName = "Blablablabla",
                Balance = 111111111111,
                CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE"),
                UserId = Guid.Parse("82177757-4F2C-4877-80B3-5CBF4CF49E9A")
            },
            new()
            {
                AccountId = Guid.Parse("E98B6AAE-41E6-4C81-A4F8-638ECE825BB2"),
                AccountName = "HahaHihiHehe",
                Balance = 12345.67m,
                CurrencyId = Guid.Parse("4749A472-F7FB-4114-9D79-D4CE13A7B9B7"),
                UserId = Guid.Parse("DFE23AA9-7C09-4B3A-BD10-87D38FDD809B")
            },
            new()
            {
                AccountId = Guid.Parse("4D1E5171-C378-4B90-8448-0B14DFA90963"),
                AccountName = "Zaczyna brakować mi pomysłów",
                Balance = 0.50m,
                CurrencyId = Guid.Parse("57D46BB9-0354-4718-B3E7-5BC28BBB3994"),
                UserId = Guid.Parse("A5282568-7CE5-4114-A1E5-3ABA38750D2E")
            },
            new()
            {
                AccountId = Guid.Parse("C6B974AE-FAD8-440D-9BB0-BBA38E4BA79E"),
                AccountName = "Łostatnie konto",
                Balance = 800.90m,
                CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE"),
                UserId = Guid.Parse("71B3FCB5-1C86-418E-93BD-E6148153A130")
            }
        });
    }
    
    private FinancialAccountResponse ConvertFinancialAccountToFinancialAccountResponse(FinancialAccount financialAccount)
    {
        var financialAccountResponse = financialAccount.ToFinancialAccountResponse();
        
        financialAccountResponse.CurrencyName = _currenciesService.GetCurrencyByCurrencyId(financialAccount.CurrencyId)?.CurrencyName;

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