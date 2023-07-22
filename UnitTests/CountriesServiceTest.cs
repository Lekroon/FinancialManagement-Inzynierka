using Xunit.Abstractions;

namespace UnitTests;

public class CountriesServiceTest
{
    private readonly ICountriesService _countriesService;
    private readonly ICurrenciesService _currenciesService;
    private readonly ITestOutputHelper _outputHelper;

    public CountriesServiceTest(ITestOutputHelper outputHelper)
    {
        _countriesService = new CountriesService();
        _currenciesService = new CurrenciesService();
        _outputHelper = outputHelper;
    }
    
    #region AddCountry
    
    /*
     * AddCountry test requirements:
     * 1. When CountryAddRequest is null, it should throw ArgumentNullException
     * 2. When Country required properties are null, it should throw ArgumentException
     * 3. When CountryName is duplicated, it should throw ArgumentException
     * 4. When Country is supplied properly, it should add this object to existing list of countries
     */
    
    // 1. CountryAddRequest is null
    [Fact]
    public void AddCountry_CountryAddRequestIsNull()
    {
        // Arrange
        CountryAddRequest? countryAddRequest = null;
        
        // Assert
        Assert.Throws<ArgumentNullException>(() =>
        {
            // Act
            _countriesService.AddCountry(countryAddRequest);
        });
    }

    // 2. Country properties are null
    [Fact]
    public void AddCountry_CountryPropertiesAreNull()
    {
        // Arrange
        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = null,
            CountryName = null
        };
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            // Act
            _countriesService.AddCountry(countryAddRequest);
        });
    }

    // 3. CountryName is duplicated
    [Fact]
    public void AddCountry_CountryNameIsDuplicated()
    {
        // Arrange
        var currencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var currencyResponse = _currenciesService.AddCurrency(currencyRequest);
        
        
        
        var countryAddRequest1 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };

        var countryAddRequest2 = new CountryAddRequest
        {
            CountryCurrency = currencyResponse.CurrencyId,
            CountryName = "Poland"
        };
        
        _outputHelper.WriteLine($"Currency: {currencyResponse}");
        
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            _countriesService.AddCountry(countryAddRequest1);
            _countriesService.AddCountry(countryAddRequest2);
        });
    }

    // 4. Country is added properly
    [Fact]
    public void AddCountry_CountryDetailsAreProper()
    {
        // Arrange
        var polishCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var polishCurrency = _currenciesService.AddCurrency(polishCurrencyRequest);
        
        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = polishCurrency.CurrencyId,
            CountryName = "Poland"
        };
        
        // Act
        var countryResponse = _countriesService.AddCountry(countryAddRequest);
        var listOfCountries = _countriesService.GetAllCountries();
        
        _outputHelper.WriteLine($"Currency: {polishCurrency}");
        _outputHelper.WriteLine($"Country: {countryResponse}");
        _outputHelper.WriteLine("\n\nList of Countries: ");
        foreach (var country in listOfCountries)
        {
            _outputHelper.WriteLine(country.ToString());
        }

        // Assert
        Assert.True(countryResponse.CountryId != Guid.Empty);
        Assert.Contains(countryResponse, listOfCountries);
    }

    #endregion

    #region GetAllCountries
    

    /*
     * GetAllCountries test requirements:
     * 1. Without adding any country, list should be empty (list should be empty by default)
     * 2. After adding few countries, GetAllCountries should return every added country
     */

    // 1. List should be empty by default
    [Fact]
    public void GetAllCountries_EmptyList()
    {
        // Acts
        var actualCountryResponseList = _countriesService.GetAllCountries();
        
        // Assert
        Assert.Empty(actualCountryResponseList);
    }
    
    // 2. Should return every added country
    [Fact]
    public void GetAllCountries_AddFewCountries()
    {
        // Arrange
        var polishCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var unitedStatesCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "USD"
        };
        var germanCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "EUR"
        };

        var polishCurrency = _currenciesService.AddCurrency(polishCurrencyRequest);
        var unitedStatesCurrency = _currenciesService.AddCurrency(unitedStatesCurrencyRequest);
        var germanCurrency = _currenciesService.AddCurrency(germanCurrencyRequest);
        
        var countryAddRequestList = new List<CountryAddRequest>
        {
            new()
            {
                CountryCurrency = polishCurrency.CurrencyId,
                CountryName = "Poland"
            },
            new()
            {
                CountryCurrency = unitedStatesCurrency.CurrencyId,
                CountryName = "United States of America"
            },
            new()
            {
                CountryCurrency = germanCurrency.CurrencyId,
                CountryName = "Germany"
            }
        };

        // Act
        var countryResponse = new List<CountryResponse>();
        
        foreach (var countryRequest in countryAddRequestList)
        {
            countryResponse.Add(_countriesService.AddCountry(countryRequest));
        }

        var actualCountryResponseList = _countriesService.GetAllCountries();
        
        _outputHelper.WriteLine("Currency list: ");
        foreach (var currency in _currenciesService.GetAllCurrencies())
        {
            _outputHelper.WriteLine(currency.ToString());
        }
        
        _outputHelper.WriteLine("\n\nCountry list: ");
        foreach (var country in _countriesService.GetAllCountries())
        {
            _outputHelper.WriteLine(country.ToString());
        }
        
        // Assert
        // Read each element of userResponse
        foreach (var expectedCountry in countryResponse)
        {
            Assert.Contains(expectedCountry, actualCountryResponseList);
        }
    }

    #endregion
    
    #region GetCountryById

    /*
     * GetCountryById test requirements:
     * 1. If supplied country id is null, it should return null
     * 2. If supplied proper country id, it should return valid country object
     */
    
    // 1. Supplied country id is null
    [Fact]
    public void GetCountryById_NullCountryId()
    {
        // Arrange
        Guid? countryId = null;

        // Act
        var countryResponse = _countriesService.GetCountryByCountryId(countryId);
        
        // Assert
        Assert.Null(countryResponse);
    }
    
    // 2. Supplied proper country id
    [Fact]
    public void GetCountryByCountryId_ValidCountryId()
    {
        // Arrange
        var polishCurrencyRequest = new CurrencyAddRequest
        {
            CurrencyName = "PLN"
        };
        var polishCurrency = _currenciesService.AddCurrency(polishCurrencyRequest);
        
        var countryAddRequest = new CountryAddRequest
        {
            CountryCurrency = polishCurrency.CurrencyId,
            CountryName = "Poland"
        };

        var countryResponse = _countriesService.AddCountry(countryAddRequest);

        // Act
        var foundCountry = _countriesService.GetCountryByCountryId(countryResponse.CountryId);
        
        _outputHelper.WriteLine($"Currency: {polishCurrency}");
        _outputHelper.WriteLine($"\n\nFound country: {foundCountry}");
        
        // Assert
        Assert.Equal(countryResponse, foundCountry);
    }

    #endregion
}