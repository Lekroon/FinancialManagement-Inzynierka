using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO.User;
using Services;

namespace FinancialManagement.Controllers;

[Route("[controller]/[action]")]
public class UserController : Controller
{
    private readonly ICountriesService _countriesService;
    private readonly IUsersService _usersService;

    public UserController(ICountriesService countriesService, IUsersService usersService)
    {
        _countriesService = countriesService;
        _usersService = usersService;
    }
    
    // Url: login
    [Route("")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("")]
    [HttpGet]
    public IActionResult Register()
    {
        var allCountries = _countriesService.GetAllCountries();

        ViewBag.Countries = allCountries;
        
        return View();
    }
    
    [Route("")]
    [HttpPost]
    public IActionResult Register(UserAddRequest userAddRequest)
    {
        if (!ModelState.IsValid)
        {
            var allCountries = _countriesService.GetAllCountries();
            
            var errors = ModelState.Values.SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage)
                .ToList();

            ViewBag.Countries = allCountries;
            ViewBag.Errors = errors;

            return View();
        }

        var addedUser = _usersService.AddUser(userAddRequest);

        return RedirectToAction("Users", "Lists");
    }
}