using Entities;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.DTO.Currency;
using Services.Helpers;

namespace Services;

public class CurrenciesService : ICurrenciesService
{
    private readonly List<Currency> _listOfCurrencies;

    public CurrenciesService(bool initialize = true)
    {
        _listOfCurrencies = new List<Currency>();

        if (initialize)
        {
            MockData();
        }
    }

    private void MockData()
    {
        _listOfCurrencies.AddRange(new List<Currency>
        {
            new()
            {
                CurrencyId = Guid.Parse("57D46BB9-0354-4718-B3E7-5BC28BBB3994"),
                CurrencyName = "PLN"
            },
            new()
            {
                CurrencyId = Guid.Parse("C0461F37-B50F-4CA7-8015-D040AAFC3DCE"),
                CurrencyName = "EUR"
            },
            new()
            {
                CurrencyId = Guid.Parse("4749A472-F7FB-4114-9D79-D4CE13A7B9B7"),
                CurrencyName = "USD"
            }
        });
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