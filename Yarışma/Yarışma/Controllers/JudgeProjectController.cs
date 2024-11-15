using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Yarışma.Models;

namespace Yarışma.Controllers
{
    public class JudgeProjectController : Controller
    {
        CompetitionDbContext db = new CompetitionDbContext();
        public IActionResult ProjectList()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var projects = db.ProjectEvaluations
                .Include(e => e.Project)
                    .ThenInclude(p => p.ProjectCategory)
                .Where(e => e.JudgeId == userId)
                .Select(e => new
                {
                    e.Id,
                    ProjectName = e.Project.Name,
                    ProjectCategoryName = e.Project.ProjectCategory.Name,
                    Score = e.Score,
                    Comments = e.Comments
                })
                .ToList();

            return View(projects);
        }
        public IActionResult EvaluateProject(int relationId)
        {
            var relation = db.ProjectEvaluations
                .Include(r => r.Project)
                .FirstOrDefault(r => r.Id == relationId);

            if (relation == null)
            {
                TempData["ErrorMessage"] = "Proje bulunamadı.";
                return RedirectToAction("ProjectList");
            }

            var model = new
            {
                RelationId = relation.Id,
                ProjectName = relation.Project.Name,
                ExistingScore = relation.Score,
                ExistingComments = relation.Comments
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult SaveEvaluation(int relationId, int score, string comments)
        {
            var relation = db.ProjectEvaluations.FirstOrDefault(r => r.Id == relationId);

            if (relation == null)
            {
                TempData["ErrorMessage"] = "Proje bulunamadı.";
                return RedirectToAction("ProjectList");
            }

            relation.Score = score;
            relation.Comments = comments;
            db.SaveChanges();

            TempData["SuccessMessage"] = "Proje başarıyla değerlendirildi.";
            return RedirectToAction("ProjectList");

        }
        [Authorize(Roles = "Judge")]
        public IActionResult ProjectDetails(int projectId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var evaluation = db.ProjectEvaluations
                .Include(e => e.Project)
                .ThenInclude(p => p.ProjectAnswers)
                .ThenInclude(p => p.Project.ProjectQuestions)
                .Include(e => e.Project.ProjectCategory)
                .FirstOrDefault(e => e.ProjectId == projectId && e.JudgeId == userId);

            if (evaluation == null)
            {
                TempData["ErrorMessage"] = "Proje bulunamadı veya bu projeye erişiminiz yok.";
                return RedirectToAction("ProjectList");
            }

            var model = new ProjectEvaluationViewModel
            {
                Id = evaluation.Id,
                ProjectName = evaluation.Project.Name,
                ProjectCategoryName = evaluation.Project.ProjectCategory.Name,
                Score = evaluation.Score,
                Comments = evaluation.Comments,
                ProjectQuestions = evaluation.Project.ProjectAnswers
                .Select(pa => pa.Question)
                .Distinct()
                .ToList(),
                ProjectAnswers = evaluation.Project.ProjectAnswers.ToList()
            };


            return View(model);
        }

    }
}
