using ServiceContracts.DTO.Country;
using ServiceContracts.DTO.Currency;
using ServiceContracts.DTO.FinancialAccount;
using ServiceContracts.DTO.User;
using ServiceContracts.Enums;
using Xunit.Abstractions;

namespace UnitTests;

public class FinancialAccountsServiceTests
{
    private readonly ICurrenciesService _currenciesService;
    private readonly ICountriesService _countriesService;
    private readonly IUsersService _usersService;
    private readonly IFinancialAccountsService _financialAccountsService;
    private readonly ITestOutputHelper _testOutputHelper;

    public FinancialAccountsServiceTests(ITestOutputHelper testOutputHelper)
    {
        _currenciesService = new CurrenciesService(false);
        _countriesService = new CountriesService(false);
        _usersService = new UsersService(false);
        _financialAccountsService = new FinancialAccountsService(false);
        
        _testOutputHelper = testOutputHelper;
    }

    #region AddFinancialAccount

    /*
     * Test requirements:
     * 1. When FinancialAccountAddRequest is null, it should throw ArgumentNullException
     * 2. When required properties are null, it should throw ArgumentException
     * 3. When logged user want to add account with duplicated name, it should throw ArgumentException
     * 4. When given balance is negative, it should throw ArgumentException
     * 5. When given balance have more than 2 decimal places, it should be rounded
     * 6. When financial account is added properly, it should add this object to list
     * 7. When two different users have account with the same name, it should add them to list
     */

