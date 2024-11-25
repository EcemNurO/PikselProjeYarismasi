using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yarışma.Areas.Management.Models;
using Yarışma.Models;

namespace Yarışma.Areas.Management.Controllers
{
    [Area("Management")]
    public class ContestantController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult Contestant(string searchQuery, int page = 1, int pageSize = 10)
                {
                        var filteredContestants = db.Contestants
              .Include(c => c.Projects)
                  .ThenInclude(p => p.ProjectCategory)
              .Include(c => c.contestantProfil)
              .Include(c => c.ContestantCategory)
              .Where(c =>
                  string.IsNullOrEmpty(searchQuery) ||
                  c.contestantProfil.FullName.Contains(searchQuery) ||
                  c.ContestantCategory.Name.Contains(searchQuery) ||
                  c.Projects.Name.Contains(searchQuery) ||
                  c.Projects.ProjectCategory.Name.Contains(searchQuery))
              .Skip((page - 1) * pageSize)
              .Take(pageSize)
              .ToList();


            var model = new ContestantTableVM
            {
                Contestants = filteredContestants.Select(c => new ContestantViewModel
                {
                    ContestantId = c.Id,
                    ContestantName = c.contestantProfil?.FullName,
                    ContestantCategoryName = c.ContestantCategory?.Name,
                    ProjectName = c.Projects?.Name,
                    ProjectCategoryName = c.Projects?.ProjectCategory?.Name,
                    AssignedAcademicJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(), // Akademik hakem adı
                    AssignedIndustrialJudgeName = db.ProjectEvaluations
                        .Where(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2)
                        .Select(pe => pe.Judge.JudgeProfil.FullName)
                        .FirstOrDefault(), // Endüstriyel hakem adı
                    IsAcademicJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 1),
                    IsIndustrialJudgeAssigned = db.ProjectEvaluations.Any(pe => pe.ProjectId == c.Projects.Id && pe.JudgeCategoryId == 2),
                }).ToList(),
                TotalContestants = filteredContestants.Count(),
                PageSize = pageSize,
                CurrentPage = page,
                SearchQuery = searchQuery
            };
            return View(model);
        }
    }
}
