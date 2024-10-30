using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;

namespace Yarışma.Areas.Management.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            return View();
        }
    }
}
