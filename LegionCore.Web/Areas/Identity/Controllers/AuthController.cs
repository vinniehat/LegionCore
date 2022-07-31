using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegionCore.Web;

[Authorize]
[Area("Identity")]
public class AuthController : Controller
{
    // GET
    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }
    
    // GET
    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }
    
    // GET
    [AllowAnonymous]
    public IActionResult RecoverPassword()
    {
        return View();
    }
}