using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Country;
using Services.Helpers;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _listOfCountries;
    private readonly ICurrenciesService _currenciesService;
    
    public CountriesService(bool initialize = true)
    {
        _listOfCountries = new List<Country>();
        _currenciesService = new CurrenciesService();

        if (initialize)
        {
            _listOfCountries.AddRange(new List<Country>
            {
                new()
                {
                    CountryId = Guid.Parse("83CE3D28-B429-41C4-9900-80F881DF6D28"),
                    CountryName = "Poland",
                    CurrencyId = Guid.Parse("57D46BB9-0354-4718-B3E7-5BC28BBB3994")
                },
                new()
                {
                    CountryId = Guid.Parse("332467FA-330B-4AC9-8442-37046826188E"),
                    CountryName = "Italy",
                    CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE")
                },
                new()
                {
                    CountryId = Guid.Parse("35EE3318-5D8A-4767-9C1F-D127D5DE7028"),
                    CountryName = "Germany",
                    CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE")
                },
                new()
                {
                    CountryId = Guid.Parse("6D4D8A43-A6EE-458B-B5A1-31A0A5699684"),
                    CountryName = "France",
                    CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE")
                },
                new()
                {
                    CountryId = Guid.Parse("191F892C-EB4A-4E7B-A144-C0E1EB2BB8BE"),
                    CountryName = "USA",
                    CurrencyId = Guid.Parse("4749A472-F7FB-4114-9D79-D4CE13A7B9B7")
                }
            });
        }
    }

    private CountryResponse ConvertCountryToCountryResponse(Country country)
    {
        var countryResponse = country.ToCountryResponse();
        
        countryResponse.CurrencyName = _currenciesService.GetCurrencyByCurrencyId(country.CurrencyId)?.CurrencyName;

        return countryResponse;
    }
    
    public CountryResponse AddCountry(CountryAddRequest? countryAddRequest)
    {
        // Validation: countryAddRequest parameter cannot be null
        if (countryAddRequest == null)
        {
            throw new ArgumentNullException(nameof(countryAddRequest));
        }
        
        // Model validation
        ValidationHelper.ModelValidation(countryAddRequest);
        
        // Convert object from CountryAddRequest to Country type
        var country = countryAddRequest.ToCountry();
        
        // Validation: name cannot be duplicated
        if (_listOfCountries.Any(countryInList => countryInList.CountryName == country.CountryName))
        {
            throw new ArgumentException("Country already exists");
        }

        // Generating new id for country
        country.CountryId = Guid.NewGuid();
        
        _listOfCountries.Add(country);

        return ConvertCountryToCountryResponse(country);
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _listOfCountries.Select(ConvertCountryToCountryResponse).ToList();
    }

    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        return _listOfCountries.FirstOrDefault(country => country.CountryId == countryId)?.ToCountryResponse();
    }
}