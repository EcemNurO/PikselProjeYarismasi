using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class NotEvaluatedContestantsController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult NotEvaluatedContestants(string searchQuery = "")
        {
            var notEvaluatedContestants = db.Contestants
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.contestantProfil)
                .Where(c =>
                    db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id) && // Hakem atanmış
                    !db.ScoreProjects.Any(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.Score.HasValue) && // Puanı yok
                    (string.IsNullOrEmpty(searchQuery) || // Arama sorgusu
                     c.contestantProfil.FullName.Contains(searchQuery) ||
                     c.Projects.Name.Contains(searchQuery) ||
                     c.Projects.ProjectCategory.Name.Contains(searchQuery))
                )
                .Select(c => new ContestantViewModel
                {
                    ContestantId = c.Id,
                    ContestantName = c.contestantProfil.FullName,
                    ProjectName = c.Projects.Name,
                    ProjectCategoryName = c.Projects.ProjectCategory.Name,
                    AssignedAcademicJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(),
                    AssignedIndustrialJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault()
                })
                .ToList();

            return View(new ContestantTableVM
            {
                Contestants = notEvaluatedContestants,
                SearchQuery = searchQuery
            });
        }

    }
}
