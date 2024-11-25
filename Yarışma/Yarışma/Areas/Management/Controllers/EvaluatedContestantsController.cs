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
        .Include(c => c.ContestantCategory)
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
                    ContestantCategoryName = c.ContestantCategory?.Name,
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
                .FirstOrDefault() ?? 0.0)) / 2.0 ,
                    AcademicJudgeName = db.ScoreProjects
            .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 1)
            .Select(sp => sp.ProjectEvaluation.Judge.JudgeProfil.FullName)
            .FirstOrDefault() ?? "Hakem yok",
                    IndustrialJudgeName = db.ScoreProjects
            .Where(sp => sp.ProjectEvaluation.ProjectId == c.Projects.Id && sp.ProjectEvaluation.JudgeCategoryId == 2)
            .Select(sp => sp.ProjectEvaluation.Judge.JudgeProfil.FullName)
            .FirstOrDefault() ?? "Hakem yok"
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
        public IActionResult Details(int id)
        {
            var contestant = db.Contestants
                .Include(c => c.Projects)
                    .ThenInclude(p => p.ProjectCategory)
                .Include(c => c.contestantProfil)
                .FirstOrDefault(c => c.Id == id);

            if (contestant == null)
            {
                return NotFound();
            }

            var judgeComments = db.ScoreProjects
                .Include(sp => sp.ProjectEvaluation)
                    .ThenInclude(pe => pe.Judge)
                .Where(sp => sp.ProjectEvaluation.ProjectId == contestant.Projects.Id)
                .Select(sp => new JudgeCommentViewModel
                {
                    JudgeName = sp.ProjectEvaluation.Judge.JudgeProfil.FullName,
                    JudgeCategory = sp.ProjectEvaluation.JudgeCategory.Name,
                    Category = sp.ProjectEvaluation.JudgeCategoryId == 1 ? "Akademik" : "Endüstri",
                    Comments = sp.Comments,
                    Score = sp.Score
                })
                .ToList();

            return View(new ContestantDetailsVM
            {
                ContestantId = contestant.Id,
                ContestantName = contestant.contestantProfil.FullName,
                ProjectName = contestant.Projects.Name,
                ProjectCategoryName = contestant.Projects.ProjectCategory.Name,
                JudgeComments = judgeComments
            });
        }


    }
}

