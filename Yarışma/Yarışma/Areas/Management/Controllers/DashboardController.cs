using Microsoft.AspNetCore.Mvc;

namespace Yarışma.Areas.Management.Controllers
{
	public class DashboardController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
