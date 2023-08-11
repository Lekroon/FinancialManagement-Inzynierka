namespace ServiceContracts.DTO.Country;

public class CountryResponse
{
    public Guid CountryId { get; set; }
    public Guid? CurrencyId { get; set; }
    public string? CurrencyName { get; set; }
    public string? CountryName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(CountryResponse))
        {
            return false;
        }

        var countryToCopmare = (CountryResponse)obj;

        return CountryId == countryToCopmare.CountryId &&
               CurrencyId == countryToCopmare.CurrencyId &&
               CountryName == countryToCopmare.CountryName;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return $"CountryId: {CountryId}, CountryCurrency: {CurrencyId}, CountryName: {CountryName}," +
               $" CurrencyName: {CurrencyName}";
    }
}

public static class CountryExtension
{
    public static CountryResponse ToCountryResponse(this Entities.Country country)
    {
        return new CountryResponse
        {
            CountryId = country.CountryId,
            CurrencyId = country.CurrencyId,
            CountryName = country.CountryName
        };
    }
}