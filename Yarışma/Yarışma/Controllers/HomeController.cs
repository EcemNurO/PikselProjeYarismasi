using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Yarışma.Models;

namespace Yarışma.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;

		public HomeController(ILogger<HomeController> logger)
		{
			_logger = logger;
		}
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
			var Profil = db.ContestantProfils.ToList();
			return View(Profil);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
