using Microsoft.AspNetCore.Mvc;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	public class ReportController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{


			return View();
		}
	}
}
