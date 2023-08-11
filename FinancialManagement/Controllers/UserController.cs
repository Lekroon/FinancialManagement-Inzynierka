using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Controllers;

[Route("user")]
public class UserController : Controller
{
    [Route("login")]
    [Route("/")]
    public IActionResult Login()
    {
        return View();
    }

    [Route("temp/users")]
    public IActionResult Users()
    {
        return View();
    }
}