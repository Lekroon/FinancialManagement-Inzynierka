using System.ComponentModel.DataAnnotations;

namespace Entities;

public class Currency
{
    [Key]
    public Guid CurrencyId { get; set; }
    
    [StringLength(5)]
    public string? CurrencyName { get; set; }
}