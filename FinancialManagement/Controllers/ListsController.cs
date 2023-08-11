using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using ServiceContracts;

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
    public IActionResult Accounts()
    {
        var allFinancialAccounts = _financialAccountsService.GetAllFinancialAccounts();

        return View(allFinancialAccounts);
    }
}