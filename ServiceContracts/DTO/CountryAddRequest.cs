using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO;

/// <summary>
/// DTO class for adding new User
/// </summary>
public class CountryAddRequest
{
    [Required]
    public Guid? CountryCurrency { get; set; }
    
    [Required]
    [RegularExpression(@"^[a-zA-Z\s]*$", ErrorMessage = "Use letters only!")]
    public string? CountryName { get; set; }

    public Country ToCountry()
    {
        return new Country
        {
            CountryName = CountryName,
            CountryCurrency = CountryCurrency
        };
    }
}