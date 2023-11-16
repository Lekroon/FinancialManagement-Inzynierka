using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.Country;

/// <summary>
/// DTO class for adding new Country
/// </summary>
public class CountryAddRequest
{
    [Required]
    public Guid? CountryCurrency { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Use letters only!")]
    public string? CountryName { get; set; }

    public Entities.Country ToCountry()
    {
        return new Entities.Country
        {
            CountryName = CountryName,
            CurrencyId = CountryCurrency
        };
    }
}