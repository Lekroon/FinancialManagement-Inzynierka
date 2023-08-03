using Entities;

namespace ServiceContracts.DTO;

public class CurrencyResponse
{
    public Guid CurrencyId { get; set; }
    public string? CurrencyName { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj == null)
        {
            return false;
        }

        if (obj.GetType() != typeof(CurrencyResponse))
        {
            return false;
        }

        var currencyToCopmare = (CurrencyResponse)obj;

        return CurrencyId == currencyToCopmare.CurrencyId &&
               CurrencyName == currencyToCopmare.CurrencyName;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();
        hashCode.Add(CurrencyId);
        hashCode.Add(CurrencyName);
        return hashCode.ToHashCode();
    }

    public override string ToString()
    {
        return $"CurrencyId:{CurrencyId}, CurrencyName:{CurrencyName}";
    }
}

public static class CurrencyExtension
{
    public static CurrencyResponse ToCurrencyResponse(this Currency currency)
    {
        return new CurrencyResponse
        {
            CurrencyId = currency.CurrencyId,
            CurrencyName = currency.CurrencyName
        };
    }
}