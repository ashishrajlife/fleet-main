using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FleetManagement.Web.Controllers
{
    public class HomeController : Controller
    {
        // GET /
        [AllowAnonymous]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                if (User.IsInRole("admin"))
                    ViewBag.Message = "Admin logged in";
                else if (User.IsInRole("manager"))
                    ViewBag.Message = "Manager logged in";
                else if (User.IsInRole("driver"))
                    ViewBag.Message = "Driver logged in";
                else
                    ViewBag.Message = "User logged in";
            }
            else
            {
                ViewBag.Message = "Not logged in";
            }

            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy() => View();

        [AllowAnonymous]
        public IActionResult Error() => View();
    }
}