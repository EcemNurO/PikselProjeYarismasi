using Microsoft.AspNetCore.Mvc;

namespace Yarışma.Areas.Management.Controllers
{
	public class JudgeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
