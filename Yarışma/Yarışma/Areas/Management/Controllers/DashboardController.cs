using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{

	public class DashboardController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
			DashboardVM model = new DashboardVM();
			{ 
			model.ContestantCount = db.ContestantProfils.Count(c =>
			c.Status == false
			);
			model.JudgeCount = db.JudgeProfils.Count(c => c.Status == false);
			model.CategoryCount = db.ProjectCategories.Count(c => c.Status == false);
			model.judgeProfils = db.JudgeProfils.ToList();
			model.contestantProfils =db.ContestantProfils.ToList();
			model.projectCategories =db.ProjectCategories.ToList();
			model.ContestantCategories =db.ContestantCategories.ToList();
			};

			
			return View(model);
		}
	}
}