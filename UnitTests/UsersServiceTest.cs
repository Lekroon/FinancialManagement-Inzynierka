using ServiceContracts.DTO.Country;
using ServiceContracts.DTO.Currency;
using ServiceContracts.DTO.User;
using Xunit.Abstractions;

namespace UnitTests;

public class UsersServiceTest
{
    private readonly IUsersService _usersService;
    private readonly ICurrenciesService _currenciesService;
    private readonly ICountriesService _countriesService;
    private readonly ITestOutputHelper _outputHelper;

    public UsersServiceTest(ITestOutputHelper outputHelper)
    {
        _usersService = new UsersService(false);
        _currenciesService = new CurrenciesService(false);
        _countriesService = new CountriesService(false);
        _outputHelper = outputHelper;
    }

    #region AddUser
    
    /*
     * AddUser test requirements:
     * 1. When UserAddRequest is null, it should throw ArgumentNullException
     * 2. When User required properties are null, it should throw ArgumentException
     * 3. When User login, email or phone is duplicated, it should throw ArgumentException
     * 4. When User is supplied properly, it should add this object to existing list of users
     */
    
    // 1. When UserAddRequest is null
    [Fact]
    public void AddUser_UserAddRequestIsNull()
    {
        // Arrange
        UserAddRequest? userAddRequest = null;
        
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _usersService.AddUser(userAddRequest);
        });
    }
    
    // 2. When User required properties are null
    [Fact]
    public void AddUser_UserPropertiesAreNull()
    {
        // Arrange
        var userAddRequest = new UserAddRequest
        {
            Login = null,
            Password = null,
            Email = null,
            PhoneNumber = null,
            CountryId = null,
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _usersService.AddUser(userAddRequest);
        });
    }
    
    // 3. When User login, email or phone is duplicated
    [Fact]
    public void AddUser_UserPropertiesAreDuplicated()
    {
        // Arrange
        var germanCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };
        var germanCurrency = _currenciesService.AddCurrency(germanCurrencyRequest);

        var germanyRequest = new CountryAddRequest
        {
            CountryCurrency = germanCurrency.CurrencyId,
            CountryName = "Germany"
        };
        var germany = _countriesService.AddCountry(germanyRequest);
        
        var userAddRequest1 = new UserAddRequest
        {
            Login = "SomeLogin123",
            Password = "Pass123",
            Email = "Email@gmail.com",
            PhoneNumber = "123456789",
            CountryId = germany.CountryId,
        };
        
        var userAddRequest2 = new UserAddRequest
        {
            Login = "SomeLogin123",
            Password = "Pass123",
            Email = "Email@gmail.com",
            PhoneNumber = "123456789",
            CountryId = germany.CountryId,       
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _usersService.AddUser(userAddRequest1);
            _usersService.AddUser(userAddRequest2);
        });
    }
    
    // 4. When User is supplied properly
    [Fact]
    public void AddUser_UserDetailsAreProper()
    {
        // Arrange
        var polishCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var polishCurrency = _currenciesService.AddCurrency(polishCurrencyRequest);

        var polandRequest = new CountryAddRequest
        {
            CountryCurrency = polishCurrency.CurrencyId,
            CountryName = "Poland"
        };
        var poland = _countriesService.AddCountry(polandRequest);
        
        var userAddRequest = new UserAddRequest
        {
            Login = "MyLogin",
            Password = "MyPassword",
            Email = "SomeEmail@gmail.com",
            PhoneNumber = null,
            CountryId = poland.CountryId
        };
        
        // Act
        var userResponse = _usersService.AddUser(userAddRequest);
        var listOfUsers = _usersService.GetAllUsers();
        
        _outputHelper.WriteLine($"Currency: {polishCurrency}");
        _outputHelper.WriteLine($"\nCountry: {poland}");
        _outputHelper.WriteLine($"\nUser: {userResponse}");
        
        // Assert
        Assert.True(userResponse.UserId != Guid.Empty);
        Assert.Contains(userResponse, listOfUsers);
    }
    
    #endregion

    #region GetAllUsers

    /*
     * GetAllUsers test requirements:
     * 1. Without adding any user, list should be empty (list should be empty by default)
     * 2. After adding few users, GetAllUsers should return every added user
     */

    // 1. List should be empty by default
    [Fact]
    public void GetAllUsers_EmptyList()
    {
        // Acts
        var actualUserResponseList = _usersService.GetAllUsers();
        
        // Assert
        Assert.Empty(actualUserResponseList);
    }
    
    // 2. Should return every added user
    [Fact]
    public void GetAllUsers_AddFewUsers()
    {
        // Arrange
        var currencyAddRequestList = new List<CurrencyAddRequest>
        {
            new() { CurrencyName = "USD" },
            new() { CurrencyName = "PLN" },
            new() { CurrencyName = "EUR" }
        };

        var currencyResponseList = new List<CurrencyResponse>();

        foreach (var currencyToAdd in currencyAddRequestList)
        {
            currencyResponseList.Add(_currenciesService.AddCurrency(currencyToAdd));
        }

        var countryAddRequestList = new List<CountryAddRequest>
        {
            new() { CountryCurrency = currencyResponseList.ElementAt(0).CurrencyId, CountryName = "USA" },
            new() { CountryCurrency = currencyResponseList.ElementAt(1).CurrencyId, CountryName = "Poland" },
            new() { CountryCurrency = currencyResponseList.ElementAt(2).CurrencyId, CountryName = "Germany" }
        };

        var countryResponseList = new List<CountryResponse>();

        foreach (var countryToAdd in countryAddRequestList)
        {
            countryResponseList.Add(_countriesService.AddCountry(countryToAdd));
        }
                
        var userAddRequestList = new List<UserAddRequest>
        {
            new()
            {
                Login = "MyFirstLogin",
                Password = "SomePass",
                Email = "SzklanyDom@gmail.com",
                CountryId = countryResponseList.ElementAt(1).CountryId,
                IsActive = true,
                PhoneNumber = null
            },
            new()
            {
                Login = "SomeLoginalalal",
                Password = "pa$$word12",
                Email = "uuuaaa@onet.pl",
                CountryId = countryResponseList.ElementAt(1).CountryId,
                IsActive = false,
                PhoneNumber = "123456789"
            },
            new()
            {
                Login = "L0g1N123",
                Password = "passsssssword",
                Email = "domkow@wp.pl",
                CountryId = countryResponseList.ElementAt(0).CountryId,
                IsActive = true,
                PhoneNumber = "932-872-124"
            }
        };

        // Act
        var userResponseList = new List<UserResponse>();
        
        foreach (var userRequest in userAddRequestList)
        {
            userResponseList.Add(_usersService.AddUser(userRequest));
        }

        var actualUserResponseList = _usersService.GetAllUsers();
        
        // Printing values
        _outputHelper.WriteLine("Currency list: ");
        foreach (var currencyResponse in currencyResponseList)
        {
            _outputHelper.WriteLine(currencyResponse.ToString());
        }
        
        _outputHelper.WriteLine("\nCountry list: ");
        foreach (var countryResponse in countryResponseList)
        {
            _outputHelper.WriteLine(countryResponse.ToString());
        }
        
        _outputHelper.WriteLine("\n\nExpected:");
        foreach (var excpectedUserResponse in userResponseList)
        {
            _outputHelper.WriteLine(excpectedUserResponse+"\n");
        }
        
        _outputHelper.WriteLine("\nActual:");
        foreach (var actualUserResponse in userResponseList)
        {
            _outputHelper.WriteLine(actualUserResponse+"\n");
        }
                
        // Assert
        // Read each element of userResponse
        foreach (var expectedUser in userResponseList)
        {
            Assert.Contains(expectedUser, actualUserResponseList);
        }
    }
    
    #endregion

    #region GetUserByUserId

    /*
     * GetUserByUserId test requirements:
     * 1. If supplied user id is null, it should return null
     * 2. If supplied proper user id, it should return valid user object
     */
    
    // 1. Supplied user id is null
    [Fact]
    public void GetUserByUserId_NullUserId()
    {
        // Arrange
        Guid? userId = null;

        // Act
        var userResponse = _usersService.GetUserByUserId(userId);
        
        // Assert
        Assert.Null(userResponse);
    }
    
    // 2. Supplied proper user id
    [Fact]
    public void GetUserByUserId_ValidUserId()
    {
        //Arrange
        var currencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyRequest);

        var countryRequest = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        var countryResponse = _countriesService.AddCountry(countryRequest);
        
        var userAddRequest = new UserAddRequest
        {
            Login = "MyLogin",
            Password = "MyPassword",
            Email = "SomeEmail@gmail.com",
            PhoneNumber = "123123132",
            CountryId = countryResponse.CountryId,
            IsActive = true
        };

        var userResponse = _usersService.AddUser(userAddRequest);

        // Act
        var foundUser = _usersService.GetUserByUserId(userResponse.UserId);
        
        _outputHelper.WriteLine($"Currency list: {currencyResponse}");
        _outputHelper.WriteLine($"Country list: {countryResponse}");
        _outputHelper.WriteLine($"\n\nUser response: {userResponse}");
        _outputHelper.WriteLine($"\nUser found: {foundUser}");
        // Assert
        Assert.Equal(userResponse, foundUser);
    }
    
    #endregion

    #region GetUserByLoginOrEmail

    /*
     * GetUserByLoginOrEmail test requirements:
     * 1. if supplied loginOrEmail parameter is null, it should return null
     * 2. if supplied proper loginOrEmail, it should return matching user
     */

    // 1. Supplied loginOrEmail parameter is null
    [Fact]
    public void GetUserByLoginOrEmail_NullLoginOrEmail()
    {
        // Act
        string? loginOrEmail = null;

        // Arrange
        var userResponse = _usersService.GetUserByUserLoginOrEmail(loginOrEmail);
        
        // Assert
        Assert.Null(userResponse);
    }
    
    // 2. Supplied proper loginOrEmail
    [Fact]
    public void GetUserByLoginOrEmail_ValidLoginOrEmail()
    {
        // Act
        var loginOrEmail = "Bartosz.Tanski@gmail.com";
        
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
        
        var userRequest = new UserAddRequest
        {
            Login = "Login123",
            Password = "pass12345678",
            Email = "Bartosz.Tanski@gmail.com",
            CountryId = countryResponse.CountryId,
            IsActive = true,
            PhoneNumber = "215-222-111"
        };

        var userResponse = _usersService.AddUser(userRequest);
        
        // Assert
        var foundUser = _usersService.GetUserByUserLoginOrEmail(loginOrEmail);

        _outputHelper.WriteLine($"Given value: {loginOrEmail}");
        _outputHelper.WriteLine($"\nCurrency: {currencyResponse}");
        _outputHelper.WriteLine($"\nCountry: {countryResponse}");
        _outputHelper.WriteLine($"\n\nUser response: {userResponse}");
        _outputHelper.WriteLine($"\nUser found: {foundUser}");
        Assert.Equal(userResponse, foundUser);
    }


    #endregion
}