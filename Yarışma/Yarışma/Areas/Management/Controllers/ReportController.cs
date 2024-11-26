using Microsoft.AspNetCore.Mvc;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ReportController : Controller
	{
		CompetitionDbContext db = new CompetitionDbContext();
		public IActionResult Index()
		{
            var report = new ReportVM
            {
                RemainingTime = CalculateRemainingTime(),
                TotalContestants = db.Contestants.Count(),
                TotalJudges = db.Judges.Count(),
                TotalCategories = db.ProjectCategories.Count(),
                

                ContestantCategories = db.Contestants
        .GroupBy(c => c.ContestantCategory.Name)
        .Select(g => new CategoryReport { Name = g.Key, Count = g.Count() })
        .ToList(),

                JudgeCategories = db.Judges
        .GroupBy(j => j.JudgeCategory.Name)
        .Select(g => new CategoryReport { Name = g.Key, Count = g.Count() })
        .ToList(),

                ProjectCategories = db.Contestants
            .Where(c => c.Projects != null) 
            .GroupBy(c => c.Projects.ProjectCategory.Name)
            .Select(g => new CategoryReport { Name = g.Key, Count = g.Count() })
            .ToList()

            };


            return View(report);
        }
        private int CalculateRemainingTime()
        {
            var endDate = db.Periods.FirstOrDefault()?.ProjectEndDate ?? DateTime.Now;
            return (int)(endDate - DateTime.Now).TotalDays;
        }
    }
}
