using Microsoft.AspNetCore.Mvc;

namespace LegionCore.Web.Controllers;

public class IndexController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}