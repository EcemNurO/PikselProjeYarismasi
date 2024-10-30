using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;

namespace Yarışma.Controllers
{
	public class AccountController : Controller
	{
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public IActionResult Login(LoginViewModel model)
		{
			return View();
		}
	}
}
