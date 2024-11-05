using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	[Area("Management")]
	public class DashboardController : Controller
	{
	
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
            var vm = new DashboardVM
            {
                ContestantCount = db.Contestants.Count(),
                JudgeCount = db.Judges.Count(),
                CategoryCount = db.ContestantCategories.Count(),
                EvaluatedCount = db.ContestantJudges.Count(cj => cj.Judge.ProjectEvaluation != null),
                NotEvaluatedCount = db.Contestants.Count() - db.ContestantJudges.Count(cj => cj.Judge.ProjectEvaluation != null),
                ProjectCategories = db.ProjectCategories.ToList(),
                JudgeCategories = db.JudgeCategories.ToList()
            };


            return View(vm);
		}
	}
}