    // 1. When FinancialAccountAddRequest is null
    [Fact]
    public void AddFiancialAccount_FinancialAccountAddRequestIsNull()
    {
        // Arrange
        FinancialAccountAddRequest? financialAccountAddRequest = null;
        
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequest);
        });
    }
    
    // 2. When required properties are null
    [Fact]
    public void AddFinancialAccount_PropertiesAreNull()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };

        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);
        
        var financialAccountAddRequest = new FinancialAccountAddRequest
        {
            UserId = null,
            CurrencyId = currencyResponse.CurrencyId,
            AccountName = "SomeName",
            Balance = 200.15m
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var response = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest);
            _testOutputHelper.WriteLine(response.ToString());
        });
    }
    
    // 3. When account name is duplicated
    [Fact]
    public void AddFinancialAccount_AccountNameIsDuplicated()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "testowy@gmail.com",
            IsActive = true,
            Login = "MyLogin123",
            Password = "Abcdefghahasgara",
            PhoneNumber = "553123122"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var financialAccountAddRequest1 = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 300.12m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };

        var financialAccountAddRequest2 = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 306.12m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequest1);
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequest2);
        });
    }

    // 4. Given balance is negative
    [Fact]
    public void AddFinancialAccount_BalanceIsNegative()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Germany"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "testowy@gmail.com",
            IsActive = true,
            Login = "MyLogin123",
            Password = "Abcdefghahasgara",
            PhoneNumber = "553123122"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var financialAccountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = -100.10m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        
        // Assert 
        Assert.Throws<ArgumentException>(() =>
        {
            var response = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest);
            _testOutputHelper.WriteLine(response.ToString());
        });
    }
    
    // 5. Given balance have more than 2 decimal places
    [Fact]
    public void AddFinancialAccount_MoreThanTwoDecimalPlaces()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "testowy@gmail.com",
            IsActive = true,
            Login = "MyLogin123",
            Password = "Abcdefghahasgara",
            PhoneNumber = "553123122"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var financialAccountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 123.456m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };

        // Act
        var response = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest);
        var expectedValue = Math.Round((decimal)financialAccountAddRequest.Balance, 2);
        
        _testOutputHelper.WriteLine($"Request value: {financialAccountAddRequest.Balance}\n\n");
        _testOutputHelper.WriteLine($"Response value: {response}\n\n");
        _testOutputHelper.WriteLine($"Expected value: {expectedValue}\n\n");
        
        // Assert
        Assert.Equal(expectedValue, response.Balance);
    }
    
    // 6. Financial account added properly
    [Fact]
    public void AddFinancialAccount_FinancialAccountAddedProperly()
    {
// Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "testowy@gmail.com",
            IsActive = true,
            Login = "MyLogin123",
            Password = "Abcdefghahasgara",
            PhoneNumber = "553123122"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var financialAccountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 300.12m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        
        // Act
        var financialAccountResponse = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest);
        var listOfAccounts = _financialAccountsService.GetAllFinancialAccounts();
        
        _testOutputHelper.WriteLine($"Currency: {currencyResponse}");
        _testOutputHelper.WriteLine($"\nCountry: {countryResponse}");
        _testOutputHelper.WriteLine($"\nUser: {userResponse}");
        _testOutputHelper.WriteLine($"\nFinancialAccount: {financialAccountResponse}");
        
        // Assert
        Assert.True(financialAccountResponse.AccountId != Guid.Empty);
        Assert.Contains(financialAccountResponse, listOfAccounts);
    }
    
    // 7. Different users have account with the same name
    [Fact]
    public void AddFinancialAccount_DifferentUsersWithTheSameAccountName()
    {
        // Arrange
        var currencyAddRequest1 = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse1 = _currenciesService.AddCurrency(currencyAddRequest1);

        var currencyAddRequest2 = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };
        var currencyResponse2 = _currenciesService.AddCurrency(currencyAddRequest2);

        var countryAddRequest1 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse1.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);

        
        var countryAddRequest2 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse2.CurrencyId,
            CountryName = "Germany"
        };
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
        
        var userAddRequest1 = new UserAddRequest
        {
            CountryId = countryResponse1.CountryId,
            Email = "testowy@gmail.com",
            IsActive = true,
            Login = "MyLogin123",
            Password = "Abcdefghahasgara",
            PhoneNumber = "553123122"
        };
        var userResponse1 = _usersService.AddUser(userAddRequest1);

        var userAddRequest2 = new UserAddRequest
        {
            CountryId = countryResponse2.CountryId,
            Email = "inny@gmail.com",
            IsActive = true,
            Login = "DrugiUser",
            Password = "ĄĘźć12345",
            PhoneNumber = "123456789"
        };
        var userResponse2 = _usersService.AddUser(userAddRequest2);
        
        var financialAccountAddRequest1 = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 300.123m,
            CurrencyId = currencyResponse1.CurrencyId,
            UserId = userResponse1.UserId
        };

        var financialAccountAddRequest2 = new FinancialAccountAddRequest
        {
            AccountName = "Testowe",
            Balance = 500.99m,
            CurrencyId = currencyResponse2.CurrencyId,
            UserId = userResponse2.UserId
        };
        
        // Act
        var financialAccountResponse1 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest1);
        var financialAccountResponse2 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest2);
        
        var listOfAccounts = _financialAccountsService.GetAllFinancialAccounts();
        
        _testOutputHelper.WriteLine($"First Currency: {currencyResponse1}");
        _testOutputHelper.WriteLine($"\nFirst Country: {countryResponse1}");
        _testOutputHelper.WriteLine($"\nFirst User: {userResponse1}");
        _testOutputHelper.WriteLine($"\nFirst FinancialAccount: {financialAccountResponse1}");

        _testOutputHelper.WriteLine($"\n\nSecond Currency: {currencyResponse2}");
        _testOutputHelper.WriteLine($"\nSecond Country: {countryResponse2}");
        _testOutputHelper.WriteLine($"\nSecond User: {userResponse2}");
        _testOutputHelper.WriteLine($"\nSecond FinancialAccount: {financialAccountResponse2}");

        // Assert
        Assert.True(financialAccountResponse1.AccountId != Guid.Empty);
        Assert.True(financialAccountResponse2.AccountId != Guid.Empty);
        
        Assert.Contains(financialAccountResponse1, listOfAccounts);
        Assert.Contains(financialAccountResponse2, listOfAccounts);
    }
    
    #endregion
    
    #region GetAllFinancialAccounts

    /*
     * Test requirements:
     * 1. Without adding any accounts, list should be empty by default
     * 2. It should return every properly added account
     */

    // 1. List should be empty by default
    [Fact]
    public void GetAllFinancialAccounts_ListShouldBeEmpty()
    {
        // Acts
        var responseList = _financialAccountsService.GetAllFinancialAccounts();
        
        // Assert
        Assert.Empty(responseList);
    }
    
    // 2. It should return every added account
    [Fact]
    public void GetAllFinancialAccounts_ReturnEveryAccount()
    {
        // Arrange
        
        // createing currencies
        var currencyAddRequest1 = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyAddRequest2 = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };
        var currencyAddRequest3 = new CurrencyAddRequest
        {
            CurrencyName = "USD"
        };

        var currencyResponse1 = _currenciesService.AddCurrency(currencyAddRequest1);
        var currencyResponse2 = _currenciesService.AddCurrency(currencyAddRequest2);
        var currencyResponse3 = _currenciesService.AddCurrency(currencyAddRequest3);
        
        // creating countries
        var countryAddRequest1 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse1.CurrencyId,
            CountryName = "Poland"
        };
        var countryAddRequest2 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse2.CurrencyId,
            CountryName = "Germany"
        };
        var countryAddRequest3 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse3.CurrencyId,
            CountryName = "United States"
        };

        var countryResponse1 = _countriesService.AddCountry(countryAddRequest1);
        var countryResponse2 = _countriesService.AddCountry(countryAddRequest2);
        var countryResponse3 = _countriesService.AddCountry(countryAddRequest3);
        
        // creating users
        var userAddRequest1 = new UserAddRequest
        {
            CountryId = countryResponse1.CountryId,
            Email = "RandomEmail@gmail.com",
            IsActive = true,
            Login = "FirstCreatedUser",
            Password = "LosoweHaselko123",
            PhoneNumber = "123456789"
        };
        var userAddRequest2 = new UserAddRequest
        {
            CountryId = countryResponse2.CountryId,
            Email = "SomeDummyMail@gmail.com",
            IsActive = false,
            Login = "DrugiUzytkonwikLosowyyyyy",
            Password = "MyFavPass12345",
            PhoneNumber = "987654321"
        };
        var userAddRequest3 = new UserAddRequest
        {
            CountryId = countryResponse3.CountryId,
            Email = "NotMyEmail@op.pl",
            IsActive = true,
            Login = "Aaaaaabcdef",
            Password = "PasswordBlablabla",
            PhoneNumber = "123987564"
        };

        var userResponse1 = _usersService.AddUser(userAddRequest1);
        var userResponse2 = _usersService.AddUser(userAddRequest2);
        var userResponse3 = _usersService.AddUser(userAddRequest3);
        
        // creating financial accounts
        var financialAccountAddRequest1 = new FinancialAccountAddRequest
        {
            AccountName = "SomeRandomName",
            Balance = 2000,
            CurrencyId = currencyResponse1.CurrencyId,
            UserId = userResponse1.UserId
        };
        var financialAccountAddRequest2 = new FinancialAccountAddRequest
        {
            AccountName = "DifferentName",
            Balance = 300.155m,
            CurrencyId = currencyResponse2.CurrencyId,
            UserId = userResponse2.UserId
        };
        var financialAccountAddRequest3 = new FinancialAccountAddRequest
        {
            AccountName = "SomeRandomName",
            Balance = 500.20m,
            CurrencyId = currencyResponse3.CurrencyId,
            UserId = userResponse3.UserId
        };
        var financialAccountAddRequest4 = new FinancialAccountAddRequest
        {
            AccountName = "YetDifferentName",
            Balance = 5000.5m,
            CurrencyId = currencyResponse2.CurrencyId,
            UserId = userResponse2.UserId
        };
        var financialAccountAddRequest5 = new FinancialAccountAddRequest
        {
            AccountName = "SomeVeryRandomName",
            Balance = 999.99m,
            CurrencyId = currencyResponse3.CurrencyId,
            UserId = userResponse3.UserId
        };

        var financialAccountResponse1 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest1);
        var financialAccountResponse2 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest2);
        var financialAccountResponse3 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest3);
        var financialAccountResponse4 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest4);
        var financialAccountResponse5 = _financialAccountsService.AddFinancialAccount(financialAccountAddRequest5);

        var financialAccountResponseList = new List<FinancialAccountResponse>
        {
            financialAccountResponse1,
            financialAccountResponse2,
            financialAccountResponse3,
            financialAccountResponse4,
            financialAccountResponse5,
        };
        
        // creating lists
        var currenciesList = _currenciesService.GetAllCurrencies();
        var countriesList = _countriesService.GetAllCountries();
        var usersList = _usersService.GetAllUsers();
        var financialAccountsList = _financialAccountsService.GetAllFinancialAccounts();
        
        _testOutputHelper.WriteLine("Currencies:");
        foreach (var currency in currenciesList)
        {
            _testOutputHelper.WriteLine("\n" + currency);
        }
        
        _testOutputHelper.WriteLine("Countries:");
        foreach (var country in countriesList)
        {
            _testOutputHelper.WriteLine("\n" + country);
        }
        
        _testOutputHelper.WriteLine("Users:");
        foreach (var user in usersList)
        {
            _testOutputHelper.WriteLine("\n" + user);
        }
        
        _testOutputHelper.WriteLine("\n\nFinancial Accounts:");        
        foreach (var account in financialAccountsList)
        {
            _testOutputHelper.WriteLine("\n" + account);
        }
        
        foreach (var expectedAccount in financialAccountsList)
        {
            Assert.Contains(expectedAccount, financialAccountResponseList);
        }
    }
    
    #endregion

    #region GetFilteredFinancialAccounts

    /*
     * Test requirements:
     * 1. If search string is empty and searchBy is "AccountName", it returns all financial accounts.
     * 2. It should return matching accounts based on given account name (searchBy) and search string
     */
    
    [Fact]
    public void GetFilteredFinancialAccounts_EmptySearchString()
    {
        // Arrange
        GenerateFinancialAccounts();
            
        // getting all values
        var createdCurrencies = _currenciesService.GetAllCurrencies();
        var createdCountries = _countriesService.GetAllCountries();
        var createdUsers = _usersService.GetAllUsers();
        var createdFinancialAccounts = _financialAccountsService.GetAllFinancialAccounts();
        
        _testOutputHelper.WriteLine("Created objects:\n");
        
        _testOutputHelper.WriteLine("Currencies:");
        foreach (var currency in createdCurrencies)
        {
            _testOutputHelper.WriteLine(currency.ToString());
        }
        
        _testOutputHelper.WriteLine("\nCountries:");
        foreach (var country in createdCountries)
        {
            _testOutputHelper.WriteLine(country + "\n");
        }
        
        _testOutputHelper.WriteLine("\nUsers:");
        foreach (var user in createdUsers)
        {
            _testOutputHelper.WriteLine(user + "\n");
        }
        
        _testOutputHelper.WriteLine("\nFinancial accounts:");
        foreach (var financialAccounts in createdFinancialAccounts)
        {
            _testOutputHelper.WriteLine(financialAccounts + "\n");
        }
        
        _testOutputHelper.WriteLine("---------------------------------------------------------");
        _testOutputHelper.WriteLine("Expected:");
        foreach (var financialAccount in createdFinancialAccounts)
        {
            if (financialAccount.AccountName == null)
            {
                return;
            }
            
            if (financialAccount.AccountName.Contains("", StringComparison.OrdinalIgnoreCase))
            {
                _testOutputHelper.WriteLine("Account name: " + financialAccount.AccountName);
            }
        }
        
        // Act
        var foundFinancialAccounts = _financialAccountsService.GetFilteredFinancialAccounts("");
        
        _testOutputHelper.WriteLine("\nActual:");
        foreach (var financialAccount in foundFinancialAccounts)
        {
            if (financialAccount.AccountName == null)
            {
                return;
            }
            
            if (financialAccount.AccountName.Contains("", StringComparison.OrdinalIgnoreCase))
            {
                _testOutputHelper.WriteLine("Account name: " + financialAccount.AccountName);
            }
        }
        
        // Assert
        foreach (var financialAccount in foundFinancialAccounts)
        {
            Assert.Contains(financialAccount, foundFinancialAccounts);
        }
    }

    [Fact]
    public void GetFilteredFinancialAccounts_SearchByAccountName()
    {
        // Arrange
        GenerateFinancialAccounts();
        
        // created values
        var createdCurrencies = _currenciesService.GetAllCurrencies();
        var createdCountries = _countriesService.GetAllCountries();
        var createdUsers = _usersService.GetAllUsers();
        var createdFinancialAccounts = _financialAccountsService.GetAllFinancialAccounts();
        
        _testOutputHelper.WriteLine("Created objects:\n");
        
        _testOutputHelper.WriteLine("Currencies:");
        foreach (var currency in createdCurrencies)
        {
            _testOutputHelper.WriteLine(currency.ToString());
        }
        
        _testOutputHelper.WriteLine("\nCountries:");
        foreach (var country in createdCountries)
        {
            _testOutputHelper.WriteLine(country + "\n");
        }
        
        _testOutputHelper.WriteLine("\nUsers:");
        foreach (var user in createdUsers)
        {
            _testOutputHelper.WriteLine(user + "\n");
        }
        
        _testOutputHelper.WriteLine("\nFinancial accounts:");
        foreach (var financialAccounts in createdFinancialAccounts)
        {
            _testOutputHelper.WriteLine(financialAccounts + "\n");
        }
        
        _testOutputHelper.WriteLine("---------------------------------------------------------");
        _testOutputHelper.WriteLine("Expected:");
        foreach (var financialAccount in createdFinancialAccounts)
        {
            if (financialAccount.AccountName == null)
            {
                return;
            }
            
            if (financialAccount.AccountName.Contains("my", StringComparison.OrdinalIgnoreCase))
            {
                _testOutputHelper.WriteLine("Account name: " + financialAccount.AccountName);
            }
        }
        
        // Act
        var filteredFinancialAccounts =
            _financialAccountsService.GetFilteredFinancialAccounts( "my");

        // Assert
        _testOutputHelper.WriteLine("\nActual:");
        foreach (var financialAccount in filteredFinancialAccounts)
        {
            _testOutputHelper.WriteLine("Account name: " + financialAccount.AccountName);
        }
        
        foreach (var financialAccount in createdFinancialAccounts)
        {
            if (financialAccount.AccountName == null)
            {
                return;
            }
            
            if (financialAccount.AccountName.Contains("my", StringComparison.OrdinalIgnoreCase))
            {
                Assert.Contains(financialAccount, filteredFinancialAccounts);
            }
        }
    }    
    
    #endregion
    
    #region GetSortedFinancialAccounts
    
    // When sorted based on eg. financials account balance ascending, it should return sorted financial accounts
    [Fact]
    public void GetSortedFinancialAccounts()
    {
        // Arrange
        GenerateFinancialAccounts();
        var generatedFinancialAccounts = _financialAccountsService.GetAllFinancialAccounts();
        
        // Act
        var sortedFinancialAccounts = _financialAccountsService.GetSortedFinancialAccounts(
            generatedFinancialAccounts,
            nameof(FinancialAccount.AccountName),
            SortOrderOptions.Asc);
        
        _testOutputHelper.WriteLine("Created financial accounts:\n");
        foreach (var financialAccount in generatedFinancialAccounts)
        {
            _testOutputHelper.WriteLine(financialAccount + "\n");
        }
        
        _testOutputHelper.WriteLine("\nExpected sorted financial accounts:\n");
        
        var expectedSortedFinancialAccounts = generatedFinancialAccounts
            .OrderBy(account => account.AccountName)
            .ToList();
        
        foreach (var expectedSortedAccount in expectedSortedFinancialAccounts)
        {
            _testOutputHelper.WriteLine(expectedSortedAccount + "\n");
        }
        
        _testOutputHelper.WriteLine("---------------------------------------------------------");

        _testOutputHelper.WriteLine("Sorted financial accounts:\n");
        foreach (var sortedFinancialAccount in sortedFinancialAccounts)
        {
            _testOutputHelper.WriteLine(sortedFinancialAccount + "\n");
        }
        
        // Assert
        for (var i = 0; i < expectedSortedFinancialAccounts.Count; i++)
        {
            Assert.Equal(expectedSortedFinancialAccounts[i], sortedFinancialAccounts[i]);
        }
    }
    
    #endregion

    #region UpdateFinancialAccount

    /*
     * Test requirements:
     * 1. When FinancialAccountUpdateRequest is null, it should throw ArgumentNullException
     * 2. When AccountId is null or invalid, it should throw ArgumentException
     * 3. When AccountName is null or invalid, it should throw ArgumentException
     * 4. When Balance is null or invalid, it should throw ArgumentException
     */
    
    // 1. FinancialAccountUpdateRequest is null
    [Fact]
    public void UpdateFinancialAccount_FinancialAccountUpdateRequestIsNull()
    {
        // Arrange
        FinancialAccountUpdateRequest? accountUpdateRequest = null;
        
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            _financialAccountsService.UpdateFinancialAccount(accountUpdateRequest);
        });
    }
    
    // 2. AccountId is null or invalid
    [Fact]
    public void UpdateFinancialAccount_AccountIdIsInvalid()
    {
        // Arrange
        var accountUpdateRequest = new FinancialAccountUpdateRequest
        {
            AccountId = Guid.NewGuid(),
            AccountName = "Upd4ted AccountName"
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _financialAccountsService.UpdateFinancialAccount(accountUpdateRequest);
        });
    }
    
    // 3. AccountName is null or invalid
    [Fact]
    public void UpdateFinancialAccount_AccountNameIsNullOrInvalid()
    {
        // Arrange
        // creating account
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "jakistamMail@gmail.com",
            IsActive = true,
            Login = "MojLogin123!",
            Password = "123456789"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var accountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Nazwa przed modyfikacją",
            Balance = 1000.50m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        var accountResponse = _financialAccountsService.AddFinancialAccount(accountAddRequest);
        
        _testOutputHelper.WriteLine("GENERATED OBJECTS:");
        _testOutputHelper.WriteLine("1. CURRENCY:\n" + currencyResponse);
        _testOutputHelper.WriteLine("2. COUNTRY:\n" + countryResponse);
        _testOutputHelper.WriteLine("3. USER:\n" + userResponse);
        _testOutputHelper.WriteLine("4. FINANCIAL ACCOUNT:\n" + accountResponse);
        
        // updating account
        var accountUpdateRequest = accountResponse.ToFinancialAccountUpdateRequest();
        
        _testOutputHelper.WriteLine("--------------------------------------------");
        _testOutputHelper.WriteLine("UPDATE REQUEST:");
        _testOutputHelper.WriteLine($"Account ID: {accountUpdateRequest.AccountId}\n" +
                                    $"Account name: {accountUpdateRequest.AccountName}\n" +
                                    $"Balance: {accountUpdateRequest.Balance}\n");

        // more than 60 characters
        accountUpdateRequest.AccountName = "0123456789012345678901234567890123456789012345678901234567890123456789";
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _financialAccountsService.UpdateFinancialAccount(accountUpdateRequest);
        });
    }
    
    // 4. Balance is null or invalid
    [Fact]
    public void UpdateFinancialAccount_BalanceIsNullOrInvalid()
    {
        // Arrange
        // creating account
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Germany"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "myMainMail@gmail.com",
            IsActive = true,
            Login = "UnbreakableLogin11!",
            Password = "Unbreak4bleP4ssw0rd",
            PhoneNumber = "511622733"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var accountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Drugie konto przed mody",
            Balance = 5000,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        var accountResponse = _financialAccountsService.AddFinancialAccount(accountAddRequest);
        
        _testOutputHelper.WriteLine("GENERATED OBJECTS:");
        _testOutputHelper.WriteLine("1. CURRENCY:\n" + currencyResponse);
        _testOutputHelper.WriteLine("2. COUNTRY:\n" + countryResponse);
        _testOutputHelper.WriteLine("3. USER:\n" + userResponse);
        _testOutputHelper.WriteLine("4. FINANCIAL ACCOUNT:\n" + accountResponse);
        
        // updating account
        var accountUpdateRequest = accountResponse.ToFinancialAccountUpdateRequest();
        
        _testOutputHelper.WriteLine("--------------------------------------------");
        _testOutputHelper.WriteLine("UPDATE REQUEST:");
        _testOutputHelper.WriteLine($"Account ID: {accountUpdateRequest.AccountId}\n" +
                                    $"Account name: {accountUpdateRequest.AccountName}\n" +
                                    $"Balance: {accountUpdateRequest.Balance}\n");

        accountUpdateRequest.AccountName = "Modified";
        accountUpdateRequest.Balance = null;
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _financialAccountsService.UpdateFinancialAccount(accountUpdateRequest);
        });
    }
    
    // 5. If update values are correct, account should be updated
    [Fact]
    public void UpdateFinancialAccount_ProperUpdateValues()
    {
        // Arrange
        // creating account
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "jakistamMail@gmail.com",
            IsActive = true,
            Login = "MojLogin123!",
            Password = "123456789"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var accountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "AccountBeforeModification",
            Balance = 9999.199m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        var accountResponse = _financialAccountsService.AddFinancialAccount(accountAddRequest);
        
        _testOutputHelper.WriteLine("GENERATED OBJECTS:");
        _testOutputHelper.WriteLine("1. CURRENCY:\n" + currencyResponse);
        _testOutputHelper.WriteLine("2. COUNTRY:\n" + countryResponse);
        _testOutputHelper.WriteLine("3. USER:\n" + userResponse);
        _testOutputHelper.WriteLine("4. FINANCIAL ACCOUNT:\n" + accountResponse);
        
        // updating account
        var accountUpdateRequest = accountResponse.ToFinancialAccountUpdateRequest();
        
        _testOutputHelper.WriteLine("--------------------------------------------");
        _testOutputHelper.WriteLine("UPDATE REQUEST:");
        _testOutputHelper.WriteLine($"Account ID: {accountUpdateRequest.AccountId}\n" +
                                    $"Account name: {accountUpdateRequest.AccountName}\n" +
                                    $"Balance: {accountUpdateRequest.Balance}\n");

        accountUpdateRequest.AccountName = "ModifiedAccountName";
        accountUpdateRequest.Balance = 1000;

        var updatedAccountResponse = _financialAccountsService.UpdateFinancialAccount(accountUpdateRequest);
        
        _testOutputHelper.WriteLine("\nUPDATED FINANCIAL ACCOUNT:");
        _testOutputHelper.WriteLine($"Account ID: {updatedAccountResponse.AccountId}\n" +
                                    $"Account name: {updatedAccountResponse.AccountName}\n" +
                                    $"Balance: {updatedAccountResponse.Balance}\n");
        
        // Act
        var financialAccountFromGet = _financialAccountsService.GetFinancialAccountById(updatedAccountResponse.AccountId);
        
        // Assert
        Assert.Equal(financialAccountFromGet, updatedAccountResponse);
    }

    #endregion

    #region DeleteFinancialAccount

    /*
     * Test requirements:
     * 1. If given id is invalid, it should return false
     * 2. If given id is valid, it should return true and delete existing financial account
     */

    // 1. Given id is invalidvalid
    [Fact]
    public void DeleteFinancialAccount_InvalidAccountId()
    {
        // Arrange
        var invalidId = Guid.NewGuid();
        
        // Act
        var isDeleted = _financialAccountsService.DeleteFinancialAccount(invalidId);
        
        // Assert
        Assert.False(isDeleted);
    }
    
    // 2. Given id is valid
    [Fact]
    public void DeleteFinancialAccount_ValidAccountId()
    {
        // Arrange
        // creating account
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        var userAddRequest = new UserAddRequest
        {
            CountryId = countryResponse.CountryId,
            Email = "jakistamMail@gmail.com",
            IsActive = true,
            Login = "MojLogin123!",
            Password = "123456789"
        };
        var userResponse = _usersService.AddUser(userAddRequest);

        var accountAddRequest = new FinancialAccountAddRequest
        {
            AccountName = "Nazwa przed modyfikacją",
            Balance = 1000.50m,
            CurrencyId = currencyResponse.CurrencyId,
            UserId = userResponse.UserId
        };
        var accountResponse = _financialAccountsService.AddFinancialAccount(accountAddRequest);
        
        _testOutputHelper.WriteLine("GENERATED OBJECTS:");
        _testOutputHelper.WriteLine("1. CURRENCY:\n" + currencyResponse);
        _testOutputHelper.WriteLine("2. COUNTRY:\n" + countryResponse);
        _testOutputHelper.WriteLine("3. USER:\n" + userResponse);
        _testOutputHelper.WriteLine("4. FINANCIAL ACCOUNT:\n" + accountResponse);
        
        // Act
        var isDeleted = _financialAccountsService.DeleteFinancialAccount(accountResponse.AccountId);
        
        // Assert
        Assert.True(isDeleted);
    }

    #endregion

    #region Private methods

    private void GenerateFinancialAccounts()
    {
        // creating currencies
        var currenciesAddRequestList = new List<CurrencyAddRequest>
        {
            new() { CurrencyName = "PLN" },
            new() { CurrencyName = "EUR" },
            new() { CurrencyName = "USD" }
        };
        var currenciesResponseList = new List<CurrencyResponse>
        {
            _currenciesService.AddCurrency(currenciesAddRequestList[0]),
            _currenciesService.AddCurrency(currenciesAddRequestList[1]),
            _currenciesService.AddCurrency(currenciesAddRequestList[2]),
        };
        
        // creating countries
        var countriesAddRequestList = new List<CountryAddRequest>
        {
            new() { CountryCurrency = currenciesResponseList[0].CurrencyId, CountryName = "Poland" },
            new() { CountryCurrency = currenciesResponseList[1].CurrencyId, CountryName = "Germany" },
            new() { CountryCurrency = currenciesResponseList[2].CurrencyId, CountryName = "United States" }
        };
        var countriesResponseList = new List<CountryResponse>
        {
            _countriesService.AddCountry(countriesAddRequestList[0]),
            _countriesService.AddCountry(countriesAddRequestList[1]),
            _countriesService.AddCountry(countriesAddRequestList[2])
        };
        
        // creating users
        var usersAddRequestList  = new List<UserAddRequest>
        {
            new()
            {
                CountryId = countriesResponseList[0].CountryId, 
                Email = "Somemail@gmail.com",
                IsActive = true,
                Login = "MyLogin123",
                Password = "Abcdefqwert"
            },
            new()
            {
                CountryId = countriesResponseList[1].CountryId, 
                Email = "dummymail123456@gmail.com",
                IsActive = true,
                Login = "Blablabla000",
                Password = "MamNadziejeZeZdam"
            },
            new()
            {
                CountryId = countriesResponseList[2].CountryId, 
                Email = "maildoesntexists@gmail.com",
                IsActive = false,
                Login = "BylebyToSkonczyc",
                Password = "Plz3-MusiByc8Znakow"
            }
        };
        var userResponseList = new List<UserResponse>
        {
            _usersService.AddUser(usersAddRequestList[0]),
            _usersService.AddUser(usersAddRequestList[1]),
            _usersService.AddUser(usersAddRequestList[2])
        };
        
        // creating financial accounts
        var financialAccountAddRequestList = new List<FinancialAccountAddRequest>
        {
            new()
            {
                AccountName = "ZobaczMojePierwszeKonto",
                Balance = 3000.15m,
                CurrencyId = currenciesResponseList[0].CurrencyId,
                UserId = userResponseList[0].UserId
            },
            new()
            {
                AccountName = "AToMeineErsterAccount",
                Balance = 150,
                CurrencyId = currenciesResponseList[1].CurrencyId,
                UserId = userResponseList[1].UserId
            },
            new()
            {
                AccountName = "BooMyFirstAccount",
                Balance = 10000.973m,
                CurrencyId = currenciesResponseList[2].CurrencyId,
                UserId = userResponseList[2].UserId
            },
            new()
            {
                AccountName = "DoSortowania-MojeDrugieKonto",
                Balance = 420,
                CurrencyId = currenciesResponseList[0].CurrencyId,
                UserId = userResponseList[0].UserId
            },
            new()
            {
                AccountName = "ItsMySecondAccount",
                Balance = 999.88m,
                CurrencyId = currenciesResponseList[2].CurrencyId,
                UserId = userResponseList[2].UserId
            }
        };
        var createdFinancialAccounts = new List<FinancialAccountResponse>
        {
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequestList[0]),
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequestList[1]),
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequestList[2]),
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequestList[3]),
            _financialAccountsService.AddFinancialAccount(financialAccountAddRequestList[4]),
        };
    }

    #endregion
}