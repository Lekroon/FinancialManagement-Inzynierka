using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.FinancialAccount;
using ServiceContracts.Enums;

namespace FinancialManagement.Controllers;

[Route("lists")]
public class ListsController : Controller
{
    private readonly ICurrenciesService _currenciesService;
    private readonly ICountriesService _countriesService;
    private readonly IUsersService _usersService;
    private readonly IFinancialAccountsService _financialAccountsService;

    public ListsController(ICurrenciesService currenciesService, 
        ICountriesService countriesService, 
        IUsersService usersService,
        IFinancialAccountsService financialAccountsService)
    {
        _currenciesService = currenciesService;
        _countriesService = countriesService;
        _usersService = usersService;
        _financialAccountsService = financialAccountsService;
    }
    
    [Route("currencies")]
    public IActionResult Currencies()
    {
        var allCurrencies = _currenciesService.GetAllCurrencies();
        
        return View(allCurrencies);
    }
    
    [Route("countries")]
    public IActionResult Countries()
    {
        var allCountries = _countriesService.GetAllCountries();
        
        return View(allCountries);
    }
    
    [Route("users")]
    public IActionResult Users()
    {
        var allUsers = _usersService.GetAllUsers();

        return View(allUsers);
    }
    
    [Route("accounts")]
    public IActionResult Accounts(string? searchString, 
        string sortBy = nameof(FinancialAccountResponse.AccountName),
        SortOrderOptions sortOrder = SortOrderOptions.Asc)
    {
        // Search
        var allFinancialAccounts = _financialAccountsService.GetFilteredFinancialAccounts(searchString);
        ViewBag.SearchString = searchString ?? string.Empty;
        
        // Sort
        var sortedFinancialAccounts = 
            _financialAccountsService.GetSortedFinancialAccounts(allFinancialAccounts, sortBy, sortOrder);
        ViewBag.SortBy = sortBy;
        ViewBag.SortOrder = sortOrder.ToString();
        
        return View(sortedFinancialAccounts);
    }
}