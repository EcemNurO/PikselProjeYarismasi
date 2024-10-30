using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
	public class ReportController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
            var reportVM = new ReportVM
            {
                contestantProfils = db.ContestantProfils.ToList(), // Tüm yarışmacı profilleri
                judgeProfils = db.JudgeProfils.ToList(),           // Tüm hakem profilleri
                projectCategories = db.ProjectCategories.ToList(), // Tüm proje kategorileri
                JudgeCategories = db.JudgeCategories.ToList(),     // Tüm hakem kategorileri
                ContestantCategories = db.ContestantCategories.ToList() // Tüm yarışmacı kategorileri
            };

            return View(reportVM);
		}
	}
}
