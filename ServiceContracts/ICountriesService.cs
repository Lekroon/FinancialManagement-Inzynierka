using ServiceContracts.DTO;

namespace ServiceContracts;

/// <summary>
/// Represents business logic for manipulating Country entity
/// </summary>
public interface ICountriesService
{
    /// <summary>
    /// Adds a country object to the list of countries
    /// </summary>
    /// <param name="countryAddRequest">Country object to add</param>
    /// <returns>Returns country object after adding it (including generated id)</returns>
    CountryResponse AddCountry(CountryAddRequest? countryAddRequest);

    /// <summary>
    /// Returns all countries from list
    /// </summary>
    List<CountryResponse> GetAllCountries();

    /// <summary>
    /// Returns country object based on given country id
    /// </summary>
    /// <param name="countryId">Represents country id (Guid) that you want to search</param>
    /// <returns>Matching country object as UserResponse</returns>
    CountryResponse? GetCountryByCountryId(Guid? countryId);
}