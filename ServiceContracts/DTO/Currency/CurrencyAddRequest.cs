using System.ComponentModel.DataAnnotations;
using Entities;

namespace ServiceContracts.DTO;

public class CurrencyAddRequest
{
    [Required]
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Use letters only!")]
    public string? CurrencyName { get; set; }

    public Currency ToCurrency()
    {
        return new Currency
        {
            CurrencyName = CurrencyName
        };
    }
}