using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Currency;
using Services.Helpers;

namespace Services;

public class CurrenciesService : ICurrenciesService
{
    private readonly List<Currency> _listOfCurrencies;

    public CurrenciesService()
    {
        _listOfCurrencies = new List<Currency>();
    }

    public CurrencyResponse AddCurrency(CurrencyAddRequest? currencyAddRequest)
    { 
        // Validation: countryAddRequest parameter cannot be null
        if (currencyAddRequest == null)
        {
            throw new ArgumentNullException(nameof(currencyAddRequest));
        }
        
        // Model validation
        ValidationHelper.ModelValidation(currencyAddRequest);
        
        // Convert object from CountryAddRequest to Country type
        var currency = currencyAddRequest.ToCurrency();

        // Validation: name cannot be duplicated
        if (_listOfCurrencies.Any(currencyInList => currencyInList.CurrencyName == currency.CurrencyName))
        {
            throw new ArgumentException("Currency already exists");
        }

        // Generating new id for country
        currency.CurrencyId = Guid.NewGuid();
        
        _listOfCurrencies.Add(currency);

        return currency.ToCurrencyResponse();
    }

    public List<CurrencyResponse> GetAllCurrencies()
    {
        return _listOfCurrencies.Select(currency => currency.ToCurrencyResponse()).ToList();
    }

    public CurrencyResponse? GetCurrencyByCurrencyId(Guid? currencyId)
    {
        return _listOfCurrencies.FirstOrDefault(currency => currency.CurrencyId == currencyId)?.ToCurrencyResponse();
    }
}