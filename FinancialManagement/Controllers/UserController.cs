using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace FinancialManagement.Controllers;

[Route("user")]
public class UserController : Controller
{
    private readonly ICountriesService _countriesService;

    public UserController(ICountriesService countriesService)
    {
        _countriesService = countriesService;
    }
    
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("register")]
    public IActionResult Register()
    {
        var allCountries = _countriesService.GetAllCountries();
        
        return View(allCountries);
    }
}