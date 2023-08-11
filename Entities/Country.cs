namespace Entities;

/// <summary>
/// Domain model used for storing Country details
/// </summary>
public class Country
{
    public Guid CountryId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CountryName { get; set; }
}