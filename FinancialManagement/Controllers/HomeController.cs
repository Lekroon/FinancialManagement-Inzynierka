﻿using Microsoft.AspNetCore.Mvc;

namespace FinancialManagement.Controllers;

public class HomeController : Controller
{
    [Route("")]
    public IActionResult Index()
    {
        return View();
    }
}