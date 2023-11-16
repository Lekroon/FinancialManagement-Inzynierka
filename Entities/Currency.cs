using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Currency
{
    [Key]
    public Guid CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
}