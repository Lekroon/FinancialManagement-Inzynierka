using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Controllers;

[Route("user")]
public class UserController : Controller
{
    [Route("login")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("register")]
    public IActionResult Register()
    {
        return View();
    }
}