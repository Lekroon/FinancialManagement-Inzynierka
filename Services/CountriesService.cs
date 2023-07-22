using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using Services.Helpers;

namespace Services;

public class CountriesService : ICountriesService
{
    private readonly List<Country> _listOfCountries;
    
    public CountriesService()
    {
        _listOfCountries = new List<Country>();
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

        return country.ToCountryResponse();
    }

    public List<CountryResponse> GetAllCountries()
    {
        return _listOfCountries.Select(country => country.ToCountryResponse()).ToList();
    }

    public CountryResponse? GetCountryByCountryId(Guid? countryId)
    {
        return _listOfCountries.FirstOrDefault(country => country.CountryId == countryId)?.ToCountryResponse();
    }
}