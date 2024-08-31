using Casko.AspNetCore.RobotsTxt;
using Microsoft.AspNetCore.Mvc;

namespace Casko.RobotsTxt.App.Controllers;

public class DefaultController(IRobotsTxt robotsTxt) : Controller
{
    public IActionResult Index()
    {
        ViewData["Title"] = "RobotsTxt";
        ViewData["RobotsTxt"] = robotsTxt.GetRobotsTxt(HttpContext);
            
        return View();
    }
}