using Microsoft.AspNetCore.Mvc;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	[Area("Management")]
	public class ThematicAreaController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
			
			return View();
		}
	}
}
