using ServiceContracts.DTO.Currency;
using Xunit.Abstractions;

namespace UnitTests;

public class CurrenciesServiceTest
{
    private readonly ICurrenciesService _currenciesService;
    private readonly ITestOutputHelper _outputHelper;

    public CurrenciesServiceTest(ITestOutputHelper outputHelper)
    {
        _currenciesService = new CurrenciesService();
        _outputHelper = outputHelper;
    }
    
    #region AddCurrency
    
    /*
     * AddCurrency test requirements:
     * 1. When CurrencyAddRequest is null, it should throw ArgumentNullException
     * 2. When Currency required properties are null, it should throw ArgumentException
     * 3. When CurrencyName is duplicated, it should throw ArgumentException
     * 4. When Currency is supplied properly, it should add this object to existing list of countries
     */
    
    // 1. CurrencyAddRequest is null
    [Fact]
    public void AddCurrency_CurrencyAddRequestIsNull()
    {
        // Arrange
        CurrencyAddRequest? currencyAddRequest = null;
        
        _outputHelper.WriteLine($"currencyAddRequest value: {currencyAddRequest}");
        
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _currenciesService.AddCurrency(currencyAddRequest);
        });
    }

    // 2. Currency properties are null
    [Fact]
    public void AddCurrency_CurrencyPropertiesAreNull()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = null
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _currenciesService.AddCurrency(currencyAddRequest);
        });
    }

    // 3. CurrencyName is duplicated
    [Fact]
    public void AddCurrency_CurrencyNameIsDuplicated()
    {
        // Arrange
        var currencyAddRequest1 = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };

        var currencyAddRequest2 = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _currenciesService.AddCurrency(currencyAddRequest1);
            _currenciesService.AddCurrency(currencyAddRequest2);
        });
    }

    [Fact]
    public void AddCurrency_CurrencyDetailsAreProper()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };

        // Act
        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);
        var listOfCurrencies = _currenciesService.GetAllCurrencies();
        
        _outputHelper.WriteLine($"currencyResponse value: {currencyResponse}");
        _outputHelper.WriteLine("\n\nlistOfCurrencies values: ");
        foreach (var currencyInList in listOfCurrencies)
        {
            _outputHelper.WriteLine(currencyInList.ToString());
        }
        
        // Assert
        Assert.True(currencyResponse.CurrencyId != Guid.Empty);
        Assert.Contains(currencyResponse, listOfCurrencies);
    }

    #endregion

    #region GetAllCurrencies
    
    /*
     * GetAllCurrencies test requirements:
     * 1. Without adding any currency, list should be empty (list should be empty by default)
     * 2. After adding few currencies, GetAllCurrencies should return every added currency
     */

    // 1. List should be empty by default
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        // Acts
        var actualCurrencyResponseList = _currenciesService.GetAllCurrencies();
        
        // Assert
        Assert.Empty(actualCurrencyResponseList);
    }
    
    // 2. Should return every added country
    [Fact]
    public void GetAllCurrencies_AddFewCurrencies()
    {
        // Arrange
        var currencyAddRequestList = new List<CurrencyAddRequest>
        {
            new()
            {
                CurrencyName = "PLN"
            },
            new()
            {
                CurrencyName = "EUR"
            },
            new()
            {
                CurrencyName = "USD"
            }
        };

        // Act
        var currenciesResponse = new List<CurrencyResponse>();
        
        foreach (var currencyRequest in currencyAddRequestList)
        {
            currenciesResponse.Add(_currenciesService.AddCurrency(currencyRequest));
        }

        var actualCurrencyResponseList = _currenciesService.GetAllCurrencies();
        
        _outputHelper.WriteLine("Currency list: ");
        foreach (var currencyResponse in actualCurrencyResponseList)
        {
            _outputHelper.WriteLine(currencyResponse.ToString());
        }
        
        // Assert
        // Read each element of userResponse
        foreach (var expectedCurrency in currenciesResponse)
        {
            Assert.Contains(expectedCurrency, actualCurrencyResponseList);
        }
    }

    #endregion
    
    #region GetCurrencyById

    /*
     * GetCurrencyById test requirements:
     * 1. If supplied currency id is null, it should return null
     * 2. If supplied proper currency id, it should return valid currency object
     */
    
    // 1. Supplied currency id is null
    [Fact]
    public void GetCurrencyByCurrencyId_NullCurrencyId()
    {
        // Arrange
        Guid? currencyId = null;

        // Act
        var currencyResponse = _currenciesService.GetCurrencyByCurrencyId(currencyId);
        
        // Assert
        Assert.Null(currencyResponse);
    }
    
    // 2. Supplied proper currency id
    [Fact]
    public void GetCurrencyByCurrencyId_ValidCurrencyId()
    {
        // Arrange
        var currencyAddRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };

        var currencyResponse = _currenciesService.AddCurrency(currencyAddRequest);

        // Act
        var foundCurrency = _currenciesService.GetCurrencyByCurrencyId(currencyResponse.CurrencyId);
        
        _outputHelper.WriteLine($"Given id: {currencyResponse.CurrencyId}");
        _outputHelper.WriteLine($"Found currency: {foundCurrency}");
        _outputHelper.WriteLine($"currencyResponse: {currencyResponse}");
        
        // Assert
        Assert.Equal(currencyResponse, foundCurrency);
    }

    #endregion
}