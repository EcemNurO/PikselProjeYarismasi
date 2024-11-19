using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class EvaluatedContestantsController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult EvaluatedContestants(string searchQuery = "", int page = 1, int pageSize = 10)
        {
            var filteredContestants = db.Contestants
       .Include(c => c.Projects)
           .ThenInclude(p => p.ProjectCategory)
       .Include(c => c.contestantProfil)
       .Where(c =>
           db.ScoreProjects.Any(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.Score.HasValue) // Puanı olanlar
           &&
           (string.IsNullOrEmpty(searchQuery) || // Arama sorgusu
            c.contestantProfil.FullName.Contains(searchQuery) ||
            c.Projects.Name.Contains(searchQuery) ||
            c.Projects.ProjectCategory.Name.Contains(searchQuery))
       )
       .ToList();

            // Sayfalama
            var paginatedContestants = filteredContestants
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ContestantViewModel
                {
                    ContestantId = c.Id,
                    ContestantName = c.contestantProfil.FullName,
                    ProjectName = c.Projects.Name,
                    ProjectCategoryName = c.Projects.ProjectCategory.Name,
                    AcademicJudgeScore = db.ScoreProjects
                        .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 1)
                        .Select(sp => sp.Score ?? 0) // Null olanları 0 yap
                        .FirstOrDefault(),
                    IndustrialJudgeScore = db.ScoreProjects
                        .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 2)
                        .Select(sp => sp.Score ?? 0) // Null olanları 0 yap
                        .FirstOrDefault(),
                    AverageScore = ((db.ScoreProjects
                .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 1)
                .Select(sp => (double?)sp.Score) // Score nullable hale getirilir
                .FirstOrDefault() ?? 0.0) + // Nullable double varsa kullanılır, yoksa 0.0
                (db.ScoreProjects
                .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 2)
                .Select(sp => (double?)sp.Score) // Score nullable hale getirilir
                .FirstOrDefault() ?? 0.0)) / 2.0 // Ortalama alınır
                })
                .ToList();

            return View(new ContestantTableVM
            {
                Contestants = paginatedContestants,
                TotalContestants = filteredContestants.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                SearchQuery = searchQuery
            });
        }

    }
}

