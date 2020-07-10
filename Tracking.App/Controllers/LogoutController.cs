using Microsoft.AspNetCore.Mvc;

namespace Tracking.App.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("access_token");
            HttpContext.Session.Remove("username");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}