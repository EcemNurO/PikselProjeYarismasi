using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	public class ContestantTableController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
			ContestantTableVM vm = new ContestantTableVM();
			vm.JudgeProfils = db.JudgeProfils.ToList();
			vm.ContestantProfils = db.ContestantProfils.ToList();
			vm.Judge = db.Judges.ToList();
			vm.Contestant = db.Contestants.ToList();
			vm.JudgeCategories = db.JudgeCategories.ToList();
			vm.contestantCategories = db.ContestantCategories.ToList();

			return View(vm);
		}
	}
}
