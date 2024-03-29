﻿using System.ComponentModel.DataAnnotations;

namespace ServiceContracts.DTO.Currency;

public class CurrencyAddRequest
{
    [Required]
    [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Use letters only!")]
    
    [MaxLength(5)]
    public string? CurrencyName { get; set; }

    public Entities.Currency ToCurrency()
    {
        return new Entities.Currency
        {
            CurrencyName = CurrencyName
        };
    }
}