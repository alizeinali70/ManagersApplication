using Microsoft.AspNetCore.Mvc;

namespace ManagersApplication.Server.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
