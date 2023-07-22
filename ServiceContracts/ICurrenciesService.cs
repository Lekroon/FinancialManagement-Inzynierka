using ServiceContracts.DTO;

namespace ServiceContracts;

public interface ICurrenciesService
{
    CurrencyResponse AddCurrency(CurrencyAddRequest? currencyAddRequest);

    List<CurrencyResponse> GetAllCurrencies();

    CurrencyResponse? GetCurrencyByCurrencyId(Guid? currencyId);
}