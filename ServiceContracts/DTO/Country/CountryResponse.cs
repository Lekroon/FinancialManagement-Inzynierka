namespace ServiceContracts.DTO.Country;

public class CountryResponse
{
    public Guid CountryId { get; set; }
    public Guid? CountryCurrency { get; set; }
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
               CountryCurrency == countryToCopmare.CountryCurrency &&
               CountryName == countryToCopmare.CountryName;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(CountryId);
        hashCode.Add(CountryCurrency);
        hashCode.Add(CountryName);
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        return $"CountryId: {CountryId}, CountryCurrency: {CountryCurrency}, CountryName: {CountryName}";
    }
}

public static class CountryExtension
{
    public static CountryResponse ToCountryResponse(this Entities.Country country)
    {
        return new CountryResponse
        {
            CountryId = country.CountryId,
            CountryCurrency = country.CountryCurrency,
            CountryName = country.CountryName
        };
    }
}